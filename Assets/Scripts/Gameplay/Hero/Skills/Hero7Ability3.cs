using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero7Ability3 : HeroAbilityShared
	{
        private int heroID = 7;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int chanceToCastSkill;

        private int physicsDamage;

        private int magicDamage;

        private float skillRange;

        private float countdownTime;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        [SerializeField]
        private HeroAbilityAOEShared skillObject;
        
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
			HeroAbilitySpec_7_3 heroSkillParameter_7_ = new HeroAbilitySpec_7_3();
			heroSkillParameter_7_ = (HeroAbilitySpec_7_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			chanceToCastSkill = heroSkillParameter_7_.getParam(currentSkillLevel - 1).chance_to_cast;
			physicsDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			countdownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).countdown_time / 1000f;
			cooldownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_7_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			heroModel.OnAttackEvent += HeroModel_OnAttackEvent;
			InitFXs();
		}

		private void HeroModel_OnAttackEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && IsAbleToCastSkill() && unLock && heroModel.currentTarget)
			{
				base.StartCoroutine(CastSkill(heroModel.currentTarget));
			}
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.TIMER_BOMB_EXPLOSION);
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(skillObject.gameObject);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private bool IsAbleToCastSkill()
		{
			return chanceToCastSkill <= UnityEngine.Random.Range(0, 100);
		}

		private IEnumerator CastSkill(EnemyData target)
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_2);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_2
			});
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			HeroAbilityAOEShared bullet = MonoSingleton<BulletPool>.Instance.GetHeroSkillAOECommon("Hero7Skill3TimerBomb");
			bullet.transform.position = target.transform.position;
			bullet.Init_DamgeAfterTime(new SharedStrikeDamage(physicsDamage, magicDamage, skillRange), countdownTime, FXPool.TIMER_BOMB_EXPLOSION, 1f);
			yield break;
		}
	}
}
