using System;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class GameplayHintOverviewDialog : GameplayDialogHandler
	{
        [Space]
        [SerializeField]
        private Image imageAvatar;

        [SerializeField]
        private Text tipName;

        [SerializeField]
        private Text tipDescription;

        private int tipID;

        public void Init(int tipID)
		{
			this.tipID = tipID;
			base.OpenWithScaleAnimation();
			GameplayDirector.Instance.gameSpeedController.PauseGame();
			imageAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("NewTip/avatar_tip_{0}", tipID));
			imageAvatar.SetNativeSize();
			tipName.text = Singleton<GameplayTipsSynopsis>.Instance.GetName(tipID);
			tipDescription.text = Singleton<GameplayTipsSynopsis>.Instance.GetDescription(tipID).Replace('@', '\n').Replace('#', '-');
			SendEventOpenPopup();
		}

		private void SendEventOpenPopup()
		{
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_OpenTipsPopup("New Tips", tipID);
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
		}
	}
}
