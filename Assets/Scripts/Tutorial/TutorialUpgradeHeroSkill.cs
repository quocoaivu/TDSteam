using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialUpgradeHeroSkill : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Upgrade skill level cho hero, hoÃ n thÃ nh tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			// Báº£n gá»‘c auto-pass khi skillPoint > 1; predicate Ä‘Ã£ cÃ³ Ä‘iá»u kiá»‡n skillPoint == 1
			// nÃªn tráº£ vá» false sáºµn trong trÆ°á»ng há»£p Ä‘Ã³ â€” chá»‰ cáº§n bá» side-effect.
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& HeroStore.Instance.GetCurrentSkillPoint(2) >= 1
				&& HeroStore.Instance.GetSkillPoint(2, 0) == 1
				&& MapProgressStore.Instance.GetMapIDUnlocked() >= 2;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_UPGRADE_HERO_SKILL;
	}
}
