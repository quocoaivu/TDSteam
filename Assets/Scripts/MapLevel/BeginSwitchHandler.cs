using System;
using Data;
using MetaGame;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class BeginSwitchHandler : MonoBehaviour
	{
		private void Awake()
		{
			startButton = base.GetComponent<Button>();
		}

		private void Update()
		{
			if (heroesInputGroupController.HeroesSelectedController.IsChooseAtLeastOneHero())
			{
				SetStartButtonEnable();
			}
			else
			{
				SetStartButtonDisable();
			}
		}

		private void SetStartButtonEnable()
		{
			startButton.enabled = true;
			material.SetFloat("_EffectAmount", 0f);
			textNotification.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(readyContentID);
			textNotification.color = readyColor;
		}

		private void SetStartButtonDisable()
		{
			startButton.enabled = false;
			material.SetFloat("_EffectAmount", 1f);
			textNotification.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(notReadyContentID);
			textNotification.color = notReadyColor;
		}

		private void DoLoadSceneGameplay()
		{
			MapProgressStore.Instance.SaveLastMapPlayed(mapLevelSelectPopupController.CurrentMapID);
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.GameplaySceneName);
		}

		public void OnClick()
		{
			base.Invoke("OnPrepareToLoad", 0.1f);
			UISfxDirector.Instance.PlayStartGameAtMapLevel();
		}

		private void OnPrepareToLoad()
		{
			LoadingScreen.Instance.ShowLoading();
			base.Invoke("DoLoadSceneGameplay", 0.3f);
			FormatDirector.Instance.gameMode = GameFormat.CampaignMode;
			mapLevelSelectPopupController.OnStartGame();
			mapLevelSelectPopupController.InitListHeroesIDSelected();
			mapLevelSelectPopupController.SaveListHeroIDSelected();
			SendEvent_StartGame();
		}

		private void SendEvent_StartGame()
		{
			int mapID = mapLevelSelectPopupController.CurrentMapID + 1;
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_StartGame_MapLevel(mapID);
		}

		[SerializeField]
		private ZoneLevelSelectDialogHandler mapLevelSelectPopupController;

		[SerializeField]
		private HerosInputClusterHandler heroesInputGroupController;

		[Space]
		[Header("Image material")]
		[SerializeField]
		private Material material;

		private Button startButton;

		[Space]
		[Header("Text notification")]
		[SerializeField]
		private Text textNotification;

		[SerializeField]
		private int readyContentID;

		[SerializeField]
		private int notReadyContentID;

		[SerializeField]
		private Color readyColor;

		[SerializeField]
		private Color notReadyColor;
	}
}
