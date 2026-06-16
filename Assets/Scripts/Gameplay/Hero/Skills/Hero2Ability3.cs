using System;
using System.Collections;
using Data;
using DG.Tweening;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero2Ability3 : HeroAbilityShared
	{
        private int heroID = 2;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private float aoeRange;

        private int damage;

        private string buffkey = "Slow";

        private int slowValue;

        private float slowDuration;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private float animationTime = 2.5f;

        private OnHitStatusApplier effectAttackSender;

        private float cooldownTimeTracking;

        private Tweener tween;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        [SerializeField]
        private VisualEffectSpawner effectCaster;

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
			cooldownTimeTracking = Mathf.MoveTowards(cooldownTimeTracking, 0f, Time.deltaTime);
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_2_3 heroSkillParameter_2_ = new HeroAbilitySpec_2_3();
			heroSkillParameter_2_ = (HeroAbilitySpec_2_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			aoeRange = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).aoeRange / GameRecord.PIXEL_PER_UNIT;
			damage = heroSkillParameter_2_.getParam(currentSkillLevel - 1).damage;
			slowValue = heroSkillParameter_2_.getParam(currentSkillLevel - 1).slow_value;
			slowDuration = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).slow_duration / 1000f;
			cooldownTime = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_2_.getParam(currentSkillLevel - 1).description;
			cooldownTimeTracking = cooldownTime;
			effectAttackSender.buffKey = buffkey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = slowValue;
			effectAttackSender.debuffEffectDuration = slowDuration;
			effectAttackSender.damageFXType = DamageVfxType.Stun;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
			heroModel.OnSpecialStateEvent += HeroModel_OnSpecialStateEvent;
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.GROUND_STOMP);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_STUN);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && unLock)
			{
				base.StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(animationTime);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_2);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_2
			});
			yield break;
		}

		private void CastFX()
		{
			effectCaster.CastEffect(FXPool.GROUND_STOMP, 2f, base.transform.position);
		}

		private void HeroModel_OnSpecialStateEvent()
		{
			DamageWithAOE();
			CastFX();
			cooldownTimeTracking = cooldownTime;
		}

		private bool IsCooldownDone()
		{
			return cooldownTimeTracking == 0f;
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageKind.Range, new SharedStrikeDamage(damage, 0, aoeRange), effectAttackSender);
		}

	}
}
