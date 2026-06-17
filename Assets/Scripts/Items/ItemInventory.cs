using System.Collections.Generic;

namespace Items
{
	// In-run bag of tower items the player currently holds. NOT persisted: items are picked up during
	// a match and reset between runs (roguelite). Equipping an item activates the tower's matching
	// ability via TurretMasteryHandler.EquipItem. Lazy singleton -> no scene wiring, like
	// TowerSkillPointStore. (Drop sources + per-run reset wiring come in Phase 7.)
	public class ItemInventory
	{
		public static ItemInventory Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ItemInventory();
				}
				return instance;
			}
		}

		public void Add(TowerItem item)
		{
			items.Add(item);
		}

		public void Remove(TowerItem item)
		{
			items.Remove(item);
		}

		public void Clear()
		{
			items.Clear();
		}

		// Best owned item for a tower's ability slot (highest level), or null if the player owns none.
		public TowerItem GetBestItemForSlot(int towerID, int slotIndex)
		{
			TowerItem best = null;
			for (int i = 0; i < items.Count; i++)
			{
				TowerItem item = items[i];
				if (item.towerID != towerID || item.slotIndex != slotIndex)
				{
					continue;
				}
				if (best == null || item.level > best.level)
				{
					best = item;
				}
			}
			return best;
		}

		private readonly List<TowerItem> items = new List<TowerItem>();

		private static ItemInventory instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
