using System;
using Bootstrap;
using Data;
using Gameplay;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;

namespace FreeResources
{
	public class WatchAdvertSwitchHandler : FreeResources.FreeResourcesSwitchHandler
	{
        private int currentWatchAmount;

        public static bool changedVideoStatus;

        public override void InitData()
		{
			base.InitData();
			SetGemData();
			SetWatchCountData();
			SetDisplayByRemoteSetting();
		}

		private void SetGemData()
		{
			if (!oneTimeOnlyReward)
			{
				titleReceived.SetActive(false);
			}
			//gemAmount.text = "+ " + NativeSpecificServicesSource.Services.FacebookServices.GetFreeResources("reward_id_watch_ad").ToString();
		}

		private void SetWatchCountData()
		{
			currentWatchAmount = FreeResourcesStore.Instance.GetCurrentWatchAdsPerDay();
			if (currentWatchAmount > 0)
			{
				titleReceived.SetActive(false);
				gemAmount.gameObject.SetActive(true);
				HideCountdownTime();
			}
			else
			{
				titleReceived.SetActive(true);
				gemAmount.gameObject.SetActive(false);
				DisplayCountdownTime();
			}
		}

		public override void OnClick()
		{
            AdsSettings.showRewardedVideoCombination();
            AdsSettings.gems = 1;
   //         base.OnClick();
   //if (currentWatchAmount > 0)
   //{
   //	if (ClipPlayerDirector.Instance.CheckIfVideoExits())
   //	{
   //                 //ClipPlayerDirector.Instance.playVideoGameplay_ForGem();
   //             }
   //	else
   //	{
   //		string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(19);
   //		SingletonMonoBehaviour<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
   //	}
   //}
   //else
   //{
   //	UnityEngine.Debug.Log("Da het luot xem video trong ngay!");
   //}
        }

		private void DisplayCountdownTime()
		{
			timeCountDown.gameObject.SetActive(true);
		}

		private void HideCountdownTime()
		{
			timeCountDown.gameObject.SetActive(false);
		}

		private void SetDisplayByRemoteSetting()
		{
			if (visualDependOnRemoteSetting)
			{
				if (Bootstrap.GameBootstrap.Instance.RemoteConfig.IsDisplayFreeGem())
				{
					gemAmount.gameObject.SetActive(true);
					notification.gameObject.SetActive(true);
					icon.sprite = sprite_gem_chest;
					icon.SetNativeSize();
				}
				else
				{
					gemAmount.gameObject.SetActive(false);
					notification.gameObject.SetActive(false);
					icon.sprite = sprite_normal;
					titleReceived.SetActive(false);
				}
			}
		}

		private void Update()
		{
			if (!WatchAdvertSwitchHandler.changedVideoStatus && MonoSingleton<GameRecord>.Instance.PlayedVideoGems)
			{
				WatchAdvertSwitchHandler.changedVideoStatus = true;
				currentWatchAmount--;
				FreeResourcesStore.Instance.SetCurrentWatchAdsPerDay(currentWatchAmount);
				if (currentWatchAmount == 0)
				{
					DisplayCountdownTime();
				}
				InitData();
			}
		}
	}
}
