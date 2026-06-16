using System;
using System.Collections.Generic;
using Data;
using HeroCamp;
using LifetimePopup;
using OfferPopup;
using Store;
using UnityEngine;
//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Extension;
////using UnityEngine.Purchasing.Security;
//using UnityEngine.SceneManagement;

//namespace Services.PlatformSpecific.Android
//{
//	public class InappBillingAndroid : MonoBehaviour, IInappBilling, IStoreListener
//	{
//		private void Start()
//		{
//			if (InappBillingAndroid.m_StoreController == null)
//			{
//				InitializePurchasing();
//			}
//		}

//		public void InitializePurchasing()
//		{
//			if (IsInitialized())
//			{
//				return;
//			}
//			ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(), new IPurchasingModule[0]);
//			configurationBuilder.AddProduct("kd.v2.gem_pack_1", ProductType.Consumable);
//			configurationBuilder.AddProduct("kd.v2.gem_pack_2", ProductType.Consumable);
//			configurationBuilder.AddProduct("kd.v2.gem_pack_3", ProductType.Consumable);
//			configurationBuilder.AddProduct("kd.v2.gem_pack_4", ProductType.Consumable);
//			configurationBuilder.AddProduct("kd.v2.gem_pack_5", ProductType.Consumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackOffer[0], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackOffer[3], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackOffer[4], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackOffer[5], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackOffer[6], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackOffer[7], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackOffer[8], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackOffer[9], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPack[0], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPack[1], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackSale[0], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackSale[1], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackSale[2], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackSale[3], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackSale[4], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackSale[5], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackSale[6], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackSale[7], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDHeroPackSale[8], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDSpecialPack[0], ProductType.Consumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDSpecialPack[1], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDSpecialPack[2], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDSpecialPack[3], ProductType.NonConsumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDSpecialPack[4], ProductType.Subscription);
//			configurationBuilder.AddProduct(MarketingSetup.productIDSpecialPack[5], ProductType.Consumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDSpecialPack[6], ProductType.Consumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDSpecialPack[7], ProductType.Consumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDSpecialPack[8], ProductType.Subscription);
//			configurationBuilder.AddProduct(MarketingSetup.productIDItemsPack[0], ProductType.Consumable);
//			configurationBuilder.AddProduct(MarketingSetup.productIDComboHeroes[0], ProductType.NonConsumable);
//			UnityPurchasing.Initialize(this, configurationBuilder);
//			UnityEngine.Debug.Log("Unity purchase Init products completed!");
//		}

//		private bool IsInitialized()
//		{
//			return InappBillingAndroid.m_StoreController != null && InappBillingAndroid.m_StoreExtensionProvider != null;
//		}

//		private void BuyProductID(string productId)
//		{
//			if (IsInitialized())
//			{
//				Product product = InappBillingAndroid.m_StoreController.products.WithID(productId);
//				if (product != null && product.availableToPurchase)
//				{
//					UnityEngine.Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
//					InappBillingAndroid.m_StoreController.InitiatePurchase(product);
//					decimal localizedProductPrice = GetLocalizedProductPrice(productId);
//					string isocurrencyCode = GetISOCurrencyCode(productId);
//					NativeSpecificServicesSource.Services.Analytics.SendEvent_BeginCheckout(localizedProductPrice, isocurrencyCode);
//				}
//				else
//				{
//					UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//				}
//			}
//			else
//			{
//				UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
//			}
//		}

//		public void RestorePurchases()
//		{
//			if (!IsInitialized())
//			{
//				UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
//				return;
//			}
//			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
//			{
//				UnityEngine.Debug.Log("RestorePurchases started ...");
//				IAppleExtensions extension = InappBillingAndroid.m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
//				extension.RestoreTransactions(delegate(bool result)
//				{
//					UnityEngine.Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//				});
//			}
//			else
//			{
//				UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//			}
//		}

//		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//		{
//			UnityEngine.Debug.Log(">>>>>>>> OnInitialized: PASS");
//			InappBillingAndroid.m_StoreController = controller;
//			InappBillingAndroid.m_StoreExtensionProvider = extensions;
//			ITransactionHistoryExtensions extension = extensions.GetExtension<ITransactionHistoryExtensions>();
//			IGooglePlayStoreExtensions extension2 = extensions.GetExtension<IGooglePlayStoreExtensions>();
//			introductory_info_dict = null;
//			foreach (Product product in controller.products.all)
//			{
//				if (product.availableToPurchase)
//				{
//					UnityEngine.Debug.Log(string.Join(" - ", new string[]
//					{
//						product.metadata.localizedTitle,
//						product.metadata.localizedDescription,
//						product.metadata.isoCurrencyCode,
//						product.metadata.localizedPrice.ToString(),
//						product.metadata.localizedPriceString,
//						product.transactionID,
//						product.receipt
//					}));
//					if (product.receipt != null)
//					{
//						if (product.definition.type == ProductType.Subscription)
//						{
//							if (checkIfProductIsAvailableForSubscriptionManager(product.receipt))
//							{
//								string intro_json = (introductory_info_dict != null && introductory_info_dict.ContainsKey(product.definition.storeSpecificId)) ? introductory_info_dict[product.definition.storeSpecificId] : null;
//								SubscriptionManager subscriptionManager = new SubscriptionManager(product, intro_json);
//								SubscriptionInfo subscriptionInfo = subscriptionManager.getSubscriptionInfo();
//								SubscriptionType subscriptionTypeEnum = GameKit.productIdToSubscriptionEnum[subscriptionInfo.getProductId()];
//								if (subscriptionInfo.isExpired() == Result.False)
//								{
//									UnityEngine.Debug.LogFormat(">>>>>>>>> set {0} expire date: {1}", new object[]
//									{
//										subscriptionInfo.getProductId(),
//										subscriptionInfo.getExpireDate().ToLocalTime()
//									});
//									if (subscriptionTypeEnum == SubscriptionType.dailyBooster)
//									{
//										GameKit.SetEndSubscriptionTime(subscriptionTypeEnum, GameKit.GetMoment0(subscriptionInfo.getExpireDate().ToLocalTime()));
//										DateTime lastTimeCheckInSubscription = GameKit.GetLastTimeCheckInSubscription(SubscriptionType.dailyBooster);
//										if ((GameKit.GetNow() - lastTimeCheckInSubscription).Days > 14)
//										{
//											GameKit.SetLastTimeCheckInSubscription(SubscriptionType.dailyBooster, GameKit.GetNow());
//										}
//									}
//									else
//									{
//										GameKit.SetEndSubscriptionTime(subscriptionTypeEnum, subscriptionInfo.getExpireDate().ToLocalTime());
//									}
//								}
//								else
//								{
//									GameKit.SetEndSubscriptionTime(subscriptionTypeEnum, GameKit.GetNow().AddDays(-1.0));
//								}
//							}
//							else
//							{
//								UnityEngine.Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
//							}
//						}
//						else
//						{
//							UnityEngine.Debug.Log("the product is not a subscription product");
//						}
//					}
//					else
//					{
//						UnityEngine.Debug.Log("the product should have a valid receipt");
//					}
//				}
//			}
//		}

//		private bool checkIfProductIsAvailableForSubscriptionManager(string receipt)
//		{
//			Dictionary<string, object> dictionary = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
//			if (!dictionary.ContainsKey("Store") || !dictionary.ContainsKey("Payload"))
//			{
//				UnityEngine.Debug.Log("The product receipt does not contain enough information");
//				return false;
//			}
//			string text = (string)dictionary["Store"];
//			string text2 = (string)dictionary["Payload"];
//			if (text2 != null)
//			{
//				if (text != null)
//				{
//					if (!(text == "GooglePlay"))
//					{
//						if (text == "AppleAppStore" || text == "AmazonApps" || text == "MacAppStore")
//						{
//							return true;
//						}
//					}
//					else
//					{
//						Dictionary<string, object> dictionary2 = (Dictionary<string, object>)MiniJson.JsonDecode(text2);
//						if (!dictionary2.ContainsKey("json"))
//						{
//							UnityEngine.Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
//							return false;
//						}
//						Dictionary<string, object> dictionary3 = (Dictionary<string, object>)MiniJson.JsonDecode((string)dictionary2["json"]);
//						if (dictionary3 == null || !dictionary3.ContainsKey("developerPayload"))
//						{
//							UnityEngine.Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
//							return false;
//						}
//						string json = (string)dictionary3["developerPayload"];
//						Dictionary<string, object> dictionary4 = (Dictionary<string, object>)MiniJson.JsonDecode(json);
//						if (dictionary4 == null || !dictionary4.ContainsKey("is_free_trial") || !dictionary4.ContainsKey("has_introductory_price_trial"))
//						{
//							UnityEngine.Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
//							return false;
//						}
//						return true;
//					}
//				}
//				return false;
//			}
//			return false;
//		}

//		public void OnInitializeFailed(InitializationFailureReason error)
//		{
//			UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//		}

//		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//		{
//			bool flag = true;
//			CrossPlatformValidator crossPlatformValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
//			try
//			{
//				IPurchaseReceipt[] array = crossPlatformValidator.Validate(args.purchasedProduct.receipt);
//				UnityEngine.Debug.Log("Receipt is valid. Contents:");
//				foreach (IPurchaseReceipt purchaseReceipt in array)
//				{
//					UnityEngine.Debug.Log(purchaseReceipt.productID);
//					UnityEngine.Debug.Log(purchaseReceipt.purchaseDate);
//					UnityEngine.Debug.Log(purchaseReceipt.transactionID);
//				}
//			}
//			catch (IAPSecurityException)
//			{
//				UnityEngine.Debug.Log("Invalid receipt, not unlocking content");
//				flag = false;
//			}
//			if (!flag)
//			{
//				return PurchaseProcessingResult.Complete;
//			}
//			if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_1", StringComparison.Ordinal))
//			{
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				int gemPackValue = shopItemLookup.GetGemPackValue("kd.v2.gem_pack_1");
//				PlayerCurrencyStore.Instance.ChangeGem(gemPackValue, true);
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_1");
//				PrizeItem[] listData = new PrizeItem[]
//				{
//					new PrizeItem
//					{
//						rewardType = PrizeKind.Gem,
//						value = gemPackValue,
//						isDisplayQuantity = true
//					}
//				};
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(listData);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_2", StringComparison.Ordinal))
//			{
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				int gemPackValue2 = shopItemLookup.GetGemPackValue("kd.v2.gem_pack_2");
//				PlayerCurrencyStore.Instance.ChangeGem(gemPackValue2, true);
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_2");
//				PrizeItem[] listData2 = new PrizeItem[]
//				{
//					new PrizeItem
//					{
//						rewardType = PrizeKind.Gem,
//						value = gemPackValue2,
//						isDisplayQuantity = true
//					}
//				};
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(listData2);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_3", StringComparison.Ordinal))
//			{
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				int gemPackValue3 = shopItemLookup.GetGemPackValue("kd.v2.gem_pack_3");
//				PlayerCurrencyStore.Instance.ChangeGem(gemPackValue3, true);
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_3");
//				PrizeItem[] listData3 = new PrizeItem[]
//				{
//					new PrizeItem
//					{
//						rewardType = PrizeKind.Gem,
//						value = gemPackValue3,
//						isDisplayQuantity = true
//					}
//				};
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(listData3);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_4", StringComparison.Ordinal))
//			{
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				int gemPackValue4 = shopItemLookup.GetGemPackValue("kd.v2.gem_pack_4");
//				PlayerCurrencyStore.Instance.ChangeGem(gemPackValue4, true);
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_4");
//				PrizeItem[] listData4 = new PrizeItem[]
//				{
//					new PrizeItem
//					{
//						rewardType = PrizeKind.Gem,
//						value = gemPackValue4,
//						isDisplayQuantity = true
//					}
//				};
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(listData4);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_5", StringComparison.Ordinal))
//			{
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				int gemPackValue5 = shopItemLookup.GetGemPackValue("kd.v2.gem_pack_5");
//				PlayerCurrencyStore.Instance.ChangeGem(gemPackValue5, true);
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_5");
//				PrizeItem[] listData5 = new PrizeItem[]
//				{
//					new PrizeItem
//					{
//						rewardType = PrizeKind.Gem,
//						value = gemPackValue5,
//						isDisplayQuantity = true
//					}
//				};
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(listData5);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackOffer[0], StringComparison.Ordinal))
//			{
//				DealPackSingleHero offerBundleSingleHero = readDataOfferBundle.GetOfferBundleSingleHero(MarketingSetup.productIDHeroPackOffer[0]);
//				int heroID = offerBundleSingleHero.heroID;
//				if (!HeroStore.Instance.IsHeroOwned(heroID))
//				{
//					HeroStore.Instance.UnlockHero(heroID);
//				}
//				int[] itemsAmount = offerBundleSingleHero.itemsAmount;
//				for (int j = 0; j < itemsAmount.Length; j++)
//				{
//					PowerUpItemStore.Instance.ChangeItemQuantity(j, itemsAmount[j]);
//				}
//				PrizeItem[] array3 = new PrizeItem[5];
//				array3[0] = new PrizeItem
//				{
//					rewardType = PrizeKind.SingleHero,
//					itemID = heroID,
//					isDisplayQuantity = false
//				};
//				for (int k = 0; k < itemsAmount.Length; k++)
//				{
//					PrizeItem rewardItem = new PrizeItem();
//					rewardItem.rewardType = PrizeKind.Item;
//					rewardItem.itemID = k;
//					rewardItem.value = itemsAmount[k];
//					rewardItem.isDisplayQuantity = true;
//					array3[k + 1] = rewardItem;
//				}
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(array3);
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPackOffer[0]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackOffer[3], StringComparison.Ordinal))
//			{
//				DealPackSingleHero offerBundleSingleHero2 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingSetup.productIDHeroPackOffer[3]);
//				int heroID2 = offerBundleSingleHero2.heroID;
//				if (!HeroStore.Instance.IsHeroOwned(heroID2))
//				{
//					HeroStore.Instance.UnlockHero(heroID2);
//				}
//				int[] itemsAmount2 = offerBundleSingleHero2.itemsAmount;
//				for (int l = 0; l < itemsAmount2.Length; l++)
//				{
//					PowerUpItemStore.Instance.ChangeItemQuantity(l, itemsAmount2[l]);
//				}
//				PrizeItem[] array4 = new PrizeItem[5];
//				array4[0] = new PrizeItem
//				{
//					rewardType = PrizeKind.SingleHero,
//					itemID = heroID2,
//					isDisplayQuantity = false
//				};
//				for (int m = 0; m < itemsAmount2.Length; m++)
//				{
//					PrizeItem rewardItem2 = new PrizeItem();
//					rewardItem2.rewardType = PrizeKind.Item;
//					rewardItem2.itemID = m;
//					rewardItem2.value = itemsAmount2[m];
//					rewardItem2.isDisplayQuantity = true;
//					array4[m + 1] = rewardItem2;
//				}
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(array4);
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPackOffer[3]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackOffer[4], StringComparison.Ordinal))
//			{
//				DealPackSingleHero offerBundleSingleHero3 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingSetup.productIDHeroPackOffer[4]);
//				int heroID3 = offerBundleSingleHero3.heroID;
//				if (!HeroStore.Instance.IsHeroOwned(heroID3))
//				{
//					HeroStore.Instance.UnlockHero(heroID3);
//				}
//				int[] itemsAmount3 = offerBundleSingleHero3.itemsAmount;
//				for (int n = 0; n < itemsAmount3.Length; n++)
//				{
//					PowerUpItemStore.Instance.ChangeItemQuantity(n, itemsAmount3[n]);
//				}
//				PrizeItem[] array5 = new PrizeItem[5];
//				array5[0] = new PrizeItem
//				{
//					rewardType = PrizeKind.SingleHero,
//					itemID = heroID3,
//					isDisplayQuantity = false
//				};
//				for (int num = 0; num < itemsAmount3.Length; num++)
//				{
//					PrizeItem rewardItem3 = new PrizeItem();
//					rewardItem3.rewardType = PrizeKind.Item;
//					rewardItem3.itemID = num;
//					rewardItem3.value = itemsAmount3[num];
//					rewardItem3.isDisplayQuantity = true;
//					array5[num + 1] = rewardItem3;
//				}
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(array5);
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPackOffer[4]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackOffer[5], StringComparison.Ordinal))
//			{
//				DealPackSingleHero offerBundleSingleHero4 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingSetup.productIDHeroPackOffer[5]);
//				int heroID4 = offerBundleSingleHero4.heroID;
//				if (!HeroStore.Instance.IsHeroOwned(heroID4))
//				{
//					HeroStore.Instance.UnlockHero(heroID4);
//				}
//				int[] itemsAmount4 = offerBundleSingleHero4.itemsAmount;
//				for (int num2 = 0; num2 < itemsAmount4.Length; num2++)
//				{
//					PowerUpItemStore.Instance.ChangeItemQuantity(num2, itemsAmount4[num2]);
//				}
//				PrizeItem[] array6 = new PrizeItem[5];
//				array6[0] = new PrizeItem
//				{
//					rewardType = PrizeKind.SingleHero,
//					itemID = heroID4,
//					isDisplayQuantity = false
//				};
//				for (int num3 = 0; num3 < itemsAmount4.Length; num3++)
//				{
//					PrizeItem rewardItem4 = new PrizeItem();
//					rewardItem4.rewardType = PrizeKind.Item;
//					rewardItem4.itemID = num3;
//					rewardItem4.value = itemsAmount4[num3];
//					rewardItem4.isDisplayQuantity = true;
//					array6[num3 + 1] = rewardItem4;
//				}
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(array6);
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPackOffer[5]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackOffer[6], StringComparison.Ordinal))
//			{
//				DealPackSingleHero offerBundleSingleHero5 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingSetup.productIDHeroPackOffer[6]);
//				int heroID5 = offerBundleSingleHero5.heroID;
//				if (!HeroStore.Instance.IsHeroOwned(heroID5))
//				{
//					HeroStore.Instance.UnlockHero(heroID5);
//				}
//				int[] itemsAmount5 = offerBundleSingleHero5.itemsAmount;
//				for (int num4 = 0; num4 < itemsAmount5.Length; num4++)
//				{
//					PowerUpItemStore.Instance.ChangeItemQuantity(num4, itemsAmount5[num4]);
//				}
//				PrizeItem[] array7 = new PrizeItem[5];
//				array7[0] = new PrizeItem
//				{
//					rewardType = PrizeKind.SingleHero,
//					itemID = heroID5,
//					isDisplayQuantity = false
//				};
//				for (int num5 = 0; num5 < itemsAmount5.Length; num5++)
//				{
//					PrizeItem rewardItem5 = new PrizeItem();
//					rewardItem5.rewardType = PrizeKind.Item;
//					rewardItem5.itemID = num5;
//					rewardItem5.value = itemsAmount5[num5];
//					rewardItem5.isDisplayQuantity = true;
//					array7[num5 + 1] = rewardItem5;
//				}
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(array7);
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPackOffer[6]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackOffer[7], StringComparison.Ordinal))
//			{
//				DealPackSingleHero offerBundleSingleHero6 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingSetup.productIDHeroPackOffer[7]);
//				int heroID6 = offerBundleSingleHero6.heroID;
//				if (!HeroStore.Instance.IsHeroOwned(heroID6))
//				{
//					HeroStore.Instance.UnlockHero(heroID6);
//				}
//				int[] itemsAmount6 = offerBundleSingleHero6.itemsAmount;
//				for (int num6 = 0; num6 < itemsAmount6.Length; num6++)
//				{
//					PowerUpItemStore.Instance.ChangeItemQuantity(num6, itemsAmount6[num6]);
//				}
//				PrizeItem[] array8 = new PrizeItem[5];
//				array8[0] = new PrizeItem
//				{
//					rewardType = PrizeKind.SingleHero,
//					itemID = heroID6,
//					isDisplayQuantity = false
//				};
//				for (int num7 = 0; num7 < itemsAmount6.Length; num7++)
//				{
//					PrizeItem rewardItem6 = new PrizeItem();
//					rewardItem6.rewardType = PrizeKind.Item;
//					rewardItem6.itemID = num7;
//					rewardItem6.value = itemsAmount6[num7];
//					rewardItem6.isDisplayQuantity = true;
//					array8[num7 + 1] = rewardItem6;
//				}
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(array8);
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPackOffer[7]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackOffer[8], StringComparison.Ordinal))
//			{
//				DealPackSingleHero offerBundleSingleHero7 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingSetup.productIDHeroPackOffer[8]);
//				int heroID7 = offerBundleSingleHero7.heroID;
//				if (!HeroStore.Instance.IsHeroOwned(heroID7))
//				{
//					HeroStore.Instance.UnlockHero(heroID7);
//				}
//				int[] itemsAmount7 = offerBundleSingleHero7.itemsAmount;
//				for (int num8 = 0; num8 < itemsAmount7.Length; num8++)
//				{
//					PowerUpItemStore.Instance.ChangeItemQuantity(num8, itemsAmount7[num8]);
//				}
//				PrizeItem[] array9 = new PrizeItem[5];
//				array9[0] = new PrizeItem
//				{
//					rewardType = PrizeKind.SingleHero,
//					itemID = heroID7,
//					isDisplayQuantity = false
//				};
//				for (int num9 = 0; num9 < itemsAmount7.Length; num9++)
//				{
//					PrizeItem rewardItem7 = new PrizeItem();
//					rewardItem7.rewardType = PrizeKind.Item;
//					rewardItem7.itemID = num9;
//					rewardItem7.value = itemsAmount7[num9];
//					rewardItem7.isDisplayQuantity = true;
//					array9[num9 + 1] = rewardItem7;
//				}
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(array9);
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPackOffer[8]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackOffer[9], StringComparison.Ordinal))
//			{
//				DealPackSingleHero offerBundleSingleHero8 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingSetup.productIDHeroPackOffer[9]);
//				int heroID8 = offerBundleSingleHero8.heroID;
//				if (!HeroStore.Instance.IsHeroOwned(heroID8))
//				{
//					HeroStore.Instance.UnlockHero(heroID8);
//				}
//				int[] itemsAmount8 = offerBundleSingleHero8.itemsAmount;
//				for (int num10 = 0; num10 < itemsAmount8.Length; num10++)
//				{
//					PowerUpItemStore.Instance.ChangeItemQuantity(num10, itemsAmount8[num10]);
//				}
//				PrizeItem[] array10 = new PrizeItem[5];
//				array10[0] = new PrizeItem
//				{
//					rewardType = PrizeKind.SingleHero,
//					itemID = heroID8,
//					isDisplayQuantity = false
//				};
//				for (int num11 = 0; num11 < itemsAmount8.Length; num11++)
//				{
//					PrizeItem rewardItem8 = new PrizeItem();
//					rewardItem8.rewardType = PrizeKind.Item;
//					rewardItem8.itemID = num11;
//					rewardItem8.value = itemsAmount8[num11];
//					rewardItem8.isDisplayQuantity = true;
//					array10[num11 + 1] = rewardItem8;
//				}
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(array10);
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPackOffer[9]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPack[0], StringComparison.Ordinal))
//			{
//				int heroID9 = shopItemLookup.GetHeroID(MarketingSetup.productIDHeroPack[0]);
//				if (!HeroStore.Instance.IsHeroOwned(heroID9))
//				{
//					HeroStore.Instance.UnlockHero(heroID9);
//				}
//				PrizeItem[] listData6 = new PrizeItem[]
//				{
//					new PrizeItem
//					{
//						rewardType = PrizeKind.SingleHero,
//						itemID = heroID9,
//						isDisplayQuantity = false
//					}
//				};
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(listData6);
//				if (SceneManager.GetActiveScene().name.Equals(GameSceneLoader.WorldMapSceneName))
//				{
//					HeroBarracksDialogHandler.Instance.UpgradeNBuyGroupController.RefreshStatus();
//				}
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPack[0]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPack[1], StringComparison.Ordinal))
//			{
//				int heroID10 = shopItemLookup.GetHeroID(MarketingSetup.productIDHeroPack[1]);
//				if (!HeroStore.Instance.IsHeroOwned(heroID10))
//				{
//					HeroStore.Instance.UnlockHero(heroID10);
//				}
//				PrizeItem[] listData7 = new PrizeItem[]
//				{
//					new PrizeItem
//					{
//						rewardType = PrizeKind.SingleHero,
//						itemID = heroID10,
//						isDisplayQuantity = false
//					}
//				};
//				SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(listData7);
//				if (SceneManager.GetActiveScene().name.Equals(GameSceneLoader.WorldMapSceneName))
//				{
//					HeroBarracksDialogHandler.Instance.UpgradeNBuyGroupController.RefreshStatus();
//				}
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDHeroPack[1]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackSale[0], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDHeroPackSale[0]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackSale[1], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDHeroPackSale[1]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackSale[2], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDHeroPackSale[2]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackSale[3], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDHeroPackSale[3]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackSale[4], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDHeroPackSale[4]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackSale[5], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDHeroPackSale[5]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackSale[6], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDHeroPackSale[6]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackSale[7], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDHeroPackSale[7]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDHeroPackSale[8], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDHeroPackSale[8]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDSpecialPack[0], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDSpecialPack[0]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDSpecialPack[1], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDSpecialPack[1]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDSpecialPack[2], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDSpecialPack[2]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDSpecialPack[3], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchase(MarketingSetup.productIDSpecialPack[3]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDSpecialPack[4], StringComparison.Ordinal))
//			{
//				string intro_json = (introductory_info_dict != null && introductory_info_dict.ContainsKey(args.purchasedProduct.definition.storeSpecificId)) ? introductory_info_dict[args.purchasedProduct.definition.storeSpecificId] : null;
//				SubscriptionManager subscriptionManager = new SubscriptionManager(args.purchasedProduct, intro_json);
//				SubscriptionInfo subscriptionInfo = subscriptionManager.getSubscriptionInfo();
//				SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(args.purchasedProduct.definition.id);
//				SubscriptionType subId = SubscriptionType.dailyBooster;
//				DateTime moment = GameKit.GetMoment0(GameKit.GetNow());
//				GameKit.SetEndSubscriptionTime(subId, GameKit.GetMoment0(subscriptionInfo.getExpireDate().ToLocalTime()));
//				GameKit.SetLastTimeCheckInSubscription(subId, moment.AddDays(-1.0));
//				DailyCheckinDirector.Instance.CheckDailyBooster();
//				GameSignalCenter.Instance.Trigger(GameSignalKind.OnCompletePurchase, null);
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDSpecialPack[4]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDSpecialPack[7], StringComparison.Ordinal))
//			{
//				SalePackSetupRecord dataSaleBundle2 = ShopPackRecord.GetDataSaleBundle(args.purchasedProduct.definition.id);
//				SubscriptionType subId2 = GameKit.productIdToSubscriptionEnum[args.purchasedProduct.definition.id];
//				DateTime localTime = GameKit.GetNow().AddDays((double)dataSaleBundle2.Subcribedur);
//				GameKit.SetEndSubscriptionTime(subId2, localTime);
//				SingletonMonoBehaviour<LifespanSurface>.Instance.NotifyPopupController.Init(string.Format(GameKit.GetLocalization("ATTACK_BOOSTER_NOTI"), localTime.ToString("MM\\/dd\\/yyyy HH:mm"), dataSaleBundle2.Itemquatities[0]), "OK", null, null);
//				GameSignalCenter.Instance.Trigger(GameSignalKind.OnCompletePurchase, null);
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDSpecialPack[7]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDSpecialPack[8], StringComparison.Ordinal))
//			{
//				string intro_json2 = (introductory_info_dict != null && introductory_info_dict.ContainsKey(args.purchasedProduct.definition.storeSpecificId)) ? introductory_info_dict[args.purchasedProduct.definition.storeSpecificId] : null;
//				SubscriptionManager subscriptionManager2 = new SubscriptionManager(args.purchasedProduct, intro_json2);
//				SubscriptionInfo subscriptionInfo2 = subscriptionManager2.getSubscriptionInfo();
//				SalePackSetupRecord dataSaleBundle3 = ShopPackRecord.GetDataSaleBundle(args.purchasedProduct.definition.id);
//				SubscriptionType subId3 = GameKit.productIdToSubscriptionEnum[args.purchasedProduct.definition.id];
//				DateTime localTime2 = subscriptionInfo2.getExpireDate().ToLocalTime();
//				GameKit.SetEndSubscriptionTime(subId3, localTime2);
//				SingletonMonoBehaviour<LifespanSurface>.Instance.NotifyPopupController.Init(string.Format(GameKit.GetLocalization("ATTACK_BOOSTER_NOTI"), localTime2.ToString("MM\\/dd\\/yyyy HH:mm"), dataSaleBundle3.Itemquatities[0]), "OK", null, null);
//				GameSignalCenter.Instance.Trigger(GameSignalKind.OnCompletePurchase, null);
//				NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(MarketingSetup.productIDSpecialPack[8]);
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDSpecialPack[5], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchaseHeroMaxLevel();
//			}
//			else if (string.Equals(args.purchasedProduct.definition.id, MarketingSetup.productIDSpecialPack[6], StringComparison.Ordinal))
//			{
//				ProcessDataAfterPurchaseHeroPet();
//			}
//			else
//			{
//				UnityEngine.Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//			}
//			return PurchaseProcessingResult.Complete;
//		}

//		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//		{
//			UnityEngine.Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//		}

//		private void ProcessDataAfterPurchase(string productID)
//		{
//			SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(productID);
//			int[] heroid = dataSaleBundle.Heroid;
//			int[] itemids = dataSaleBundle.Itemids;
//			int[] itemquatities = dataSaleBundle.Itemquatities;
//			int gembonus = dataSaleBundle.Gembonus;
//			if (heroid.Length > 0)
//			{
//				for (int i = 0; i < heroid.Length; i++)
//				{
//					if (!HeroStore.Instance.IsHeroOwned(heroid[i]))
//					{
//						HeroStore.Instance.UnlockHero(heroid[i]);
//						if (dataSaleBundle.Havepet)
//						{
//							HeroStore.Instance.UnlockPet(heroid[i]);
//						}
//						if (dataSaleBundle.Herolevel > 0)
//						{
//							HeroStore.Instance.LevelUpTo(heroid[i], dataSaleBundle.Herolevel);
//						}
//					}
//				}
//				if (SceneManager.GetActiveScene().name.Equals(GameSceneLoader.WorldMapSceneName) && heroid.Length == 1 && dataSaleBundle.Herolevel == 0)
//				{
//					SingletonMonoBehaviour<LifespanSurface>.Instance.AskToBuyDialogHandler.InitBuyHeroLevel(productID);
//				}
//			}
//			if (gembonus > 0)
//			{
//				PlayerCurrencyStore.Instance.ChangeGem(gembonus, true);
//			}
//			if (itemids.Length > 0)
//			{
//				for (int j = 0; j < itemids.Length; j++)
//				{
//					PowerUpItemStore.Instance.ChangeItemQuantity(j, itemquatities[j]);
//				}
//			}
//			PrizeItem[] array = new PrizeItem[heroid.Length + itemids.Length + ((gembonus <= 0) ? 0 : 1)];
//			int num = 0;
//			for (int k = 0; k < heroid.Length; k++)
//			{
//				array[k] = new PrizeItem
//				{
//					rewardType = PrizeKind.SingleHero,
//					itemID = heroid[k],
//					isDisplayQuantity = false
//				};
//				num++;
//			}
//			if (gembonus > 0)
//			{
//				array[num] = new PrizeItem
//				{
//					rewardType = PrizeKind.Gem,
//					value = gembonus,
//					isDisplayQuantity = true
//				};
//				num++;
//			}
//			for (int l = 0; l < itemids.Length; l++)
//			{
//				array[num] = new PrizeItem
//				{
//					rewardType = PrizeKind.Item,
//					itemID = itemids[l],
//					value = itemquatities[l],
//					isDisplayQuantity = true
//				};
//				num++;
//			}
//			if (dataSaleBundle.Bundletype.Equals(ShopPackKind.Starter.ToString()))
//			{
//				UnityEngine.Debug.Log("mua thÃ nh cÃ´ng bundle starter!");
//				SaleBundleStore.Instance.SetSpecialPackBought(dataSaleBundle.Productid);
//				SaleBundleStore.Instance.SetLastTimePlay();
//			}
//			if (dataSaleBundle.Bundletype.Equals(ShopPackKind.TimeLimited.ToString()))
//			{
//				UnityEngine.Debug.Log("mua thÃ nh cÃ´ng bundle limited!");
//				SaleBundleStore.Instance.SetSpecialPackBought(dataSaleBundle.Productid);
//				SaleBundleStore.Instance.SetLastTimePlay();
//			}
//			SingletonMonoBehaviour<LifespanSurface>.Instance.StorePopupController.SaleBundleGroupController.RefreshItemStatus();
//			SingletonMonoBehaviour<LifespanSurface>.Instance.RewardPopupController.Init(array);
//			UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", productID));
//			NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(productID);
//		}

//		public void PurchaseHero(string productID)
//		{
//			BuyProductID(productID);
//		}

//		public void PurchaseGem(string productID)
//		{
//			BuyProductID(productID);
//		}

//		public void PurchaseOfferBundle(string productID)
//		{
//			BuyProductID(productID);
//		}

//		public void PurchaseSaleBundle(string productID)
//		{
//			BuyProductID(productID);
//		}

//		public void PurchaseHeroMaxLevel(string productID, string heroItemID)
//		{
//			tempHeroItemID = heroItemID;
//			BuyProductID(productID);
//		}

//		private void ProcessDataAfterPurchaseHeroMaxLevel()
//		{
//			SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(tempHeroItemID);
//			int[] heroid = dataSaleBundle.Heroid;
//			if (heroid.Length == 1)
//			{
//				HeroStore.Instance.LevelUpTo(heroid[0], 9);
//				if (SceneManager.GetActiveScene().name.Equals(GameSceneLoader.WorldMapSceneName))
//				{
//					SingletonMonoBehaviour<LifespanSurface>.Instance.AskToBuyDialogHandler.InitBuyHeroPet(tempHeroItemID);
//				}
//			}
//			NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(tempHeroItemID);
//		}

//		public void PurchaseHeroPet(string productID, string heroItemID)
//		{
//			tempHeroItemID = heroItemID;
//			BuyProductID(productID);
//		}

//		private void ProcessDataAfterPurchaseHeroPet()
//		{
//			SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(tempHeroItemID);
//			int[] heroid = dataSaleBundle.Heroid;
//			if (heroid.Length == 1)
//			{
//				HeroStore.Instance.UnlockPet(heroid[0]);
//				string localization = GameKit.GetLocalization("CONFIRMED_BUY_PET_HERO");
//				SingletonMonoBehaviour<LifespanSurface>.Instance.NotifyPopupController.Init(localization, false, false);
//			}
//			NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyItem(tempHeroItemID);
//		}

//		public void PurchaseSubscription(string subscritionId, SubscriptionType subType)
//		{
//			BuyProductID(subscritionId);
//		}

//		public string GetLocalizedProductTitle(string productID)
//		{
//			string result = string.Empty;
//			if (InappBillingAndroid.m_StoreController != null)
//			{
//				result = InappBillingAndroid.m_StoreController.products.WithID(productID).metadata.localizedTitle;
//			}
//			return result;
//		}

//		public string GetLocalizedProductDescription(string productID)
//		{
//			string result = string.Empty;
//			if (InappBillingAndroid.m_StoreController != null)
//			{
//				result = InappBillingAndroid.m_StoreController.products.WithID(productID).metadata.localizedDescription;
//			}
//			return result;
//		}

//		public string GetLocalizedProductPriceString(string productID)
//		{
//			string result = string.Empty;
//			if (InappBillingAndroid.m_StoreController != null)
//			{
//				result = InappBillingAndroid.m_StoreController.products.WithID(productID).metadata.localizedPriceString;
//			}
//			return result;
//		}

//		public decimal GetLocalizedProductPrice(string productID)
//		{
//			decimal result = 0m;
//			if (InappBillingAndroid.m_StoreController != null)
//			{
//				result = InappBillingAndroid.m_StoreController.products.WithID(productID).metadata.localizedPrice;
//			}
//			return result;
//		}

//		public string GetISOCurrencyCode(string productID)
//		{
//			string result = string.Empty;
//			if (InappBillingAndroid.m_StoreController != null)
//			{
//				result = InappBillingAndroid.m_StoreController.products.WithID(productID).metadata.isoCurrencyCode;
//			}
//			return result;
//		}

//		public string GetFormatedProductPrice(string ISOCurrencyCode, decimal amount, int noDecimalFracment)
//		{
//			string empty = string.Empty;
//			noDecimalFracment = ((noDecimalFracment != 0) ? 2 : 0);
//			string currencySymbol = GetCurrencySymbol(ISOCurrencyCode);
//			return GetCurrencySymbol(ISOCurrencyCode) + " " + string.Format("{0:n" + noDecimalFracment + "}", amount);
//		}

//		private string GetCurrencySymbol(string ISOCurrencyCode)
//		{
//			if (ISOCurrencyCode == "USD" || ISOCurrencyCode == "AUD" || ISOCurrencyCode == "SGD")
//			{
//				return "$";
//			}
//			if (ISOCurrencyCode == "EUR")
//			{
//				return "â‚¬";
//			}
//			if (ISOCurrencyCode == "VND")
//			{
//				return "Ä‘";
//			}
//			if (ISOCurrencyCode == "KRW")
//			{
//				return "â‚©";
//			}
//			if (ISOCurrencyCode == "GBP")
//			{
//				return "Â£";
//			}
//			if (ISOCurrencyCode == "CNY")
//			{
//				return "Â¥";
//			}
//			if (ISOCurrencyCode == "JPY")
//			{
//				return "Â¥";
//			}
//			if (ISOCurrencyCode == "MYR")
//			{
//				return "RM";
//			}
//			if (ISOCurrencyCode == "CHF")
//			{
//				return "CHF";
//			}
//			if (ISOCurrencyCode == "INR")
//			{
//				return "â‚¹";
//			}
//			if (ISOCurrencyCode == "RUB")
//			{
//				return "â‚½";
//			}
//			if (ISOCurrencyCode == "THB")
//			{
//				return "à¸¿";
//			}
//			if (ISOCurrencyCode == "HKD")
//			{
//				return "HK$";
//			}
//			if (ISOCurrencyCode == "BRL")
//			{
//				return "R$";
//			}
//			return ISOCurrencyCode;
//		}

//		private static IStoreController m_StoreController;

//		private static IExtensionProvider m_StoreExtensionProvider;

//		private Dictionary<string, string> introductory_info_dict;

//		[SerializeField]
//		private ShopItemLookup shopItemLookup;

//		[SerializeField]
//		private OfferBundleLoader offerBundleLoader;

//		private string tempHeroItemID = string.Empty;
//	}
//}
