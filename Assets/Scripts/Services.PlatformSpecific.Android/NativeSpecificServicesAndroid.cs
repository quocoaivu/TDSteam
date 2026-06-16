using System;
using UnityEngine;

//namespace Services.PlatformSpecific.Android
//{
//	public class NativeSpecificServicesAndroid : MonoBehaviour, INativeSpecificServices
//	{
//		public IMetrics Analytics
//		{
//			get
//			{
//				return analyticsAndroid;
//			}
//		}

//        public IInappBilling InApPurchase
//        {
//            get
//            {
//                return inappPurchaseAndroid;
//            }
//        }

//        public ISocialServices FacebookServices
//		{
//			get
//			{
//				return facebookServicesAndroid;
//			}
//		}

//		public IAdvert Ad
//		{
//			get
//			{
//				return adAndroid;
//			}
//		}

//		public IPlayerDossier UserProfile
//		{
//			get
//			{
//				return userProfileAndroid;
//			}
//		}

//		public IRecordCloudSaver DataCloudSaver
//		{
//			get
//			{
//				return dataCloudSaverAndroid;
//			}
//		}

//		public IAlert GameNotification
//		{
//			get
//			{
//				return notificationAndroid;
//			}
//		}

//		public string StoreLink
//		{
//			get
//			{
//				return "market://details?id=com.zonmob.HeroLegend.KingdomDefense.TowerGame";
//			}
//		}

//		private void Awake()
//		{
//			if (NativeSpecificServicesSource.Services != null)
//			{
//				UnityEngine.Object.Destroy(base.gameObject);
//				return;
//			}
//			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
//			NativeSpecificServicesSource.Services = this;
//		}

//		[SerializeField]
//		private MetricsAndroid analyticsAndroid;

//		//[SerializeField]
//		//private InappBillingAndroid inappPurchaseAndroid;

//		[SerializeField]
//		private SocialServicesAndroid facebookServicesAndroid;

//		[SerializeField]
//		private AdvertAndroid adAndroid;

//		[SerializeField]
//		private PlayerDossierAndroid userProfileAndroid;

//		[SerializeField]
//		private RecordCloudSaverAndroid dataCloudSaverAndroid;

//		[SerializeField]
//		private AlertAndroid notificationAndroid;
//	}
//}
