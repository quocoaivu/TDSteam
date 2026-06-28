using System;
using Parameter;

namespace Gameplay
{
	public class Turret1Mastery1Ability1 : TurretMasteryShared
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
			TryToUnlockSkillSlash();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			duration = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			cooldownTime = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			unlock = true;
		}

		public void TryToUnlockSkillSlash()
		{
			if (unlock)
			{
				towerSpawnAllyController.UnlockSkillSlash(duration, cooldownTime, description, true);
				towerModel.towerSoundController.PlayCastSkillSound(skillID);
			}
		}

		private int towerID = 1;

		private int ultimateBranch = 1;

		private int skillID = 1;

		private TurretEntity towerModel;

		private TurretSummonMinionHandler towerSpawnAllyController;

		private float duration;

		private float cooldownTime;

		private string description;
	}
}
