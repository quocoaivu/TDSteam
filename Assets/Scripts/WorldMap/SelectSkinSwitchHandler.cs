using Data;
using UnityEngine;
using UnityEngine.UI;

namespace WorldMap
{
	public class SelectSkinSwitchHandler : SwitchHandler
	{

        [SerializeField]
        private SelectSkinSwitchClusterHandler selectThemeButtonGroupController;

        [SerializeField]
        private int themeID;


        private Button button;
        private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = base.GetComponent<Button>();
		}

		public void ViewUnlock()
		{
			button.interactable = true;
		}

		public void ViewLock()
		{
			button.interactable = false;
		}

		public override void OnClick()
		{
			base.OnClick();
			SelectTheme();
		}

		private void SelectTheme()
		{
			ThemeStore.Instance.SaveLastThemePlayed(themeID);
			MonoSingleton<UIRootHandler>.Instance.MapThemesController.SpawnTheme(themeID);
			selectThemeButtonGroupController.SetSelectedImage();
		}
	}
}
