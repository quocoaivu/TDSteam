using System;
using Data;
using HeroCamp;
using LifetimePopup;
using OfferPopup;
using Store;
using UnityEngine;
using UnityEngine.Serialization;

namespace Services.PlatformSpecific.Editor
{
	public class InappBillingEditor : MonoBehaviour, IInappBilling
	{
		public string GetLocalizedProductDescription(string productID)
		{
			return shopItemLookup.GetGemPackDescription(productID);
		}

		public decimal GetLocalizedProductPrice(string productID)
		{
			return 0.01m;
		}

		public string GetLocalizedProductPriceString(string productID)
		{
			return shopItemLookup.GetGemPackPriceString(productID);
		}

		public string GetLocalizedProductTitle(string productID)
		{
			return shopItemLookup.GetGemPackTitle(productID);
		}

		public string GetFormatedProductPrice(string ISOCurrencyCode, decimal amount, int noDecimalFracment)
		{
			return amount.ToString("C" + noDecimalFracment);
		}

		public string GetISOCurrencyCode(string productID)
		{
			return "$";
		}

		public void PurchaseGem(string productID)
		{
			int gemPackValue = shopItemLookup.GetGemPackValue(productID);
			PlayerCurrencyStore.Instance.ChangeGem(gemPackValue, true);
			UnityEngine.Debug.Log("Test Mode IAP: Buy " + gemPackValue + " Gem Success!");
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.UpdateGemStatus();
			NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(productID);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = gemPackValue,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			NativeSpecificServicesSource.Services.DataCloudSaver.AutoBackUpData();
		}

		public void PurchaseOfferBundle(string productID)
		{
			DealPackSingleHero offerBundleSingleHero = MonoSingleton<LifespanSurface>.Instance.OfferPopupController.OfferBundleLoader.GetOfferBundleSingleHero(productID);
			int heroID = offerBundleSingleHero.heroID;
			if (!HeroStore.Instance.IsHeroOwned(heroID))
			{
				HeroStore.Instance.UnlockHero(heroID);
			}
			int[] itemsAmount = offerBundleSingleHero.itemsAmount;
			for (int i = 0; i < itemsAmount.Length; i++)
			{
				PowerUpItemStore.Instance.ChangeItemQuantity(i, itemsAmount[i]);
			}
			UnityEngine.Debug.Log("Test Mode IAP: Buy Hero Bundle " + productID + " Success!");
			PrizeItem[] array = new PrizeItem[5];
			array[0] = new PrizeItem
			{
				rewardType = PrizeKind.SingleHero,
				itemID = heroID,
				isDisplayQuantity = false
			};
			for (int j = 0; j < itemsAmount.Length; j++)
			{
				PrizeItem rewardItem = new PrizeItem();
				rewardItem.rewardType = PrizeKind.Item;
				rewardItem.itemID = j;
				rewardItem.value = itemsAmount[j];
				rewardItem.isDisplayQuantity = true;
				array[j + 1] = rewardItem;
			}
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(array);
			NativeSpecificServicesSource.Services.DataCloudSaver.AutoBackUpData();
		}

		public void PurchaseHero(string productID)
		{
			int heroID = shopItemLookup.GetHeroID(productID);
			if (!HeroStore.Instance.IsHeroOwned(heroID))
			{
				HeroStore.Instance.UnlockHero(heroID);
			}
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.SingleHero,
					itemID = heroID,
					isDisplayQuantity = false
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			HeroBarracksDialogHandler.Instance.UpgradeNBuyGroupController.RefreshStatus();
			NativeSpecificServicesSource.Services.DataCloudSaver.AutoBackUpData();
		}

		public void PurchaseSaleBundle(string productID)
		{
			SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(productID);
			int[] heroid = dataSaleBundle.Heroid;
			int[] itemids = dataSaleBundle.Itemids;
			int[] itemquatities = dataSaleBundle.Itemquatities;
			int gembonus = dataSaleBundle.Gembonus;
			if (heroid.Length > 0)
			{
				for (int i = 0; i < heroid.Length; i++)
				{
					if (!HeroStore.Instance.IsHeroOwned(heroid[i]))
					{
						HeroStore.Instance.UnlockHero(heroid[i]);
						if (dataSaleBundle.Havepet)
						{
							HeroStore.Instance.UnlockPet(heroid[i]);
						}
						if (dataSaleBundle.Herolevel > 0)
						{
							HeroStore.Instance.LevelUpTo(heroid[i], dataSaleBundle.Herolevel);
						}
					}
				}
				if (heroid.Length == 1 && dataSaleBundle.Herolevel == 0)
				{
					MonoSingleton<LifespanSurface>.Instance.AskToBuyDialogHandler.InitBuyHeroLevel(productID);
				}
			}
			if (gembonus > 0)
			{
				PlayerCurrencyStore.Instance.ChangeGem(gembonus, true);
			}
			if (itemids.Length > 0)
			{
				for (int j = 0; j < itemids.Length; j++)
				{
					PowerUpItemStore.Instance.ChangeItemQuantity(j, itemquatities[j]);
				}
			}
			PrizeItem[] array = new PrizeItem[heroid.Length + itemids.Length + ((gembonus <= 0) ? 0 : 1)];
			int num = 0;
			for (int k = 0; k < heroid.Length; k++)
			{
				array[k] = new PrizeItem
				{
					rewardType = PrizeKind.SingleHero,
					itemID = heroid[k],
					isDisplayQuantity = false
				};
				num++;
			}
			if (gembonus > 0)
			{
				array[num] = new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = gembonus,
					isDisplayQuantity = true
				};
				num++;
			}
			for (int l = 0; l < itemids.Length; l++)
			{
				array[num] = new PrizeItem
				{
					rewardType = PrizeKind.Item,
					itemID = itemids[l],
					value = itemquatities[l],
					isDisplayQuantity = true
				};
				num++;
			}
			if (dataSaleBundle.Bundletype.Equals(ShopPackKind.Starter.ToString()))
			{
				UnityEngine.Debug.Log("mua thÃ nh cÃ´ng bundle starter!");
				SaleBundleStore.Instance.SetSpecialPackBought(dataSaleBundle.Productid);
				SaleBundleStore.Instance.SetLastTimePlay();
			}
			if (dataSaleBundle.Bundletype.Equals(ShopPackKind.TimeLimited.ToString()))
			{
				UnityEngine.Debug.Log("mua thÃ nh cÃ´ng bundle limited!");
				SaleBundleStore.Instance.SetSpecialPackBought(dataSaleBundle.Productid);
				SaleBundleStore.Instance.SetLastTimePlay();
			}
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.SaleBundleGroupController.RefreshItemStatus();
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(array);
		}

		public void PurchaseHeroMaxLevel(string productID, string heroItemID)
		{
			SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(heroItemID);
			int[] heroid = dataSaleBundle.Heroid;
			if (heroid.Length == 1)
			{
				HeroStore.Instance.LevelUpTo(heroid[0], 9);
				MonoSingleton<LifespanSurface>.Instance.AskToBuyDialogHandler.InitBuyHeroPet(heroItemID);
			}
		}

		public void PurchaseHeroPet(string productID, string heroItemID)
		{
			SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(heroItemID);
			int[] heroid = dataSaleBundle.Heroid;
			if (heroid.Length == 1)
			{
				HeroStore.Instance.UnlockPet(heroid[0]);
				string localization = GameKit.GetLocalization("CONFIRMED_BUY_PET_HERO");
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(localization, false, false);
			}
		}

		public void PurchaseSubscription(string subscritionId, SubscriptionType subType)
		{
			SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(subscritionId);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"____ processing subscription ",
				subscritionId,
				" ",
				dataSaleBundle
			}));
			if (subType != SubscriptionType.dailyBooster)
			{
				if (subType == SubscriptionType.fiftyPercentAtkBoost || subType == SubscriptionType.doubleAttack)
				{
					DateTime localTime = GameKit.GetNow().AddDays((double)dataSaleBundle.Subcribedur);
					GameKit.SetEndSubscriptionTime(subType, localTime);
					MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(string.Format(GameKit.GetLocalization("ATTACK_BOOSTER_NOTI"), localTime.ToString("MM\\/dd\\/yyyy HH:mm"), dataSaleBundle.Itemquatities[0]), "OK", null, null);
				}
			}
			else
			{
				DateTime moment = GameKit.GetMoment0(GameKit.GetNow());
				GameKit.SetEndSubscriptionTime(subType, moment.AddDays((double)dataSaleBundle.Subcribedur).AddMinutes(-4.0));
				GameKit.SetLastTimeCheckInSubscription(subType, moment.AddDays(-1.0));
				DailyCheckinDirector.Instance.CheckDailyBooster();
			}
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnCompletePurchase, null);
		}

		[SerializeField]
		[FormerlySerializedAs("readDataShopItemAttribute")]
		private ShopItemLookup shopItemLookup;

		[SerializeField]
		[FormerlySerializedAs("readDataOfferBundle")]
		private OfferBundleLoader offerBundleLoader;
	}
}
