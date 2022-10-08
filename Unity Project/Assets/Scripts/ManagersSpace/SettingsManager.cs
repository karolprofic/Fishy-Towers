using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine;

namespace ManagersSpace
{
	public class SettingsManager : Manager
	{
        //public/inspector
        [SerializeField] private float musicLevel = 0.7f;
        [SerializeField] private float effectsLevel = 0.8f;
        //Todo: SystemLanguage zamiast stringa
        [SerializeField] private string languageName;

        //unity methods
        private void Start()
        {
            SetLanguage(languageName);
            SetMusicVolume(musicLevel);
            SetEffectsVolume(effectsLevel);
        }
        
        private void OnEnable()
        {
            //Przenieść ustawianie wartość do deklaracji zmiennych
            musicLevel = 0.7f;
            effectsLevel = 0.8f;  
            languageName = "pl";

            //Todo: "MusicVolume" powinien być jako const string na początku klasy
            //Todo: hasKey do usnięcia z dźwięków zamiast tego użyć PlayerPrefs.GetFloat(MusicVolume, musicLevel)
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                musicLevel = PlayerPrefs.GetFloat("MusicVolume"); 
            }
            if (PlayerPrefs.HasKey("EffectsVolume"))
            {
                effectsLevel = PlayerPrefs.GetFloat("EffectsVolume"); 
            }
            
            //Todo: przy pierwszym uruchomieniu switch (Application.systemLanguage) jeżeli jest polski ustawiami polski w każdym innnym przypadku angielski
            if (PlayerPrefs.HasKey("LanguageName"))
            {
                languageName = PlayerPrefs.GetString("LanguageName"); 
            }
        }
        
        //public methods
        public void SetLanguage(string newLanguageName)
        {
            LocaleIdentifier selectedLanguageIdentifier = new LocaleIdentifier(newLanguageName);
            foreach (var language in LocalizationSettings.AvailableLocales.Locales)
            {
                LocaleIdentifier languageIdentifier = language.Identifier;
                if (languageIdentifier == selectedLanguageIdentifier)
                {
                    LocalizationSettings.SelectedLocale = language;
                }
            }
            languageName = newLanguageName;
            PlayerPrefs.SetString("LanguageName", languageName);
            PlayerPrefs.Save();
        }

        public void SetMusicVolume(float audioLevel)
        {
            musicLevel = audioLevel;
            Managers.Audio.ChangeMusicVolume(musicLevel);
            PlayerPrefs.SetFloat("MusicVolume", musicLevel);
            PlayerPrefs.Save();
        }

        public void SetEffectsVolume(float audioLevel)
        {
            effectsLevel = audioLevel;
            Managers.Audio.ChangeEffectsVolume(effectsLevel);
            PlayerPrefs.SetFloat("EffectsVolume", effectsLevel);
            PlayerPrefs.Save();
        }

        public float GetMusicVolume()
        {
            return musicLevel;
        }

        public float GetEffectsVolume()
        {
            return effectsLevel;
        }
    }
}
