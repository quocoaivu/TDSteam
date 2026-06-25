using System;
using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialGetFreeFortuneCrateByClip : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("Click button watch video, hoÃ n thÃ nh Tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			// Mirrors the Map 0 end-game tutorial (TutorialOpenFortuneCrate), one map later: only show in the
			// early window for Map 1. GetMapIDUnlocked() is already bumped to 2 by the time the end-game prize
			// dialog opens after the first Map 1 victory (2 < 3 -> shows); once Map 2 is cleared it reaches 3 and
			// the tutorial stops re-appearing on Map 1 replays even if the player never watched the video (its
			// own "passed" flag is only set when they do). Without this guard it replayed on every Map 1 finish.
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& MonoSingleton<GameRecord>.Instance.MapID == 1
				&& MapProgressStore.Instance.GetMapIDUnlocked() < 3;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_GET_LUCKY_CHEST_BY_VIDEO;
	}
}
