using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class TurretPrebuilder : MonoBehaviour
	{
		private void Start()
		{
			BuildTowers();
		}

		private void BuildTowers()
		{
			foreach (TurretPrebuild towerPrebuild in listTowersPrebuild)
			{
				BuildTower(towerPrebuild.towerID, towerPrebuild.towerLevel, towerPrebuild.buildRegionID);
			}
		}

		private void BuildTower(int towerID, int towerLevel, int buildRegionID)
		{
			TurretEntity towerModel = MonoSingleton<TowerPool>.Instance.GetTower(towerID, towerLevel);
			towerModel.StartBuild(towerID, towerLevel, buildRegionID);
			towerModel.Appear();
			towerModel.transform.position = MonoSingleton<ConstructSectorDirector>.Instance.listRegions[buildRegionID].transform.position;
			MonoSingleton<ConstructSectorDirector>.Instance.listRegions[buildRegionID].DisplayNotBuildable();
		}

		[SerializeField]
		private List<TurretPrebuild> listTowersPrebuild = new List<TurretPrebuild>();
	}
}
