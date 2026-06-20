using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
	// Popup listing every item the player holds this run, one cell per item. Click a cell to pick it up
	// (carry it onto a tower to equip). Also a drop target: clicking it while carrying a shop offer buys it
	// into the bag, carrying an equipped item here unequips it. Spawns cells from a prefab into a container
	// and pools them across opens. Open from a HUD button -> OpenInventory(). Mirrors ItemShopPanel.
	public class ItemInventoryPanel : GameplayDialogHandler, IItemDropTarget, IItemPanel, IPointerClickHandler
	{
		// Refresh whenever the bag changes (e.g. items returned from a sold tower don't go through this panel).
		private void OnEnable()
		{
			ItemInventory.Instance.OnChanged += RefreshAll;
			RefreshAll();
		}

		private void OnDisable()
		{
			ItemInventory.Instance.OnChanged -= RefreshAll;
		}

		// Clicking the panel while carrying drops the item here (clicks on a cell are routed here too).
		public void OnPointerClick(PointerEventData eventData)
		{
			if (ItemCarryController.IsCarryingItem)
			{
				ItemCarryController.Instance.DropOnto(this);
			}
		}

		// Rebuilds the cell list from the current inventory, then plays the popup open animation.
		public void OpenInventory()
		{
			RefreshAll();
			OpenWithScaleAnimation();
		}

		// Rebuilds the cells in place (after a drop changed the inventory), without re-animating.
		public void RefreshOpen()
		{
			RefreshAll();
		}

		public bool OnItemDropped(ItemCarryController carrier)
		{
			TowerItem item = carrier.Carried;
			if (item == null)
			{
				return false;
			}
			switch (carrier.Source)
			{
			case DragSource.Shop:
				// Buying adds the item to the inventory and marks the offer sold.
				if (carrier.Shop != null && carrier.Shop.TryBuy(carrier.ShopOffer))
				{
					RefreshAll();
					return true;
				}
				// TryBuy fails when the bag is full or there isn't enough gold.
				if (ItemInventory.Instance.IsFull)
				{
					ItemFeedback.InventoryFull();
				}
				else
				{
					ItemFeedback.NotEnoughGold();
				}
				return false;
			case DragSource.TowerSlot:
				if (carrier.SourceEquipment == null)
				{
					return false;
				}
				// Don't unequip into a full bag, or the item would be lost.
				if (ItemInventory.Instance.IsFull)
				{
					ItemFeedback.InventoryFull();
					return false;
				}
				carrier.SourceEquipment.Unequip(item);
				ItemInventory.Instance.Add(item);
				RefreshAll();
				return true;
			default:
				// Inventory -> inventory: nothing to do.
				return false;
			}
		}

		private void RefreshAll()
		{
			IReadOnlyList<TowerItem> items = ItemInventory.Instance.Items;
			// Fixed grid of slots; the bag never holds more than this (full adds are rejected).
			int slotCount = ItemInventory.CAPACITY;
			EnsurePool(slotCount);
			// Lay cells out in a grid ourselves (no Layout Group component needed). Cells anchor top-left with
			// pivot at their top-left, so column * step goes right and -row * step goes down. Slots past the
			// item count show as empty frames.
			for (int i = 0; i < cellPool.Count; i++)
			{
				bool visible = (i < slotCount);
				cellPool[i].gameObject.SetActive(visible);
				if (!visible)
				{
					continue;
				}
				if (i < items.Count)
				{
					cellPool[i].Bind(items[i]);
				}
				else
				{
					cellPool[i].Clear();
				}
				RectTransform rt = cellPool[i].transform as RectTransform;
				int column = i % COLUMNS;
				int row = i / COLUMNS;
				rt.anchoredPosition = new Vector2(column * CELL_STEP, -row * CELL_STEP);
			}
			// Size the content to the grid (COLUMNS wide, as many rows as the slots need).
			RectTransform content = cellContainer as RectTransform;
			if (content != null)
			{
				int rows = (slotCount + COLUMNS - 1) / COLUMNS;
				content.sizeDelta = new Vector2(COLUMNS * CELL_STEP, rows * CELL_STEP);
			}
			if (emptyLabel != null)
			{
				emptyLabel.SetActive(items.Count == 0);
			}
		}

		// Grows the pool to at least 'count' cells; cells are reused across opens (no per-open Instantiate
		// churn). Pooling per project convention.
		private void EnsurePool(int count)
		{
			if (count > 0 && (cellPrefab == null || cellContainer == null))
			{
				Debug.LogError("[Inventory] cellPrefab or cellContainer is not assigned in the Inspector.");
				return;
			}
			while (cellPool.Count < count)
			{
				ItemInventoryCell cell = Instantiate(cellPrefab, cellContainer, false);
				cellPool.Add(cell);
			}
		}

#if UNITY_EDITOR
		// Editor-only: seed a few items so the cell UI can be verified without farming drops/shop.
		// Right-click the component header in Play mode -> "Debug: Add Test Items". Remove before ship.
		[ContextMenu("Debug: Add Test Items")]
		private void DebugAddTestItems()
		{
			Debug.Log("[Inventory] DebugAddTestItems clicked");
			for (int i = 0; i < 3; i++)
			{
				TowerItem item = ItemFactory.CreateRandom();
				if (item != null)
				{
					ItemInventory.Instance.Add(item);
				}
			}
			// Self-open so we can verify the panel without relying on the HUD button wiring.
			OpenInventory();
		}
#endif

		[SerializeField]
		private ItemInventoryCell cellPrefab;

		[SerializeField]
		private Transform cellContainer;

		[SerializeField]
		private GameObject emptyLabel;

		// Cell size (64 in ItemCell.prefab) + spacing between cells.
		private const float CELL_STEP = 72f;

		// Items per row in the inventory grid.
		private const int COLUMNS = 3;

		private readonly List<ItemInventoryCell> cellPool = new List<ItemInventoryCell>();
	}
}
