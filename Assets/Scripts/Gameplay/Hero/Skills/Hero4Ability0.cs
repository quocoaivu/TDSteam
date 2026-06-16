using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Ability0 : HeroAbilityShared
	{
        private int heroID = 4;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int physicsDamage;

        private float skillRange;

        private float duration;

        private float cooldownTime;

        private string description;

        private string useType;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        private string buffKey = "Slow";

        [SerializeField]
        private Hero4Ability0Breakdown breakdown;

        private void Start()
		{
			HerosDirector.Instance.onCastHeroSkillToAssignedPosition += Instance_onCastHeroSkillToAssignedPosition;
		}

		private void OnDestroy()
		{
			if (HerosDirector.Instance != null)
			{
				HerosDirector.Instance.onCastHeroSkillToAssignedPosition -= Instance_onCastHeroSkillToAssignedPosition;
			}
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
			HeroAbilitySpec_4_0 heroSkillParameter_4_ = new HeroAbilitySpec_4_0();
			heroSkillParameter_4_ = (HeroAbilitySpec_4_0)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_4_.getParam(currentSkillLevel - 1).physics_damage;
			skillRange = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			duration = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_4_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_4_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(breakdown.gameObject);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_STUN);
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
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			yield return new WaitForSeconds(delayTimeCastSkill);
			Breakdown(targetPosition);
			yield break;
		}

		private void Breakdown(Vector2 targetPosition)
		{
			Hero4Ability0Breakdown hero4Skill0Breakdown = MonoSingleton<BulletPool>.Instance.GetHero4Skill0Breakdown();
			hero4Skill0Breakdown.transform.position = targetPosition;
			hero4Skill0Breakdown.Init(physicsDamage, skillRange, buffKey, duration);
			MonoSingleton<LensHandler>.Instance.ShakeNormal();
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
		}
	}
}
