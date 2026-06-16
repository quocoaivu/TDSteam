using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class FXSfxHandler : MonoBehaviour
	{
        private AudioSource audioSource;

        private void Awake()
		{
			audioSource = base.GetComponent<AudioSource>();
			UpdateVolume();
		}

		private void Update()
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			if (Setup.Instance.Sound)
			{
				audioSource.volume = MonoSingleton<TrooperSfxDirector>.Instance.VolumeReader.GetGameplayEffectVolume();
			}
			else
			{
				audioSource.volume = 0f;
			}
		}
	}
}
