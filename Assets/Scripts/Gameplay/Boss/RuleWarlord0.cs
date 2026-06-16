using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Gameplay
{
	public class RuleWarlord0 : EnemyBrain
	{
		public override void OnAppear()
		{
			base.OnAppear();
			timeTracking = ((!activateAtStart) ? (coolDownTime / 1000f) : 0f);
		}

		private void Start()
		{
			enemyMovementController = base.EnemyModel.EnemyMovementController;
			enemyAnimationController = base.EnemyModel.EnemyAnimationController;
			waitForAnimation = new WaitForSeconds(attackAnimationDuration);
			MonoSingleton<EnemyPool>.Instance.InitAdditionEnemy(minionId);
		}

		public override void Update()
		{
			base.Update();
			if (attacking)
			{
				return;
			}
			if (ShouldAttack())
			{
				base.StartCoroutine(CreateMinions());
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		private bool ShouldAttack()
		{
			return timeTracking == 0f && enemyMovementController.Speed >= minSpeed;
		}

		private IEnumerator CreateMinions()
		{
			attacking = true;
			onStartAttack.Dispatch();
			if (!base.IsCurrentSpeedGreaterThanMinSpeed())
			{
				yield return null;
			}
			if (!base.IsEnemyAlive())
			{
				yield return null;
			}
			if (GameKit.IsValidEnemy(base.EnemyModel))
			{
				base.EnemyModel.SetSpecialStateDuration(attackAnimationDuration);
				base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimation.animSpecialAttack);
				base.EnemyModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
				{
					EnemyAnimation.animSpecialAttack
				});
				base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
				yield return waitForAnimation;
				int randomGate = minionGate[generateRandomNumber(0, minionGate.Count)];
				UnityEngine.Debug.Log("Spawn " + minionAmount + " minions");
				for (int i = 0; i < minionAmount; i++)
				{
					MonoSingleton<EnemyPool>.Instance.SpawnAdditionEnemyAtGate(minionId, 0f, randomGate, randomPosition);
					yield return new WaitForSeconds(UnityEngine.Random.Range(randomTimeDelayMin, randomTimeDelayMax));
				}
			}
			timeTracking = coolDownTime / 1000f;
			attacking = false;
			onFinishAttack.Dispatch();
			yield break;
		}

		public void OnDisable()
		{
			attacking = false;
		}

		public int generateRandomNumber(int min, int max)
		{
			int num = UnityEngine.Random.Range(min, max);
			if (num == lastGate)
			{
				return generateRandomNumber(min, max);
			}
			lastGate = num;
			return num;
		}

		[Header("Events")]
		[SerializeField]
		private OrderedUnityEvent onStartAttack = new OrderedUnityEvent();

		[SerializeField]
		private OrderedUnityEvent onFinishAttack = new OrderedUnityEvent();

		[SerializeField]
		private OrderedUnityEvent onCancelAttack = new OrderedUnityEvent();

		[Space]
		[Header("Skill paramerters")]
		[SerializeField]
		private float coolDownTime;

		[SerializeField]
		private int minionAmount;

		[SerializeField]
		private int minionId;

		[SerializeField]
		private List<int> minionGate;

		[Space]
		[Header("Common setting")]
		[SerializeField]
		private bool activateAtStart;

		[SerializeField]
		private float attackAnimationDuration = 1f;

		[SerializeField]
		private float minSpeed = 0.05f;

		[SerializeField]
		private float randomTimeDelayMin;

		[SerializeField]
		private float randomTimeDelayMax = 2f;

		[SerializeField]
		private float attackAnimationDurationRatio;

		[SerializeField]
		private float randomPosition;

		private float timeTracking;

		private bool attacking;

		private int lastGate;

		private EnemyMovement enemyMovementController;

		private EnemyAnimation enemyAnimationController;

		private WaitForSeconds waitForAnimation;
	}
}
