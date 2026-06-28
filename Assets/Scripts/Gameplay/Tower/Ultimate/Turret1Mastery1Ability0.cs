using System;
using Parameter;

namespace Gameplay
{
	public class Turret1Mastery1Ability0 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
			towerSpawnAllyController = towerModel.towerSpawnAllyController;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
			TryToUnlockAbility();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			dodgeChance = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			ignoreArmorChance = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			unlock = true;
		}

		public void TryToUnlockAbility()
		{
			if (unlock)
			{
				towerSpawnAllyController.UnlockDodgeAbility(dodgeChance);
				towerSpawnAllyController.UnlockIgnoreArmorAbility(ignoreArmorChance);
				towerModel.towerSoundController.PlayCastSkillSound(skillID);
			}
		}

		private int towerID = 1;

		private int ultimateBranch = 1;

		private int skillID;

		private TurretEntity towerModel;

		private TurretSummonMinionHandler towerSpawnAllyController;

		private int dodgeChance;

		private int ignoreArmorChance;
	}
}
