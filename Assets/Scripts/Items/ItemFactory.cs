using Parameter;
using UnityEngine;

namespace Items
{
	// Builds TowerItem instances from the loaded ItemSpec catalog. Shared by drops (ItemDropRoller) and
	// the shop (ItemShopPanel) so the "pick a definition -> runtime item" rule lives in one place.
	public static class ItemFactory
	{
		// Rolls a random item definition. Returns null if no specs are loaded (data missing).
		public static TowerItem CreateRandom()
		{
			System.Collections.Generic.IReadOnlyList<ItemSpec> all = ItemSpecCatalog.Instance.All;
			if (all.Count == 0)
			{
				return null;
			}
			return Create(all[Random.Range(0, all.Count)]);
		}

		public static TowerItem Create(ItemSpec spec)
		{
			return new TowerItem(spec.itemId, spec.towerId, spec.name, spec.statTypes, spec.statValues, spec.rarity, spec.icon);
		}
	}
}
