using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Ability3 : HeroAbilityShared
	{
        private int heroID = 3;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private float skillRange;

        private int physicsDamage;

        private int magicDamage;

        private float timeStep;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        private float offsetHigh = 6f;

        [SerializeField]
        private Hero3Ability3SunStrike sunStrike;
        
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
			HeroAbilitySpec_3_3 heroSkillParameter_3_ = new HeroAbilitySpec_3_3();
			heroSkillParameter_3_ = (HeroAbilitySpec_3_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			skillRange = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			physicsDamage = heroSkillParameter_3_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_3_.getParam(currentSkillLevel - 1).magic_damage;
			timeStep = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).time_step / 1000f;
			cooldownTime = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_3_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(sunStrike.gameObject);
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
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_2);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_2
			});
			CastSunStrike();
			yield break;
		}

		private void CastSunStrike()
		{
			EnemyData enemyWithHighestHealth = MonoSingleton<GameRecord>.Instance.getEnemyWithHighestHealth(true, false);
			if (enemyWithHighestHealth)
			{
				Hero3Ability3SunStrike hero3Skill3SunStrike = MonoSingleton<BulletPool>.Instance.GetHero3Skill3SunStrike();
				hero3Skill3SunStrike.transform.position = enemyWithHighestHealth.transform.position;
				hero3Skill3SunStrike.Init(physicsDamage, magicDamage, skillRange, timeStep, offsetHigh - enemyWithHighestHealth.transform.position.y);
			}
			timeTracking = cooldownTime;
		}
	}
}
