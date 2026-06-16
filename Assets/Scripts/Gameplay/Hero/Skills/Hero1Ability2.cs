using System;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero1Ability2 : HeroAbilityShared
	{
        private int heroID = 1;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private string buffKey = "IncreaseAttackSpeed";

        private int temporaryAttackSpeedBonus;

        private float duration;

        private float cooldownTime;

        private bool isAvailableToUse;


        private float cooldownTimeTracking;
        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_1_2 heroSkillParameter_1_ = new HeroAbilitySpec_1_2();
			heroSkillParameter_1_ = (HeroAbilitySpec_1_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			temporaryAttackSpeedBonus = heroSkillParameter_1_.getParam(currentSkillLevel - 1).attack_speed_increase;
			duration = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
		}

		public override void Update()
		{
			base.Update();
			if (!unLock)
			{
				return;
			}
			if (heroModel && !heroModel.IsAlive)
			{
				return;
			}
			if (cooldownTimeTracking == 0f)
			{
				isAvailableToUse = true;
			}
			cooldownTimeTracking = Mathf.MoveTowards(cooldownTimeTracking, 0f, Time.deltaTime);
		}

		public void AddBuffAttackSpeed()
		{
			if (unLock && isAvailableToUse)
			{
				heroModel.BuffsHolder.AddBuff(buffKey, new BuffStatus(true, (float)temporaryAttackSpeedBonus, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				isAvailableToUse = false;
				cooldownTimeTracking = cooldownTime;
			}
		}
	}
}
