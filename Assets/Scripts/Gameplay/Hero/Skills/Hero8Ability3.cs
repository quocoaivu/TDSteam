using System;
using Data;
using Parameter;

namespace Gameplay
{
	public class Hero8Ability3 : HeroAbilityShared
	{
        private int heroID = 8;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private int attackRangeBonusPercentage;

        private string buffKey = "BuffAttackRangeByPercentage";

        private string description;
        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_8_3 heroSkillParameter_8_ = new HeroAbilitySpec_8_3();
			heroSkillParameter_8_ = (HeroAbilitySpec_8_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			attackRangeBonusPercentage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).attack_range_bonus_percentage;
			description = heroSkillParameter_8_.getParam(currentSkillLevel - 1).description;
			base.CustomInvoke(new Action(BuffAttackRange), 1f);
		}

		private void BuffAttackRange()
		{
			heroModel.BuffsHolder.AddBuff(buffKey, new BuffStatus(true, (float)attackRangeBonusPercentage, 999999f), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
		}
	}
}
