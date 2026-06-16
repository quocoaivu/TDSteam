using System;
using Parameter;

namespace Gameplay
{
	public class Turret1Mastery0Ability1 : TurretMasteryShared
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
			ReadParameter(ultiLevel);
			TryToAddPassiveArmor();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			bonusArmorPhysics = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			bonusArmorMagic = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
		}

		public void TryToAddPassiveArmor()
		{
			if (unlock)
			{
				towerSpawnAllyController.AddPassiveArmor(bonusArmorPhysics, bonusArmorMagic);
			}
		}

		private int towerID = 1;

		private int ultimateBranch;

		private int skillID = 1;

		private float bonusArmorPhysics;

		private float bonusArmorMagic;

		private TurretEntity towerModel;

		private TurretSummonMinionHandler towerSpawnAllyController;
	}
}
