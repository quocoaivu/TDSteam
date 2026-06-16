using System;
using Data;
using LifetimePopup;
using GameCore;
using RewardPopup;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SalePackItem : SwitchHandler
{
	private void Update()
	{
		if (base.gameObject.activeSelf && bundleParam != null && bundleParam.Bundletype.Equals(ShopPackKind.TimeLimited.ToString()) && timeCountdown && currentTimeCount.TotalSeconds > 0.0)
		{
			UpdateTimeCountdown();
		}
	}

	private void UpdateTimeCountdown()
	{
		currentTimeCount = currentTimeCount.Subtract(TimeSpan.FromSeconds((double)Time.deltaTime));
		if (currentTimeCount.TotalSeconds < 0.0)
		{
			Hide();
			SaleBundleStore.Instance.SetSpecialPackExpired(productID);
			SaleBundleStore.Instance.SetLastTimePlay();
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.SaleBundleGroupController.RefreshItemStatus();
			return;
		}
		string text = string.Format("{0:D2}:{1:D2}:{2:D2}", currentTimeCount.Hours + currentTimeCount.Days * 24, currentTimeCount.Minutes, currentTimeCount.Seconds);
		timeCountdown.text = text;
	}

	public virtual void Init()
	{
		bundleParam = ShopPackRecord.GetDataSaleBundle(productID);
		SetTitle();
		SetListHero();
		SetListItem();
		SetListPrice();
		if (bundleParam.Bundletype.Equals(ShopPackKind.TimeLimited.ToString()))
		{
			currentTimeCount = SaleBundleStore.Instance.getCountdownTime(productID);
			Show();
		}
		else
		{
			Show();
		}
	}

	public void RefreshStatus()
	{
		bundleParam = ShopPackRecord.GetDataSaleBundle(productID);
		int[] heroid = bundleParam.Heroid;
		bool flag = false;
		if (heroid.Length > 0)
		{
			for (int i = 0; i < heroid.Length; i++)
			{
				if (HeroStore.Instance.IsHeroOwned(heroid[i]))
				{
					flag = true;
					Hide();
				}
			}
		}
		if (flag)
		{
			Hide();
		}
	}

	private void SetTitle()
	{
		if (title)
		{
			title.text = GameKit.GetLocalization(titleID);
		}
	}

	private void SetListHero()
	{
		int[] heroid = bundleParam.Heroid;
		if (heroid.Length == 1)
		{
			int num = heroid[0];
			if (heroAvatar)
			{
				heroAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesAvatar/avatar_hero_{0}", num));
			}
			if (heroName)
			{
				heroName.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesName/name_hero_{0}", num));
			}
			if (heroLevel)
			{
				heroLevel.text = (bundleParam.Herolevel + 1).ToString();
			}
		}
	}

	public virtual void SetListItem()
	{
		int[] itemids = bundleParam.Itemids;
		int[] itemquatities = bundleParam.Itemquatities;
		int gembonus = bundleParam.Gembonus;
		if (itemids.Length > 0 && gembonus > 0)
		{
			PrizeItem[] array = new PrizeItem[itemids.Length + 1];
			array[0] = new PrizeItem
			{
				rewardType = PrizeKind.Gem,
				value = gembonus,
				isDisplayQuantity = true
			};
			for (int i = 0; i < itemids.Length; i++)
			{
				PrizeItem rewardItem = new PrizeItem();
				rewardItem.rewardType = PrizeKind.Item;
				rewardItem.itemID = itemids[i];
				rewardItem.value = itemquatities[i];
				rewardItem.isDisplayQuantity = true;
				array[i + 1] = rewardItem;
			}
			if (itemGroup)
			{
				itemGroup.InitListItems(array);
			}
		}
		else if (itemGroup)
		{
			itemGroup.HideAllItems();
		}
	}

	private void SetListPrice()
	{
		//int saleoffpercent = bundleParam.Saleoffpercent;
		//decimal num;
		//if (GameUtils.IsInternetConnectionAvailable())
		//{
  //          num = NativeSpecificServicesSource.Services.InApPurchase.GetLocalizedProductPrice(productID);
  //      }
		//else
		//{
		//	num = (decimal)bundleParam.Defaultprice;
		//}
  //      string isocurrencyCode = NativeSpecificServicesSource.Services.InApPurchase.GetISOCurrencyCode(productID);
  //      int noDecimalFracment = (int)BitConverter.GetBytes(decimal.GetBits(num)[3])[2];
  //      currentPrice.text = NativeSpecificServicesSource.Services.InApPurchase.GetFormatedProductPrice(isocurrencyCode, num, noDecimalFracment);
  //      if (saleOffRate != null)
		//{
		//	saleOffRate.text = "-" + saleoffpercent.ToString() + "%";
		//}
	}

	public override void OnClick()
	{
		base.OnClick();
		ProcessPurchase();
	}

	public virtual void ProcessPurchase()
	{
		//NativeSpecificServicesSource.Services.InApPurchase.PurchaseSaleBundle(productID);
	}

	private void Show()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		onShow.Invoke();
	}

	public void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
		onHide.Invoke();
	}

	public UnityEvent onShow;

	public UnityEvent onHide;

	[Space]
	public string productID;

	public string titleID;

	[Space]
	public Text title;

	public Text currentPrice;

	[SerializeField]
	private Text saleOffRate;

	[HideInInspector]
	public SalePackSetupRecord bundleParam;

	[Space]
	[Header("Show up item")]
	public PrizeItemClusterHandler itemGroup;

	[SerializeField]
	private Image heroAvatar;

	[SerializeField]
	private Image heroName;

	[SerializeField]
	private Text heroLevel;

	[Space]
	[Header("Time Limited Sale")]
	[SerializeField]
	private Text timeCountdown;

	private TimeSpan currentTimeCount;
}
