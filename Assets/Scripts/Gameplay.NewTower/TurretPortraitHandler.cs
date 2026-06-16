using System;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.NewTower
{
	public class TurretPortraitHandler : MonoBehaviour
	{
		private void Start()
		{
			towerNameText.text = Singleton<TurretSynopsis>.Instance.GetTowerName(towerID);
			towerLevelText.text = "Level " + (towerLevel + 1).ToString();
		}

		[SerializeField]
		private Text towerNameText;

		[SerializeField]
		private Text towerLevelText;

		[SerializeField]
		private int towerID;

		[SerializeField]
		private int towerLevel;
	}
}
