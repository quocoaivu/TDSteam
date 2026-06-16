using System;
using Bootstrap;
using LifetimePopup;
using Services.PlatformSpecific;
using UnityEngine;

namespace Gameplay
{
	public class ClipPlayerDirector
	{
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

		public void playVideoGameplay_ForMoney()
		{
			//NativeSpecificServicesSource.Services.Ad.ShowOfferVideo(new OfferVideoCallback(OfferVideoGameplayCallback_Money));
			MonoSingleton<GameRecord>.Instance.IsPlayingVideoAds = true;
		}

		private void OfferVideoGameplayCallback_Money(bool completed)
		{
			if (completed)
			{
				GetGameplayVideoReward_Money();
			}
			MonoSingleton<GameRecord>.Instance.IsPlayingVideoAds = false;
		}

		private void GetGameplayVideoReward_Money()
		{
			int rewardValue = MonoSingleton<UIRootHandler>.Instance.freeResourcesPopupController.AdRewardProvider.GetRewardValue("unity_vd_rd_money");
			MonoSingleton<GameRecord>.Instance.IncreaseMoney(rewardValue);
			UnityEngine.Debug.Log("Get reward video: Money + " + rewardValue);
			MonoSingleton<GameRecord>.Instance.PlayedGameplayVideo_ForMoney = true;
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Money,
					value = rewardValue,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			SendEvent_GetRewardMoneyComplete();
		}

		private void SendEvent_GetRewardMoneyComplete()
		{
			int currentMapID = MonoSingleton<GameRecord>.Instance.MapID + 1;
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_WatchGameplayVideoRewardComplete(currentMapID, "Reward: Money");
		}

		public void playVideoGameplay_ForLife()
		{
			//NativeSpecificServicesSource.Services.Ad.ShowOfferVideo(new OfferVideoCallback(OfferVideoGameplayCallback_Life));
			MonoSingleton<GameRecord>.Instance.IsPlayingVideoAds = true;
		}

		private void OfferVideoGameplayCallback_Life(bool completed)
		{
			if (completed)
			{
				GetGameplayVideoReward_Life();
			}
			MonoSingleton<GameRecord>.Instance.IsPlayingVideoAds = false;
		}

		private void GetGameplayVideoReward_Life()
		{
			int rewardValue = MonoSingleton<UIRootHandler>.Instance.freeResourcesPopupController.AdRewardProvider.GetRewardValue("unity_vd_rd_life");
			GameplayDirector.Instance.gameLogicController.IncreaseHealth(rewardValue);
			MonoSingleton<GameRecord>.Instance.PlayedGameplayVideo_ForLife = true;
			UnityEngine.Debug.Log("Get reward video: Life + " + rewardValue);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Life,
					value = rewardValue,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			SendEvent_GetRewardLifeComplete();
		}

		private void SendEvent_GetRewardLifeComplete()
		{
			int currentMapID = MonoSingleton<GameRecord>.Instance.MapID + 1;
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_WatchGameplayVideoRewardComplete(currentMapID, "Reward: Life");
		}

		public void playVideoGameplay_ForOpenChestOffer()
		{
			//NativeSpecificServicesSource.Services.Ad.ShowOfferVideo(new OfferVideoCallback(OfferVideoGameplayCallback_OpenChestOffer));
			MonoSingleton<GameRecord>.Instance.IsPlayingVideoAds = true;
		}

		private void OfferVideoGameplayCallback_OpenChestOffer(bool completed)
		{
			if (completed)
			{
				GetGameplayVideoReward_OpenChestOffer();
				MonoSingleton<GameRecord>.Instance.PlayedVideoLucky = true;
			}
			MonoSingleton<GameRecord>.Instance.IsPlayingVideoAds = false;
		}

		private void GetGameplayVideoReward_OpenChestOffer()
		{
			UnityEngine.Debug.Log("Get reward video: Open Chest Offer + " + 3);
			MonoSingleton<GameRecord>.Instance.ChangeOpenChestTurn(3);
			MonoSingleton<GameRecord>.Instance.ChangeOpenChestOffer();
		}

		public void playVideoEndGame()
		{
			//NativeSpecificServicesSource.Services.Ad.ShowOfferVideo(new OfferVideoCallback(OfferVideoEndGameCallback));
			MonoSingleton<GameRecord>.Instance.IsPlayingVideoAds = true;
		}

		private void OfferVideoEndGameCallback(bool completed)
		{
			MonoSingleton<GameRecord>.Instance.PlayedVideoEndGame = completed;
			UnityEngine.Debug.Log("play video end game = " + completed);
			if (completed)
			{
				GameplayDirector.Instance.GetEndingVideoReward();
				MonoSingleton<GameRecord>.Instance.PlayedVideoEndGame = true;
				GameplayDirector.Instance.gameSpeedController.UnPauseGame();
			}
			MonoSingleton<GameRecord>.Instance.IsPlayingVideoAds = false;
		}

		public void TryToShowInterstitialAds_EndGame()
		{
			int num = Bootstrap.GameBootstrap.Instance.RemoteConfig.ChanceToShowInterAds_EndGame();
			if (UnityEngine.Random.Range(0, 100) < num && MonoSingleton<GameRecord>.Instance.MapID >= 2)
			{
				//NativeSpecificServicesSource.Services.Ad.ShowInterstitial();
				UnityEngine.Debug.Log("Show Interstitial Ads");
			}
		}

		private static ClipPlayerDirector instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
