using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Items
{
	// Item-equip section shown inside the tower popup (EnhanceTurretDialogHandler). One slot per ability
	// the tower's canonical prefab carries; equipping pulls the matching item from the in-run inventory
	// and activates it via TurretMasteryHandler. Per-tower (Phase 6): slots map to the abilities that
	// physically exist on the spawned prefab. Mirrors TowerSkillTreePanel.
	public class TowerItemPanel : MonoBehaviour
	{
		// Binds the panel to the tower whose popup just opened, then refreshes every slot.
		public void Init(TurretEntity tower)
		{
			currentTower = tower;
			for (int i = 0; i < slotButtons.Count; i++)
			{
				slotButtons[i].Init(this);
			}
			RefreshAll();
		}

		public void OnSlotClicked(ItemSlotButton slot)
		{
			TowerItem item = slot.OwnedItem;
			if (item == null)
			{
				return;
			}
			currentTower.towerUltimateController.EquipItem(slot.SlotIndex, item.level);
			RefreshAll();
		}

		private void RefreshAll()
		{
			for (int i = 0; i < slotButtons.Count; i++)
			{
				int slotIndex = slotButtons[i].SlotIndex;
				TowerItem owned = ItemInventory.Instance.GetBestItemForSlot(currentTower.Id, slotIndex);
				bool equipped = currentTower.towerUltimateController.GetEquippedLevel(slotIndex) >= 0;
				slotButtons[i].Refresh(owned, equipped);
			}
		}

		[SerializeField]
		private List<ItemSlotButton> slotButtons = new List<ItemSlotButton>();

		private TurretEntity currentTower;
	}
}
