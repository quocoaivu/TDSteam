using System;
using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace RewardPopup
{
	public class SingleHeroClusterHandler : MonoBehaviour
	{
		public void InitValue(string bundleID)
		{
			DealPackSingleHero offerBundleSingleHero = MonoSingleton<LifespanSurface>.Instance.OfferPopupController.OfferBundleLoader.GetOfferBundleSingleHero(bundleID);
			heroAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_hero_{0}", offerBundleSingleHero.heroID));
			for (int i = 0; i < itemsAvatar.Length; i++)
			{
				itemsAvatar[i].sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_pw_{0}", i));
				itemsAmount[i].text = offerBundleSingleHero.itemsAmount[i].ToString();
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
		private Image heroAvatar;

		[SerializeField]
		private Image[] itemsAvatar;

		[SerializeField]
		private Text[] itemsAmount;
	}
}
