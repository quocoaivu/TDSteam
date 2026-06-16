using System;
using Data;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
	public class PowerUpStoreItem : SwitchHandler
	{
		[SerializeField]
		private int itemID;

		[SerializeField]
		private Text itemName;

		[SerializeField]
		private Text itemQuantity;

		[SerializeField]
		private Text itemDescription;

		[SerializeField]
		private Text itemPrice;

		public void UpdateItemsQuantity()
		{
            itemQuantity.text = PowerUpItemStore.Instance.GetCurrentItemQuantity(itemID).ToString();
		}

		public void InitInfo()
		{
			itemName.text = Singleton<PowerUpItemDescription>.Instance.GetName(itemID);
			UpdateItemsQuantity();
			itemDescription.text = Singleton<PowerUpItemDescription>.Instance.GetDescription(itemID).Replace('@', '\n').Replace('#', '-');
			itemPrice.text = Singleton<PowerUpItemSpec>.Instance.GetPrice(itemID).ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			TryToBuyItem();
		}

		private void TryToBuyItem()
		{
			if (ShopCalculator.IsEnoughMoneyToBuy(itemID))
			{
				ProcessItem();
			}
			else
			{
				MonoSingleton<LifespanSurface>.Instance.StorePopupController.PlayAnimationNotEnoughGem();
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(20);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, true, false);
			}
		}

		private void ProcessItem()
		{
			int price = Singleton<PowerUpItemSpec>.Instance.GetPrice(itemID);
			PlayerCurrencyStore.Instance.ChangeGem(-price, true);
			PowerUpItemStore.Instance.ChangeItemQuantity(itemID, 1);
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.ShowBuyEffect();
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.UpdateGemStatus();
			SendEventBuyPowerupItem();
			UISfxDirector.Instance.PlayBuySuccess();
			UpdateItemsQuantity();
		}

		private void SendEventBuyPowerupItem()
		{
			int id = itemID;
			int price = Singleton<PowerUpItemSpec>.Instance.GetPrice(id);
			string itemDisplayName = Singleton<PowerUpItemDescription>.Instance.GetName(id);
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_BuyPowerupItem(itemDisplayName, price);
		}
	}
}
