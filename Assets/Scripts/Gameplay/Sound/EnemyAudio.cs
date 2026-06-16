using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class EnemyAudio : MonoBehaviour
	{
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip attack;

        [SerializeField]
        private AudioClip dead;

        [SerializeField]
        private int playDeadSoundRate = 30;

        [SerializeField]
        private bool haveVibrateScreenOnAttack;

        private void Awake()
		{
			audioSource = base.GetComponent<AudioSource>();
			audioSource.ignoreListenerPause = true;
		}

		private void Update()
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			audioSource.volume = MonoSingleton<TrooperSfxDirector>.Instance.VolumeReader.GetGameplayEffectVolume();
		}

		public void PlayAttack()
		{
			if (Setup.Instance.Sound && attack && audioSource.isActiveAndEnabled)
			{
				audioSource.clip = attack;
				audioSource.Play();
			}
			if (haveVibrateScreenOnAttack)
			{
				MonoSingleton<LensHandler>.Instance.ShakeNormal();
			}
		}

		public void PlayDead()
		{
			if (Setup.Instance.Sound && dead && UnityEngine.Random.Range(0, 100) < playDeadSoundRate && audioSource.isActiveAndEnabled)
			{
				audioSource.clip = dead;
				audioSource.Play();
			}
		}
	}
}
