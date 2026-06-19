using Gameplay;
using TMPro;
using UnityEngine;

namespace Items
{
	// One equipped-item slot on the tower popup. Shows the item in this slot (or empty) and lets the
	// player drag it out to the inventory to unequip. Place one per equip slot (TowerEquipment.SLOT_COUNT)
	// and register it in TowerItemPanel.slotButtons.
	public class ItemSlotButton : MonoBehaviour
	{
		// item == null -> slot empty. equipment is the holder to unequip from when the slot is dragged out.
		public void Refresh(TowerItem item, TowerEquipment equipment)
		{
			if (nameText != null)
			{
				nameText.SetText(item != null ? item.name : "—");
			}
			if (emptyOverlay != null)
			{
				emptyOverlay.SetActive(item == null);
			}
			if (draggable != null)
			{
				if (item != null)
				{
					draggable.SetTowerSlotPayload(item, equipment);
				}
				else
				{
					draggable.ClearPayload();
				}
			}
		}

		[SerializeField]
		private TMP_Text nameText;

		[SerializeField]
		private GameObject emptyOverlay;

		[SerializeField]
		private DraggableItem draggable;
	}
}
