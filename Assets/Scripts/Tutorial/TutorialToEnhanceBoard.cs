using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialToEnhanceBoard : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Má»Ÿ panel upgrade, hoÃ n thÃ nh tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& PlayerCurrencyStore.Instance.GetCurrentStar() >= 1
				&& MapProgressStore.Instance.GetMapIDUnlocked() < 2;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_GO_GLOBAL_UPGRADE;
	}
}
