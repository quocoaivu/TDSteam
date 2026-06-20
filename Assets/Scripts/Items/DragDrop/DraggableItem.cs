using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
	// Sits on a shop offer / inventory cell / equipped slot. Carries the item payload and where it came
	// from. One unified interaction (no hold-drag): a click picks the item up onto the cursor; clicking
	// again while carrying drops it onto the panel under the cursor (resolved via the parent IItemDropTarget).
	// World towers are handled separately by InputFilterDirector. Requires a raycast-target Graphic on the
	// same GameObject to receive the click.
	public class DraggableItem : MonoBehaviour, IPointerClickHandler
	{
		public TowerItem Payload
		{
			get
			{
				return payload;
			}
		}

		public void SetInventoryPayload(TowerItem item)
		{
			payload = item;
			source = DragSource.Inventory;
			sourceEquipment = null;
			shop = null;
			shopOffer = null;
		}

		public void SetShopPayload(TowerItem item, ItemShopPanel panel, ItemShopOfferButton offer)
		{
			payload = item;
			source = DragSource.Shop;
			sourceEquipment = null;
			shop = panel;
			shopOffer = offer;
		}

		public void SetTowerSlotPayload(TowerItem item, TowerEquipment equipment)
		{
			payload = item;
			source = DragSource.TowerSlot;
			sourceEquipment = equipment;
			shop = null;
			shopOffer = null;
		}

		// Empty cell/slot: nothing to pick up.
		public void ClearPayload()
		{
			payload = null;
			sourceEquipment = null;
			shop = null;
			shopOffer = null;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			// Already carrying: this click drops the carried item onto the panel this cell belongs to.
			if (ItemCarryController.IsCarryingItem)
			{
				ItemCarryController.Instance.DropOnto(GetComponentInParent<IItemDropTarget>());
				return;
			}
			// Otherwise pick this item up onto the cursor.
			if (payload == null)
			{
				return;
			}
			ItemCarryController.Instance.PickUp(payload, source, shop, shopOffer, sourceEquipment,
				GetComponentInParent<IItemPanel>());
		}

		private TowerItem payload;

		private DragSource source;

		private TowerEquipment sourceEquipment;

		private ItemShopPanel shop;

		private ItemShopOfferButton shopOffer;
	}
}
