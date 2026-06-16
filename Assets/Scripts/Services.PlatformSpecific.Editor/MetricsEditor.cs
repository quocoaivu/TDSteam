using System;
using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class MetricsEditor : MonoBehaviour, IMetrics
	{
		public void SendEvent_OpenGlobalUpgrade(int remainingStar, int totalStar)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Open GlobalUpgrade: remainingStar: {0}, totalStar: {1}", remainingStar, totalStar));
		}

		public void SendEvent_OpenGuide(int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Open Guide: max mapID Unlocked: {0}", maxMapIDUnlocked));
		}

		public void SendEvent_OpenHeroCamp(int totalGem, int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Open HeroCamp: totalGem: {0}, max mapID Unlocked: {1}", totalGem, maxMapIDUnlocked));
		}

		public void SendEvent_OpenStore(int totalGem, int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Open Store: totalGem: {0}, max mapID Unlocked: {1}", totalGem, maxMapIDUnlocked));
		}

		public void SendEvent_OpenSceneMainMenu()
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Open Scene MainMenu", new object[0]));
		}

		public void SendEvent_OpenSceneWorldMap(int totalGem, int totalStar, int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Open Scene WorldMap: totalGem: {0}, totalStar: {1}, maxMapIdUnlocked: {2}", totalGem, totalStar, maxMapIDUnlocked));
		}

		public void SendEvent_OpenMapLevelSelect(int mapID)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Open Map level select at mapID : {0}", mapID));
		}

		public void SendEvent_ChooseHeroAtMapLevelSelect(string heroName)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Map level choose hero : {0}", heroName));
		}

		public void SendEvent_StartGame_MapLevel(int mapID)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Map level start at mapID: {0}", mapID));
		}

		public void SendEvent_BuyPowerupItem(string itemName, int itemPrice)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Buy Powerup Item: item name: {0}, price: {1}", itemName, itemPrice));
		}

		public void SendEvent_ResetGlobalUpgrade(int remainingStar, int totalStar)
		{
		}

		public void SendEvent_UnlockedHero(string heroName)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Unlock Hero: {0}", heroName));
		}

		public void SendEvent_ClickButtonBuyHero()
		{
			UnityEngine.Debug.Log("MetricsEditor.SendEvent: Click button buy Hero");
		}

		public void SendEvent_BoughtHero(int heroID, string heroName, int previousGem, int currentGem, int maxMapIDUnlocked, int heroOwnedAmount)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Bought Hero ID {0} {1}, previous gem: {2}, current gem: {3}, max mapID unlocked: {4}, hero owned amound: {5}", new object[]
			{
				heroID,
				heroName,
				previousGem,
				currentGem,
				maxMapIDUnlocked,
				heroOwnedAmount
			}));
		}

		public void SendEvent_UpgradeHeroLevel(string heroName, int previousGem, int currentGem, int currentHeroLevel, int maxMapIDUnlocked, int heroOwnedAmount)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Upgrade level Hero: {0}, previous gem: {1}, current gem: {2}, current hero level: {3}, max mapID unlocked : {4}, hero owned amount : {5}", new object[]
			{
				heroName,
				previousGem,
				currentGem,
				currentHeroLevel,
				maxMapIDUnlocked,
				heroOwnedAmount
			}));
		}

		public void SendEvent_StartGame(int mapID, string hero1Name, int hero1Level, string hero2Name, int hero2Level, string hero3Name, int hero3Level, int puItem0Quantity, int puItem1Quantity, int puItem2Quantity, int puItem3Quantity)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"MetricsEditor.SendEvent: Start Game:Map ",
				mapID,
				"\n hero1 name ",
				hero1Name,
				" hero1 level ",
				hero1Level,
				"\n hero2 name ",
				hero2Name,
				" hero2 level ",
				hero2Level,
				"\n hero3 name ",
				hero3Name,
				" hero3 level ",
				hero3Name,
				"\n item quantity = ",
				puItem0Quantity,
				"_",
				puItem1Quantity,
				"_",
				puItem2Quantity,
				"_",
				puItem3Quantity
			}));
		}

		public void SendEvent_EndGame(int mapID, int starEarned, int gemCollectInMap, int playedAmount)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"MetricsEditor.SendEvent: End Game:Map ",
				mapID,
				"\n Star Earned = ",
				starEarned,
				"\n Gem collected = ",
				gemCollectInMap,
				"\n Played count = ",
				playedAmount
			}));
		}

		public void SendEvent_RestartGame_Setting(int mapID, int previousStarEarned)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Restart Game at Setting: mapID: {0}, previous star earned: {1}", mapID, previousStarEarned));
		}

		public void SendEvent_RestartGame_EndGame(int mapID, int previousStarEarned)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Restart Game at End Game: mapID: {0}, previous star earned: {1}", mapID, previousStarEarned));
		}

		public void SendEvent_CallEarlyEnemies(int mapID, int currentWave, string callType)
		{
		}

		public void SendEvent_ShowTipsButton(string tipType, int id)
		{
		}

		public void SendEvent_OpenTipsPopup(string tipType, int id)
		{
		}

		public void SendEvent_UserSetting_Music(int music)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: User Setting Music: {0}", music));
		}

		public void SendEvent_UserSetting_Sound(int sound)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: User Setting Sound: {0}", sound));
		}

		public void SendEvent_UserSetting_Vibrate(int vibrate)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: User Setting Vibrate: {0}", vibrate));
		}

		public void SendEvent_DoneTutorial(int step)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Done Tut Start Game Step: {0}", step));
		}

		public void SendEvent_BuyItem(string productID)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Buy Item: {0}", productID));
		}

		public void SendEvent_UsePowerupItem(string itemName)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: User Item: {0}", itemName));
		}

		public void SendEvent_WatchGameplayVideoRewardComplete(int currentMapID, string rewardType)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Get video reward : {0} at map : {1}", rewardType, currentMapID));
		}

		public void SendEvent_ShareLinkGameComplete(StageTag sceneName, int mapID)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Share link game complete at scene: {0} mapID : {1}", sceneName.ToString(), mapID));
		}

		public void SendEvent_GetFreeResourcesComplete(FreeResourcesKind freeResourcesType, int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent: Get free gem Type: {0} mapID unlocked : {1}", freeResourcesType.ToString(), maxMapIDUnlocked));
		}

		public void SendEvent_FreeChestOffer(int mapID, FreeCrateDealKind type, int offerCount)
		{
			UnityEngine.Debug.Log(string.Format("MetricsEditor.SendEvent:Offer Free Chest: mapID - {0} , kiá»ƒu offer - {1} , láº§n thá»© - {2} ", mapID, type.ToString(), offerCount));
		}

		public void SendEvent_EndGameDailyTrial(int currentDay, int wavePassed, int playCount, int mapIDCampaignUnlocked)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"MetricsEditor.SendEvent: End Game Daily Trial:Current day = ",
				currentDay,
				"\n wavePassed = ",
				wavePassed,
				"\n Playcount = ",
				playCount,
				"\n mapIdUnlock = ",
				mapIDCampaignUnlocked
			}));
		}

		public void SendEvent_UnlockPet(int heroOwnedAmount, int petOwnedAmount, string petBoughtName)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"MetricsEditor.SendEvent: Unlock Pet:Hero Owned Amount = ",
				heroOwnedAmount,
				"\n pet Owned Amount = ",
				petOwnedAmount,
				"\n pet Bought Name = ",
				petBoughtName
			}));
		}

		public void SendEvent_WatchAds(string adsName)
		{
			UnityEngine.Debug.Log("MetricsEditor.SendEvent: Watch Ads:Ads name = " + adsName);
		}

		public void SendEvent_WatchedVideoReward()
		{
			UnityEngine.Debug.Log("MetricsEditor.SendEvent: Watch Video Reward");
		}

		public void SendEvent_EndGame()
		{
			UnityEngine.Debug.Log("MetricsEditor.SendEvent: End Game Campaign");
		}

		public void SendEvent_EndGameTournament()
		{
			UnityEngine.Debug.Log("MetricsEditor.SendEvent: End Game Tournament");
		}

		public void SendEvent_CompleteEvent(int eventId)
		{
			UnityEngine.Debug.Log("MetricsEditor.SendEvent: complete quest");
		}

		public void SendEvent_ReceiveFriendReward()
		{
			UnityEngine.Debug.Log("MetricsEditor.SendEvent: ReceiveFriendReward");
		}

		public void SendEvent_BeginCheckout(decimal value, string currency)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"MetricsEditor.SendEvent: Begin Checkout  Value = ",
				value,
				" Currency = ",
				currency
			}));
		}
	}
}
