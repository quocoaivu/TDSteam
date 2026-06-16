using Data;
using UnityEngine;

namespace WorldMap
{
	public class SwitchSkinSwitchHandler : SwitchHandler
	{
        [SerializeField]
        private SelectSkinSwitchClusterHandler selectThemeButtonGroupController;

        [SerializeField]
        private bool isNext;

        [SerializeField]
        private bool isPrevious;

        private int currentThemeSelected;


        private int maxThemeIDUnlocked;
        public override void OnClick()
		{
			base.OnClick();
			currentThemeSelected = ThemeStore.Instance.GetLastThemeIDPlayed();
			maxThemeIDUnlocked = ThemeStore.Instance.GetThemeIDUnlocked();
			if (isNext)
			{
				currentThemeSelected++;
			}
			if (isPrevious)
			{
				currentThemeSelected--;
			}
			currentThemeSelected = Mathf.Clamp(currentThemeSelected, 0, maxThemeIDUnlocked);
			ThemeStore.Instance.SaveLastThemePlayed(currentThemeSelected);
			MonoSingleton<UIRootHandler>.Instance.MapThemesController.SpawnTheme(currentThemeSelected);
			selectThemeButtonGroupController.SetSelectedImage();
		}
	}
}
