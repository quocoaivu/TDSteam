using System;
using MetaGame;
using GameCore;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class TowerInformationUIManager : BaseMonoBehaviour
	{
		public TowerInformationPopup TowerInforPanel
		{
			get
			{
				return towerInforPanel;
			}
			set
			{
				towerInforPanel = value;
			}
		}

		private void Start()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode != GameFormat.TournamentMode)
					{
					}
				}
			}
			else if (ZoneRuleSpec.Instance.HaveTutorialTowerInMap(MonoSingleton<GameRecord>.Instance.MapID))
			{
				base.CustomInvoke(new Action(InitTowerInformationPanel), delayTimeToCheck);
			}
		}

		public void InitTowerInformationPanel()
		{
			towerInforPanelPath = "NewTower/NewTowerPanel";
			TowerInforPanel = UnityEngine.Object.Instantiate<TowerInformationPopup>(Common.AssetLoader.Load<TowerInformationPopup>(towerInforPanelPath));
			TowerInforPanel.transform.SetParent(panelParent);
			TowerInforPanel.transform.localScale = Vector3.one;
			TowerInforPanel.Init();
		}

		[SerializeField]
		private Transform panelParent;

		[Space]
		[SerializeField]
		private float delayTimeToCheck;

		private string towerInforPanelPath;

		private TowerInformationPopup towerInforPanel;
	}
}
