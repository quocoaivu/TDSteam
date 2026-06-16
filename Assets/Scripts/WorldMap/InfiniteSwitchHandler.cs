namespace WorldMap
{
	public class InfiniteSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenEndlessPopup();
		}

		private void OpenEndlessPopup()
		{
			MonoSingleton<UIRootHandler>.Instance.endlessPopupController.Init();
		}
	}
}
