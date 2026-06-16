using System;
using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class AdvertEditor : MonoBehaviour, IAdvert
	{
		public bool IsOfferVideoAvailable
		{
			get
			{
				return true;
			}
		}

		public void RequestAds()
		{
			UnityEngine.Debug.Log("Editor Request all ads services!");
		}

		public void ShowInterstitial()
		{
			UnityEngine.Debug.Log("Show Editor Ads Interstitial Success!");
			NativeSpecificServicesSource.Services.Analytics.SendEvent_WatchAds("FB/Admob Ads");
		}

		public void ShowOfferVideo(OfferVideoCallback offerVideoCallback)
		{
			if (offerVideoCallback != null)
			{
				offerVideoCallback(true);
				NativeSpecificServicesSource.Services.Analytics.SendEvent_WatchAds("Unity Ads");
				NativeSpecificServicesSource.Services.Analytics.SendEvent_WatchedVideoReward();
			}
		}
	}
}
