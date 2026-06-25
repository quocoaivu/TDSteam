using System;
using System.Collections.Generic;
using System.Diagnostics;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class TurretSeekEnemyHandler : TurretHandler
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<EnemyData> OnTargetRemoved;

		public List<EnemyData> Targets
		{
			get
			{
				return targets;
			}
			private set
			{
				targets = value;
			}
		}

		public int DesiredTargetsNumber
		{
			get
			{
				return desiredTargetsNumber;
			}
			private set
			{
				desiredTargetsNumber = value;
			}
		}

		public float AttackRangeMax
		{
			get
			{
				return attackRangeMax;
			}
			private set
			{
				attackRangeMax = value;
			}
		}

		public float BuffedAttackRangeMax
		{
			get
			{
				return buffedAttackRangeMax;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			SetParameter();
			base.TowerModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		public override void OnAppear()
		{
			base.OnAppear();
			base.TowerModel.IsSilent = false;
		}

		private void SetParameter()
		{
			originalParameter = base.TowerModel.OriginalParameter;
			AttackRangeMax = base.TowerModel.OriginalParameter.range;
			buffedAttackRangeMax = AttackRangeMax;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			base.TowerModel.IsSilent = false;
			targets.Clear();
		}

		public override void OnBuildFinished()
		{
			base.OnBuildFinished();
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			base.TowerModel.IsSilent = (base.TowerModel.BuffsHolder.GetBuffsValue(silentBuffKeys) > 0f);
			if (base.TowerModel.IsSilent)
			{
				ApplyBuffSilent();
			}
			if (increaseAttackRangeBuffKeys.Contains(buffKey))
			{
				ApplyBuffIncreaseAttackRange();
			}
		}

		private void ApplyBuffSilent()
		{
			foreach (EnemyData obj in Targets)
			{
				if (OnTargetRemoved != null)
				{
					OnTargetRemoved(obj);
				}
			}
			Targets.Clear();
		}

		private void ApplyBuffIncreaseAttackRange()
		{
			float num = 1f + base.TowerModel.BuffsHolder.GetBuffsValue(increaseAttackRangeBuffKeys) / 100f;
			if (num != 0f)
			{
				buffedAttackRangeMax = attackRangeMax * num;
			}
			else
			{
				buffedAttackRangeMax = attackRangeMax;
			}
		}

		public override void Update()
		{
			base.Update();
			if (!base.TowerModel.IsSilent && Targets.Count < DesiredTargetsNumber)
			{
				FindNewTarget();
			}
			UpdateRemoveTarget();
		}

		private void FindNewTarget()
		{
			FindAllNewTargetsInRange();
			if (allNewTargets.Count == 0)
			{
				return;
			}
			int num = 0;
			while (Targets.Count < DesiredTargetsNumber && num < allNewTargets.Count)
			{
				Targets.Add(allNewTargets[num]);
				num++;
			}
		}

		private void UpdateRemoveTarget()
		{
			if (Targets.Count == 0)
			{
				return;
			}
			for (int i = Targets.Count - 1; i >= 0; i--)
			{
				EnemyData enemyModel = Targets[i];
				if (MonoSingleton<GameRecord>.Instance.SqrDistance(base.TowerModel.gameObject, enemyModel.gameObject) > buffedAttackRangeMax * buffedAttackRangeMax || !enemyModel.gameObject.activeSelf || !enemyModel.IsAlive || enemyModel.IsInTunnel)
				{
					RemoveTarget(enemyModel);
				}
			}
		}

		private void RemoveTarget(EnemyData target)
		{
			if (OnTargetRemoved != null)
			{
				OnTargetRemoved(target);
			}
			Targets.Remove(target);
		}

		private void FindAllNewTargetsInRange()
		{
			allNewTargets.Clear();
			List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
			bool canTargetAir = originalParameter.canTargetAir;
			bool isRoundAttack = originalParameter.isRoundAttack;
			float sqrMaxRange = buffedAttackRangeMax * buffedAttackRangeMax;
			for (int i = 0; i < listActiveEnemy.Count; i++)
			{
				EnemyData enemy = listActiveEnemy[i];
				if (Targets.Contains(enemy)) continue;
				if (enemy.IsUnderground || enemy.IsInTunnel) continue;
				if (!canTargetAir && enemy.IsAir) continue;
				if (!MonoSingleton<GameRecord>.Instance.IsInRange(base.TowerModel.gameObject, enemy.gameObject, sqrMaxRange, 0f)) continue;
				allNewTargets.Add(enemy);
			}
			SortByPriority(allNewTargets, originalParameter.targetPriority);
		}

		private void SortByPriority(List<EnemyData> list, Parameter.TargetPriority priority)
		{
			if (list.Count <= 1) return;
			Vector3 towerPos = base.TowerModel.CachedPosition;
			switch (priority)
			{
			case Parameter.TargetPriority.First:
				list.Sort((a, b) => b.EnemyMovementController.currentTweenPosition
					.CompareTo(a.EnemyMovementController.currentTweenPosition));
				break;
			case Parameter.TargetPriority.Last:
				list.Sort((a, b) => a.EnemyMovementController.currentTweenPosition
					.CompareTo(b.EnemyMovementController.currentTweenPosition));
				break;
			case Parameter.TargetPriority.Strongest:
				list.Sort((a, b) => b.EnemyHealthController.OriginHealth
					.CompareTo(a.EnemyHealthController.OriginHealth));
				break;
			case Parameter.TargetPriority.Weakest:
				list.Sort((a, b) => a.EnemyHealthController.CurrentHealth
					.CompareTo(b.EnemyHealthController.CurrentHealth));
				break;
			case Parameter.TargetPriority.Closest:
				list.Sort((a, b) =>
					Vector3.SqrMagnitude(a.transform.position - towerPos)
					.CompareTo(Vector3.SqrMagnitude(b.transform.position - towerPos)));
				break;
			}
		}

		[SerializeField]
		private int desiredTargetsNumber;

		private List<string> silentBuffKeys = new List<string>
		{
			"Silent"
		};

		private List<string> increaseAttackRangeBuffKeys = new List<string>
		{
			"AttackRangeIncrementCommon"
		};

		[SerializeField]
		private List<EnemyData> targets = new List<EnemyData>();

		private float attackRangeMax;

		private float buffedAttackRangeMax;

		private TurretSpec originalParameter;

		private List<EnemyData> allNewTargets = new List<EnemyData>();
	}
}
