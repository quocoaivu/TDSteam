using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
	// One purchasable offer in the in-match item shop. Shows the item name + gold cost; clicking buys it.
	// The panel drives Refresh and performs the actual purchase. Mirrors SkillTreeNodeButton / ItemSlotButton.
	public class ItemShopOfferButton : MonoBehaviour
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

		// offer == null -> already bought (slot spent); affordable gates interactivity.
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
			if (button != null)
			{
				button.interactable = (offer != null && affordable);
			}
		}

		private void OnClick()
		{
			if (panel != null)
			{
				panel.OnOfferClicked(this);
			}
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
		private Color affordableColor = Color.yellow;

		[SerializeField]
		private Color unavailableColor = Color.white;

		private TowerItem offer;

		private ItemShopPanel panel;
	}
}
