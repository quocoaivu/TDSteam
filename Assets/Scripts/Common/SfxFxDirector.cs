using System;
using MetaGame;
using UnityEngine;

namespace Common
{
	public class SfxFxDirector : MonoBehaviour, ISfxFxDirector
	{
        private const int BrokenEffectInstanceId = -1;

        private static ISfxFxDirector instance;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }
        [SerializeField]
        private AudioSource audioSourcePrototype;

        [SerializeField]
        private int maxConcurrentEffect = 5;

        private AudioSource[] audioSources;

        private int[] effectInstanceIds;

        private int currentIndex;

        private int lastEffectInstanceId;

        public static ISfxFxDirector Instance
		{
			get
			{
				return SfxFxDirector.instance;
			}
			private set
			{
				SfxFxDirector.instance = value;
			}
		}

		public void Awake()
		{
			HandleSingletonInstance();
			CreateAudioSources();
			InitializeEffectInstamceIds();
		}

		private void HandleSingletonInstance()
		{
			if (SfxFxDirector.instance == null)
			{
				SfxFxDirector.instance = this;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void CreateAudioSources()
		{
			audioSources = new AudioSource[maxConcurrentEffect];
			for (int i = 0; i < maxConcurrentEffect; i++)
			{
				audioSources[i] = UnityEngine.Object.Instantiate<AudioSource>(audioSourcePrototype);
				audioSources[i].transform.SetParent(base.transform);
			}
		}

		private void InitializeEffectInstamceIds()
		{
			effectInstanceIds = new int[maxConcurrentEffect];
			for (int i = 0; i < maxConcurrentEffect; i++)
			{
				effectInstanceIds[i] = lastEffectInstanceId;
			}
		}

		private void NextIndex()
		{
			currentIndex = (currentIndex + 1) % maxConcurrentEffect;
		}

		public int PlayEffect(AudioClip audioClip, float volumeScale)
		{
			int num = -1;
			if (Setup.Instance.Sound)
			{
				lastEffectInstanceId++;
				num = lastEffectInstanceId;
				effectInstanceIds[currentIndex] = num;
				audioSources[currentIndex].clip = audioClip;
				audioSources[currentIndex].Play();
				NextIndex();
			}
			return num;
		}

		public void TryStopEffect(int effectInstanceId)
		{
			for (int i = 0; i < maxConcurrentEffect; i++)
			{
				if (effectInstanceIds[i] == effectInstanceId)
				{
					effectInstanceIds[i] = -1;
					audioSources[i].Stop();
				}
			}
		}
	}
}
