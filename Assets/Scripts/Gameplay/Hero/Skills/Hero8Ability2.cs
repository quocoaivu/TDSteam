using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero8Ability2 : HeroAbilityShared
	{
        private int heroID = 8;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;

        private string buffKey = "Stun";

        private int enemyAffected;

        private float skillRange;

        private float duration;
        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        [SerializeField]
        private RangeDamageCaster damageToEnemiesInRange;

        private OnHitStatusApplier effectAttack;

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
			if (IsCooldownDone())
			{
				base.StartCoroutine(CastSkill());
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			if (unLock)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(heroModel.transform.position, skillRange);
			}
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_8_2 heroSkillParameter_8_ = new HeroAbilitySpec_8_2();
			heroSkillParameter_8_ = (HeroAbilitySpec_8_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			enemyAffected = 1;
			skillRange = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			duration = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_8_.getParam(currentSkillLevel - 1).description;
			effectAttack = default(OnHitStatusApplier);
			effectAttack.buffKey = buffKey;
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = 100;
			effectAttack.debuffEffectDuration = duration;
			effectAttack.damageFXType = DamageVfxType.Stun;
			timeTracking = cooldownTime * 0.6f;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_STUN);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			timeTracking = cooldownTime;
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_1);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_1
			});
			yield return new WaitForSeconds(delayTimeCastSkill);
			List<EnemyData> enemiesInAoeRange = GameKit.GetListEnemiesInRange(base.gameObject, new SharedStrikeDamage(0, 0, skillRange));
			if (enemiesInAoeRange.Count > 0)
			{
				damageToEnemiesInRange.CastDamage(enemyAffected, DamageKind.Range, new SharedStrikeDamage(0, 0, skillRange), effectAttack);
			}
			yield break;
		}
	}
}
