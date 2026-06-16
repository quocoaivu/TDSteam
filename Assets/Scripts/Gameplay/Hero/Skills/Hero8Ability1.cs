using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero8Ability1 : HeroAbilityShared
	{
        private int heroID = 8;

        private int skillID = 1;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int physicsDamage;

        private int magicDamage;

        private int attackDecreasePercentage;

        private float duration;

        private float knockBackDistance;

        private float skillRange;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        [SerializeField]
        private float knockBackDuration;

        private string buffKey = "DecreaseAttackByPercentage";

        private OnHitStatusApplier effectAttack;

        private EnemyData currentEnemy;

        private List<EnemyData> listNearbyEnemies = new List<EnemyData>();

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
			HeroAbilitySpec_8_1 heroSkillParameter_8_ = new HeroAbilitySpec_8_1();
			heroSkillParameter_8_ = (HeroAbilitySpec_8_1)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).magic_damage;
			attackDecreasePercentage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).attack_damage_decrease_percentage;
			duration = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).duration / 1000f;
			knockBackDistance = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).knock_back_distance / GameRecord.PIXEL_PER_UNIT;
			skillRange = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_8_.getParam(currentSkillLevel - 1).description;
			effectAttack = default(OnHitStatusApplier);
			effectAttack.buffKey = buffKey;
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = attackDecreasePercentage;
			effectAttack.debuffEffectDuration = duration;
			effectAttack.damageFXType = DamageVfxType.None;
			timeTracking = cooldownTime * 0.5f;
			InitFXs();
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_SELL_TOWER_ON_ALLY);
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
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_0);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_0
			});
			timeTracking = cooldownTime;
			currentEnemy = heroModel.GetCurrentTarget();
			if (currentEnemy != null)
			{
				listNearbyEnemies = GameKit.GetListEnemiesInRange(currentEnemy.gameObject, new SharedStrikeDamage(physicsDamage, magicDamage, skillRange));
			}
			yield return new WaitForSeconds(delayTimeCastSkill);
			float speed = knockBackDistance / knockBackDuration;
			if (listNearbyEnemies.Count > 0)
			{
				foreach (EnemyData enemyModel in listNearbyEnemies)
				{
					if (GameKit.IsValidEnemy(enemyModel) && !enemyModel.OriginalParameter.isBoss && !enemyModel.IsAir && !enemyModel.IsUnderground && !enemyModel.IsInTunnel)
					{
						enemyModel.ProcessDamage(DamageKind.Range, new SharedStrikeDamage(physicsDamage, magicDamage, 0f), effectAttack);
						if (GameKit.IsValidEnemy(enemyModel))
						{
							enemyModel.SetSpecialStateDuration(knockBackDuration);
							enemyModel.enemyFsmController.GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[0]);
						}
					}
				}
				float countdown = knockBackDuration;
				while (countdown > 0f)
				{
					countdown -= Time.deltaTime;
					for (int i = listNearbyEnemies.Count - 1; i >= 0; i--)
					{
						PushbackSequence(listNearbyEnemies[i], speed);
					}
					yield return null;
				}
			}
			yield break;
		}

		private void PushbackSequence(EnemyData target, float knockbackSpeed)
		{
			if (GameKit.IsValidEnemy(target) && !target.OriginalParameter.isBoss && !target.IsAir && !target.IsUnderground && !target.IsInTunnel)
			{
				LineDirector.Current.RequestMove(target, target.monsterPathData, -knockbackSpeed * Time.deltaTime, false, 0f);
			}
		}
	}
}
