using System;
using Services.PlatformSpecific;
using UnityEngine;

namespace Tutorial
{
	public class TutorialAnalytics : MonoBehaviour
	{
		public void SendEventDoneTutorial(int tutorialStep)
		{
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_DoneTutorial(tutorialStep);
		}
	}
}
