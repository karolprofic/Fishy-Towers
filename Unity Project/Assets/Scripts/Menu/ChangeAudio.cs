using ManagersSpace;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAudio : MonoBehaviour
{
    //public/inspector
    [SerializeField]
    private Slider musicSlider;
    
    [SerializeField]
    private Slider effectsSlider;

    //unity methods
    private void Start()
    {
        musicSlider.value = Managers.Settings.GetMusicVolume();
        effectsSlider.value = Managers.Settings.GetEffectsVolume();
        musicSlider.onValueChanged.AddListener(delegate {
            Managers.Settings.SetMusicVolume(musicSlider.value); 
        });
        effectsSlider.onValueChanged.AddListener(delegate {
            Managers.Settings.SetEffectsVolume(effectsSlider.value);
        });
    }

}
