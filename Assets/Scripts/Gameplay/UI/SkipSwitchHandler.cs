using System;
using UnityEngine;

namespace Gameplay
{
	public class SkipSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			endGameVideoController.StopCountDown();
			endGameVideoController.Close();
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
			GameplayDirector.Instance.gameLogicController.Defeated();
		}

		[SerializeField]
		private EndGameClipHandler endGameVideoController;
	}
}
