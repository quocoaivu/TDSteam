using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero0Ability1 : HeroAbilityShared
	{
        private int heroID;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private int damage;

        private float aoeRange;

        private float duration;

        private int change_percent;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_0_1 heroSkillParameter_0_ = new HeroAbilitySpec_0_1();
			heroSkillParameter_0_ = (HeroAbilitySpec_0_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			damage = heroSkillParameter_0_.getParam(currentSkillLevel - 1).damage;
			duration = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).duration / 1000f;
			change_percent = heroSkillParameter_0_.getParam(currentSkillLevel - 1).change_percent;
			aoeRange = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).aoeRange / GameRecord.PIXEL_PER_UNIT;
			heroModel.OnAttackEvent += HeroModel_OnBeHitEvent;
			heroModel.OnSpecialStateEvent += HeroModel_OnSpecialStateEvent;
		}

		private void HeroModel_OnSpecialStateEvent()
		{
			DamageWithAOE();
		}

		private void HeroModel_OnBeHitEvent()
		{
			if (UnityEngine.Random.Range(0, 100) < change_percent && unLock)
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
			heroModel.SetSpecialStateDuration(duration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_0);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_0
			});
			yield break;
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageKind.Range, new SharedStrikeDamage(damage, 0, aoeRange));
		}
	}
}
