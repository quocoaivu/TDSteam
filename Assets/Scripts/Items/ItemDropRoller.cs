using GameCore;
using Gameplay;
using UnityEngine;

namespace Items
{
	// Rolls a chance on each enemy death to drop a tower item into the in-run inventory. Auto-collected
	// (matching the game's gem/gold drops); a floating indicator shows at the kill position. A gold
	// reroll shop is the other acquisition path (ItemShopPanel).
	public static class ItemDropRoller
	{
		private const int DROP_CHANCE_PERCENT = 8;

		public static void TryDropOnKill(Vector3 position)
		{
			if (Random.Range(0, 100) >= DROP_CHANCE_PERCENT)
			{
				return;
			}
			TowerItem item = ItemFactory.CreateRandom();
			if (item == null)
			{
				return;
			}
			ItemInventory.Instance.Add(item);
			ShowDropVisual(position, item.name);
		}

		private static void ShowDropVisual(Vector3 position, string name)
		{
			ItemDropHandler visual = MonoSingleton<FXPool>.Instance.GetDroppedItem();
			if (visual == null)
			{
				return;
			}
			visual.gameObject.SetActive(true);
			visual.transform.position = position;
			visual.Init(name);
		}
	}
}
