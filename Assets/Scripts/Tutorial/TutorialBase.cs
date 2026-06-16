using System;
using System.Collections.Generic;
using Data;
using Common;
using UnityEngine;

namespace Tutorial
{
	public abstract class TutorialBase : MonoBehaviour
	{
        [Space]
        [SerializeField]
        private OrderedUnityEvent onShowTutorial;

        [SerializeField]
        private OrderedUnityEvent onPassedTutorial;

        [Space]
        [SerializeField]
        private List<OrderedUnityEvent> steps = new List<OrderedUnityEvent>();

        private bool showed;

        private int currentStep;


        protected abstract bool ShouldShowTutorial();

		protected abstract void SaveTutorialPassed();

		public void CheckCondition()
		{
			if (ShouldShowTutorial())
			{
				TutorialStore.Instance.currentTutorial = this;
				Common.SSRTrace.Log("Show Tut thÃ´i!");
				showed = true;
				onShowTutorial.Dispatch();
				TryDispactStep(0);
			}
			else
			{
				Common.SSRTrace.Log("ÄÃ£ hoÃ n thÃ nh tut, hoáº·c khÃ´ng Ä‘á»§ Ä‘iá»u kiá»‡n Ä‘á»ƒ hiá»‡n!");
			}
		}

		public void SetTutorialPassed()
		{
			if (showed)
			{
				showed = false;
				onPassedTutorial.Dispatch();
				SaveTutorialPassed();
			}
		}

		public void TryToSetTutorialPassed()
		{
			onPassedTutorial.Dispatch();
			SaveTutorialPassed();
		}

		public void TryToSaveTutorialPassed()
		{
			SaveTutorialPassed();
		}

		public void NextStep()
		{
			if (showed && currentStep < steps.Count - 1)
			{
				currentStep++;
				TryDispactStep(currentStep);
			}
		}

		public void TryMoveToStep(int stepId)
		{
			if (showed)
			{
				if (currentStep > stepId)
				{
					return;
				}
				if (currentStep != stepId)
				{
					TryDispactStep(stepId);
				}
			}
		}

		private void TryDispactStep(int stepId)
		{
			while (stepId < steps.Count && steps[stepId].UnityEventsCount <= 0)
			{
				stepId++;
			}
			if (stepId < steps.Count)
			{
				currentStep = stepId;
				steps[currentStep].Dispatch();
			}
		}
	}
}
