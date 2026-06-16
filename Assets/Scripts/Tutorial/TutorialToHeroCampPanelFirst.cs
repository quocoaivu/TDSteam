using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialToHeroCampPanelFirst : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Má»Ÿ panel hero camp, hoÃ n thÃ nh tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& HeroStore.Instance.IsHeroOwned(1)
				&& MapProgressStore.Instance.GetMapIDUnlocked() < 2
				&& HeroStore.Instance.GetCurrentHeroLevel(1) < 1;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_GO_HERO_CAMP_1ST;
	}
}
