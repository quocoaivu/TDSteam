using System;
using LifetimePopup;
using GameCore;
using Parameter;
using Services.PlatformSpecific;
using WorldMap;

namespace UserProfile
{
	public class RestoreSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			if (!GameUtils.IsInternetConnectionAvailable())
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(134);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
				return;
			}
			if (NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Google() || NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Facebook())
			{
				MonoSingleton<UIRootHandler>.Instance.userProfilePopupController.InitConfirmPopup(CloudRecordInteraction.Restore);
			}
			else
			{
				string notiContent2 = Singleton<AlertSynopsis>.Instance.GetNotiContent(133);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent2, false, false);
			}
		}
	}
}
