using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero9Ability2 : HeroAbilityShared
	{
        private int heroID = 9;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int percentHealthActivate;

        private int healthAmountPercentage;

        private float duration;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;
        
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
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_9_2 heroSkillParameter_9_ = new HeroAbilitySpec_9_2();
			heroSkillParameter_9_ = (HeroAbilitySpec_9_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			percentHealthActivate = heroSkillParameter_9_.getParam(currentSkillLevel - 1).health_percentage_active;
			healthAmountPercentage = heroSkillParameter_9_.getParam(currentSkillLevel - 1).health_amount;
			duration = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_9_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime * 0.65f;
			heroModel.OnBeHitEvent += HeroModel_OnBeHitEvent;
		}

		private void HeroModel_OnBeHitEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && IsInThreatenState() && unLock && heroModel.currentTarget)
			{
				base.StartCoroutine(CastSkill());
			}
		}

		private bool IsInThreatenState()
		{
			bool result = false;
			if ((float)heroModel.HeroHealthController.CurrentHealth <= (float)percentHealthActivate / 100f * (float)heroModel.HeroHealthController.OriginHealth)
			{
				result = true;
			}
			return result;
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_HEAL_0);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(duration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_1);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_1
			});
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			heroModel.HeroHealthController.AddHealth((int)((float)(heroModel.HeroHealthController.OriginHealth * healthAmountPercentage) / 100f));
			VisualEffectInstance fx = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_HEAL_0);
			fx.Init(duration - delayTimeCastSkill, heroModel.BuffsHolder.transform);
			yield break;
		}
	}
}
