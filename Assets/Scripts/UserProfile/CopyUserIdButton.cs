using System;
using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class CopyUserIdButton : SwitchHandler
	{
        [SerializeField]
        private Text userID;

        public override void OnClick()
		{
			base.OnClick();
			CopyUserID();
		}

		private void CopyUserID()
		{
			if (string.IsNullOrEmpty(userID.text))
			{
				string localization = GameKit.GetLocalization("COPY_USERID_NOT_SUCCESS");
				MonoSingleton<LifespanSurface>.Instance.ToastPopupController.Init(localization);
			}
			else
			{
				UniClipboard.SetText(userID.text);
				string localization2 = GameKit.GetLocalization("COPY_USERID_SUCCESS");
				MonoSingleton<LifespanSurface>.Instance.ToastPopupController.Init(localization2);
			}
		}
	}
}
