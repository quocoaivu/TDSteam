using System;
using Data;
using Parameter;

namespace Gameplay
{
	public class Hero2Ability2 : HeroAbilityShared
	{
        private int heroID = 2;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private int attackCountToCrit;

        private string description;

        private int currentAttackCount;
        
		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_2_2 heroSkillParameter_2_ = new HeroAbilitySpec_2_2();
			heroSkillParameter_2_ = (HeroAbilitySpec_2_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			attackCountToCrit = heroSkillParameter_2_.getParam(currentSkillLevel - 1).count_crit;
			description = heroSkillParameter_2_.getParam(currentSkillLevel - 1).description;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_CRITICAL);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (unLock)
			{
				currentAttackCount++;
				if (currentAttackCount == attackCountToCrit)
				{
					DamageCritEnemy();
					currentAttackCount = 0;
				}
			}
		}

		private void DamageCritEnemy()
		{
			if (heroModel.currentTarget)
			{
				heroModel.HeroAttackController.DamageToEnemy(heroModel.currentTarget);
				heroModel.currentTarget.EnemyEffectController.PlayDamageFX(DamageVfxType.Critical, 2f);
			}
		}

	}
}
