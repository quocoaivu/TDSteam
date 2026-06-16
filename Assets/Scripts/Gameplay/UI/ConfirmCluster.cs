using System;
using Data;
using MetaGame;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class ConfirmCluster : MonoBehaviour
	{
        [SerializeField]
        private Text warning;

        [SerializeField]
        private int warningQuitID;

        [SerializeField]
        private int warningRestartID;


        private CancelGameKind cancelGameType;
        public void Init(CancelGameKind cancelGameType)
		{
			this.cancelGameType = cancelGameType;
			if (cancelGameType != CancelGameKind.Quit)
			{
				if (cancelGameType == CancelGameKind.Restart)
				{
					warning.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(warningRestartID);
				}
			}
			else
			{
				warning.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(warningQuitID);
			}
		}

		public void Click_YES()
		{
			LoadingScreen.Instance.ShowLoading();
			CancelGameKind cancelGameType = this.cancelGameType;
			if (cancelGameType != CancelGameKind.Quit)
			{
				if (cancelGameType == CancelGameKind.Restart)
				{
					base.Invoke("DoRestart", 1f);
				}
			}
			else
			{
				base.Invoke("DoQuit", 1f);
			}
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
		}

		public void Click_NO()
		{
			MonoSingleton<UIRootHandler>.Instance.settingPopupController.Close();
		}

		private void DoQuit()
		{
			FormatDirector.Instance.gameMode = GameFormat.CampaignMode;
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.WorldMapSceneName);
		}

		private void DoRestart()
		{
			SendEventRestartGame();
			GameplayDirector.Instance.ReloadCurrentScene();
		}

		private void SendEventRestartGame()
		{
			int mapID = MonoSingleton<GameRecord>.Instance.MapID;
			int starEarnedByMap = MapProgressStore.Instance.GetStarEarnedByMap(mapID);
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_RestartGame_Setting(mapID + 1, starEarnedByMap);
		}
	}
}
