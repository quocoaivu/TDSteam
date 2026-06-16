using System;
using MetaGame;

namespace DailyTrial
{
	public class BeginSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			base.Invoke("OnPrepareToLoad", 0.1f);
			UISfxDirector.Instance.PlayStartGameAtMapLevel();
		}

		private void OnPrepareToLoad()
		{
			LoadingScreen.Instance.ShowLoading();
			base.Invoke("DoLoadSceneGameplay", 0.3f);
			FormatDirector.Instance.gameMode = GameFormat.DailyTrialMode;
		}

		private void DoLoadSceneGameplay()
		{
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.GameplaySceneName);
		}
	}
}
