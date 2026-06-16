using System;
using System.Collections;
using Data;
using DG.Tweening;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero2Ability1 : HeroAbilityShared
	{
        private int heroID = 2;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private float armorBonus;

        private float duration;

        private float cooldownTime;

        private string description;

        private float cooldownTimeTracking;

        private Tweener tween;

        private string buffKey = "IncreaseArmorPhysics";

        [SerializeField]
        private float animationTime;
        
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
			HeroAbilitySpec_2_1 heroSkillParameter_2_ = new HeroAbilitySpec_2_1();
			heroSkillParameter_2_ = (HeroAbilitySpec_2_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			armorBonus = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).armorBonus / 100f;
			duration = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_2_.getParam(currentSkillLevel - 1).description;
			cooldownTimeTracking = cooldownTime;
			heroModel.OnBeHitEvent += HeroModel_OnBeHitEvent;
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_HEAL_1);
		}

		private void HeroModel_OnBeHitEvent()
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
			AddPassiveArmor();
			CastArmorFX();
			heroModel.SetSpecialStateDuration(animationTime);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_0);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_0
			});
			yield break;
		}

		private void CastArmorFX()
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_HEAL_1);
			effect.transform.position = base.transform.position;
			effect.Init(duration, base.transform, heroModel.GetComponent<SpriteRenderer>().sprite.rect.width);
		}

		private void AddPassiveArmor()
		{
			heroModel.BuffsHolder.AddBuff(buffKey, new BuffStatus(true, armorBonus, duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
			cooldownTimeTracking = cooldownTime;
		}

		private bool IsCooldownDone()
		{
			return cooldownTimeTracking == 0f;
		}

	}
}
