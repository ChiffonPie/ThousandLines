using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ThousandLines
{
	public class AudioManager : MonoBehaviour
	{
		/*
		[SerializeField]
		private AudioSource m_Music;

		[SerializeField]
		private List<AudioSource> m_SoundPool = new List<AudioSource>();

		private List<AudioClip> m_Caches = new List<AudioClip>();
		private Dictionary<string, List<string>> m_AudioMap = new Dictionary<string, List<string>>();

		public bool IsMusicEnabled
		{
			set
			{
				var audioSource = this.m_Music;
				if (audioSource == null || audioSource.clip == null)
					return;

				if (value)
				{
					if (audioSource.isPlaying)
						return;

					audioSource.Play();
				}
				else
				{
					audioSource.Stop();
				}
			}
		}

		public bool IsSoundEffect
		{
			get
			{
				var defaultValue = Convert.ToInt32(true);
				var value = PlayerPrefs.GetInt("Setting_Sound", defaultValue);

				return Convert.ToBoolean(value);
			}
			set
			{
				if (value)
				{
					PlayerPrefs.SetInt("Setting_Sound", Convert.ToInt32(true));
				}
				else
				{
					PlayerPrefs.SetInt("Setting_Sound", Convert.ToInt32(false));
					this.Stop();
				}
			}
		}

		public bool IsBgm
		{
			get
			{
				var defaultValue = Convert.ToInt32(true);
				var value = PlayerPrefs.GetInt("Setting_Bgm", defaultValue);

				return Convert.ToBoolean(value);
			}
			set
			{
				if (value)
				{
					PlayerPrefs.SetInt("Setting_Bgm", Convert.ToInt32(true));
					this.PlayMusic(1, "MAIN");
				}
				else
				{
					PlayerPrefs.SetInt("Setting_Bgm", Convert.ToInt32(false));
					this.m_Music.Stop();
				}
			}
		}

		public async UniTask LoadAudios()
		{
			var list = await AddressablesUtility.LoadAssetsAsync<AudioClip>("audio");

			this.m_Caches.Clear();
			this.m_Caches.AddRange(list);
		}

		private AudioClip LoadAudioClip(string audioName)
		{
			var clip = this.m_Caches.FirstOrDefault(c => c.name == audioName);
			if (clip == null)
			{
				clip = AddressablesUtility.LoadAsset<AudioClip>("audio", audioName);
				this.m_Caches.Add(clip);
			}

			return clip;
		}


		public void Add(string key, string audioName)
		{
			if (this.m_AudioMap.TryGetValue(key, out var list))
			{
				list.Add(audioName);
			}
			else
			{
				list = new List<string>();
				list.Add(audioName);

				this.m_AudioMap.Add(key, list);
			}
		}

		private List<string> GetNames(string key)
		{
			if (this.m_AudioMap.TryGetValue(key, out var list))
			{
				return list;
			}

			return null;
		}


		public void PlayMusic(float volume, string key, string audioName = null)
		{
			if (this.m_Music == null)
				this.m_Music = this.AddComponent<AudioSource>();

			var list = this.GetNames(key);
			if (list == null)
				throw new NullReferenceException();

			if (audioName == null)
				audioName = list.Random();

			var audioSource = this.m_Music;
			if (audioSource.clip == null || !audioSource.clip.name.Equals(audioName))
				audioSource.clip = this.LoadAudioClip(audioName);

			if (this.IsBgm)
			{
				audioSource.loop = true;
				audioSource.volume = volume;
				audioSource.Play();
			}
		}

		public void PlaySound(string audioName, bool loop = false, float volume = 1)
		{
			Debug.Log($"PLAY_SOUND: {audioName}");

			var audioSource = this.m_SoundPool.Find(c => !c.isPlaying);
			if (audioSource == null)
			{
				audioSource = this.CreatedInChild<AudioSource>("AudioSource");
				this.m_SoundPool.Add(audioSource);
			}

			if (this.IsSoundEffect)
			{
				var clip = this.LoadAudioClip(audioName);
				if (clip == null)
					throw new NullReferenceException();

				audioSource.clip = clip;
				audioSource.loop = loop;
				audioSource.volume = volume;
				audioSource.Play();
			}
		}

		public void Stop(string audioName = null)
		{
			this.m_SoundPool.ForEach(audioSource =>
			{
				if (audioSource.clip == null)
					return;

				if (audioName == null || audioName.Equals(audioSource.clip.name))
					audioSource.Stop();
			});
		}

		public void StopLoop(string audioName = null)
		{
			this.m_SoundPool.ForEach(audioSource =>
			{
				if (audioSource.clip == null)
					return;

				if (audioName == null || audioName.Equals(audioSource.clip.name))
					audioSource.loop = false;
			});
		}

		*/
	}
}