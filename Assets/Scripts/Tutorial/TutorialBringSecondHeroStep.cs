using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialBringSecondHeroStep : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("HoÃ n thÃ nh bÆ°á»›c chá»n Second Hero");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID) && HeroStore.Instance.IsHeroOwned(1);
		}

		public new void TryToSetTutorialPassed()
		{
			if (ShouldShowTutorial())
			{
				base.SetTutorialPassed();
			}
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_BRING_SECOND_HERO;
	}
}
