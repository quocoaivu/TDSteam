using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialSelectSecondZone : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Click vÃ o chá»n map, hoÃ n thÃ nh bÆ°á»›c chá»n map!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID);
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_SELECT_SECOND_MAP;
	}
}
