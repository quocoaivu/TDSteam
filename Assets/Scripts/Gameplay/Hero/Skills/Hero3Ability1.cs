using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Ability1 : HeroAbilityShared
	{
        private int heroID = 3;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int magicDamageBonus;

        private float attackSpeedIncrease;

        private float duration;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private Hero3Ability1MagicOrb magicOrb;

        private string buffKey0 = "IncreaseAttackSpeed";

        private string buffKey1 = "IncreaseDamageMagic";

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
			if (IsCooldownDone())
			{
				base.StartCoroutine(CastSkill());
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public override void OnHeroReturnPool()
		{
			base.OnHeroReturnPool();
			magicOrb.Hide();
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_3_1 heroSkillParameter_3_ = new HeroAbilitySpec_3_1();
			heroSkillParameter_3_ = (HeroAbilitySpec_3_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			magicDamageBonus = heroSkillParameter_3_.getParam(currentSkillLevel - 1).magic_damage_bonus;
			attackSpeedIncrease = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).attack_speed_increase;
			duration = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_3_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(1f);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_0);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_0
			});
			AbilitiesBuff();
			yield break;
		}

		private void AbilitiesBuff()
		{
			magicOrb.Init(duration);
			heroModel.BuffsHolder.AddBuff(buffKey0, new BuffStatus(true, attackSpeedIncrease, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
			heroModel.BuffsHolder.AddBuff(buffKey1, new BuffStatus(true, (float)magicDamageBonus, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
			timeTracking = cooldownTime;
		}
	}
}
