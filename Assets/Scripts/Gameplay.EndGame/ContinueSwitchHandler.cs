using System;
using LifetimePopup;
using MetaGame;

namespace Gameplay.EndGame
{
	public class ContinueSwitchHandler : SwitchHandler
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
			ClipPlayerDirector.Instance.TryToShowInterstitialAds_EndGame();
			FormatDirector.Instance.gameMode = GameFormat.CampaignMode;
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.WorldMapSceneName);
			MonoSingleton<LifespanSurface>.Instance.TryOpenAskRatePopup(MonoSingleton<GameRecord>.Instance.MapID);
		}
	}
}
