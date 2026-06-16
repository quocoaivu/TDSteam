using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bootstrap;
using Data;
//using Facebook.Unity;
using LifetimePopup;
using GameCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace Services.PlatformSpecific.Android
{
	public class SocialServicesAndroid : MonoBehaviour, ISocialServices
	{
		public int GetFreeResources(string rewardID)
		{
			return facebookServicesRewardProvider.GetRewardAmount_Gem(rewardID);
		}

		public void LogIn()
		{
			//if (!FB.IsLoggedIn)
			//{
			//	FB.LogInWithReadPermissions(listPermissionLogin, new FacebookDelegate<ILoginResult>(LogIn0CallBack));
			//}
			//else
			//{
			//	GetRewardLogin();
			//}
		}

		//private void LogIn0CallBack(ILoginResult result)
		//{
		//	UnityEngine.Debug.Log(" Result callback login with read permission =  " + result.ToString());
		//	if (FB.IsLoggedIn)
		//	{
		//		GetRewardLogin();
		//	}
		//	else
		//	{
		//		UnityEngine.Debug.Log("User cancelled read permission login");
		//	}
		//}

		private void GetRewardLogin()
		{
			if (!FreeResourcesStore.Instance.IsUserGetReward_LogInFacebook())
			{
				int rewardAmount_Gem = facebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_login");
				PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
				UnityEngine.Debug.Log("FB Login Success + reward:" + rewardAmount_Gem);
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

		public void InviteFriend()
		{
			//FB.AppRequest("Let's play Kingdom Defense!", null, null, null, null, null, string.Empty, new FacebookDelegate<IAppRequestResult>(InviteFriendCallback));
		}

		public void InviteToGroup()
		{
			if (GameUtils.IsInternetConnectionAvailable())
			{
				if (GameUtils.CheckPackageAppIsPresent("com.facebook.katana"))
				{
					Application.OpenURL(MarketingSetup.fbGroupLinkApp);
				}
				else
				{
					Application.OpenURL(MarketingSetup.fbGroupLinkWeb);
				}
				GetRewardJoinGroup();
			}
			else
			{
				UnityEngine.Debug.Log("No Internet Connection!");
			}
		}

		private void GetRewardJoinGroup()
		{
			if (!Bootstrap.GameBootstrap.Instance.RemoteConfig.IsDisplayFreeGem())
			{
				return;
			}
			if (!FreeResourcesStore.Instance.IsUserGetReward_JoinGroup())
			{
				int rewardAmount_Gem = facebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_join_group");
				PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
				UnityEngine.Debug.Log("FB Invite Group Success + reward:" + rewardAmount_Gem);
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
			else
			{
				UnityEngine.Debug.Log("ÄÃ£ nháº­n thÆ°á»Ÿng join group!");
			}
		}

		public void LikeFanpage()
		{
			if (GameUtils.IsInternetConnectionAvailable())
			{
				if (GameUtils.CheckPackageAppIsPresent("com.facebook.katana"))
				{
					Application.OpenURL(MarketingSetup.fbFanpageLinkApp);
				}
				else
				{
					Application.OpenURL(MarketingSetup.fbFanpageLinkWeb);
				}
				GetRewardLikeFanpage();
			}
			else
			{
				UnityEngine.Debug.Log("No Internet Connection!");
			}
		}

		private void GetRewardLikeFanpage()
		{
			if (!Bootstrap.GameBootstrap.Instance.RemoteConfig.IsDisplayFreeGem())
			{
				return;
			}
			if (!FreeResourcesStore.Instance.IsUserGetReward_LikeFanpage())
			{
				int rewardAmount_Gem = facebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_like_fanpage");
				PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
				UnityEngine.Debug.Log("FB Like Fanpage Success + reward:" + rewardAmount_Gem);
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
			else
			{
				UnityEngine.Debug.Log("ÄÃ£ nháº­n thÆ°á»Ÿng like fanpage!");
			}
		}

		public void ShareFanpage()
		{
			//FB.ShareLink(new Uri(MarketingSetup.fbFanpageLinkWeb), string.Empty, string.Empty, null, new FacebookDelegate<IShareResult>(ShareFanpageCallBack));
		}

		//private void ShareFanpageCallBack(IShareResult result)
		//{
		//	if (!string.IsNullOrEmpty(result.Error))
		//	{
		//		UnityEngine.Debug.Log("Share fanpage Error: " + result.Error);
		//	}
		//	else if (result.Cancelled)
		//	{
		//		UnityEngine.Debug.Log("User cancelled Share fanpage!");
		//	}
		//	else
		//	{
		//		GetRewardShareFanpage();
		//	}
		//}

		private void GetRewardShareFanpage()
		{
			if (!Bootstrap.GameBootstrap.Instance.RemoteConfig.IsDisplayFreeGem())
			{
				return;
			}
			int rewardAmount_Gem = facebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_share_fanpage");
			PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
			UnityEngine.Debug.Log("ShareLink Fanpage Success + reward: " + rewardAmount_Gem);
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
			//this.sceneName = sceneName;
			//this.currentMapID = currentMapID;
			//FB.ShareLink(new Uri(MarketingSetup.rateGameLink), string.Empty, string.Empty, null, new FacebookDelegate<IShareResult>(ShareLinkGameCallback));
		}

		//private void ShareLinkGameCallback(IShareResult result)
		//{
		//	if (!string.IsNullOrEmpty(result.Error))
		//	{
		//		UnityEngine.Debug.Log("Share link game Error: " + result.Error);
		//	}
		//	else if (result.Cancelled)
		//	{
		//		UnityEngine.Debug.Log("User cancelled Share link game!");
		//	}
		//	else
		//	{
		//		GetRewardShareLinkGame();
		//	}
		//}

		private void GetRewardShareLinkGame()
		{
			if (!Bootstrap.GameBootstrap.Instance.RemoteConfig.IsDisplayFreeGem())
			{
				return;
			}
			int rewardAmount_Gem = facebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_share_link_game");
			PlayerCurrencyStore.Instance.ChangeGem(rewardAmount_Gem, true);
			UnityEngine.Debug.Log("ShareLink Game Success + reward: " + rewardAmount_Gem);
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
			//currentImageID = imageID;
			//if (!FB.IsLoggedIn)
			//{
			//	LogIn_Addition_Promotion();
			//}
			//else
			//{
			//	TryToSharePromotion();
			//}
		}

		private void TryToSharePromotion()
		{
			//if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
			//{
			//	base.StartCoroutine(ISharePromotionImage(currentImageID));
			//	UnityEngine.Debug.Log("Share promotion image with publish actions");
			//}
			//else
			//{
			//	ShareFanpage();
			//	UnityEngine.Debug.Log("no publish actions, share fanpage link");
			//}
		}

		private IEnumerator ISharePromotionImage(int imageID)
		{
			yield return new WaitForEndOfFrame();
			//Texture2D image = Resources.Load(string.Format("Publish/promotion_{0}", imageID), typeof(Texture2D)) as Texture2D;
			//byte[] promotionImg = image.EncodeToPNG();
			//WWWForm wwwForm = new WWWForm();
			//wwwForm.AddBinaryData("image", promotionImg, "Promotion.png");
			//FB.API("me/photos", HttpMethod.POST, new FacebookDelegate<IGraphResult>(SharePromotionImageCallback), wwwForm);
			yield break;
		}

		//private void SharePromotionImageCallback(IGraphResult result)
		//{
		//	if (!string.IsNullOrEmpty(result.Error))
		//	{
		//		UnityEngine.Debug.Log("Share Promotion Error: " + result.Error);
		//	}
		//	else if (result.Cancelled)
		//	{
		//		UnityEngine.Debug.Log("User cancelled share Promotion!");
		//	}
		//	else
		//	{
		//		GetRewardSharePromotion();
		//	}
		//}

		private void GetRewardSharePromotion()
		{
			int rewardAmount_Gem = facebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_share_promotion_image");
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

		public void LogIn_Addition_Promotion()
		{
			//if (!FB.IsLoggedIn)
			//{
			//	FB.LogInWithReadPermissions(listPermissionLogin, new FacebookDelegate<ILoginResult>(LogIn0CallBack_Addition_Promotion));
			//}
		}

		//private void LogIn0CallBack_Addition_Promotion(ILoginResult result)
		//{
		//	UnityEngine.Debug.Log(" Result callback login with read permission =  " + result.ToString());
		//	if (FB.IsLoggedIn)
		//	{
		//		TryToSharePromotion();
		//	}
		//	else
		//	{
		//		UnityEngine.Debug.Log("User cancelled read permission login");
		//	}
		//}

		public void ShareScreenShot()
		{
			//if (!FB.IsLoggedIn)
			//{
			//	LogIn_Addition_Screenshot();
			//}
			//else
			//{
			//	TryToShareScreenshot();
			//}
		}

		private void TryToShareScreenshot()
		{
			//if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
			//{
			//	base.StartCoroutine(TakeScreenshot());
			//	UnityEngine.Debug.Log("Share screenshot with publish actions");
			//}
			//else
			//{
			//	ShareFanpage();
			//	UnityEngine.Debug.Log("no publish actions, share fanpage link");
			//}
		}

		private IEnumerator TakeScreenshot()
		{
			yield return new WaitForEndOfFrame();
			//int width = Screen.width;
			//int height = Screen.height;
			//Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
			//tex.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
			//tex.Apply();
			//byte[] screenshot = tex.EncodeToPNG();
			//WWWForm wwwForm = new WWWForm();
			//wwwForm.AddBinaryData("image", screenshot, "Screenshot.png");
			//FB.API("me/photos", HttpMethod.POST, new FacebookDelegate<IGraphResult>(ShareScreenshotCallback), wwwForm);
			yield break;
		}

		//private void ShareScreenshotCallback(IGraphResult result)
		//{
		//	if (!string.IsNullOrEmpty(result.Error))
		//	{
		//		UnityEngine.Debug.Log("Share Screenshot Error: " + result.Error);
		//	}
		//	else if (result.Cancelled)
		//	{
		//		UnityEngine.Debug.Log("User cancelled Share Screenshot!");
		//	}
		//	else
		//	{
		//		GetRewardShareScreenshot();
		//	}
		//}

		private void GetRewardShareScreenshot()
		{
			int rewardAmount_Gem = facebookServicesRewardProvider.GetRewardAmount_Gem("reward_id_share_screenshot");
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

		public void LogIn_Addition_Screenshot()
		{
			//if (!FB.IsLoggedIn)
			//{
			//	FB.LogInWithReadPermissions(listPermissionLogin, new FacebookDelegate<ILoginResult>(LogIn0CallBack_Addition_Screenshot));
			//}
		}

		//private void LogIn0CallBack_Addition_Screenshot(ILoginResult result)
		//{
		//	UnityEngine.Debug.Log(" Result callback login with read permission =  " + result.ToString());
		//	if (FB.IsLoggedIn)
		//	{
		//		TryToShareScreenshot();
		//	}
		//	else
		//	{
		//		UnityEngine.Debug.Log("User cancelled read permission login");
		//	}
		//}

		public void SendEvent_GetFreeResourcesComplete(FreeResourcesKind freeResourcesType)
		{
			int currentMapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked() + 1;
			NativeSpecificServicesSource.Services.Analytics.SendEvent_GetFreeResourcesComplete(freeResourcesType, currentMapIDUnlocked);
		}

		private List<string> listPermissionLogin = new List<string>
		{
			"public_profile",
			"email",
			"user_friends"
		};

		[SerializeField]
		[FormerlySerializedAs("readDataFacebookServicesReward")]
		private FacebookServicesRewardProvider facebookServicesRewardProvider;

		private StageTag sceneName;

		private int currentMapID;

		private int currentImageID;
	}
}
