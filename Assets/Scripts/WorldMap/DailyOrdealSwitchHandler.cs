namespace WorldMap
{
	public class DailyOrdealSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenDailyTrialPanel();
		}

		private void OpenDailyTrialPanel()
		{
			MonoSingleton<UIRootHandler>.Instance.dailyTrialPopupController.Init();
		}
	}
}
