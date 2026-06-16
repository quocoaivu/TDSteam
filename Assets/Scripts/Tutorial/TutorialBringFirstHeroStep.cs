using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialBringFirstHeroStep : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("HoÃ n thÃ nh bÆ°á»›c chá»n First Hero!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID);
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_BRING_FIRST_HERO;
	}
}
