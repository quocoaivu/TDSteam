using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero1Ability0 : HeroAbilityShared
	{
        private int heroID = 1;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private HeroSpec heroParameter;

        private int numberOfProjectile;

        private float range;

        private int offsetHigh;

        private float duration;

        private float delayTime;

        private int damage;

        private float cooldownTime;

        private string description;

        private string useType;

        private RaycastHit2D hit;

        [SerializeField]
        private LayerMask avaiableCastSkillLayerMask;

        [SerializeField]
        private Hero1Ability0Projectile projectilePrefab;

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
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			heroParameter = HeroParameterManager.Instance.GetHeroParameter(heroID, currentLevel);
			HeroAbilitySpec_1_0 heroSkillParameter_1_ = new HeroAbilitySpec_1_0();
			heroSkillParameter_1_ = (HeroAbilitySpec_1_0)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberOfProjectile = heroSkillParameter_1_.getParam(currentSkillLevel - 1).number_of_projectile;
			range = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).range / GameRecord.PIXEL_PER_UNIT;
			offsetHigh = heroSkillParameter_1_.getParam(currentSkillLevel - 1).offsetHigh;
			duration = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).duration / 1000f;
			delayTime = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).delayTime / 1000f;
			damage = heroSkillParameter_1_.getParam(currentSkillLevel - 1).damage;
			cooldownTime = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_1_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_1_.getParam(currentSkillLevel - 1).use_type;
			InitFxs();
		}

		public override float GetCooldownTime()
		{
			return cooldownTime;
		}

		public override string GetUseType()
		{
			return useType;
		}

		public void Init(HeroSpec _heroParameter, int _numberOfArrow, int _range, int _offsetHigh, int _duration, int _delayTime, int _damage, int _cooldownTime, string _descsription)
		{
			heroParameter = _heroParameter;
			numberOfProjectile = _numberOfArrow;
			range = (float)_range / GameRecord.PIXEL_PER_UNIT;
			offsetHigh = _offsetHigh;
			duration = (float)_duration / 1000f;
			delayTime = (float)_delayTime / 1000f;
			damage = _damage;
			cooldownTime = (float)_cooldownTime;
			description = _descsription;
		}

		private void InitFxs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(projectilePrefab.gameObject);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.LIGHTNING_PROJECTILE_SHADOW);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.LIGHTNING_PROJECTILE_RANGE);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.LIGHTNING_EXPLOSION);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_FADE_SCREEN);
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			CastEffectSkillRange(targetPosition);
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[1].DoCooldown();
			VisualEffectInstance fxCanvas = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_FADE_SCREEN);
			fxCanvas.transform.position = Vector3.zero;
			float fadeTime = duration * ((float)offsetHigh - targetPosition.y) + (float)numberOfProjectile * delayTime;
			fxCanvas.Init(1.5f * fadeTime);
			fxCanvas.DoFadeIn(fadeTime / 2f, 0.392156869f);
			for (int i = 0; i < numberOfProjectile; i++)
			{
				yield return new WaitForSeconds(delayTime);
				Hero1Ability0Projectile bullet = MonoSingleton<BulletPool>.Instance.GetLightningBullet();
				bullet.transform.position = new Vector2(targetPosition.x, (float)offsetHigh) + UnityEngine.Random.insideUnitCircle * range;
				bullet.Init(damage, range, duration * ((float)offsetHigh - targetPosition.y), (float)offsetHigh - targetPosition.y);
			}
			yield return new WaitForSeconds(fadeTime / 4f);
			fxCanvas.DoFadeOut(fadeTime / 4f, 0.392156869f);
			yield break;
		}

		private void CastEffectSkillRange(Vector2 targetPosition)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.LIGHTNING_PROJECTILE_RANGE);
			effect.transform.position = targetPosition;
			effect.Init(0.75f);
		}
	}
}
