using System;
using Data;
using Parameter;

namespace Gameplay
{
	public class Hero5Ability1 : HeroAbilityShared
	{
        private int heroID = 5;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int bonusDamage;

        private float duration;

        private string description;

        private string buffkey = "IncreaseDamagePhysics";

        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_5_1 heroSkillParameter_5_ = new HeroAbilitySpec_5_1();
			heroSkillParameter_5_ = (HeroAbilitySpec_5_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			bonusDamage = heroSkillParameter_5_.getParam(currentSkillLevel - 1).bonus_damage;
			duration = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).duration / 1000f;
			description = heroSkillParameter_5_.getParam(currentSkillLevel - 1).description;
			base.CustomInvoke(new Action(BuffPhysicsDamage), 1f);
		}

		private void BuffPhysicsDamage()
		{
			heroModel.BuffsHolder.AddBuff(buffkey, new BuffStatus(true, (float)bonusDamage, duration), BuffStackRule.StackUp, BuffStackRule.ChooseMax);
		}
	}
}
