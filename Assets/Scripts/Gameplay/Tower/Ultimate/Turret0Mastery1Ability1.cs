using System;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret0Mastery1Ability1 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToBleed = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			bonusDamagePercentage = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			duration = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			unlock = true;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_BLEED);
		}

		public override void OnTowerAttack()
		{
			TryToCastBleed();
		}

		public void TryToCastBleed()
		{
			if (!unlock)
			{
				return;
			}
			if (UnityEngine.Random.Range(0, 100) < chanceToBleed && towerModel.towerFindEnemyController.Targets.Count > 0)
			{
				CastBleed(towerModel.towerFindEnemyController.Targets[0]);
			}
		}

		private void CastBleed(EnemyData enemyModel)
		{
			enemyModel.ProcessEffect(buffKey, bonusDamagePercentage, duration, DamageVfxType.Bleed);
			towerModel.towerSoundController.PlayCastSkillSound(skillID);
		}

		private int towerID;

		private int ultimateBranch = 1;

		private int skillID = 1;

		private string buffKey = "Bleed";

		private int chanceToBleed;

		private int bonusDamagePercentage;

		private float duration;

		private TurretEntity towerModel;
	}
}
