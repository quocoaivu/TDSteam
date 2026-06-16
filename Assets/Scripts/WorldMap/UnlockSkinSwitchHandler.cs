using UnityEngine;

namespace WorldMap
{
	public class UnlockSkinSwitchHandler : SwitchHandler
	{
        [SerializeField]
        private int themeIDToUnlock;


        public override void OnClick()
		{
			base.OnClick();
			InitUnlockThemePanel();
		}

		private void InitUnlockThemePanel()
		{
			MonoSingleton<UIRootHandler>.Instance.unlockThemePopupController.Init(themeIDToUnlock);
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}
	}
}
