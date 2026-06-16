using System;
using System.Collections.Generic;
using Parameter;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class TurretStrikeHandler : TurretHandler
	{
		public override void Update()
		{
			base.Update();
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			if (ShouldAttack() && IsReloadDone())
			{
				Attack();
				reloadTimeTracking = currentReloadTime;
			}
			reloadTimeTracking = Mathf.MoveTowards(reloadTimeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, (float)base.TowerModel.OriginalParameter.attackRangeMax / GameRecord.PIXEL_PER_UNIT);
		}

		private bool ShouldAttack()
		{
			return towerFindEnemyController.Targets.Count != 0;
		}

		private bool IsReloadDone()
		{
			return reloadTimeTracking == 0f;
		}

		public override void Initialize()
		{
			base.Initialize();
			originalParameter = base.TowerModel.OriginalParameter;
			towerFindEnemyController = base.TowerModel.towerFindEnemyController;
			towerFindEnemyController.OnTargetRemoved += TowerFindEnemyController_OnTargetRemoved;
		}

		private void TowerFindEnemyController_OnTargetRemoved(EnemyData target)
		{
			RemoveTarget(target);
		}

		public override void OnAppear()
		{
			base.OnAppear();
			originReloadTime = (float)base.TowerModel.OriginalParameter.reload / 1000f;
			currentReloadTime = originReloadTime;
			reloadTimeTracking = originReloadTime;
			base.TowerModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseAttackSpeedBuffKeys.Contains(buffKey))
			{
				ApplyIncreaseAttackSpeed();
			}
		}

		private void ApplyIncreaseAttackSpeed()
		{
			float buffsValue = base.TowerModel.BuffsHolder.GetBuffsValue(increaseAttackSpeedBuffKeys);
			if (buffsValue > 0f)
			{
				currentReloadTime = originReloadTime / (buffsValue / 100f);
			}
			else
			{
				currentReloadTime = originReloadTime;
			}
			currentReloadTime = Mathf.Clamp(currentReloadTime, 0.1f, 999f);
		}

		public void Attack()
		{
			StartAttack.Invoke();
			int num = 0;
			if (singleTargetBehavior)
			{
				num = Mathf.Min(towerFindEnemyController.Targets.Count, singleTargetAttackControllers.Count);
				for (int i = 0; i < num; i++)
				{
					foreach (TurretStrikeSingleMarkHandler towerAttackSingleTargetController in singleTargetAttackControllers)
					{
						towerAttackSingleTargetController.StartAttack(towerFindEnemyController.Targets[i]);
					}
				}
			}
			if (multiTargetBehavior)
			{
				if (towerFindEnemyController.Targets.Count == 1)
				{
					num = singleTargetAttackControllers.Count;
					for (int j = 0; j < num; j++)
					{
						singleTargetAttackControllers[j].StartAttack(towerFindEnemyController.Targets[0]);
					}
				}
				else
				{
					num = Mathf.Min(towerFindEnemyController.Targets.Count, singleTargetAttackControllers.Count);
					for (int k = 0; k < num; k++)
					{
						singleTargetAttackControllers[k].StartAttack(towerFindEnemyController.Targets[k]);
					}
				}
			}
		}

		public void StopAttack()
		{
			foreach (TurretStrikeSingleMarkHandler towerAttackSingleTargetController in singleTargetAttackControllers)
			{
				towerAttackSingleTargetController.StopAttack();
			}
		}

		public void RemoveTarget(EnemyData enemyController)
		{
			foreach (TurretStrikeSingleMarkHandler towerAttackSingleTargetController in singleTargetAttackControllers)
			{
				if (towerAttackSingleTargetController.Target == enemyController)
				{
					towerAttackSingleTargetController.StopAttack();
				}
			}
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			StopAttack();
		}

		[SerializeField]
		private float maxAngleCanAttack = 360f;

		[SerializeField]
		private Transform gun;

		[SerializeField]
		private List<TurretStrikeSingleMarkHandler> singleTargetAttackControllers = new List<TurretStrikeSingleMarkHandler>();

		private TurretSpec originalParameter;

		private GameObject gunBarrelRight;

		private GameObject gunBarrelLeft;

		private float originReloadTime;

		private float currentReloadTime;

		private float reloadTimeTracking;

		public UnityEvent StartAttack;

		private TurretSeekEnemyHandler towerFindEnemyController;

		[SerializeField]
		private bool singleTargetBehavior;

		[SerializeField]
		private bool multiTargetBehavior;

		private List<string> increaseAttackSpeedBuffKeys = new List<string>
		{
			"IncreaseAttackSpeedByPercentage"
		};
	}
}
