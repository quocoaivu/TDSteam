using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class TurretSfxHandler : TurretHandler
	{
		private void Awake()
		{
			audioSourceNormal = base.GetComponent<AudioSource>();
			audioSourceSkill = base.GetComponentInChildren<AudioSource>();
			if (towerAttackSingleTargetCommonController)
			{
				towerAttackSingleTargetCommonController.OnFireBullet += TowerAttackSingleTargetCommonController_OnFireBullet;
			}
		}

		public override void Update()
		{
			base.Update();
			UpdateVolume();
		}

		private void OnDestroy()
		{
			if (towerAttackSingleTargetCommonController)
			{
				towerAttackSingleTargetCommonController.OnFireBullet -= TowerAttackSingleTargetCommonController_OnFireBullet;
			}
		}

		private void TowerAttackSingleTargetCommonController_OnFireBullet(ProjectileEntity arg1, EnemyData arg2)
		{
			PlayAttack();
		}

		private void UpdateVolume()
		{
			audioSourceNormal.volume = MonoSingleton<TrooperSfxDirector>.Instance.VolumeReader.GetGameplayEffectVolume();
		}

		public void PlayAttack()
		{
			if (Setup.Instance.Sound && attack && audioSourceNormal.isActiveAndEnabled)
			{
				audioSourceNormal.clip = attack;
				audioSourceNormal.Play();
			}
		}

		public void PlayHitEnemyWithArmor()
		{
			if (Setup.Instance.Sound && hitEnemyWithArmor && audioSourceNormal.isActiveAndEnabled)
			{
				audioSourceNormal.clip = hitEnemyWithArmor;
				audioSourceNormal.Play();
			}
		}

		public void PlayHitEnemyWithoutArmor()
		{
			if (Setup.Instance.Sound && hitEnemyWithoutArmor && audioSourceNormal.isActiveAndEnabled)
			{
				audioSourceNormal.clip = hitEnemyWithoutArmor;
				audioSourceNormal.Play();
			}
		}

		public void PlayExplosion()
		{
			if (Setup.Instance.Sound && explosion && audioSourceNormal.isActiveAndEnabled)
			{
				audioSourceNormal.clip = explosion;
				audioSourceNormal.Play();
			}
		}

		public void PlayCastSkillSound(int index)
		{
			if (Setup.Instance.Sound && castSkill.Length > index && castSkill[index] != null && audioSourceSkill.isActiveAndEnabled)
			{
				audioSourceSkill.clip = castSkill[index];
				audioSourceSkill.Play();
			}
		}

		[SerializeField]
		private TurretStrikeSingleMarkSharedHandler towerAttackSingleTargetCommonController;

		private AudioSource audioSourceNormal;

		private AudioSource audioSourceSkill;

		[Space]
		[SerializeField]
		private AudioClip attack;

		[Space]
		[SerializeField]
		private AudioClip hitEnemyWithArmor;

		[Space]
		[SerializeField]
		private AudioClip hitEnemyWithoutArmor;

		[Space]
		[SerializeField]
		private AudioClip explosion;

		[SerializeField]
		private AudioClip[] castSkill;
	}
}
