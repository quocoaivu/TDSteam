using System;
using Data;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace UserProfile
{
	public class ChangeSectorSwitchHandler : SwitchHandler
	{
        [SerializeField]
        private Image image;

        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private int imageMaxHeight;

        public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<UIRootHandler>.Instance.userProfilePopupController.ChangeRegionPopupController.Init();
		}

		private void Start()
		{
			UpdateImage();
		}

		public void UpdateImage()
		{
			string userRegionCode = UserProfileStore.Instance.GetUserRegionCode();
			image.sprite = Common.AssetLoader.Load<Sprite>(string.Format("CountryFlags2/{0}", userRegionCode));
			image.SetNativeSize();
			float num = rectTransform.sizeDelta.x / rectTransform.sizeDelta.y;
			if (rectTransform.sizeDelta.y > (float)imageMaxHeight)
			{
				int num2 = imageMaxHeight;
				float x = (float)imageMaxHeight * num;
				rectTransform.sizeDelta = new Vector2(x, (float)num2);
			}
		}

	}
}
