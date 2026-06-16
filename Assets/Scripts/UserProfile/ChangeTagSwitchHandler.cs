using System;
using WorldMap;

namespace UserProfile
{
	public class ChangeTagSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenPopupChangeName();
		}

		private void OpenPopupChangeName()
		{
			MonoSingleton<UIRootHandler>.Instance.userProfilePopupController.ChangeNamePopupController.Init();
		}
	}
}
