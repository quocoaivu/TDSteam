using System;
using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace RewardPopup
{
	public class MultiHeroClusterHandler : MonoBehaviour
	{
		public void InitValue(string bundleID)
		{
			OfferBundleComboHeroes offerBundleComboHeroes = MonoSingleton<LifespanSurface>.Instance.OfferPopupController.OfferBundleLoader.GetOfferBundleComboHeroes(bundleID);
			for (int i = 0; i < heroesAvatar.Length; i++)
			{
				heroesAvatar[i].sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_hero_{0}", offerBundleComboHeroes.listHeroesID[i]));
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
		private Image[] heroesAvatar;
	}
}
