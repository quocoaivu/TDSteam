using Parameter;
using UnityEngine;

namespace Items
{
	// Builds TowerItem instances from the loaded ItemSpec catalog. Shared by drops (ItemDropRoller) and
	// the shop (ItemShopPanel) so the "pick a definition -> runtime item" rule lives in one place.
	public static class ItemFactory
	{
		// Rolls a random item definition, weighted by rarity so stronger (higher-rarity) items show up
		// less often. Returns null if no specs are loaded (data missing).
		public static TowerItem CreateRandom()
		{
			System.Collections.Generic.IReadOnlyList<ItemSpec> all = ItemSpecCatalog.Instance.All;
			if (all.Count == 0)
			{
				return null;
			}
			int totalWeight = 0;
			for (int i = 0; i < all.Count; i++)
			{
				totalWeight += RarityWeight(all[i].rarity);
			}
			int roll = Random.Range(0, totalWeight);
			for (int i = 0; i < all.Count; i++)
			{
				roll -= RarityWeight(all[i].rarity);
				if (roll < 0)
				{
					return Create(all[i]);
				}
			}
			return Create(all[all.Count - 1]); // fallback, shouldn't be reached
		}

		// Per-item drop weight by rarity: each common item is 6x as likely as each rare item.
		private static int RarityWeight(int rarity)
		{
			switch (rarity)
			{
			case 0:
				return 60;
			case 1:
				return 30;
			default:
				return 10; // rarity 2+
			}
		}

		public static TowerItem Create(ItemSpec spec)
		{
			return new TowerItem(spec.itemId, spec.towerId, spec.name, spec.statTypes, spec.statValues, spec.rarity, spec.icon, spec.skillBranch, spec.skillId);
		}
	}
}
