using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero6Ability1 : HeroAbilityShared
	{
        private int heroID = 6;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int attackDamageBonusPercentage;

        private float duration;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        private string increaseDamageBuffkey = "BuffAttackByPercentage";

        [SerializeField]
        private float animDuration;
        
		public override void Update()
		{
			base.Update();
			if (!MonoSingleton<GameRecord>.Instance.IsGameStart)
			{
				return;
			}
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

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_6_1 heroSkillParameter_6_ = new HeroAbilitySpec_6_1();
			heroSkillParameter_6_ = (HeroAbilitySpec_6_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			attackDamageBonusPercentage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).percent_attack_damage_bonus;
			duration = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_6_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime;
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.INFERNO_GOLEM_AURA);
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
			heroModel.SetSpecialStateDuration(animDuration);
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
			List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
			foreach (CharacterEntity characterModel in listActiveAlly)
			{
				characterModel.BuffsHolder.AddBuff(increaseDamageBuffkey, new BuffStatus(true, (float)attackDamageBonusPercentage, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.INFERNO_GOLEM_AURA);
				effect.transform.position = characterModel.transform.position;
				effect.Init(duration, characterModel.transform);
			}
			timeTracking = cooldownTime;
		}
	}
}
