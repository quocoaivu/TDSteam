using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero7Ability2 : HeroAbilityShared
	{
        private int heroID = 7;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int chanceToCastSkill;

        private int physicsDamage;

        private int magicDamage;

        private float skillRange;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        [SerializeField]
        private GameObject targetPosition;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;
        
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
			HeroAbilitySpec_7_2 heroSkillParameter_7_ = new HeroAbilitySpec_7_2();
			heroSkillParameter_7_ = (HeroAbilitySpec_7_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			chanceToCastSkill = heroSkillParameter_7_.getParam(currentSkillLevel - 1).chance_to_cast;
			physicsDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_7_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.GROUND_BREAK);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(targetPosition.transform.position, skillRange);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private bool IsAbleToCastSkill()
		{
			return chanceToCastSkill <= UnityEngine.Random.Range(0, 100);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && IsAbleToCastSkill() && unLock)
			{
				base.StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_1);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_1
			});
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			damageToAOERange.CastDamage(targetPosition, DamageKind.Range, new SharedStrikeDamage(physicsDamage, magicDamage, skillRange));
			VisualEffectInstance fx = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.GROUND_BREAK);
			fx.Init(3f, targetPosition.transform.position);
			yield break;
		}
	}
}
