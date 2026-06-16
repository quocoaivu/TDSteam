using System;
using Data;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class TowerPool : MonoSingleton<TowerPool>
	{
        public void InitTowerPool()
		{
			int numberOfTower = TowerParameterManager.Instance.GetNumberOfTower();
			for (int i = 0; i < numberOfTower; i++)
			{
				int num = GetMaxLevelTowerCanUpgrade(i);
				for (int j = 0; j <= num; j++)
				{
					string arg = PoolNames.Tower(i, j);
					TurretEntity prefab = Common.AssetLoader.Load<TurretEntity>(string.Format("Towers/{0}", arg));
					if (prefab == null)
					{
						Debug.LogError("TowerPool.InitTowerPool: tower prefab not found: " + arg);
						continue;
					}
					TurretEntity towerModel = Instantiate(prefab);
					towerModel.gameObject.SetActive(false);
					// Towers that fire through the common controller pull "bullet_{i}_{j}" at runtime.
					// Detect that here so we can pre-pool the bullet alongside the tower (below).
					bool firesCommonBullet = towerModel.GetComponentInChildren<TurretStrikeSingleMarkSharedHandler>(true) != null;
					Common.GameObjectPool.ManagePool(towerModel.gameObject, 0);
					Common.GameObjectPool.Despawn(towerModel.gameObject);
					// Pre-pool the matching bullet at load so a missing prefab surfaces now (BulletPool
					// logs an error) instead of mid-combat, and the pool is warm before the first shot.
					// Barracks/gold/custom-ultimate towers lack that controller and have no common bullet.
					if (firesCommonBullet)
					{
						MonoSingleton<BulletPool>.Instance.InitBulletsFromTower(i, j);
					}
				}
			}
		}

		// Max upgrade level allowed for tower `towerIndex` in the current game mode.
		private int GetMaxLevelTowerCanUpgrade(int towerIndex)
		{
			switch (FormatDirector.Instance.gameMode)
			{
				case GameFormat.CampaignMode:
					return ZoneRuleSpec.Instance.GetMaxLevelTowerCanUpgrade_Campaign(GameRecord.Instance.MapID, towerIndex);
				case GameFormat.DailyTrialMode:
					int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
					return DailyOrdealSpec.Instance.GetMaxLevelTowerCanUpgrade(currentDayIndex, towerIndex);
				case GameFormat.TournamentMode:
					string currentSeasonID = ZoneRuleSpec.Instance.GetCurrentSeasonID();
					return ZoneRuleSpec.Instance.GetMaxLevelTowerCanUpgrade_Tournament(currentSeasonID, towerIndex);
				default:
					return 0;
			}
		}

		public void InitWeaponStation(GameObject weapon)
		{
			GameObject gameObject = Instantiate(weapon);
			gameObject.gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(gameObject.gameObject, 0);
			Common.GameObjectPool.Despawn(gameObject);
		}

		public ArmamentBay GetWeaponStation(string name)
		{
			string gameObjectName = PoolNames.Clone(name);
			GameObject gameObject = Common.GameObjectPool.Spawn(gameObjectName, default(Vector3), default(Quaternion));
			ArmamentBay component = gameObject.GetComponent<ArmamentBay>();
			component.transform.parent = transform;
			return component;
		}

		public TurretEntity GetTower(int id, int level)
		{
			string gameObjectName = PoolNames.Clone(PoolNames.Tower(id, level));
			GameObject gameObject = Common.GameObjectPool.Spawn(gameObjectName, default(Vector3), default(Quaternion));
			TurretEntity component = gameObject.GetComponent<TurretEntity>();
			component.gameObject.SetActive(true);
			component.transform.parent = transform;
			return component;
		}

		public void Despawn(TurretEntity tower)
		{
			tower.transform.position = Common.GameObjectPool.PoolPosition;
			tower.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(tower.gameObject);
		}

		public void Despawn(GameObject go)
		{
			go.transform.position = Common.GameObjectPool.PoolPosition;
			go.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(go);
		}

		public void InitMiniDragon(GameObject miniDragonPrefab)
		{
			GameObject gameObject = Instantiate(miniDragonPrefab);
			gameObject.gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(gameObject.gameObject, 2);
			Common.GameObjectPool.Despawn(gameObject.gameObject);
		}

		public MiniWyrmHandler GetMiniDragon()
		{
			GameObject gameObject = Common.GameObjectPool.Spawn("MiniDragon(Clone)", default(Vector3), default(Quaternion));
			MiniWyrmHandler component = gameObject.GetComponent<MiniWyrmHandler>();
			// Return inactive on purpose: caller (Turret3Mastery0Ability0) sets position/parent
			// and calls Init() before activating, so Awake/OnEnable don't run prematurely.
			component.gameObject.SetActive(false);
			return component;
		}
	}
}
