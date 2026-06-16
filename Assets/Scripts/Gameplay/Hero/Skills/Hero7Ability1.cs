using System;
using System.Collections;
using Data;
using DG.Tweening;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero7Ability1 : HeroAbilityShared
	{
        private int heroID = 7;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int percentHealthActivate;

        private int amountOfTrap;

        private float trapLifeTime;

        private int physicsDamage;

        private int magicDamage;

        private int slowPercent;

        private float slowDuration;

        private float skillRange;

        private float cooldownTime;

        private float timeTracking;

        private string description;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        [SerializeField]
        private HeroAbilityAOEShared skillObject;

        private string buffKey = "Slow";

        private OnHitStatusApplier effectAttackSender;

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
			HeroAbilitySpec_7_1 heroSkillParameter_7_ = new HeroAbilitySpec_7_1();
			heroSkillParameter_7_ = (HeroAbilitySpec_7_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			percentHealthActivate = heroSkillParameter_7_.getParam(currentSkillLevel - 1).percent_health_activate;
			amountOfTrap = heroSkillParameter_7_.getParam(currentSkillLevel - 1).amount_of_trap;
			trapLifeTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).trap_life_time / 1000f;
			physicsDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).magic_damage;
			slowPercent = heroSkillParameter_7_.getParam(currentSkillLevel - 1).slow_percent;
			slowDuration = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).slow_duration / 1000f;
			skillRange = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_7_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			heroModel.OnBeHitEvent += HeroModel_OnBeHitEvent;
			effectAttackSender = default(OnHitStatusApplier);
			effectAttackSender.buffKey = buffKey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = slowPercent;
			effectAttackSender.debuffEffectDuration = slowDuration;
			effectAttackSender.damageFXType = DamageVfxType.Slow;
			InitFXs();
		}

		private void HeroModel_OnBeHitEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && IsInThreatenState() && unLock && heroModel.currentTarget)
			{
				base.StartCoroutine(CastSkill(heroModel.currentTarget));
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
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(skillObject.gameObject);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_SLOW);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.TIMER_BOMB_EXPLOSION);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill(EnemyData target)
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_0);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_0
			});
			timeTracking = cooldownTime;
			Vector3 offsetVector = target.transform.position - heroModel.transform.position;
			offsetVector = Vector3.Normalize(offsetVector);
			Vector3 newPosition = heroModel.transform.position - offsetVector;
			heroModel.transform.DOMove(newPosition, animDuration, false).SetEase(Ease.Linear).OnComplete(new TweenCallback(MoveToAssignedPositionComplete));
			yield return new WaitForSeconds(delayTimeCastSkill);
			for (int i = 0; i < amountOfTrap; i++)
			{
				HeroAbilityAOEShared heroSkillAOECommon = MonoSingleton<BulletPool>.Instance.GetHeroSkillAOECommon("Hero7Skill1Trap");
				heroSkillAOECommon.transform.position = target.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * skillRange;
				heroSkillAOECommon.Init_DamageOnTrap(new SharedStrikeDamage(physicsDamage, magicDamage, skillRange), effectAttackSender, trapLifeTime, FXPool.TIMER_BOMB_EXPLOSION, 1f);
			}
			yield break;
		}

		private void MoveToAssignedPositionComplete()
		{
			heroModel.SetAssignedPosition(heroModel.transform.position);
			if (heroModel.currentTarget)
			{
				heroModel.currentTarget.EnemyFindTargetController.Target = null;
				heroModel.AddTarget(null);
			}
		}
	}
}
