using System;
using Data;
using LifetimePopup;
using GameCore;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace UserProfile
{
	public class BackupSwitchHandler : SwitchHandler
	{
        [SerializeField]
        private Button button;

        [SerializeField]
        private Image image;

        private void Start()
		{
			if (PassSpecificCondition())
			{
				SetClickable();
			}
			else
			{
				SetUnClickable();
			}
		}

		private bool PassSpecificCondition()
		{
			return MapProgressStore.Instance.GetMapIDPassed() >= 0;
		}

		private void SetClickable()
		{
			button.enabled = true;
			image.color = Color.white;
		}

		private void SetUnClickable()
		{
			button.enabled = false;
			image.color = Color.gray;
		}

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
				MonoSingleton<UIRootHandler>.Instance.userProfilePopupController.InitConfirmPopup(CloudRecordInteraction.Backup);
			}
			else
			{
				string notiContent2 = Singleton<AlertSynopsis>.Instance.GetNotiContent(133);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent2, false, false);
			}
		}
	}
}
