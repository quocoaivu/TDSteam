using System;
using Data;

namespace Services.PlatformSpecific
{
	public class RecordRestoreDeliver
	{
		public void DispatchToAllDataWriter(RecordRestoreDeliver data)
		{
			HeroStore.Instance.RestoreDataFromCloud(data.userData_Hero);
			GlobalUpgradeStore.Instance.RestoreDataFromCloud(data.userData_GlobalUpgrade);
			MapProgressStore.Instance.RestoreDataFromCloud(data.userData_Map);
			ThemeStore.Instance.RestoreDataFromCloud(data.userData_Theme);
			PowerUpItemStore.Instance.RestoreDataFromCloud(data.userData_PowerupItem);
			UserProfileStore.Instance.RestoreDataFromCloud(data.userData_UserProfile);
			PlayerCurrencyStore.Instance.RestoreDataFromCloud(data.userData_UserProfile);
			TutorialStore.Instance.RestoreDataFromCloud(data.userData_Tutorial);
			DailyTrialStore.Instance.RestoreDataFromCloud(data.userData_DailyTrial);
			OffersStore.Instance.RestoreDataFromCloud(data.userData_Offer);
			FreeResourcesStore.Instance.RestoreDataFromCloud(data.userData_FreeResources);
			SaleBundleStore.Instance.RestoreDataFromCloud(data.userData_SaleBundle);
			DailyRewardStore.Instance.RestoreDataFromCloud(data.userData_DailyReward);
		}

		public PlayerRecord_Hero userData_Hero;

		public PlayerRecord_GlobalEnhance userData_GlobalUpgrade;

		public PlayerRecord_Zone userData_Map;

		public PlayerRecord_Skin userData_Theme;

		public PlayerRecord_PowerupItem userData_PowerupItem;

		public PlayerRecord_PlayerDossier userData_UserProfile;

		public PlayerRecord_Tutorial userData_Tutorial;

		public PlayerRecord_DailyOrdeal userData_DailyTrial;

		public PlayerRecord_Deal userData_Offer;

		public PlayerRecord_FreeResources userData_FreeResources;

		public PlayerRecord_SalePack userData_SaleBundle;

		public PlayerRecord_DailyPrize userData_DailyReward;
	}
}
