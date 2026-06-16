using System;
using Data;
using LifetimePopup;
using UnityEngine;
using UnityEngine.Serialization;

namespace Services.PlatformSpecific.Editor
{
	public class SocialServicesEditor : MonoBehaviour, ISocialServices
	{
		public FacebookServicesRewardProvider FacebookServicesRewardProvider
		{
			get
			{
				return facebookServicesRewardProvider;
			}
			set
			{
				facebookServicesRewardProvider = value;
			}
		}

		public int GetFreeResources(string rewardID)
		{
			return FacebookServicesRewardProvider.GetRewardAmount_Gem(rewardID);
		}

		public void InviteFriend()
		{
			int rewardAmount_Gem = FacebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_invite_friend");
			PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
			int num = FreeResourcesStore.Instance.GetCurrentGemCollectedByInvite();
			num += rewardAmount_Gem;
			FreeResourcesStore.Instance.SetCurrentGemCollectedByInvite(num);
			UnityEngine.Debug.Log("Test FB Invite Friend Success + reward: " + rewardAmount_Gem);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = rewardAmount_Gem,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			SendEvent_GetFreeResourcesComplete(FreeResourcesKind.InviteFriend);
		}

		public void InviteToGroup()
		{
			if (!FreeResourcesStore.Instance.IsUserGetReward_JoinGroup())
			{
				int rewardAmount_Gem = FacebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_join_group");
				PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
				UnityEngine.Debug.Log("Test FB Invite Group Success + reward:" + rewardAmount_Gem);
				FreeResourcesStore.Instance.SetOneTimeRewardStatus("one_time_join_group");
				PrizeItem[] listData = new PrizeItem[]
				{
					new PrizeItem
					{
						rewardType = PrizeKind.Gem,
						value = rewardAmount_Gem,
						isDisplayQuantity = true
					}
				};
				MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
				SendEvent_GetFreeResourcesComplete(FreeResourcesKind.JoinGroup);
			}
		}

		public void LikeFanpage()
		{
			if (!FreeResourcesStore.Instance.IsUserGetReward_LikeFanpage())
			{
				int rewardAmount_Gem = FacebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_like_fanpage");
				PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
				UnityEngine.Debug.Log("Test FB Like Fanpage Success + reward:" + rewardAmount_Gem);
				FreeResourcesStore.Instance.SetOneTimeRewardStatus("one_time_like_fanpage");
				PrizeItem[] listData = new PrizeItem[]
				{
					new PrizeItem
					{
						rewardType = PrizeKind.Gem,
						value = rewardAmount_Gem,
						isDisplayQuantity = true
					}
				};
				MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
				SendEvent_GetFreeResourcesComplete(FreeResourcesKind.LikeFanpage);
			}
		}

		public void LogIn()
		{
			if (!FreeResourcesStore.Instance.IsUserGetReward_LogInFacebook())
			{
				int rewardAmount_Gem = FacebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_login");
				PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
				UnityEngine.Debug.Log("Test FB Login Success + reward:" + rewardAmount_Gem);
				FreeResourcesStore.Instance.SetOneTimeRewardStatus("one_time_login");
				PrizeItem[] listData = new PrizeItem[]
				{
					new PrizeItem
					{
						rewardType = PrizeKind.Gem,
						value = rewardAmount_Gem,
						isDisplayQuantity = true
					}
				};
				MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
				SendEvent_GetFreeResourcesComplete(FreeResourcesKind.Login);
			}
		}

		public void LogOut()
		{
			UnityEngine.Debug.Log("Test FB Logout Success ");
		}

		public void ShareFanpage()
		{
			int rewardAmount_Gem = FacebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_share_fanpage");
			PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
			UnityEngine.Debug.Log("Test FB Share Fanpage Success + reward: " + rewardAmount_Gem);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = rewardAmount_Gem,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			SendEvent_GetFreeResourcesComplete(FreeResourcesKind.ShareFanpage);
		}

		public void ShareLinkGame(StageTag sceneName, int currentMapID)
		{
			int rewardAmount_Gem = FacebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_share_link_game");
			PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
			UnityEngine.Debug.Log("Test FB Share Link Game Success + reward: " + rewardAmount_Gem);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = rewardAmount_Gem,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			NativeSpecificServicesSource.Services.Analytics.SendEvent_ShareLinkGameComplete(sceneName, currentMapID);
		}

		public void SharePromotionImage(int imageID)
		{
			int rewardAmount_Gem = FacebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_share_promotion_image");
			PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
			UnityEngine.Debug.Log("Test FB Share Promotion Image + reward: " + rewardAmount_Gem);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = rewardAmount_Gem,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
		}

		public void ShareScreenShot()
		{
			int rewardAmount_Gem = FacebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_share_screenshot");
			PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
			UnityEngine.Debug.Log("Test FB Shace Screenshot Success + reward: " + rewardAmount_Gem);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = rewardAmount_Gem,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
		}

		public void SendEvent_GetFreeResourcesComplete(FreeResourcesKind freeResourcesType)
		{
			int currentMapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked() + 1;
			NativeSpecificServicesSource.Services.Analytics.SendEvent_GetFreeResourcesComplete(freeResourcesType, currentMapIDUnlocked);
		}

		[SerializeField]
		[FormerlySerializedAs("readDataFacebookServicesReward")]
		private FacebookServicesRewardProvider facebookServicesRewardProvider;
	}
}
