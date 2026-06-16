using System;
using System.Collections.Generic;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class PrimerTurretHandler : GeneralDialogHandler
	{
		public void Init()
		{
			Open();
			InitListButton();
			SetButtonsStatus();
			base.CustomInvoke(new Action(InitDefaultData), Time.deltaTime);
		}

		private void InitListButton()
		{
			if (listSelectTower.Count < 1)
			{
				int numberOfTower = TowerParameterManager.Instance.GetNumberOfTower();
				for (int i = 0; i < numberOfTower; i++)
				{
					listSelectTower.Add(new List<SelectTurretSwitchHandler>());
					for (int j = 0; j < 5; j++)
					{
						string path = string.Format("UI WorldMap/Guide/SelectTower/select_tower", new object[0]);
						SelectTurretSwitchHandler selectTowerButtonController = UnityEngine.Object.Instantiate<SelectTurretSwitchHandler>(Common.AssetLoader.Load<SelectTurretSwitchHandler>(path));
						selectTowerButtonController.transform.SetParent(listButtonsHolder[i]);
						selectTowerButtonController.Init(i, j);
						selectTowerButtonController.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
						listSelectTower[i].Add(selectTowerButtonController);
					}
				}
			}
		}

		private void SetButtonsStatus()
		{
			int mapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked();
			int[] maxTowerLevelByMapID = ZoneRuleSpec.Instance.GetMaxTowerLevelByMapID(mapIDUnlocked);
			int numberOfTower = TowerParameterManager.Instance.GetNumberOfTower();
			for (int i = 0; i < numberOfTower; i++)
			{
				for (int j = 0; j <= 4; j++)
				{
					if (j <= maxTowerLevelByMapID[i])
					{
						listSelectTower[i][j].SetUnLock();
					}
					else
					{
						listSelectTower[i][j].SetLock();
					}
				}
			}
		}

		public void RefreshTowerInformation()
		{
			SetAvatar();
			towerInformationController.Init(currentTowerIDSelected, currentTowerLvSelected);
		}

		private void InitDefaultData()
		{
			towerInformationController.HideAll();
			if (listSelectTower.Count > 0)
			{
				listSelectTower[0][0].OnClick();
			}
		}

		private void SetAvatar()
		{
			towerAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Towers/p_tower_{0}_{1}", currentTowerIDSelected, currentTowerLvSelected));
		}

		public override void Open()
		{
			base.Open();
		}

		public override void Close()
		{
			base.Close();
			PrimerDialogHandler.Instance.HideSelectedTowerImage();
		}

		[NonSerialized]
		public int currentTowerIDSelected;

		[NonSerialized]
		public int currentTowerLvSelected;

		[SerializeField]
		private Transform[] listButtonsHolder;

		[SerializeField]
		private TurretOverviewHandler towerInformationController;

		private List<List<SelectTurretSwitchHandler>> listSelectTower = new List<List<SelectTurretSwitchHandler>>();

		[SerializeField]
		private Image towerAvatar;
	}
}
