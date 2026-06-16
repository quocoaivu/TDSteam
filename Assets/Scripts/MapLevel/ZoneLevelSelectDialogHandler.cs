using System;
using Data;
using Gameplay;
using MetaGame;
using Parameter;
using Services.PlatformSpecific;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class ZoneLevelSelectDialogHandler : GameplayDialogHandler
	{
		public int CurrentMapID
		{
			get
			{
				return currentMapID;
			}
			set
			{
				currentMapID = value;
			}
		}

		public HerosInputClusterHandler HeroesInputGroupController
		{
			get
			{
				return heroesInputGroupController;
			}
			set
			{
				heroesInputGroupController = value;
			}
		}

		public PowerUpItemClusterHandler PowerUpItemGroupController
		{
			get
			{
				return powerUpItemGroupController;
			}
			set
			{
				powerUpItemGroupController = value;
			}
		}

		public GameFormatSelectClusterHandler GameModeSelectGroupController
		{
			get
			{
				return gameModeSelectGroupController;
			}
			set
			{
				gameModeSelectGroupController = value;
			}
		}

		public void Init(int mapID)
		{
			CurrentMapID = mapID;
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			SetLevelName();
			DisplayStarGroup();
			HeroesInputGroupController.HeroesSelectedController.InitHeroesSlot();
			HeroesInputGroupController.HeroesSelectController.InitListHeroToSelect();
			PowerUpItemGroupController.RefreshPowerupItems();
			GameModeSelectGroupController.InitDefault();
			OnInitPopup();
			SendEventOpenPopup();
		}

		private void SendEventOpenPopup()
		{
			int mapID = CurrentMapID + 1;
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_OpenMapLevelSelect(mapID);
		}

		private void OnInitPopup()
		{
			OnInitPopupEvent.Dispatch();
		}

		public void OnChooseHero()
		{
			OnChooseHeroEvent.Dispatch();
		}

		public void OnStartGame()
		{
			OnStartGameEvent.Dispatch();
		}

		private void SetLevelName()
		{
			levelName.text = (CurrentMapID + 1).ToString();
		}

		private void DisplayStarGroup()
		{
			starGroupController.DisplayStarGroup(MapProgressStore.Instance.GetStarEarnedByMap(CurrentMapID));
		}

		public void InitListHeroesIDSelected()
		{
			int maxHeroAllowed = ZoneRuleSpec.Instance.GetMaxHeroAllowed(CurrentMapID);
			CrossSceneData.Instance.ClearListHeroID();
			for (int i = 0; i < maxHeroAllowed; i++)
			{
				if (HeroesInputGroupController.HeroesSelectedController.listHeroesIDSelected[i] >= 0)
				{
					CrossSceneData.Instance.AddHeroIDToList(HeroesInputGroupController.HeroesSelectedController.listHeroesIDSelected[i]);
				}
			}
		}

		public void SaveListHeroIDSelected()
		{
			HeroPrepareStore.Instance.Save(HeroesInputGroupController.HeroesSelectedController.listHeroesIDSelected);
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			SaveListHeroIDSelected();
			if (HeroesInputGroupController.AskToBuyHeroDialogHandler.isOpen)
			{
				HeroesInputGroupController.AskToBuyHeroDialogHandler.CloseWithScaleAnimation();
			}
		}

		[Header("Events")]
		[SerializeField]
		private OrderedUnityEvent OnInitPopupEvent;

		[SerializeField]
		private OrderedUnityEvent OnChooseHeroEvent;

		[SerializeField]
		private OrderedUnityEvent OnStartGameEvent;

		[Space]
		[Header("Star and level")]
		[SerializeField]
		private StarClusterHandler starGroupController;

		[SerializeField]
		private Text levelName;

		private int currentMapID;

		[Space]
		[Header("Controllers")]
		[SerializeField]
		private HerosInputClusterHandler heroesInputGroupController;

		[SerializeField]
		private PowerUpItemClusterHandler powerUpItemGroupController;

		[SerializeField]
		private GameFormatSelectClusterHandler gameModeSelectGroupController;
	}
}
