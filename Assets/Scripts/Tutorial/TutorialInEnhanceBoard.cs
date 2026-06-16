using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialInEnhanceBoard : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("ÄÃ£ upgrade thÃ nh cÃ´ng 1 tier, hoÃ n thÃ nh tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			// Hiá»‡n khi chÆ°a pass, cÃ³ sao Ä‘á»ƒ tiÃªu, vÃ  chÆ°a upgrade type 2 (level < 1).
			// (Sá»­a tá»« Ä‘iá»u kiá»‡n auto-pass gá»‘c '>= 0' â€” luÃ´n Ä‘Ãºng nÃªn tut khÃ´ng bao giá» hiá»‡n â€” thÃ nh '>= 1'.)
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& PlayerCurrencyStore.Instance.GetCurrentStar() >= 1
				&& GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(2) < 1;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_GLOBAL_UPGRADE;
	}
}
