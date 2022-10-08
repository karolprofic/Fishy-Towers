using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ManagersSpace
{
	public class AudioManager : Manager
	{
		//properties
		public AudioSource SoundsSource => effectsSource;
		
		//public/inspector
		[SerializeField] private List<AudioClip> audioClips;
		[SerializeField] private AudioSource musicSource;
		[SerializeField] private AudioSource effectsSource;
		[SerializeField] private int audioClipIndex;
		
		[SerializeField] private AudioMixer musicMixer;
		[SerializeField] private AudioMixer soundsMixer;

		//unity methods
		private void Start()
		{
			musicSource.volume = Managers.Settings.GetMusicVolume();
			effectsSource.volume = Managers.Settings.GetEffectsVolume();
			Shuffle(audioClips);
			PlayMusic();
		}
		
		//public methods
		public void ChangeEffectsVolume(float value)
		{
			effectsSource.volume = value;
		}

		public void ChangeMusicVolume(float value)
		{
			musicSource.volume = value;
		}
		
		//private methods
		private void PlayMusic()
		{
			musicSource.clip = audioClips[audioClipIndex];
			musicSource.Play();
			audioClipIndex++;
			if (audioClipIndex > audioClips.Count)
				audioClipIndex = 0;
			Invoke(nameof(PlayMusic), musicSource.clip.length);
		}
		
		private void PlayEffect(AudioClip effect)
		{
			effectsSource.PlayOneShot(effect);
		}
		
		//Todo: zrobić jako static i prznieść do Ext.cs
		private void Shuffle<T>(List<T> inputList)
		{
			for (int i = 0; i < inputList.Count - 1; i++)
			{
				T temp = inputList[i];
				int rand = Random.Range(i, inputList.Count);
				inputList[i] = inputList[rand];
				inputList[rand] = temp;
			}
		}
	}
	
}
