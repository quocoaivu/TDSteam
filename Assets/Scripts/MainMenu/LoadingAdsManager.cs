using System;
using Bootstrap;
using Data;
using Services.PlatformSpecific;
using UnityEngine;

namespace MainMenu
{
	public class LoadingAdsManager
	{
		public static LoadingAdsManager Instance
		{
			get
			{
				if (LoadingAdsManager.instance == null)
				{
					LoadingAdsManager.instance = new LoadingAdsManager();
				}
				return LoadingAdsManager.instance;
			}
			set
			{
				LoadingAdsManager.instance = value;
			}
		}

		public void TryToShowInterstitialAds_Loading()
		{
			int num = Bootstrap.GameBootstrap.Instance.RemoteConfig.ChanceToShowInterAds_Loading();
			if (UnityEngine.Random.Range(0, 100) < num && PlayerSaveStore.Instance.GetPlayCount() >= 2)
			{
                //NativeSpecificServicesSource.Services.Ad.ShowInterstitial();
                AdsSettings.showInterstitialCombination();
                UnityEngine.Debug.Log("Show Interstitial Ads");
			}
		}

		private static LoadingAdsManager instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
