using System;
using Parameter;

namespace Gameplay
{
	public class Turret2Mastery0Ability1 : TurretMasteryShared
	{
		public int DamageBurn
		{
			get
			{
				return damageBurn;
			}
			set
			{
				damageBurn = value;
			}
		}

		public int ChanceToCastSkill
		{
			get
			{
				return chanceToCastSkill;
			}
			set
			{
				chanceToCastSkill = value;
			}
		}

		public float Duration
		{
			get
			{
				return duration;
			}
			set
			{
				duration = value;
			}
		}

		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			firstTimeUpgrade = true;
			ReadParameter(ultiLevel);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.METEOR_EXPLOSION);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_BURNING);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			firstTimeUpgrade = false;
		}

		private void ReadParameter(int currentSkillLevel)
		{
			ChanceToCastSkill = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			DamageBurn = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			Duration = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			skillRange = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameRecord.PIXEL_PER_UNIT;
		}

		private int towerID = 2;

		private int ultimateBranch;

		private int skillID = 1;

		private int chanceToCastSkill;

		private int damageBurn;

		private float duration;

		private float skillRange;

		private TurretEntity towerModel;

		public bool firstTimeUpgrade;
	}
}
