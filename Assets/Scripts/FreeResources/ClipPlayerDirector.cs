using System;
using Data;
using Gameplay;
using LifetimePopup;
using Services.PlatformSpecific;
using UnityEngine;

namespace FreeResources
{
	public class ClipPlayerDirector
	{
        private static ClipPlayerDirector instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }

        public static ClipPlayerDirector Instance
		{
			get
			{
				if (ClipPlayerDirector.instance == null)
				{
					ClipPlayerDirector.instance = new ClipPlayerDirector();
				}
				return ClipPlayerDirector.instance;
			}
			set
			{
				ClipPlayerDirector.instance = value;
			}
		}

		public bool CheckIfVideoExits()
		{
            return NativeSpecificServicesSource.Services.Ad.IsOfferVideoAvailable;
        }

        public void PlayRewardVideo(OfferVideoCallback onCloseRewardVideoCallback)
		{
			//NativeSpecificServicesSource.Services.Ad.ShowOfferVideo(onCloseRewardVideoCallback);
		}

		public void playVideoGameplay_ForGem()
		{
			//NativeSpecificServicesSource.Services.Ad.ShowOfferVideo(new OfferVideoCallback(OfferVideoGameplayCallback_Gem));
		}

		private void OfferVideoGameplayCallback_Gem(bool completed)
		{
			if (completed)
			{
				MonoSingleton<GameRecord>.Instance.PlayedVideoGems = true;
				GetReward();
			}
		}

		private void GetReward()
		{
            int freeResources = NativeSpecificServicesSource.Services.FacebookServices.GetFreeResources("reward_id_watch_ad");
            PlayerCurrencyStore.Instance.ChangeGem(freeResources, true);
			UnityEngine.Debug.Log("Get reward video: Gem + " + freeResources);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = freeResources,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			//NativeSpecificServicesSource.Services.FacebookServices.SendEvent_GetFreeResourcesComplete(FreeResourcesKind.WatchAds);
		}
	}
}
