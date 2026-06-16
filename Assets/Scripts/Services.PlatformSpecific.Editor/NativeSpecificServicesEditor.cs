using System;
using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class NativeSpecificServicesEditor : MonoBehaviour, INativeSpecificServices
	{
		public IMetrics Analytics
		{
			get
			{
				return analyticsEditor;
			}
		}

		public IInappBilling InApPurchase
		{
			get
			{
				return inappPurchaseEditor;
			}
		}

		public ISocialServices FacebookServices
		{
			get
			{
				return facebookServicesEditor;
			}
		}

		public IAdvert Ad
		{
			get
			{
				return adEditor;
			}
		}

		public IPlayerDossier UserProfile
		{
			get
			{
				return userProfileEditor;
			}
		}

		public IRecordCloudSaver DataCloudSaver
		{
			get
			{
				return dataCloudSaverEditor;
			}
		}

		public IAlert GameNotification
		{
			get
			{
				return gameNotificationEditor;
			}
		}

		public string StoreLink
		{
			get
			{
				return "market://details?id=com.zonmob.HeroLegend.KingdomDefense.TowerGame";
			}
		}

		public void Awake()
		{
			if (NativeSpecificServicesSource.Services != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			NativeSpecificServicesSource.Services = this;
		}

		[SerializeField]
		private MetricsEditor analyticsEditor;

		[SerializeField]
		private InappBillingEditor inappPurchaseEditor;

		[SerializeField]
		private SocialServicesEditor facebookServicesEditor;

		[SerializeField]
		private AdvertEditor adEditor;

		[SerializeField]
		private PlayerDossierEditor userProfileEditor;

		[SerializeField]
		private RecordCloudSaverEditor dataCloudSaverEditor;

		[SerializeField]
		private AlertEditor gameNotificationEditor;
	}
}
