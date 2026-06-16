using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Ability2 : HeroAbilityShared
	{
        private int heroID = 3;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private float skillRange;

        private int slowPercent;

        private int damageBurn;

        private float duration;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        private string buffKey = "Slow";

        [SerializeField]
        private Hero3Ability2IceTrap iceTrap;

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

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_3_2 heroSkillParameter_3_ = new HeroAbilitySpec_3_2();
			heroSkillParameter_3_ = (HeroAbilitySpec_3_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			skillRange = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			slowPercent = heroSkillParameter_3_.getParam(currentSkillLevel - 1).slow_percent;
			damageBurn = heroSkillParameter_3_.getParam(currentSkillLevel - 1).damage_burn;
			duration = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_3_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(iceTrap.gameObject);
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
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_1);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_1
			});
			CastIceTrap();
			yield break;
		}

		private void CastIceTrap()
		{
			EnemyData currentTarget = heroModel.currentTarget;
			if (currentTarget)
			{
				Hero3Ability2IceTrap hero3Skill2IceTrap = MonoSingleton<BulletPool>.Instance.GetHero3Skill2IceTrap();
				hero3Skill2IceTrap.transform.position = currentTarget.transform.position;
				hero3Skill2IceTrap.Init(damageBurn, buffKey, slowPercent, duration);
			}
			timeTracking = cooldownTime;
		}
	}
}
