using System;
using System.Collections;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero8Ability0 : HeroAbilityShared
	{
        private int heroID = 8;

        private int skillID;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int numberOfProjectile;

        private int physicsDamage;

        private int magicDamage;

        private float skillRange;

        private float cooldownTime;

        private string description;

        private string useType;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeToAttack;

        [SerializeField]
        private float delayTimeBetweenMissile;

        [Space]
        [SerializeField]
        private ProjectileEntity missilePrefab;

        [SerializeField]
        private string missileName;

        [SerializeField]
        private Transform[] listGunBarrel;
        
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
			HeroAbilitySpec_8_0 heroSkillParameter_8_ = new HeroAbilitySpec_8_0();
			heroSkillParameter_8_ = (HeroAbilitySpec_8_0)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberOfProjectile = heroSkillParameter_8_.getParam(currentSkillLevel - 1).number_of_projectile;
			physicsDamage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_8_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_8_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(missilePrefab.gameObject);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.GROUND_AIMING_1);
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
			MonoSingleton<GameplayUIHeroDirector>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			yield return new WaitForSeconds(delayTimeToAttack);
			VisualEffectInstance fx = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.GROUND_AIMING_1);
			fx.Init(3.25f, targetPosition);
			for (int i = 0; i < numberOfProjectile; i++)
			{
				yield return new WaitForSeconds(delayTimeBetweenMissile);
				ProjectileEntity bullet = MonoSingleton<BulletPool>.Instance.GetBulletByName(missileName);
				bullet.transform.position = listGunBarrel[UnityEngine.Random.Range(0, listGunBarrel.Length)].position;
				bullet.InitFromHero(heroModel, new SharedStrikeDamage(physicsDamage, magicDamage, skillRange), targetPosition);
			}
			yield break;
		}
	}
}
