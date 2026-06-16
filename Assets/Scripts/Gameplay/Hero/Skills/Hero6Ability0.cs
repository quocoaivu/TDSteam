using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero6Ability0 : HeroAbilityShared
	{
        private int heroID = 6;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int physicsDamage;

        private int magicDamage;

        private float stunDuration;

        private float skillRange;

        private float cooldownTime;

        private string description;

        private string useType;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        [SerializeField]
        private float effectLifeTime;

        private string buffKey = "Stun";

        [SerializeField]
        private HeroAbilityAOEShared skillObject;

        private OnHitStatusApplier effectAttackSender;
        
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
			HeroAbilitySpec_6_0 heroSkillParameter_6_ = new HeroAbilitySpec_6_0();
			heroSkillParameter_6_ = (HeroAbilitySpec_6_0)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).magic_damage;
			stunDuration = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).stun_duration / 1000f;
			skillRange = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_6_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_6_.getParam(currentSkillLevel - 1).use_type;
			effectAttackSender = default(OnHitStatusApplier);
			effectAttackSender.buffKey = buffKey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = 100;
			effectAttackSender.debuffEffectDuration = stunDuration;
			effectAttackSender.damageFXType = DamageVfxType.Electric;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(skillObject.gameObject);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_ELECTRIC);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_FADE_SCREEN);
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
			VisualEffectInstance fxCanvas = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_FADE_SCREEN);
			fxCanvas.transform.position = Vector3.zero;
			float fadeTime = animDuration;
			fxCanvas.Init(fadeTime);
			fxCanvas.DoFadeIn(fadeTime / 2f, 0.392156869f);
			yield return new WaitForSeconds(delayTimeCastSkill);
			fxCanvas.DoFadeOut(fadeTime / 2f, 0.392156869f);
			HeroAbilityAOEShared bullet = MonoSingleton<BulletPool>.Instance.GetHeroSkillAOECommon("Hero6Skill0Thunder");
			bullet.transform.position = targetPosition;
			bullet.Init_DamageImmediately(new SharedStrikeDamage(physicsDamage, magicDamage, skillRange), effectAttackSender, effectLifeTime);
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
			yield break;
		}
	}
}
