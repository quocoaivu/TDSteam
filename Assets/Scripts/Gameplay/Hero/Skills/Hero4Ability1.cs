using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Ability1 : HeroAbilityShared
	{
        private int heroID = 4;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private float physicsArmorBonus;

        private float magicArmorBonus;

        private float duration;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private Hero4Ability1Inferno buffEffect;

        private string buffKey0 = "IncreaseArmorPhysics";


        private string buffKey1 = "IncreaseArmorMagic";
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
			buffEffect.Hide();
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_4_1 heroSkillParameter_4_ = new HeroAbilitySpec_4_1();
			heroSkillParameter_4_ = (HeroAbilitySpec_4_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsArmorBonus = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).physics_armor_bonus / 100f;
			magicArmorBonus = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).magic_armor_bonus / 100f;
			duration = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_4_.getParam(currentSkillLevel - 1).description;
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
			buffEffect.Init(duration);
			heroModel.BuffsHolder.AddBuff(buffKey0, new BuffStatus(true, physicsArmorBonus, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
			heroModel.BuffsHolder.AddBuff(buffKey1, new BuffStatus(true, magicArmorBonus, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
			timeTracking = cooldownTime;
		}
	}
}
