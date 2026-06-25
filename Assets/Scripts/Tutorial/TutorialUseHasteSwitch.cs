using System;
using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialUseHasteSwitch : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("HoÃ n thÃ nh tut sá»­ dá»¥ng X2!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID) && GameplayTutorialDirector.IsFirstPlayTutorialMap();
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_USE_SPEED_UP;
	}
}
