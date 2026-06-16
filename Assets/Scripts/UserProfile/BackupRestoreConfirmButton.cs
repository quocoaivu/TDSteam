using System;
using Data;
using Services.PlatformSpecific;
using WorldMap;

namespace UserProfile
{
	public class BackupRestoreConfirmButton : SwitchHandler
	{
        private CloudRecordInteraction cloudDataInteraction;

        public void Init(CloudRecordInteraction cloudDataInteraction)
		{
			this.cloudDataInteraction = cloudDataInteraction;
		}

		public override void OnClick()
		{
			base.OnClick();
			Confirm();
		}

		private void Confirm()
		{
			CloudRecordInteraction cloudDataInteraction = this.cloudDataInteraction;
			if (cloudDataInteraction != CloudRecordInteraction.Backup)
			{
				if (cloudDataInteraction == CloudRecordInteraction.Restore)
				{
					//NativeSpecificServicesSource.Services.UserProfile.RestoreData();
				}
			}
			else
			{
				//NativeSpecificServicesSource.Services.UserProfile.BackupData();
				UserProfileStore.Instance.SaveLastTimeBackup();
			}
			MonoSingleton<UIRootHandler>.Instance.userProfilePopupController.ConfirmPopup.CloseWithScaleAnimation();
		}
	}
}
