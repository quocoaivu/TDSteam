using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Ability3 : HeroAbilityShared
	{
        private int heroID = 5;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int arrowNumber;

        private float skillRange;

        private int damagePhysics;

        private float cooldownTime;

        private float duration;

        private float delayTime;

        private float timeTracking;

        private string description;

        private bool isCastSkill;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private Hero5Ability3LightningSpear lightningSpear;

        [SerializeField]
        private Transform gunBarrel;

        private void Start()
		{
			MonoSingleton<ZoneHandler>.Instance.OnEnemyReachGate += Instance_OnEnemyReachGate;
		}

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
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_5_3 heroSkillParameter_5_ = new HeroAbilitySpec_5_3();
			heroSkillParameter_5_ = (HeroAbilitySpec_5_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			arrowNumber = heroSkillParameter_5_.getParam(currentSkillLevel - 1).arrow_number;
			skillRange = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			damagePhysics = heroSkillParameter_5_.getParam(currentSkillLevel - 1).damage_physics;
			cooldownTime = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			duration = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).duration / 1000f;
			delayTime = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).delay_time / 1000f;
			description = heroSkillParameter_5_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(lightningSpear.gameObject);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.LIGHTNING_EXPLOSION_2);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void Instance_OnEnemyReachGate(Vector2 targetPosition)
		{
			TryToCastSkill(targetPosition);
		}

		private void TryToCastSkill(Vector2 targetPosition)
		{
			if (!unLock)
			{
				return;
			}
			if (IsCooldownDone() && !isCastSkill)
			{
				base.StartCoroutine(CastSkill(targetPosition));
			}
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			UnityEngine.Debug.Log("Chironia cast skill!");
			isCastSkill = true;
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			for (int i = 0; i < arrowNumber; i++)
			{
				Hero5Ability3LightningSpear lSpear = MonoSingleton<BulletPool>.Instance.GetHero5Skill3LightningSpear();
				lSpear.transform.position = gunBarrel.position;
				Vector2 newTargetPosistion = targetPosition + UnityEngine.Random.insideUnitCircle * 0.3f;
				lSpear.Init(skillRange, damagePhysics, duration, newTargetPosistion);
				yield return new WaitForSeconds(delayTime);
			}
			timeTracking = cooldownTime;
			isCastSkill = false;
			yield break;
		}
	}
}
