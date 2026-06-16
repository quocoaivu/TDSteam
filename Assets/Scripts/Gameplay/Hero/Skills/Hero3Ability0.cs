using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Ability0 : HeroAbilityShared
	{
        private int heroID = 3;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int physicsDamage;

        private int magicDamage;

        private float skillRange;

        private float cooldownTime;

        private float meteorSpeed;

        private float duration;

        private string description;

        private string useType;

        [SerializeField]
        private Hero3Ability0Comet meteorPrefab;

        private float offsetHigh = 6f;

        private void Start()
		{
			HerosDirector.Instance.onCastHeroSkillToAssignedPosition += Instance_onCastHeroSkillToAssignedPosition;
		}

		private void OnDestroy()
		{
			HerosDirector.Instance.onCastHeroSkillToAssignedPosition -= Instance_onCastHeroSkillToAssignedPosition;
		}

		private void Instance_onCastHeroSkillToAssignedPosition(int heroID, Vector2 targetPosition)
		{
			if (this.heroID == heroID)
			{
				base.StartCoroutine(CastSkill(targetPosition));
			}
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_3_0 heroSkillParameter_3_ = new HeroAbilitySpec_3_0();
			heroSkillParameter_3_ = (HeroAbilitySpec_3_0)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_3_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_3_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			meteorSpeed = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).meteor_speed / GameRecord.PIXEL_PER_UNIT;
			duration = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_3_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_3_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(meteorPrefab.gameObject);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.METEOR_EXPLOSION2);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.METEOR_SELF_EXPLOSION);
		}

		public override float GetCooldownTime()
		{
			return cooldownTime;
		}

		public override string GetUseType()
		{
			return useType;
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(1f);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			yield return new WaitForSeconds(0.5f);
			CreateMeteor(targetPosition);
			yield break;
		}

		private void CreateMeteor(Vector2 targetPosition)
		{
			Hero3Ability0Comet hero3Skill0Meteor = MonoSingleton<BulletPool>.Instance.GetHero3Skill0Meteor();
			hero3Skill0Meteor.transform.position = new Vector2(targetPosition.x, offsetHigh);
			hero3Skill0Meteor.Init(physicsDamage, magicDamage, skillRange, duration, meteorSpeed, base.transform.position, targetPosition, offsetHigh - targetPosition.y);
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[3].DoCooldown();
		}
	}
}
