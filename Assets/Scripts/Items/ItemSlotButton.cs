using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
	// One equipped-item slot on the tower popup. Shows the item in this slot (or empty) and lets the
	// player drag it out to the inventory to unequip. Place one per equip slot (TowerEquipment.SLOT_COUNT)
	// and register it in TowerItemPanel.slotButtons.
	public class ItemSlotButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		// item == null -> slot empty. equipment is the holder to unequip from when the slot is dragged out.
		public void Refresh(TowerItem item, TowerEquipment equipment)
		{
			boundItem = item;
			if (nameText != null)
			{
				nameText.SetText(item != null ? item.name : "—");
			}
			if (emptyOverlay != null)
			{
				emptyOverlay.SetActive(item == null);
			}
			EnsureIcon();
			if (iconImage != null)
			{
				SetIcon(iconImage, item != null ? item.icon : null);
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

		// Hover shows the equipped item's tooltip (name, stats, rarity) beside the slot. Empty slots have no
		// item, so nothing is shown. Mirrors ItemShopOfferButton / ItemInventoryCell.
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

		// The hand-built slots in the scene have no icon Image wired, so make sure one exists: use the wired
		// reference if set, else reuse a child named "Icon", else create a centered one on top (last sibling)
		// so the equipped item's sprite reads like the shop/inventory icons with no Inspector wiring.
		private void EnsureIcon()
		{
			if (iconImage != null)
			{
				return;
			}
			Transform existing = base.transform.Find("Icon");
			if (existing != null)
			{
				iconImage = existing.GetComponent<Image>();
				if (iconImage != null)
				{
					return;
				}
			}
			GameObject go = new GameObject("Icon", typeof(RectTransform));
			go.transform.SetParent(base.transform, false);
			RectTransform rect = go.GetComponent<RectTransform>();
			rect.anchorMin = new Vector2(0.5f, 0.5f);
			rect.anchorMax = new Vector2(0.5f, 0.5f);
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.sizeDelta = new Vector2(96f, 96f);
			rect.anchoredPosition = Vector2.zero;
			go.transform.SetAsLastSibling();
			iconImage = go.AddComponent<Image>();
			iconImage.raycastTarget = false;
			iconImage.preserveAspect = true;
		}

		// Loads the item's sprite from Resources (via AssetLoader) and shows it. Hides the Image when there is
		// no key (empty slot) or the sprite is missing, so no white quad or leftover sprite lingers.
		private static void SetIcon(Image image, string iconKey)
		{
			Sprite sprite = string.IsNullOrEmpty(iconKey) ? null : Common.AssetLoader.Load<Sprite>(iconKey);
			image.sprite = sprite;
			image.enabled = sprite != null;
		}

		[SerializeField]
		private TMP_Text nameText;

		[SerializeField]
		private GameObject emptyOverlay;

		[SerializeField]
		private Image iconImage;

		[SerializeField]
		private DraggableItem draggable;

		// The item this slot currently holds (null when empty); drives the hover tooltip.
		private TowerItem boundItem;
	}
}
