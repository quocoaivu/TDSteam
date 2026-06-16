using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero6Ability3 : HeroAbilityShared
	{
        private int heroID = 6;

        private int skillID = 3;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private int physicsDamage;

        private int magicDamage;

        private float skillRange;

        private float timeTracking;

        private float cooldownTime;

        private string description;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        private OnHitStatusApplier effectAttackSender;

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTimeCastSkill;

        [SerializeField]
        private float effectLifeTime;

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

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_6_3 heroSkillParameter_6_ = new HeroAbilitySpec_6_3();
			heroSkillParameter_6_ = (HeroAbilitySpec_6_3)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_6_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime;
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.THOR_AIR_THUNDER);
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
			List<EnemyData> listFlyingEnemies = GameKit.GetListFlyingEnemiesInRange(base.gameObject, new SharedStrikeDamage(physicsDamage, magicDamage, true, skillRange));
			timeTracking = cooldownTime;
			if (listFlyingEnemies.Count > 0)
			{
				heroModel.SetSpecialStateDuration(animDuration);
				heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_0);
				heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
				{
					HeroMotionHandler.animPassiveSkill_0
				});
				yield return new WaitForSeconds(delayTimeCastSkill);
				foreach (EnemyData enemyModel in listFlyingEnemies)
				{
					enemyModel.ProcessDamage(DamageKind.Range, new SharedStrikeDamage(physicsDamage, magicDamage, true, skillRange));
					VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.THOR_AIR_THUNDER);
					effect.transform.position = enemyModel.transform.position;
					effect.Init(effectLifeTime, enemyModel.transform);
				}
			}
			yield break;
		}
	}
}
