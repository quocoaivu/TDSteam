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
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID) && MonoSingleton<GameRecord>.Instance.MapID == 1;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_GET_LUCKY_CHEST_BY_VIDEO;
	}
}
