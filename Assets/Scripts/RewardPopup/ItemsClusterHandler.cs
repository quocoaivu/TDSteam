using System;
using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace RewardPopup
{
	public class ItemsClusterHandler : MonoBehaviour
	{
		public void InitValue(string bundleID)
		{
			DealPackCrystalNItems offerBundleItems = MonoSingleton<LifespanSurface>.Instance.OfferPopupController.OfferBundleLoader.GetOfferBundleItems(bundleID);
			gemAmount.text = offerBundleItems.gemAmount.ToString();
			for (int i = 0; i < itemsAvatar.Length; i++)
			{
				itemsAvatar[i].sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_pw_{0}", i));
				itemsAmount[i].text = offerBundleItems.itemsAmount[i].ToString();
			}
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Text gemAmount;

		[SerializeField]
		private Image[] itemsAvatar;

		[SerializeField]
		private Text[] itemsAmount;
	}
}
