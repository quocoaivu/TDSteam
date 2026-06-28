using System;
using Parameter;

namespace Gameplay
{
	public class Turret0Mastery1Ability0 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
			AddBuff();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		// Unlock adds a permanent instant-kill buff to the tower, so locking must remove it (clearing
		// unlock alone would leave the buff active after the item is unequipped).
		public override void LockUltimate()
		{
			base.LockUltimate();
			if (towerModel != null)
			{
				towerModel.BuffsHolder.RemoveBuffs(buffKey);
			}
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToInstantKill = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			duration = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			unlock = true;
		}

		private void AddBuff()
		{
			bool isPositive = true;
			towerModel.BuffsHolder.AddBuff(buffKey, new BuffStatus(isPositive, (float)chanceToInstantKill, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
			towerModel.towerSoundController.PlayCastSkillSound(skillID);
		}

		private int towerID;

		private int ultimateBranch = 1;

		private int skillID;

		private int chanceToInstantKill;

		private float duration;

		private TurretEntity towerModel;

		private string buffKey = "InstantKillRateIncrementCommon";
	}
}
