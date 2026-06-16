namespace WorldMap
{
	public class HomeSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenSceneMainMenu();
		}

		private void OpenSceneMainMenu()
		{
			LoadingScreen.Instance.ShowLoading();
			base.Invoke("DoLoadSceneMainMenu", 1f);
		}

		private void DoLoadSceneMainMenu()
		{
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.MainMenuSceneName);
		}
	}
}
