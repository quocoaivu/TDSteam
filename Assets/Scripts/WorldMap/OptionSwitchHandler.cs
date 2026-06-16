namespace WorldMap
{
	public class OptionSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<UIRootHandler>.Instance.settingPopupController.Init();
		}
	}
}
