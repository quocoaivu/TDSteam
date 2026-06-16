using System.Collections.Generic;
using Data;
using UnityEngine;

namespace WorldMap
{
	public class SelectSkinSwitchClusterHandler : MonoBehaviour
	{
        [SerializeField]
        private GameObject selectedImage;

        [Space]
        [SerializeField]

        private List<SelectSkinSwitchHandler> listSelectThemeButtons = new List<SelectSkinSwitchHandler>();
        public void RefreshListThemesButton()
		{
			int themeIDUnlocked = ThemeStore.Instance.GetThemeIDUnlocked();
			UnlockToTheme(themeIDUnlocked);
		}

		public void SetSelectedImage()
		{
			base.Invoke("DoSelect", 0.2f);
		}

		private void DoSelect()
		{
			int lastThemeIDPlayed = ThemeStore.Instance.GetLastThemeIDPlayed();
			selectedImage.transform.position = listSelectThemeButtons[lastThemeIDPlayed].transform.position;
		}

		private void UnlockToTheme(int currentThemeIDUnlocked)
		{
			for (int i = 0; i < listSelectThemeButtons.Count; i++)
			{
				if (i <= currentThemeIDUnlocked)
				{
					listSelectThemeButtons[i].ViewUnlock();
				}
				else
				{
					listSelectThemeButtons[i].ViewLock();
				}
			}
		}
	}
}
