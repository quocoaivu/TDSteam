using System;
using Data;
using Parameter;

namespace Gameplay
{
	public class Hero0Ability2 : HeroAbilityShared
	{
        private int heroID;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;


        private float armor;
        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_0_2 heroSkillParameter_0_ = new HeroAbilitySpec_0_2();
			heroSkillParameter_0_ = (HeroAbilitySpec_0_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			armor = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).armor / 100f;
			AddPassiveArmor();
		}

		private void AddPassiveArmor()
		{
			if (unLock)
			{
				heroModel.HeroHealthController.OriginPhysicsArmor += armor;
				heroModel.HeroHealthController.OriginMagicArmor += armor;
			}
		}
	}
}
