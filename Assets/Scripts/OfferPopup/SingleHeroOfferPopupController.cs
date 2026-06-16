using System;
using System.Collections.Generic;
using DailyTrial;
using Gameplay;
using GeneralVariable;
using LifetimePopup;
using MetaGame;
using Services.PlatformSpecific;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OfferPopup
{
	public class SingleHeroOfferPopupController : GameplayDialogHandler
	{
        [Space]
        [Header("Title")]
        [SerializeField]
        private TMP_Text title;

        [SerializeField]
        private Image heroAvatar;

        [SerializeField]
        private Image heroName;

        [SerializeField]
        private Image[] heroSkills;

        [SerializeField]
        private GameObject timeCountDown;

        [SerializeField]
        private GameObject notiOneTime;

        [Space]
        [SerializeField]
        private Image[] itemsAvatar;

        [SerializeField]
        private TMP_Text[] itemsAmount;

        [Space]
        [SerializeField]
        private TMP_Text oldPrice;

        [SerializeField]
        private TMP_Text newPrice;

        [SerializeField]
        private TMP_Text saleRate;

        private static readonly Dictionary<int, string> bundleTitles = new Dictionary<int, string>
        {
            { 0, "Bundle Wukong" },
            { 3, "Bundle Shaman King" },
            { 4, "Bundle Golem" },
            { 5, "Bundle Nature Queen" },
            { 6, "Bundle Thor" },
            { 7, "Bundle Ninja Assasin" },
            { 8, "Bundle Tristana" },
            { 9, "Bundle Jungle Lord" }
        };

        private string bundleID;


        public void Init(int heroID, DealKind type)
		{
			if (type != DealKind.OneTime)
			{
				if (type == DealKind.TimeCountDown)
				{
					if (timeCountDown && notiOneTime)
					{
						timeCountDown.SetActive(true);
						notiOneTime.SetActive(false);
					}
				}
			}
			else if (timeCountDown && notiOneTime)
			{
				timeCountDown.SetActive(false);
				notiOneTime.SetActive(true);
			}
			OpenWithScaleAnimation();
			InitAvatarHero(heroID);
			InitBundleInformation(heroID);
		}

		private void InitAvatarHero(int heroID)
		{
			heroAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesAvatar/avatar_hero_{0}", heroID));
			heroName.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesName/name_hero_{0}", heroID));
			for (int i = 0; i < heroSkills.Length; i++)
			{
				heroSkills[i].sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroCamp/SkillIcons/hero_{0}_skill_{1}", heroID, i));
			}
		}

		private void InitBundleInformation(int heroID)
		{
            bundleID = MarketingSetup.productIDHeroPackOffer[heroID];
            if (!bundleTitles.TryGetValue(heroID, out string bundleTitle))
            {
                return;
            }
            int dataValueItem = 5;
            string priceNew = "$50";
            string priceOld = "$100";
            string rateSale = "50%";
            title.text = bundleTitle;
            for (int i = 0; i < itemsAmount.Length; i++)
            {
                itemsAmount[i].text = dataValueItem.ToString();
            }
            newPrice.text = priceNew;
            oldPrice.text = priceOld;
            saleRate.text = rateSale;

            //bundleID = MarketingSetup.productIDHeroPackOffer[heroID];
            ////title.text = NativeSpecificServicesSource.Services.InApPurchase.GetLocalizedProductTitle(bundleID);
            //title.text = SingletonMonoBehaviour<LifespanSurface>.Instance.OfferPopupController.OfferBundleLoader.GetOfferBundleSingleHero(productID);
            //DealPackSingleHero offerBundleSingleHero = SingletonMonoBehaviour<LifespanSurface>.Instance.OfferPopupController.OfferBundleLoader.GetOfferBundleSingleHero(bundleID);
            //UnityEngine.Debug.Log(offerBundleSingleHero);
            //for (int i = 0; i < itemsAvatar.Length; i++)
            //{
            //    itemsAvatar[i].sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_pw_{0}", i));
            //    itemsAmount[i].text = offerBundleSingleHero.itemsAmount[i].ToString();
            //}
            //int num = offerBundleSingleHero.saleRate;
            //saleRate.text = num.ToString() + "%";
            //decimal localizedProductPrice = SingletonMonoBehaviour<LifespanSurface>.Instance.OfferPopupController.OfferBundleLoader.GetOfferBundleSingleHero(bundleID);
            //decimal d = (decimal)(1f - (float)num / 100f);
            //decimal amount = decimal.Divide(localizedProductPrice, d);
            //string isocurrencyCode = (bundleID);
            //int noDecimalFracment = (int)BitConverter.GetBytes(decimal.GetBits(localizedProductPrice)[3])[2];
            //newPrice.text = SingletonMonoBehaviour<LifespanSurface>.Instance.OfferPopupController.OfferBundleLoader.GetOfferBundleSingleHero(newPrice);
            //oldPrice.text = SingletonMonoBehaviour<LifespanSurface>.Instance.OfferPopupController.OfferBundleLoader.GetOfferBundleSingleHero(oldPrice);
        }

        public void ProcessBuyItem()
		{
			// TODO: re-wire after IAP migration. See Docs/OPENIAB_REMOVAL_REINSTALL.md
			//NativeSpecificServicesSource.Services.InApPurchase.PurchaseOfferBundle(bundleID);
            // OpenIAB removed 2026-05-19. Original product IDs preserved below:
            //   wukong      -> com.developer.kingdom.defense.iap6
            //   shamanking  -> com.developer.kingdom.defense.iap7
            //   golem       -> com.developer.kingdom.defense.iap8
            //   naturequeen -> com.developer.kingdom.defense.iap9
            //   thor        -> com.developer.kingdom.defense.iap10
            //   ninjaassain -> com.developer.kingdom.defense.iap11
            //   tristana    -> com.developer.kingdom.defense.iap12
            //   junglelord  -> com.developer.kingdom.defense.iap13
            UnityEngine.Debug.Log("[IAP-stub] Buy Bundle " + bundleID);


            GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					CloseWithScaleAnimation();
				}
				else
				{
					CloseWithScaleAnimation();
					Continue();
				}
			}
			else
			{
				CloseWithScaleAnimation();
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag(GeneralVariable.GeneralDefine.WATCH_OFFER_BUTTON);
			if (gameObject)
			{
				gameObject.GetComponent<WatchDealSwitchHandler>().TurnOffButton();
			}
		}

		public void NotNow()
		{
			switch (FormatDirector.Instance.gameMode)
			{
			case GameFormat.CampaignMode:
				CloseWithScaleAnimation();
				break;
			case GameFormat.DailyTrialMode:
				CloseWithScaleAnimation();
				Continue();
				break;
			case GameFormat.TournamentMode:
				CloseWithScaleAnimation();
				break;
			default:
				CloseWithScaleAnimation();
				break;
			}
		}

		public void Continue()
		{
			LoadingScreen.Instance.ShowLoading();
			base.Invoke("DoLoad", 1f);
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
			FormatDirector.Instance.gameMode = GameFormat.CampaignMode;
		}

		private void DoLoad()
		{
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.WorldMapSceneName);
		}


	}
}
