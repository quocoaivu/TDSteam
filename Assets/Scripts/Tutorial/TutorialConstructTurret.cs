using System;
using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialConstructTurret : TutorialBase
	{
		private void Start()
		{
			base.CheckCondition();
		}

		public void InvokeBuildRegionClick(int regionID)
		{
			MonoSingleton<ConstructSectorDirector>.Instance.InvokeClickk(regionID);
		}

		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("XÃ¢y trá»¥ thÃ nh cÃ´ng hoÃ n thÃ nh tut!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID) && MonoSingleton<GameRecord>.Instance.MapID == 0;
		}

		private string tutorialID = TutorialStore.TUTORIAL_ID_BUILD_TOWER;
	}
}
