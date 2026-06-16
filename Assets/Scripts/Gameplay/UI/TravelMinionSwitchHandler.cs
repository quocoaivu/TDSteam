using System;

namespace Gameplay
{
	public class TravelMinionSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<GameRecord>.Instance.RecordingPosition = true;
			InputFilterDirector.Instance.SetIsClickingUI();
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.Close();
		}
	}
}
