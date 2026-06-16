using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Ability0 : HeroAbilityShared
	{
        private int heroID = 5;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int healAmount;

        private float skillRange;

        private float cooldownTime;

        private string description;

        private string useType;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private Hero5Ability0MendBomb healingBomb;

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
			HeroAbilitySpec_5_0 heroSkillParameter_5_ = new HeroAbilitySpec_5_0();
			heroSkillParameter_5_ = (HeroAbilitySpec_5_0)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			healAmount = heroSkillParameter_5_.getParam(currentSkillLevel - 1).heal_amount;
			skillRange = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_5_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_5_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(healingBomb.gameObject);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_HEAL_0);
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
			CastHealingStatue(targetPosition);
			yield break;
		}

		private void CastHealingStatue(Vector2 targetPosition)
		{
			Hero5Ability0MendBomb hero5Skill0HealingBomb = MonoSingleton<BulletPool>.Instance.GetHero5Skill0HealingBomb();
			hero5Skill0HealingBomb.transform.position = targetPosition;
			hero5Skill0HealingBomb.Init(healAmount, skillRange);
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
		}
	}
}
