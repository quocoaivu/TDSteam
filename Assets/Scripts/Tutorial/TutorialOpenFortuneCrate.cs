using System;
using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialOpenFortuneCrate : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Má»Ÿ rÆ°Æ¡ng may máº¯n, hoÃ n thÃ nh tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		public bool IsTutorialDone()
		{
			return TutorialStore.Instance.GetTutorialStatus(tutorialID);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& MonoSingleton<GameRecord>.Instance.MapID == 0
				&& MapProgressStore.Instance.GetMapIDUnlocked() < 2;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_LUCKY_CHEST;
	}
}
