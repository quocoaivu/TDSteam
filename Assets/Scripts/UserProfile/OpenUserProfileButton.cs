using System;
using WorldMap;

namespace UserProfile
{
	public class OpenUserProfileButton : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			InitUserProfilePopup();
		}

		private void InitUserProfilePopup()
		{
			MonoSingleton<UIRootHandler>.Instance.userProfilePopupController.Init();
		}
	}
}
