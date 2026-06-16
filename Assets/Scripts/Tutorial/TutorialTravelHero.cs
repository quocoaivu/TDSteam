using System;
using Data;
using Gameplay;
using MetaGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tutorial
{
	public class TutorialTravelHero : TutorialBase
	{
        private string tutorialID = TutorialStore.TUTORIAL_ID_HERO_MOVE;

        public void MoveHeroToPosition()
		{
			RaycastHit2D mapHit = Physics2D.Raycast(getTargetVector(), Vector3.back, 5f);
			RaycastHit2D entityHit = Physics2D.Raycast(getTargetVector(), Vector3.back, 5f);
			TapInputRecord inputData = new TapInputRecord(TapInputPhase.Up, mapHit, entityHit);
			InputFilterDirector.Instance.OnHandleInput_ClickMapToMoveHero(inputData);
		}

		private Vector2 getTargetVector()
		{
			Vector2 screenPosition = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
			Vector3 vector = Camera.main.ScreenToWorldPoint(screenPosition);
			return new Vector2(vector.x, vector.y);
		}

		protected override void SaveTutorialPassed()
		{
			Common.SSRTrace.Log("HoÃ n thÃ nh tut di chuyá»ƒn hero!");
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			if (FormatDirector.Instance.gameMode != GameFormat.CampaignMode)
			{
				return false;
			}
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID)
				&& GameplayTutorialDirector.Instance.IsTutorialMap();
		}

	}
}
