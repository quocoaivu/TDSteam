using System;

namespace LifetimePopup
{
	public class FreeResourcesSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenFacebookServicesPopup();
		}

		private void OpenFacebookServicesPopup()
		{
			MonoSingleton<LifespanSurface>.Instance.FreeResourcesPopupController.Init();
		}
	}
}
