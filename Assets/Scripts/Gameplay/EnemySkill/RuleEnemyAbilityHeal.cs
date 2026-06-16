using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class RuleEnemyAbilityHeal : EnemyBrain
	{
		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnAppear()
		{
			base.OnAppear();
			coolDownTime = coolDownTimeMillisecond / 1000f;
			if (isActiveSkillOnAppear)
			{
				coolDownTimeTracking = 0f;
			}
			else
			{
				coolDownTimeTracking = coolDownTime;
			}
			skillReady = true;
			isCastingSkill = false;
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_HEAL_0);
		}

		private void Start()
		{
			waitForAnimation = new WaitForSeconds(attackAnimationDuration);
		}

		public override void Update()
		{
			base.Update();
			if (!skillReady)
			{
				return;
			}
			if (isCastingSkill)
			{
				return;
			}
			if (!base.IsEnemyAlive())
			{
				return;
			}
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			if (IsCooldownSkillDone())
			{
				GetInRangeEnemies();
				if (inRangeEnemies.Count > 0 && ShouldCastSkill())
				{
					base.StartCoroutine(CastSkillHeal());
				}
			}
			coolDownTimeTracking = Mathf.MoveTowards(coolDownTimeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange / GameRecord.PIXEL_PER_UNIT);
		}

		private bool IsCooldownSkillDone()
		{
			return coolDownTimeTracking == 0f;
		}

		private void GetInRangeEnemies()
		{
			inRangeEnemies.Clear();
			MonoSingleton<GameRecord>.Instance.GetInRangeEnemies(base.EnemyModel.transform.position, skillRange / GameRecord.PIXEL_PER_UNIT, inRangeEnemies);
		}

		private bool ShouldCastSkill()
		{
			bool result = false;
			foreach (EnemyData enemyModel in inRangeEnemies)
			{
				if ((float)enemyModel.EnemyHealthController.CurrentHealth <= criticalHealthPercentage / 100f * (float)enemyModel.EnemyHealthController.OriginHealth)
				{
					result = true;
				}
			}
			return result;
		}

		private IEnumerator CastSkillHeal()
		{
			isCastingSkill = true;
			if (!base.IsCurrentSpeedGreaterThanMinSpeed())
			{
				yield return null;
			}
			if (!base.IsEnemyAlive())
			{
				yield return null;
			}
			base.EnemyModel.SetSpecialStateDuration(attackAnimationDuration);
			base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimation.animSpecialAttack);
			base.EnemyModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				EnemyAnimation.animSpecialAttack
			});
			base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
			GetInRangeEnemies();
			foreach (EnemyData enemyModel in inRangeEnemies)
			{
				if ((float)enemyModel.EnemyHealthController.CurrentHealth <= criticalHealthPercentage / 100f * (float)enemyModel.EnemyHealthController.OriginHealth)
				{
					enemyModel.EnemyHealthController.AddHealth(healthRestoreAmount);
					VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_HEAL_0);
					effect.Init(0.5f, enemyModel.transform);
				}
			}
			UnityEngine.Debug.Log("heal " + healthRestoreAmount);
			yield return waitForAnimation;
			coolDownTimeTracking = coolDownTime;
			isCastingSkill = false;
			yield break;
		}

		public void OnDisable()
		{
			isCastingSkill = false;
		}

		[Space]
		[Header("Parameters")]
		[SerializeField]
		private int healthRestoreAmount;

		[SerializeField]
		private float skillRange;

		[SerializeField]
		private float criticalHealthPercentage;

		[SerializeField]
		private float coolDownTimeMillisecond;

		[SerializeField]
		private float minSpeed = 0.05f;

		[SerializeField]
		private float attackAnimationDuration = 1f;

		private List<EnemyData> inRangeEnemies = new List<EnemyData>();

		private float coolDownTime;

		private float coolDownTimeTracking;

		private bool skillReady;

		private bool isCastingSkill;

		[SerializeField]
		private bool isActiveSkillOnAppear;

		private WaitForSeconds waitForAnimation;
	}
}
