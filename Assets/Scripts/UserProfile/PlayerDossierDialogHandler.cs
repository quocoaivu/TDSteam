using System;
using Data;
using Gameplay;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class PlayerDossierDialogHandler : GameplayDialogHandler
	{
        [Space]
        [Header("References")]
        [SerializeField]
        private SwitchHandler[] logInNOutButtons;

        [Space]
        [Header("Profile")]
        [SerializeField]
        private Text userDatabaseID;

        [SerializeField]
        private Text userName;

        [SerializeField]
        private Image userAvatar;

        [SerializeField]
        private Text userLeague;

        [SerializeField]
        private Text lastBackupTime;

        [Space]
        [Header("Progress")]
        [SerializeField]
        private Text worldUnlockProgress;

        [SerializeField]
        private Text mapUnlockProgress;

        [SerializeField]
        private Text dailyTrialProgress;

        [SerializeField]
        private Text heroOwnedProgress;

        [Space]
        [Header("Confirm Backup/Restore")]
        [SerializeField]
        private BackupRestoreConfirmPopup confirmPopup;

        [Space]
        [Header("User name")]
        [SerializeField]
        private ChangeTagDialogHandler changeNamePopupController;

        [Space]
        [Header("User Region")]
        [SerializeField]
        private ChangeSectorDialogHandler changeRegionPopupController;

        [SerializeField]
        private ChangeSectorSwitchHandler changeRegionButtonController;
        
		public BackupRestoreConfirmPopup ConfirmPopup
		{
			get
			{
				return confirmPopup;
			}
			set
			{
				confirmPopup = value;
			}
		}

		public ChangeTagDialogHandler ChangeNamePopupController
		{
			get
			{
				return changeNamePopupController;
			}
			set
			{
				changeNamePopupController = value;
			}
		}

		public ChangeSectorDialogHandler ChangeRegionPopupController
		{
			get
			{
				return changeRegionPopupController;
			}
			set
			{
				changeRegionPopupController = value;
			}
		}

		public ChangeSectorSwitchHandler ChangeRegionButtonController
		{
			get
			{
				return changeRegionButtonController;
			}
			set
			{
				changeRegionButtonController = value;
			}
		}

		private void Awake()
		{
			NativeSpecificServicesSource.Services.UserProfile.OnLogStatusChangeEvent += UserProfile_OnLogStatusChangeEvent;
			NativeSpecificServicesSource.Services.DataCloudSaver.OnDataRestoreCompletedEvent += DataCloudSaver_OnDataRestoreCompletedEvent;
			UserProfileStore.Instance.OnUserInforChangeEvent += Instance_OnUserInforChangeEvent;
		}

		private void OnDestroy()
		{
			NativeSpecificServicesSource.Services.UserProfile.OnLogStatusChangeEvent -= UserProfile_OnLogStatusChangeEvent;
			NativeSpecificServicesSource.Services.DataCloudSaver.OnDataRestoreCompletedEvent -= DataCloudSaver_OnDataRestoreCompletedEvent;
			if (UserProfileStore.Instance != null)
			{
				UserProfileStore.Instance.OnUserInforChangeEvent -= Instance_OnUserInforChangeEvent;
			}
		}

		private void UserProfile_OnLogStatusChangeEvent()
		{
			RefreshButtonsStatus();
			InitUserBasicInformation();
		}

		private void DataCloudSaver_OnDataRestoreCompletedEvent()
		{
			InitUserBasicInformation();
			InitUserProgress();
		}

		private void Instance_OnUserInforChangeEvent()
		{
			InitUserBasicInformation();
			InitUserProgress();
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			RefreshButtonsStatus();
			InitUserBasicInformation();
			InitUserProgress();
		}

		private void RefreshButtonsStatus()
		{
			foreach (SwitchHandler buttonController in logInNOutButtons)
			{
				buttonController.UpdateButtonStatus();
			}
		}

		private void InitUserBasicInformation()
		{
			userDatabaseID.text = UserProfileStore.Instance.GetUserID();
			userName.text = UserProfileStore.Instance.GetUserName();
			SetUserAvatar();
			ChangeRegionButtonController.UpdateImage();
			userLeague.text = UserProfileStore.Instance.GetLeagueName();
			lastBackupTime.text = UserProfileStore.Instance.GetLastTimeBackup();
		}

		private void InitUserProgress()
		{
			int themeIDUnlocked = ThemeStore.Instance.GetThemeIDUnlocked();
			int totalTheme = ThemeStore.Instance.GetTotalTheme();
			worldUnlockProgress.text = themeIDUnlocked + 1 + "/" + totalTheme;
			int mapIDPassed = MapProgressStore.Instance.GetMapIDPassed();
			int totalMap = MapProgressStore.Instance.GetTotalMap();
			mapUnlockProgress.text = mapIDPassed + 1 + "/" + totalMap;
			int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
			int num = 7;
			if (currentDayIndex + 1 == num)
			{
				dailyTrialProgress.text = num + "/" + num;
			}
			else
			{
				dailyTrialProgress.text = currentDayIndex + 1 + "/" + num;
			}
			int count = HeroStore.Instance.GetListHeroIDOwned().Count;
			int count2 = HeroParameterManager.Instance.GetListHeroID().Count;
			heroOwnedProgress.text = count + "/" + count2;
		}

		private void SetUserAvatar()
		{
			Sprite sprite = NativeSpecificServicesSource.Services.UserProfile.GetUserAvatar();
			if (sprite)
			{
				userAvatar.sprite = sprite;
			}
		}

		public void InitConfirmPopup(CloudRecordInteraction cloudDataInteraction)
		{
			ConfirmPopup.Init(cloudDataInteraction);
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
