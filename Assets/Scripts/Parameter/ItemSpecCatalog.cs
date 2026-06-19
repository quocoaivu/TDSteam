using System.Collections.Generic;

namespace Parameter
{
	// In-memory catalog of every tower-item definition loaded from tower_item_parameter.txt. Lazy
	// singleton mirroring TurretAbilitySpec: TowerDataLoader fills it on Awake; ItemFactory reads it
	// to build random items. Statics reset between play sessions like the other spec holders.
	public class ItemSpecCatalog
	{
		public static ItemSpecCatalog Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ItemSpecCatalog();
				}
				return instance;
			}
		}

		// Every loaded spec (read-only). Used by the factory to roll a random item.
		public IReadOnlyList<ItemSpec> All
		{
			get
			{
				return all;
			}
		}

		public void SetParameter(ItemSpec spec)
		{
			all.Add(spec);
		}

		public void Clear()
		{
			all.Clear();
		}

		// Specs that fit a given tower (item.towerId == tower.Id). Allocates; not for hot paths.
		public List<ItemSpec> GetForTower(int towerId)
		{
			List<ItemSpec> result = new List<ItemSpec>();
			for (int i = 0; i < all.Count; i++)
			{
				if (all[i].towerId == towerId)
				{
					result.Add(all[i]);
				}
			}
			return result;
		}

		private readonly List<ItemSpec> all = new List<ItemSpec>();

		private static ItemSpecCatalog instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
