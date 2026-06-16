using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialToHeroCampPanelSecond : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Má»Ÿ panel hero camp, hoÃ n thÃ nh tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& MapProgressStore.Instance.GetMapIDUnlocked() >= 2
				&& PlayerPrefs.GetInt(tutorialID, 0) != 1;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_GO_HERO_CAMP_2ND;
	}
}
