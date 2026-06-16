using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Store
{
	public class ShopItemLookup : MonoBehaviour
	{
		[SerializeField]
		[FormerlySerializedAs("shopItemAttribute")]
		private ShopItemData shopItemData;

		public int GetGemPackValue(string productID)
		{
			int result = -1;
			foreach (CrystalPack gemPack in shopItemData.listGemPack)
			{
				if (gemPack.gemPackID.Equals(productID))
				{
					result = gemPack.gemPackValue;
				}
			}
			return result;
		}

		public string GetGemPackTitle(string productID)
		{
			return "Test Title";
		}

		public string GetGemPackDescription(string productID)
		{
			return "Test Description";
		}

		public string GetGemPackPriceString(string productID)
		{
			return "$0.01";
		}

		public string GetHeroItemID(int heroID)
		{
			string result = string.Empty;
			foreach (HeroCard heroItem in shopItemData.listHeroItem)
			{
				if (heroItem.heroID == heroID)
				{
					result = heroItem.heroItemID;
				}
			}
			return result;
		}

		public int GetHeroID(string productID)
		{
			int result = -1;
			foreach (HeroCard heroItem in shopItemData.listHeroItem)
			{
				if (heroItem.heroItemID.Equals(productID))
				{
					result = heroItem.heroID;
				}
			}
			return result;
		}
	}
}
