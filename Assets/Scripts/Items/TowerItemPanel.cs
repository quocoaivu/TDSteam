using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Items
{
	// Item-equip section shown inside the tower popup (EnhanceTurretDialogHandler). Shows the items
	// currently equipped on the tower (up to TowerEquipment.SLOT_COUNT slots) and acts as a drop target:
	// dragging an inventory item onto it equips it; dragging an equipped slot out (to the inventory)
	// unequips it. Bind it to the open tower with Init(tower).
	public class TowerItemPanel : MonoBehaviour, IItemDropTarget
	{
		// Binds the panel to the tower whose popup just opened, then refreshes every slot.
		public void Init(TurretEntity tower)
		{
			currentTower = tower;
			RefreshAll();
		}

		// Rebuilds the slots in place (after a drop changed what's equipped).
		public void RefreshOpen()
		{
			RefreshAll();
		}

		public bool OnItemDropped(DraggableItem dragged)
		{
			if (currentTower == null || currentTower.Equipment == null)
			{
				return false;
			}
			// Only inventory items can be equipped here; other sources snap back.
			if (dragged.Source != DragSource.Inventory || dragged.Payload == null)
			{
				return false;
			}
			if (!currentTower.Equipment.Equip(dragged.Payload))
			{
				return false;
			}
			ItemInventory.Instance.Remove(dragged.Payload);
			RefreshAll();
			return true;
		}

		private void RefreshAll()
		{
			IReadOnlyList<TowerItem> equipped = (currentTower != null && currentTower.Equipment != null)
				? currentTower.Equipment.Equipped
				: null;
			TowerEquipment equipment = (currentTower != null) ? currentTower.Equipment : null;
			for (int i = 0; i < slotButtons.Count; i++)
			{
				TowerItem item = (equipped != null && i < equipped.Count) ? equipped[i] : null;
				slotButtons[i].Refresh(item, equipment);
			}
		}

		[SerializeField]
		private List<ItemSlotButton> slotButtons = new List<ItemSlotButton>();

		private TurretEntity currentTower;
	}
}
