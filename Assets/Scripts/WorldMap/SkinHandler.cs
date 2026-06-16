using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace WorldMap
{
	public class SkinHandler : MonoBehaviour
	{
        [SerializeField]
        [FormerlySerializedAs("mapButtonsController")]
        private MapButtonGroupController mapButtonGroupController;

        [Space]
        [SerializeField]
        private SkinStatueHandler themeStatueController;

        [Space]
        [SerializeField]
        private UnlockSkinSwitchHandler unlockThemeButtonController;

        [Space]
        [SerializeField]

        private int themeID;
        public void Init()
		{
			Show();
			RefreshUnlockThemeButton();
		}

		public void RefreshUnlockThemeButton()
		{
			bool flag = ThemeStore.Instance.IsNextThemeUnlock(themeID);
			bool flag2 = ThemeStore.Instance.IsReachMaxTheme(themeID);
			if (flag)
			{
				unlockThemeButtonController.Hide();
			}
			else
			{
				unlockThemeButtonController.Show();
			}
			if (flag2)
			{
				unlockThemeButtonController.Hide();
			}
		}

		private void RefreshStatue()
		{
			themeStatueController.InitStatue();
		}

		public void RefreshMapButtons(int themeID)
		{
			mapButtonGroupController.RefreshListMapButtons(themeID);
		}

		private void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}
	}
}
