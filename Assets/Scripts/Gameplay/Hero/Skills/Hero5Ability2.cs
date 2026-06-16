using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Ability2 : HeroAbilityShared
	{
        private int heroID = 5;

        private int skillID = 2;

        private int currentLevel;

        private int currentSkillLevel;

        private bool unLock;
        private float skillRange;

        private int enemyAffected;

        private int enemyMin;

        private float cooldownTime;

        private float duration;

        private float timeTracking;

        private string description;

        private bool isCastSkill;

        private List<EnemyData> enemiesInRange = new List<EnemyData>();

        private string buffKey = "Slow";

        [SerializeField]
        private float animDuration;

        [SerializeField]
        private float delayTime;

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
			if (heroModel.HeroMovementController.isMoving)
			{
				return;
			}
			if (IsCooldownDone())
			{
				TryToCastSkill();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameterManager.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroAbilitySpec_5_2 heroSkillParameter_5_ = new HeroAbilitySpec_5_2();
			heroSkillParameter_5_ = (HeroAbilitySpec_5_2)HeroAbilitySpec.Instance.GetHeroSkillsParameter(heroID, skillID);
			skillRange = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).skill_range / GameRecord.PIXEL_PER_UNIT;
			enemyAffected = heroSkillParameter_5_.getParam(currentSkillLevel - 1).enemy_affected;
			enemyMin = heroSkillParameter_5_.getParam(currentSkillLevel - 1).enemy_min;
			cooldownTime = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			duration = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).duration / 1000f;
			description = heroSkillParameter_5_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime * 0.4f;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_ROOT);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void TryToCastSkill()
		{
			GetEnemiesInAoeRange();
			if (enemiesInRange.Count >= enemyMin && !isCastSkill)
			{
				timeTracking = cooldownTime;
				base.StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animPassiveSkill_1);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animPassiveSkill_1
			});
			yield return new WaitForSeconds(delayTime);
			if (!isCastSkill)
			{
				CastRoot();
			}
			yield break;
		}

		private void CastRoot()
		{
			isCastSkill = true;
			int num = (enemiesInRange.Count <= enemyAffected) ? enemiesInRange.Count : enemyAffected;
			for (int i = 0; i < num; i++)
			{
				UnityEngine.Debug.Log("troi quai ! " + num);
				if (enemiesInRange[i] && enemiesInRange[i].Id != 25)
				{
					enemiesInRange[i].ProcessEffect(buffKey, 100, duration, DamageVfxType.Root);
				}
			}
			timeTracking = cooldownTime;
			isCastSkill = false;
		}

		private void GetEnemiesInAoeRange()
		{
			enemiesInRange.Clear();
			List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
			for (int i = 0; i < listActiveEnemy.Count; i++)
			{
				EnemyData enemyModel = listActiveEnemy[i];
				if (!enemyModel.IsAir)
				{
					if (!enemyModel.IsUnderground)
					{
						if (!enemyModel.IsInTunnel)
						{
							float num = MonoSingleton<GameRecord>.Instance.SqrDistance(base.gameObject, enemyModel.gameObject);
							if (num <= skillRange * skillRange)
							{
								enemiesInRange.Add(enemyModel);
							}
						}
					}
				}
			}
		}
	}
}
