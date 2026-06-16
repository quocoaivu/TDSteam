using System;
using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialSummonEnemy : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Click gá»i quÃ¡i, hoÃ n thÃ nh gá»i quÃ¡i!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID) && MonoSingleton<GameRecord>.Instance.MapID == 0;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_CALL_ENEMY;
	}
}
