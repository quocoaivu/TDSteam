using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
	// Makes a shop offer / inventory cell / equipped slot draggable. Carries the item payload and where
	// it came from. On release it resolves the drop:
	//   1. a UI IItemDropTarget under the cursor (inventory panel, tower equip area), or
	//   2. (inventory source only) a tower sprite in the world -> equip via raycast, or
	//   3. nothing -> snap back (the source list is untouched, so the cell stays put).
	// Requires a raycast-target Graphic on the same GameObject (Image/Text) to receive drag events.
	public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public TowerItem Payload
		{
			get
			{
				return payload;
			}
		}

		public DragSource Source
		{
			get
			{
				return source;
			}
		}

		// Set on a TowerSlot-sourced drag: the equipment to unequip from when dropped onto the inventory.
		public TowerEquipment SourceEquipment
		{
			get
			{
				return sourceEquipment;
			}
		}

		// Set on a Shop-sourced drag so the inventory drop target can run the purchase.
		public ItemShopPanel Shop
		{
			get
			{
				return shop;
			}
		}

		public ItemShopOfferButton ShopOffer
		{
			get
			{
				return shopOffer;
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

		// Empty cell/slot: nothing to drag.
		public void ClearPayload()
		{
			payload = null;
			sourceEquipment = null;
			shop = null;
			shopOffer = null;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (payload == null)
			{
				return;
			}
			DragLayer.Instance.BeginDrag(payload);
			DragLayer.Instance.MoveTo(eventData.position);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (payload == null)
			{
				return;
			}
			DragLayer.Instance.MoveTo(eventData.position);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (payload == null)
			{
				return;
			}
			DragLayer.Instance.EndDrag();

			bool handled = false;
			IItemDropTarget target = FindDropTarget(eventData);
			if (target != null)
			{
				handled = target.OnItemDropped(this);
			}
			if (!handled && source == DragSource.Inventory)
			{
				handled = TryEquipOnWorldTower(eventData.position);
			}
			if (handled)
			{
				RefreshSourcePanel();
			}
		}

		// Walks the UI raycast hits under the cursor looking for a drop target.
		private IItemDropTarget FindDropTarget(PointerEventData eventData)
		{
			if (EventSystem.current == null)
			{
				return null;
			}
			raycastResults.Clear();
			EventSystem.current.RaycastAll(eventData, raycastResults);
			for (int i = 0; i < raycastResults.Count; i++)
			{
				IItemDropTarget t = raycastResults[i].gameObject.GetComponentInParent<IItemDropTarget>();
				if (t != null)
				{
					return t;
				}
			}
			return null;
		}

		// Dropping an inventory item onto a tower sprite in the world equips it there.
		private bool TryEquipOnWorldTower(Vector2 screenPosition)
		{
			TurretEntity tower;
			if (!InputFilterDirector.RaycastTower(screenPosition, out tower))
			{
				return false;
			}
			if (tower.Equipment == null || !tower.Equipment.Equip(payload))
			{
				return false;
			}
			ItemInventory.Instance.Remove(payload);
			return true;
		}

		private void RefreshSourcePanel()
		{
			switch (source)
			{
			case DragSource.Inventory:
			{
				ItemInventoryPanel panel = GetComponentInParent<ItemInventoryPanel>();
				if (panel != null)
				{
					panel.RefreshOpen();
				}
				break;
			}
			case DragSource.TowerSlot:
			{
				TowerItemPanel panel = GetComponentInParent<TowerItemPanel>();
				if (panel != null)
				{
					panel.RefreshOpen();
				}
				break;
			}
			case DragSource.Shop:
				if (shop != null)
				{
					shop.RefreshOpen();
				}
				break;
			}
		}

		private TowerItem payload;

		private DragSource source;

		private TowerEquipment sourceEquipment;

		private ItemShopPanel shop;

		private ItemShopOfferButton shopOffer;

		private static readonly List<RaycastResult> raycastResults = new List<RaycastResult>();
	}
}
