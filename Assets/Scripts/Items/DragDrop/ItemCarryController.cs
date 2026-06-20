using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Items
{
	// The single item-movement model: click an item to pick it up (it sticks to the cursor via DragLayer),
	// then click a destination to place it (a tower in the world, or a UI drop target like the inventory /
	// tower equip panel). ESC drops it (handled centrally in UIRootHandler). Carries where the item came
	// from so the source is consumed correctly on placement (shop -> pay, inventory -> remove, tower slot
	// -> unequip). Self-builds a persistent host on first use, like DragLayer, so no scene wiring is needed.
	public class ItemCarryController : MonoBehaviour
	{
		public static ItemCarryController Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject go = new GameObject("ItemCarryController");
					DontDestroyOnLoad(go);
					instance = go.AddComponent<ItemCarryController>();
				}
				return instance;
			}
		}

		// True only when an item is actually stuck to the cursor. Never builds the singleton just to check.
		public static bool IsCarryingItem
		{
			get
			{
				return instance != null && instance.carried != null;
			}
		}

		public TowerItem Carried
		{
			get
			{
				return carried;
			}
		}

		public DragSource Source
		{
			get
			{
				return source;
			}
		}

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

		public TowerEquipment SourceEquipment
		{
			get
			{
				return sourceEquipment;
			}
		}

		// Picks the item up onto the cursor, remembering its source. Replaces whatever was being carried.
		public void PickUp(TowerItem item, DragSource source, ItemShopPanel shop, ItemShopOfferButton offer,
			TowerEquipment sourceEquipment, IItemPanel sourcePanel)
		{
			if (item == null)
			{
				return;
			}
			carried = item;
			this.source = source;
			this.shop = shop;
			shopOffer = offer;
			this.sourceEquipment = sourceEquipment;
			this.sourcePanel = sourcePanel;
			DragLayer.Instance.BeginDrag(item);
			DragLayer.Instance.MoveTo(PointerPosition());
		}

		// Drops the carry: the item leaves the cursor without being placed.
		public void Release()
		{
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.HideItemPreview();
			hoveredTower = null;
			carried = null;
			shop = null;
			shopOffer = null;
			sourceEquipment = null;
			sourcePanel = null;
			DragLayer.Instance.EndDrag();
		}

		// Places the carried item onto a UI drop target (inventory / tower equip panel). On success the
		// source panel is refreshed and the carry ends.
		public bool DropOnto(IItemDropTarget target)
		{
			if (carried == null || target == null)
			{
				return false;
			}
			if (!target.OnItemDropped(this))
			{
				return false;
			}
			RefreshSource();
			Release();
			return true;
		}

		// Equips the carried item on the tapped world tower. Succeeds only when the item fits, the tower has
		// a free slot, and (for shop items) the player can pay. Consumes the source on success.
		public bool TryEquipOn(TurretEntity tower)
		{
			if (carried == null || tower == null || tower.Equipment == null)
			{
				return false;
			}
			TowerEquipment.EquipBlock block = tower.Equipment.GetEquipBlock(carried);
			if (block != TowerEquipment.EquipBlock.None)
			{
				ItemFeedback.Equip(block);
				return false;
			}
			// Pay first for shop items so we don't equip something the player can't afford.
			if (source == DragSource.Shop && (shop == null || !shop.TryPurchase(shopOffer)))
			{
				ItemFeedback.NotEnoughGold();
				return false;
			}
			tower.Equipment.Equip(carried);
			ConsumeNonShopSource();
			RefreshSource();
			Release();
			return true;
		}

		// Removes the item from where it came from (shop is already paid for in TryEquipOn / the drop target).
		private void ConsumeNonShopSource()
		{
			if (source == DragSource.Inventory)
			{
				ItemInventory.Instance.Remove(carried);
			}
			else if (source == DragSource.TowerSlot && sourceEquipment != null)
			{
				sourceEquipment.Unequip(carried);
			}
		}

		private void RefreshSource()
		{
			if (sourcePanel != null)
			{
				sourcePanel.RefreshOpen();
			}
		}

		// Keeps the ghost glued to the cursor. ESC-to-drop is handled centrally in UIRootHandler so it can
		// take priority over the gameplay settings popup.
		private void Update()
		{
			if (carried == null)
			{
				return;
			}
			DragLayer.Instance.MoveTo(PointerPosition());
			UpdateTowerHover();
		}

		// While carrying, preview the tower under the cursor: its range ring + ContentHolder + item equip
		// panel show as a "drop here" hint. Only previews a tower that can actually take the item; it clears
		// when the cursor leaves. Equipping still happens on click (InputFilterDirector -> TryEquipOn). Only
		// re-binds when the hovered tower changes, so it's cheap per frame.
		private void UpdateTowerHover()
		{
			EnhanceTurretDialogHandler popup = MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController;
			// A normal (interactive) tower popup is open, e.g. the inventory -> equip-panel flow: don't hijack
			// it into a look-only preview. The preview never sets isOpen, so isOpen means a real popup.
			if (popup.isOpen)
			{
				return;
			}
			TurretEntity tower;
			InputFilterDirector.RaycastTower(PointerPosition(), out tower);
			if (tower != null && (tower.Equipment == null || !tower.Equipment.CanEquip(carried)))
			{
				tower = null;
			}
			if (tower == hoveredTower)
			{
				return;
			}
			hoveredTower = tower;
			if (hoveredTower != null)
			{
				popup.ShowItemPreview(hoveredTower);
			}
			else
			{
				popup.HideItemPreview();
			}
		}

		private static Vector2 PointerPosition()
		{
			Pointer pointer = Pointer.current;
			return pointer != null ? pointer.position.ReadValue() : Vector2.zero;
		}

		private TowerItem carried;

		private DragSource source;

		private ItemShopPanel shop;

		private ItemShopOfferButton shopOffer;

		private TowerEquipment sourceEquipment;

		private IItemPanel sourcePanel;

		private TurretEntity hoveredTower;

		private static ItemCarryController instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
