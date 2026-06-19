using System.Collections.Generic;

namespace Items
{
	// In-run bag of tower items the player currently holds. NOT persisted: items are picked up during
	// a match and reset between runs (roguelite). Equipping an item moves it onto a tower's
	// TowerEquipment (drag & drop); selling/unequipping returns it here. Lazy singleton -> no scene
	// wiring, like TowerSkillPointStore. Cleared per run from GameplayDirector.
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

		// Read-only view of everything the player holds this run (for the inventory UI). Display only.
		public IReadOnlyList<TowerItem> Items
		{
			get
			{
				return items;
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

		private readonly List<TowerItem> items = new List<TowerItem>();

		private static ItemInventory instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
