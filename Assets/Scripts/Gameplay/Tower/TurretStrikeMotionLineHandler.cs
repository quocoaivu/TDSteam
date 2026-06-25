using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class TurretStrikeMotionLineHandler : TurretHandler
	{
		private void Init()
		{
			towerSourceId = GameKit.GetTowerSourceId(base.TowerModel.Level, base.TowerModel.Id);
			sqBulletRadius = bulletRadius * bulletRadius;
			minMagicDam = base.TowerModel.OriginalParameter.damage;
			maxMagicDam = base.TowerModel.OriginalParameter.damage;
			CalculateParameter();
		}

		private void CalculateParameter()
		{
			if (base.TowerModel.towerFindEnemyController.Targets.Count > 0)
			{
				EnemyData target = GetTarget();
				towerAttackSingleTargetCommonController.Bullet.gameObject.SetActive(false);
				rootPositionLineShot = base.TowerModel.transform.position;
				moveDir = (target.transform.position - rootPositionLineShot).normalized;
				endPos = rootPositionLineShot + moveDir * 0.4f;
				moveDisLeft = lineShotMaxRange;
				enemyFlag.Clear();
				lazeController.Init();
				Show();
			}
		}

		private EnemyData GetTarget()
		{
			List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
			float attackRangeMax = base.TowerModel.towerFindEnemyController.AttackRangeMax;
			float num = attackRangeMax * attackRangeMax;
			for (int i = listActiveEnemy.Count - 1; i >= 0; i--)
			{
				if ((listActiveEnemy[i].transform.position - rootPositionLineShot).sqrMagnitude <= num && listActiveEnemy[i].Id == 22)
				{
					return listActiveEnemy[i];
				}
			}
			return base.TowerModel.towerFindEnemyController.Targets[0];
		}

		private void OnLineShotComplete()
		{
			Hide();
		}

		public override void OnAppear()
		{
			base.OnAppear();
			Reset();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			Reset();
		}

		public override void Update()
		{
			base.Update();
			if (moveDisLeft > 0f)
			{
				float num = Time.deltaTime * lineShotSpeed;
				moveDisLeft -= num;
				endPos += moveDir * num;
				lazeController.Resize(endPos);
				List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
				for (int i = listActiveEnemy.Count - 1; i >= 0; i--)
				{
					if (!enemyFlag.Contains(listActiveEnemy[i].GetEntityId()) && (listActiveEnemy[i].transform.position - endPos).sqrMagnitude < sqBulletRadius)
					{
						enemyFlag.Add(listActiveEnemy[i].GetEntityId());
						ObjectCache.Spawn(behitEffectPrefab, listActiveEnemy[i].transform.position);
						listActiveEnemy[i].ProcessDamage(DamageKind.Range, new SharedStrikeDamage(CharacterKind.Tower, towerSourceId, 0, UnityEngine.Random.Range(minMagicDam, maxMagicDam), 0f, base.TowerModel.finalCriticalStrikeChange, 0));
					}
				}
				if (moveDisLeft <= 0f)
				{
					lazeController.StopImmediate();
				}
			}
		}

		private void Awake()
		{
			towerAttackSingleTargetCommonController.OnFireBullet += TowerAttackSingleTargetCommonController_OnFireBullet;
		}

		private void TowerAttackSingleTargetCommonController_OnFireBullet(ProjectileEntity arg1, EnemyData arg2)
		{
			Init();
		}

		private void OnDestroy()
		{
			towerAttackSingleTargetCommonController.OnFireBullet -= TowerAttackSingleTargetCommonController_OnFireBullet;
		}

		private void Reset()
		{
			Hide();
		}

		private void Show()
		{
			lazeController.gameObject.SetActive(true);
		}

		private void Hide()
		{
			lazeController.gameObject.SetActive(false);
		}

		[Space]
		[Header("Line shot param")]
		[SerializeField]
		private LaserController lazeController;

		[SerializeField]
		private float lineShotSpeed;

		[SerializeField]
		private float lineShotMaxRange;

		public float bulletRadius = 0.45f;

		public GameObject behitEffectPrefab;

		private Vector3 rootPositionLineShot;

		private Vector3 moveDir;

		private float moveDisLeft;

		private Vector3 endPos;

		private HashSet<int> enemyFlag = new HashSet<int>();

		private float sqBulletRadius;

		private int towerSourceId;

		private int minMagicDam;

		private int maxMagicDam;

		[SerializeField]
		private TurretStrikeSingleMarkSharedHandler towerAttackSingleTargetCommonController;
	}
}
