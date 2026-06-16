using System;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret1Mastery0Ability0 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
			towerSpawnAllyController = towerModel.towerSpawnAllyController;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(bulletPrefab);
			ReadParameter(ultiLevel);
			TryToUnlockRangeAttackAbility();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			attackRangeFar = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0) / GameRecord.PIXEL_PER_UNIT;
		}

		public void TryToUnlockRangeAttackAbility()
		{
			if (unlock)
			{
				towerSpawnAllyController.UnlockRangeAttackAbility(attackRangeFar);
			}
		}

		private int towerID = 1;

		private int ultimateBranch;

		private int skillID;

		private float attackRangeFar;

		private TurretEntity towerModel;

		private TurretSummonMinionHandler towerSpawnAllyController;

		[SerializeField]
		private GameObject bulletPrefab;
	}
}
