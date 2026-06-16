using System;
using Bootstrap;
using Data;
using Gameplay;
using LifetimePopup;
using GameCore;
using Services.PlatformSpecific;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Store
{
	public class AskToBuyDialogHandler : GameplayDialogHandler
	{
        [Header("Mua Hero max level")]
        [SerializeField]
        private string productIDBuyHeroMaxLevel;

        [SerializeField]
        private TMP_Text priceBuyHero;

        [SerializeField]
        private GameObject buyHeroMaxLevelGroup;

        [Header("Mua Hero 1 level")]
        [SerializeField]
        private GameObject buyHero1LevelGroup;

        [Header("Mua Hero pet")]
        [SerializeField]
        private string productIDBuyHeroPet;

        [SerializeField]
        private TMP_Text priceBuyHeroPet;

        [SerializeField]
        [FormerlySerializedAs("buyHeroPet")]
        private GameObject buyHeroPetGroup;

        private string currentHeroProductID;

        public void InitBuyHeroLevel(string productID)
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			HideAllGroups();
			currentHeroProductID = productID;
			int heroBundleOption = Bootstrap.GameBootstrap.Instance.RemoteConfig.GetHeroBundleOption();
			if (heroBundleOption != 0)
			{
				if (heroBundleOption == 1)
				{
					buyHero1LevelGroup.SetActive(true);
				}
			}
			else
			{
				buyHeroMaxLevelGroup.SetActive(true);
				SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(productIDBuyHeroMaxLevel);
				decimal num;
				if (GameUtils.IsInternetConnectionAvailable())
				{
                    num = NativeSpecificServicesSource.Services.InApPurchase.GetLocalizedProductPrice(productIDBuyHeroMaxLevel);
                }
				else
				{
					num = (decimal)dataSaleBundle.Defaultprice;
				}
                string isoCurrencyCode = NativeSpecificServicesSource.Services.InApPurchase.GetISOCurrencyCode(productIDBuyHeroMaxLevel);
                int noDecimalFraction = (int)BitConverter.GetBytes(decimal.GetBits(num)[3])[2];
                priceBuyHero.text = NativeSpecificServicesSource.Services.InApPurchase.GetFormatedProductPrice(isoCurrencyCode, num, noDecimalFraction);
            }
		}

		public void PurchaseHeroMaxLevel()
		{
			Close();
			base.gameObject.SetActive(false);
			HideAllGroups();
			//NativeSpecificServicesSource.Services.InApPurchase.PurchaseHeroMaxLevel(productIDBuyHeroMaxLevel, currentHeroProductID);
		}

		public void PurchaseHeroOneLevel()
		{
			//NativeSpecificServicesSource.Services.Ad.ShowOfferVideo(new OfferVideoCallback(OnOfferVideoCompletedHeroOneLevel));
		}

		private void OnOfferVideoCompletedHeroOneLevel(bool completed)
		{
			if (completed)
			{
				SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(currentHeroProductID);
				int[] heroIds = dataSaleBundle.Heroid;
				if (heroIds.Length == 1)
				{
					HeroStore.Instance.LevelUp(heroIds[0]);
				}
				Close();
				base.gameObject.SetActive(false);
				HideAllGroups();
				string localization = GameKit.GetLocalization("CONFIRMED_BUY_ONE_LEVEL");
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(localization, false, false);
			}
		}

		public void InitBuyHeroPet(string productID)
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			HideAllGroups();
			buyHeroPetGroup.SetActive(true);
			currentHeroProductID = productID;
			SalePackSetupRecord dataSaleBundle = ShopPackRecord.GetDataSaleBundle(productIDBuyHeroPet);
			decimal num;
			if (GameUtils.IsInternetConnectionAvailable())
			{
                num = NativeSpecificServicesSource.Services.InApPurchase.GetLocalizedProductPrice(productIDBuyHeroPet);
            }
			else
			{
				num = (decimal)dataSaleBundle.Defaultprice;
			}
            string isoCurrencyCode = NativeSpecificServicesSource.Services.InApPurchase.GetISOCurrencyCode(productIDBuyHeroPet);
            int noDecimalFraction = (int)BitConverter.GetBytes(decimal.GetBits(num)[3])[2];
            priceBuyHeroPet.text = NativeSpecificServicesSource.Services.InApPurchase.GetFormatedProductPrice(isoCurrencyCode, num, noDecimalFraction);
        }

		public void PurchaseHeroPet()
		{
			Close();
			base.gameObject.SetActive(false);
			HideAllGroups();
			//NativeSpecificServicesSource.Services.InApPurchase.PurchaseHeroPet(productIDBuyHeroPet, currentHeroProductID);
		}

		private void HideAllGroups()
		{
			buyHeroMaxLevelGroup.SetActive(false);
			buyHero1LevelGroup.SetActive(false);
			buyHeroPetGroup.SetActive(false);
		}


	}
}
