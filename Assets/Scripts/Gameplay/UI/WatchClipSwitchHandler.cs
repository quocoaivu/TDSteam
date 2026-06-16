using System;
using UnityEngine;

namespace Gameplay
{
	public class WatchClipSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			ClipPlayerDirector.Instance.playVideoEndGame();
		}

		private void Update()
		{
			if (!WatchClipSwitchHandler.changedVideoStatus && MonoSingleton<GameRecord>.Instance.PlayedVideoEndGame)
			{
				endGameVideoController.StopCountDown();
				endGameVideoController.Close();
				WatchClipSwitchHandler.changedVideoStatus = true;
			}
		}

		[SerializeField]
		private EndGameClipHandler endGameVideoController;

		public static bool changedVideoStatus;
	}
}
