using Data;
using UnityEngine;

namespace WorldMap
{
	public class ZoneThemesHandler : MonoBehaviour
	{

        [Space]
        [Header("Select Themes Button Group")]
        [SerializeField]
        private SelectSkinSwitchClusterHandler selectThemeButtonGroupController;

        [Space]
        [Header("Prefab Themes")]
        [SerializeField]
        private GameObject prefab_theme0;

        [SerializeField]
        private GameObject prefab_theme1;

        [SerializeField]
        private GameObject prefab_theme2;

        [Space]
        [SerializeField]
        private Transform themeHolder;

        private bool theme0Exist;

        private SkinHandler _theme0Controller;

        private bool theme1Exist;

        private SkinHandler _theme1Controller;

        private bool theme2Exist;

        private SkinHandler _theme2Controller;

        private T GetOrCreateTheme<T>(GameObject prefab, ref T cache, ref bool exists) where T : Component
		{
			if (cache == null)
			{
				GameObject instance = UnityEngine.Object.Instantiate<GameObject>(prefab);
				instance.transform.SetParent(themeHolder);
				instance.transform.localPosition = Vector3.zero;
				instance.transform.localScale = Vector3.one;
				cache = instance.GetComponent<T>();
				exists = true;
			}
			return cache;
		}

		public SkinHandler theme0Controller
		{
			get
			{
				return GetOrCreateTheme(prefab_theme0, ref _theme0Controller, ref theme0Exist);
			}
		}

		public SkinHandler theme1Controller
		{
			get
			{
				return GetOrCreateTheme(prefab_theme1, ref _theme1Controller, ref theme1Exist);
			}
		}

		public SkinHandler theme2Controller
		{
			get
			{
				return GetOrCreateTheme(prefab_theme2, ref _theme2Controller, ref theme2Exist);
			}
		}

		private void Start()
		{
			SpawnDefaultTheme();
			RefreshListMapButtons();
		}

		private void SpawnDefaultTheme()
		{
			int lastThemeIDPlayed = ThemeStore.Instance.GetLastThemeIDPlayed();
			SpawnTheme(lastThemeIDPlayed);
			RefreshSelectThemesButtonStatus();
		}

		public void SpawnTheme(int themeID)
		{
			HideAll();
			if (themeID != 0)
			{
				if (themeID != 1)
				{
					if (themeID == 2)
					{
						SkinHandler theme2Controller = this.theme2Controller;
						theme2Controller.Init();
					}
				}
				else
				{
					SkinHandler theme1Controller = this.theme1Controller;
					theme1Controller.Init();
				}
			}
			else
			{
				SkinHandler theme0Controller = this.theme0Controller;
				theme0Controller.Init();
			}
			RefreshListMapButtons();
		}

		private void HideAll()
		{
			if (theme0Exist && theme0Controller)
			{
				theme0Controller.Hide();
			}
			if (theme1Exist && theme1Controller)
			{
				theme1Controller.Hide();
			}
			if (theme2Exist && theme2Controller)
			{
				theme2Controller.Hide();
			}
		}

		public void RefreshUnlockThemesStatus()
		{
			if (theme0Exist && theme0Controller)
			{
				theme0Controller.RefreshUnlockThemeButton();
			}
			if (theme1Exist && theme1Controller)
			{
				theme1Controller.RefreshUnlockThemeButton();
			}
			if (theme2Exist && theme2Controller)
			{
				theme2Controller.RefreshUnlockThemeButton();
			}
		}

		public void RefreshSelectThemesButtonStatus()
		{
			selectThemeButtonGroupController.RefreshListThemesButton();
			selectThemeButtonGroupController.SetSelectedImage();
		}

		private void RefreshListMapButtons()
		{
			if (theme0Exist && theme0Controller)
			{
				theme0Controller.RefreshMapButtons(0);
			}
			if (theme1Exist && theme1Controller)
			{
				theme1Controller.RefreshMapButtons(1);
			}
			if (theme2Exist && theme2Controller)
			{
				theme2Controller.RefreshMapButtons(2);
			}
		}

	}
}
