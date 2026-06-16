using System;
using LifetimePopup;
using GameCore;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class SignInGoogleSwitchHandler : SwitchHandler
	{
        [SerializeField]
        private Button button;

        [SerializeField]
        private Image image;
        
		public override void OnClick()
		{
			base.OnClick();
			if (!GameUtils.IsInternetConnectionAvailable())
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(134);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
				return;
			}
			NativeSpecificServicesSource.Services.UserProfile.LogIn_Google();
		}

		public override void UpdateButtonStatus()
		{
			base.UpdateButtonStatus();
			if (!NativeSpecificServicesSource.Services.UserProfile.IsLoggedIn_Google())
			{
				Show();
			}
			else
			{
				Hide();
			}
		}

		private void Hide()
		{
			button.enabled = false;
			image.color = Color.gray;
		}

		private void Show()
		{
			button.enabled = true;
			image.color = Color.white;
		}
	}
}
