using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class GameplayClipSwitchHandler : SwitchHandler
	{
        [SerializeField]
        private Image[] listImages;

        [SerializeField]
        private Button[] listButtons;

        [SerializeField]
        private Sprite icon_normal;

        [SerializeField]
        private Sprite icon_lock;

        [Space]
        [SerializeField]
        private FreeResourcesDialogHandler freeResourcesPopupController;

        [SerializeField]
        private string rewardID;

        [SerializeField]
        private Text rewardValueText;

        private bool isPlayed;

        public bool IsPlayed
		{
			get
			{
				return isPlayed;
			}
			set
			{
				isPlayed = value;
			}
		}

		private void Start()
		{
			rewardValueText.text = freeResourcesPopupController.AdRewardProvider.GetRewardValue(rewardID).ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			RefreshStatus();
		}

		public void RefreshStatus()
		{
			if (isPlayed)
			{
				foreach (Image image in listImages)
				{
					image.color = Color.gray;
				}
				foreach (Button button in listButtons)
				{
					button.interactable = false;
				}
			}
			else
			{
				foreach (Image image2 in listImages)
				{
					image2.color = Color.white;
				}
				foreach (Button button2 in listButtons)
				{
					button2.interactable = true;
				}
			}
			MonoSingleton<UIRootHandler>.Instance.RefreshStatusFreeResourcesButton();
		}
	}
}
