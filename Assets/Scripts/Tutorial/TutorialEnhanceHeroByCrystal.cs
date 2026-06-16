using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialEnhanceHeroByCrystal : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Upgrade level cho hero, hoÃ n thÃ nh tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& (HeroStore.Instance.IsHeroOwned(1) & HeroStore.Instance.GetCurrentHeroLevel(1) <= 1)
				&& MapProgressStore.Instance.GetMapIDUnlocked() < 2;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_UPGRADE_HERO_LEVEL;
	}
}
