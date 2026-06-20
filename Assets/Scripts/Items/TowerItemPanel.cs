using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
	// Item-equip section shown inside the tower popup (EnhanceTurretDialogHandler). Shows the items
	// currently equipped on the tower (up to TowerEquipment.SLOT_COUNT slots) and acts as a drop target:
	// clicking it while carrying an inventory item equips it; clicking an equipped slot picks it up to move
	// it out. Bind it to the open tower with Init(tower).
	public class TowerItemPanel : MonoBehaviour, IItemDropTarget, IItemPanel, IPointerClickHandler
	{
		// Clicking the panel while carrying drops the item here (clicks on a slot are routed here too).
		public void OnPointerClick(PointerEventData eventData)
		{
			if (ItemCarryController.IsCarryingItem)
			{
				ItemCarryController.Instance.DropOnto(this);
			}
		}

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

		public bool OnItemDropped(ItemCarryController carrier)
		{
			if (currentTower == null || currentTower.Equipment == null)
			{
				return false;
			}
			// Only inventory items can be equipped here; other sources are rejected (kept on the cursor).
			if (carrier.Source != DragSource.Inventory || carrier.Carried == null)
			{
				return false;
			}
			TowerEquipment.EquipBlock block = currentTower.Equipment.GetEquipBlock(carrier.Carried);
			if (block != TowerEquipment.EquipBlock.None)
			{
				ItemFeedback.Equip(block);
				return false;
			}
			currentTower.Equipment.Equip(carrier.Carried);
			ItemInventory.Instance.Remove(carrier.Carried);
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
