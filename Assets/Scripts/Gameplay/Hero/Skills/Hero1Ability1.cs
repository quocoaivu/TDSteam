using System;
using Data;
using Parameter;

namespace Gameplay
{
	public class Hero1Ability1 : HeroAbilityShared
	{
        private int heroID = 1;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;


        private int passiveCriticalStrikeValue;
        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_1_1 heroSkillParameter_1_ = new HeroAbilitySpec_1_1();
			heroSkillParameter_1_ = (HeroAbilitySpec_1_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			passiveCriticalStrikeValue = heroSkillParameter_1_.getParam(currentSkillLevel - 1).bonus_crit;
		}

		public void Init(bool unlock, HeroEntity heroModel, int _bonusCrit)
		{
			unLock = unlock;
			this.heroModel = heroModel;
			passiveCriticalStrikeValue = _bonusCrit;
			heroModel.OnAttackEvent += HeroModel_OnAttackEvent;
		}

		private void HeroModel_OnAttackEvent()
		{
			if (unLock)
			{
				AddPassiveCriticalStrike();
			}
		}

		private void AddPassiveCriticalStrike()
		{
			heroModel.HeroAttackController.CurrentCriticalStrikeChance = heroModel.HeroAttackController.OriginCriticalStrikeChance + passiveCriticalStrikeValue;
		}
	}
}
