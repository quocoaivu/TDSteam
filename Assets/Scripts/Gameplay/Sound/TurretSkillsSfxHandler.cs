using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class TurretSkillsSfxHandler : TurretHandler
	{
		private void Awake()
		{
			audioSource = base.GetComponent<AudioSource>();
		}

		public override void Update()
		{
			base.Update();
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			audioSource.volume = MonoSingleton<TrooperSfxDirector>.Instance.VolumeReader.GetGameplayEffectVolume();
		}

		public void PlayCastSkillSound(int index)
		{
			if (Setup.Instance.Sound && castSkill != null && index >= 0 && index < castSkill.Length && castSkill[index] != null && audioSource.isActiveAndEnabled)
			{
				audioSource.clip = castSkill[index];
				audioSource.Play();
			}
		}

		[SerializeField]
		private AudioClip[] castSkill;

		private AudioSource audioSource;
	}
}
