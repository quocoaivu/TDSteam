using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialStartGameInMapLevel : TutorialBase
	{
		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("HoÃ n thÃ nh bÆ°á»›c chá»n StartGame táº¡i MapLevelSelect");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID);
		}

		public void TryToCheckCondition()
		{
			if (TutorialStore.Instance.GetTutorialStatus(TutorialStore.TUTORIAL_ID_BRING_FIRST_HERO))
			{
				base.CheckCondition();
			}
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_START_GAME_MAP_LEVEL;
	}
}
