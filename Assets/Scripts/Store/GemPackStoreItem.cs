using System;
using LifetimePopup;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
	public class GemPackStoreItem : SwitchHandler
	{
		[SerializeField]
		private string productID;

		[Space]
		[Header("UI")]
		[SerializeField]
		private Text titleText;

		[SerializeField]
		private Text descriptionText;

		[SerializeField]
		private Text priceText;

		[SerializeField]
		private Text valueText;

		private void Start()
		{
			UpdateText();
		}

		private void OnEnable()
		{
			UpdateText();
		}

		public override void OnClick()
		{
			base.OnClick();
			ProcessPurchase();
		}

		private void ProcessPurchase()
		{
			//NativeSpecificServicesSource.Services.InApPurchase.PurchaseGem(productID);
		}

		private void UpdateText()
		{
			if (titleText != null)
			{
				//titleText.text = NativeSpecificServicesSource.Services.InApPurchase.GetLocalizedProductTitle(productID);
			}
			if (descriptionText != null)
			{
				//descriptionText.text = NativeSpecificServicesSource.Services.InApPurchase.GetLocalizedProductDescription(productID);
			}
			if (priceText != null)
			{
                decimal localizedProductPrice = NativeSpecificServicesSource.Services.InApPurchase.GetLocalizedProductPrice(productID);
                string isocurrencyCode = NativeSpecificServicesSource.Services.InApPurchase.GetISOCurrencyCode(productID);
                int noDecimalFracment = (int)BitConverter.GetBytes(decimal.GetBits(localizedProductPrice)[3])[2];
				//priceText.text = NativeSpecificServicesSource.Services.InApPurchase.GetFormatedProductPrice(isocurrencyCode, localizedProductPrice, noDecimalFracment);
			}
			if (valueText != null)
			{
				valueText.text = MonoSingleton<LifespanSurface>.Instance.StorePopupController.ShopItemLookup.GetGemPackValue(productID).ToString();
			}
		}
	}
}
