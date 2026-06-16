using System;
using UnityEngine;

namespace OfferPopup
{
	public class OfferBundleLoader : MonoBehaviour
	{
		public DealPackSingleHero GetOfferBundleSingleHero(string bundleID)
		{
			DealPackSingleHero result = new DealPackSingleHero();
			foreach (DealPackSingleHero offerBundleSingleHero in offerBundle.bundlesSingleHero)
			{
				if (offerBundleSingleHero.offerBundleID.Equals(bundleID))
				{
					result = offerBundleSingleHero;
				}
			}
			return result;
		}

		public DealPackCrystalNItems GetOfferBundleItems(string bundleID)
		{
			DealPackCrystalNItems result = new DealPackCrystalNItems();
			foreach (DealPackCrystalNItems offerBundleGemNItems in offerBundle.bundlesItems)
			{
				if (offerBundleGemNItems.offerBundleID.Equals(bundleID))
				{
					result = offerBundleGemNItems;
				}
			}
			return result;
		}

		public OfferBundleComboHeroes GetOfferBundleComboHeroes(string bundleID)
		{
			OfferBundleComboHeroes result = new OfferBundleComboHeroes();
			foreach (OfferBundleComboHeroes offerComboHeroes in offerBundle.bundlesComboHeroes)
			{
				if (offerComboHeroes.offerBundleID.Equals(bundleID))
				{
					result = offerComboHeroes;
				}
			}
			return result;
		}

		[SerializeField]
		private DealPack offerBundle;
	}
}
