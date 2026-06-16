using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialToArenaBoard : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Má»Ÿ panel tournament, hoÃ n thÃ nh tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID) && MapProgressStore.Instance.GetMapIDUnlocked() >= 4;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_GO_TOURNAMENT;
	}
}
