using System;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero1Ability3 : HeroAbilityShared
	{
        private int heroID = 1;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private int changeToCast;

        private int numberOfProjectile;

        private int damage;

        private string buffkey = "Slow";

        private int slowPecent;

        private float slowTime;

        [SerializeField]
        private Transform gunPos;

        private OnHitStatusApplier effectAttackSender;

        private List<EnemyData> inRangeEnemies = new List<EnemyData>();

        public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_1_3 heroSkillParameter_1_ = new HeroAbilitySpec_1_3();
			heroSkillParameter_1_ = (HeroAbilitySpec_1_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			changeToCast = heroSkillParameter_1_.getParam(currentSkillLevel - 1).chance_to_cast;
			numberOfProjectile = heroSkillParameter_1_.getParam(currentSkillLevel - 1).number_of_projectile;
			damage = heroSkillParameter_1_.getParam(currentSkillLevel - 1).damage;
			slowPecent = heroSkillParameter_1_.getParam(currentSkillLevel - 1).slow_percent;
			slowTime = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).slow_time / 1000f;
			effectAttackSender.buffKey = buffkey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = slowPecent;
			effectAttackSender.debuffEffectDuration = slowTime;
			effectAttackSender.damageFXType = DamageVfxType.Slow;
			MonoSingleton<BulletPool>.Instance.InitBulletsFromHeroes(heroModel.OriginalParameter.id, 1);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_SLOW);
			heroModel.OnAttackEvent += HeroModel_OnAttackEvent;
		}

		private void HeroModel_OnAttackEvent()
		{
			if (unLock)
			{
				TryCastMultiProjectile();
			}
		}

		private void TryCastMultiProjectile()
		{
			if (UnityEngine.Random.Range(0, 100) < changeToCast)
			{
				GetEnemies();
				int num = Mathf.Min(numberOfProjectile, inRangeEnemies.Count);
				UnityEngine.Debug.Log("Cast multi arrow on " + num + " enemy ");
				for (int i = 0; i < num; i++)
				{
					ProjectileEntity forHero = MonoSingleton<BulletPool>.Instance.GetForHero(heroModel.OriginalParameter.id, 1);
					forHero.transform.eulerAngles = Vector3.zero;
					int num2 = damage;
					forHero.transform.position = gunPos.position;
					forHero.gameObject.SetActive(true);
					forHero.InitFromHero(heroModel, new SharedStrikeDamage(damage, 0, 0f), inRangeEnemies[i], effectAttackSender);
				}
			}
		}

		private void GetEnemies()
		{
			MonoSingleton<GameRecord>.Instance.GetInRangeEnemies(heroModel.transform.position, (float)heroModel.OriginalParameter.attack_range_max / GameRecord.PIXEL_PER_UNIT, inRangeEnemies);
		}
	}
}
