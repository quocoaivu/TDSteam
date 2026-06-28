using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
	// One cell in the inventory popup: shows a held item's name + stat bonus and lets the player drag it
	// out (to a tower / equip area). Spawned per item by ItemInventoryPanel. The serialized field names
	// (nameText, levelText) are kept so the existing ItemCell.prefab wiring still binds; levelText now
	// shows the stat line instead of a level.
	public class ItemInventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		// The cell's own background frame (Image on this GameObject). Cached so empty slots can be dimmed.
		private void Awake()
		{
			background = GetComponent<Image>();
			if (background != null)
			{
				fullColor = background.color;
			}
		}

		public void Bind(TowerItem item)
		{
			boundItem = item;
			SetSlotDimmed(false);
			// Icon-only cells: hide the name/stat text and show just the item icon.
			if (nameText != null)
			{
				nameText.gameObject.SetActive(false);
			}
			if (levelText != null)
			{
				levelText.gameObject.SetActive(false);
			}
			if (iconImage != null)
			{
				SetIcon(iconImage, item.icon);
			}
			if (draggable != null)
			{
				draggable.SetInventoryPayload(item);
			}
		}

		// Empty slot: clear the icon and payload so the cell shows just its background frame and can't be
		// picked up. It still works as a drop target via DraggableItem (drops route to the parent panel).
		public void Clear()
		{
			boundItem = null;
			SetSlotDimmed(true);
			if (iconImage != null)
			{
				SetIcon(iconImage, null);
			}
			if (draggable != null)
			{
				draggable.ClearPayload();
			}
		}

		// Hover shows the item's tooltip beside the cell (name, stats, rarity). Empty slots have no item, so
		// nothing is shown. Mirrors ItemShopOfferButton's hover tooltip.
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (boundItem != null)
			{
				ItemTooltip.Show(boundItem, transform as RectTransform);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			ItemTooltip.Hide();
		}

		// Fades the background frame for empty slots so they read as empty; full opacity for filled slots.
		private void SetSlotDimmed(bool dimmed)
		{
			if (background == null)
			{
				return;
			}
			Color color = fullColor;
			if (dimmed)
			{
				color.a = fullColor.a * EMPTY_SLOT_ALPHA;
			}
			background.color = color;
		}

		// Loads the item's sprite from Resources (via AssetLoader) and shows it. Hides the Image when there is
		// no key or the sprite is missing, so no white quad or leftover sprite lingers.
		private static void SetIcon(Image image, string iconKey)
		{
			Sprite sprite = string.IsNullOrEmpty(iconKey) ? null : Common.AssetLoader.Load<Sprite>(iconKey);
			image.sprite = sprite;
			image.enabled = sprite != null;
		}

		[SerializeField]
		private TMP_Text nameText;

		[SerializeField]
		private TMP_Text levelText;

		[SerializeField]
		private Image iconImage;

		[SerializeField]
		private DraggableItem draggable;

		// Alpha multiplier applied to an empty slot's background frame (0 = invisible, 1 = full).
		private const float EMPTY_SLOT_ALPHA = 0.35f;

		private Image background;

		private Color fullColor = Color.white;

		// The item this cell currently holds (null for empty slots); drives the hover tooltip.
		private TowerItem boundItem;
	}
}
