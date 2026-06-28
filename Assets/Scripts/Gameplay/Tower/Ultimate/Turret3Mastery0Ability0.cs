using System;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret3Mastery0Ability0 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
			ClearData();
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(miniDragonBullet.gameObject);
			MonoSingleton<TowerPool>.Instance.InitMiniDragon(miniDragonPrefab.gameObject);
			ReadParameter(ultiLevel);
			TryToCreateMiniDragon();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			ClearData();
		}

		private void ClearData()
		{
			foreach (MiniWyrmHandler miniDragonController in miniDragonControllers)
			{
				miniDragonController.ReturnPool();
			}
			miniDragonControllers.Clear();
			List<MiniWyrmHandler> list = new List<MiniWyrmHandler>(base.GetComponentsInChildren<MiniWyrmHandler>(true));
			foreach (MiniWyrmHandler miniDragonController2 in list)
			{
				miniDragonController2.ReturnPool();
			}
		}

		private void ReadParameter(int currentSkillLevel)
		{
			numberOfDragon = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			damagePerDragon = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
		}

		public void TryToCreateMiniDragon()
		{
			if (!unlock)
			{
				return;
			}
			MiniWyrmHandler miniDragon = MonoSingleton<TowerPool>.Instance.GetMiniDragon();
			miniDragonControllers.Add(miniDragon.GetComponent<MiniWyrmHandler>());
			Vector3 position = miniDragonPositions[miniDragonControllers.Count - 1].position;
			miniDragon.Init(towerModel, damagePerDragon);
			miniDragon.transform.position = position;
			miniDragon.transform.parent = base.transform;
			miniDragon.gameObject.SetActive(true);
		}

		// Each tower attack, send the spawned mini-dragons at a target. No-op until the skill is equipped
		// (unlock false -> no dragons created -> empty list).
		public override void OnTowerAttack()
		{
			if (!unlock)
			{
				return;
			}
			StartAttack();
		}

		public void StartAttack()
		{
			List<EnemyData> targets = towerModel.towerFindEnemyController.Targets;
			if (targets.Count > 0)
			{
				target = targets[UnityEngine.Random.Range(0, targets.Count)];
				foreach (MiniWyrmHandler miniDragonController in miniDragonControllers)
				{
					miniDragonController.StartAttack(target);
				}
			}
		}

		private int towerID = 3;

		private int ultimateBranch;

		private int skillID;

		private int numberOfDragon;

		private int maxDragon = 3;

		private int damagePerDragon;

		private TurretEntity towerModel;

		private EnemyData target;

		private List<MiniWyrmHandler> miniDragonControllers = new List<MiniWyrmHandler>();

		[SerializeField]
		private MiniWyrmHandler miniDragonPrefab;

		[SerializeField]
		private Transform[] miniDragonPositions;

		[Space]
		[SerializeField]
		private GameObject miniDragonBullet;
	}
}
