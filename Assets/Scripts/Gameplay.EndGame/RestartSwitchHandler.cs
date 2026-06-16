using System;
using Data;
using Services.PlatformSpecific;

namespace Gameplay.EndGame
{
	public class RestartSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			LoadingScreen.Instance.ShowLoading();
			base.Invoke("DoLoad", 1f);
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
		}

		private void DoLoad()
		{
			SendEventRestartGame();
			GameplayDirector.Instance.ReloadCurrentScene();
		}

		private void SendEventRestartGame()
		{
			int mapID = MonoSingleton<GameRecord>.Instance.MapID;
			int starEarnedByMap = MapProgressStore.Instance.GetStarEarnedByMap(mapID);
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_RestartGame_EndGame(mapID + 1, starEarnedByMap);
		}
	}
}
