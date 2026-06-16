using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero6Ability2 : HeroAbilityShared
	{
        private int heroID = 6;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int physicsDamage;

        private int magicDamage;

        private float aoeRange;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        [SerializeField]
        private float effectLifeTime;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        [SerializeField]
        private VisualEffectSpawner effectCaster;

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
			HeroAbilitySpec_6_2 heroSkillParameter_6_ = new HeroAbilitySpec_6_2();
			heroSkillParameter_6_ = (HeroAbilitySpec_6_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).magic_damage;
			aoeRange = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).aoe_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_6_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.THOR_LANDING_THUNDER);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && unLock)
			{
				base.StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			damageToAOERange.CastDamage(DamageKind.Range, new SharedStrikeDamage(physicsDamage, magicDamage, aoeRange));
			effectCaster.CastEffect(FXPool.THOR_LANDING_THUNDER, effectLifeTime, base.transform.position);
			yield break;
		}


	}
}
