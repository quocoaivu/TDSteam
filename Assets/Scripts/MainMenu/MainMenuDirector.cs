using System;
using Services.PlatformSpecific;

namespace MainMenu
{
	public class MainMenuDirector : MonoSingleton<MainMenuDirector>
	{
		private void Start()
		{
			//analytics = NativeSpecificServicesSource.Services.Analytics;
			SendEventOpenScene();
		}

		private void SendEventOpenScene()
		{
			//analytics.SendEvent_OpenSceneMainMenu();
		}

		private IMetrics analytics;
	}
}
