using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
	// One purchasable offer in the in-match item shop. Shows the item name + gold cost; clicking buys it.
	// The panel drives Refresh and performs the actual purchase. Mirrors SkillTreeNodeButton / ItemSlotButton.
	public class ItemShopOfferButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public TowerItem Offer
		{
			get
			{
				return offer;
			}
		}

		private void Awake()
		{
			if (button != null)
			{
				button.onClick.AddListener(OnClick);
			}
		}

		public void Init(ItemShopPanel panel)
		{
			this.panel = panel;
		}

		// Click picks the offer up onto the cursor (one unified carry model). Ignored while already carrying
		// (the shop is not a drop target). Gold is spent later, when the item is placed on a tower.
		private void OnClick()
		{
			if (ItemCarryController.IsCarryingItem || offer == null)
			{
				return;
			}
			// Carry takes over with its own per-tower preview; drop the hover's class-wide highlight.
			Gameplay.TowerHighlight.ClearAll();
			ItemCarryController.Instance.PickUp(offer, DragSource.Shop, panel, this, null, panel);
		}

		// offer == null -> already bought (slot spent). Picking the item up is always allowed when it's in
		// stock; affordability is only a price-color hint now (the gold is spent later, on placement).
		public void Refresh(TowerItem offer, int cost, bool affordable)
		{
			this.offer = offer;
			if (nameText != null)
			{
				nameText.SetText(offer != null ? offer.name : "Sold");
			}
			if (costText != null)
			{
				costText.gameObject.SetActive(offer != null);
				costText.SetText("{0}", cost);
				costText.color = affordable ? affordableColor : unavailableColor;
			}
			if (soldOverlay != null)
			{
				soldOverlay.SetActive(offer == null);
			}
			if (iconImage != null)
			{
				SetIcon(iconImage, offer != null ? offer.icon : null);
			}
			if (button != null)
			{
				button.interactable = (offer != null);
			}
			if (draggable != null)
			{
				if (offer != null)
				{
					draggable.SetShopPayload(offer, panel, this);
				}
				else
				{
					draggable.ClearPayload();
				}
			}
		}

		// Hover highlight: scale the icon up and pulse every map tower this item fits. SetUpdate(true) so the
		// icon animates while the shop pauses the game. Skipped while carrying an item (the carry has its own
		// per-tower preview, and the shop falls through to clicks then).
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (offer == null || iconImage == null)
			{
				return;
			}
			iconImage.transform.DOKill();
			iconImage.transform.DOScale(hoverScale, hoverDuration).SetUpdate(true);
			if (!ItemCarryController.IsCarryingItem)
			{
				Gameplay.TowerHighlight.HighlightClass(offer.towerID);
				ItemTooltip.Show(offer, transform as RectTransform);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Gameplay.TowerHighlight.ClearAll();
			ItemTooltip.Hide();
			if (iconImage == null)
			{
				return;
			}
			iconImage.transform.DOKill();
			iconImage.transform.DOScale(1f, hoverDuration).SetUpdate(true);
		}

		// Loads the item's sprite from Resources (via AssetLoader) and shows it. Hides the Image when there is
		// no key (sold-out slot) or the sprite is missing, so no white quad or leftover sprite lingers.
		private static void SetIcon(Image image, string iconKey)
		{
			Sprite sprite = string.IsNullOrEmpty(iconKey) ? null : Common.AssetLoader.Load<Sprite>(iconKey);
			image.sprite = sprite;
			image.enabled = sprite != null;
			// Reset any leftover hover scale so a refreshed/sold offer starts at normal size.
			image.transform.DOKill();
			image.transform.localScale = Vector3.one;
		}

		[SerializeField]
		private Button button;

		[SerializeField]
		private TMP_Text nameText;

		[SerializeField]
		private TMP_Text costText;

		[SerializeField]
		private GameObject soldOverlay;

		[SerializeField]
		private Image iconImage;

		[SerializeField]
		private DraggableItem draggable;

		[SerializeField]
		private Color affordableColor = Color.yellow;

		[SerializeField]
		private Color unavailableColor = Color.white;

		[SerializeField]
		private float hoverScale = 1.15f;

		[SerializeField]
		private float hoverDuration = 0.15f;

		private TowerItem offer;

		private ItemShopPanel panel;
	}
}
