using System.Collections.Generic;
using GameCore;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Items
{
	// In-match shop to buy tower items with gold. Shows N random offers; buying spends gold and adds the
	// item to the in-run inventory; reroll spends gold to regenerate the offers. Extends GameplayDialogHandler
	// like the other gameplay popups. Open it from a gameplay button -> OpenShop(). Mirrors TowerItemPanel.
	public class ItemShopPanel : GameplayDialogHandler, IItemPanel
	{
		// While an item is on the cursor, let clicks fall through the WHOLE shop so a tap reaches a tower on
		// the map (the shop's window/backdrop images otherwise eat every click). The shop stays fully visible;
		// only its raycast blocking is turned off. Self-managed via a CanvasGroup so no scene wiring is needed.
		public override void Update()
		{
			base.Update();
			if (canvasGroup == null)
			{
				canvasGroup = GetComponent<CanvasGroup>();
				if (canvasGroup == null)
				{
					canvasGroup = gameObject.AddComponent<CanvasGroup>();
				}
			}
			canvasGroup.blocksRaycasts = !ItemCarryController.IsCarryingItem;
			UpdateCloseOnClickOutside();
		}

		// A click released outside the shop window (on the map or the dim backdrop) closes the shop, like
		// dismissing a popup. Skipped while carrying an item (those clicks place the item, not dismiss). The
		// window rect is the offers' shared parent, so no extra scene wiring is needed.
		private void UpdateCloseOnClickOutside()
		{
			if (ItemCarryController.IsCarryingItem)
			{
				pressedOutsideWindow = false;
				return;
			}
			Pointer pointer = Pointer.current;
			RectTransform window = WindowRect();
			if (pointer == null || window == null)
			{
				return;
			}
			Camera cam = CanvasCamera();
			Vector2 screenPos = pointer.position.ReadValue();
			// Require the press AND release to both land outside the window so the very click that opened the
			// shop (its press was on the HUD button, before the shop existed) can't immediately close it, and a
			// drag started inside the window doesn't dismiss it either.
			if (pointer.press.wasPressedThisFrame)
			{
				pressedOutsideWindow = !RectTransformUtility.RectangleContainsScreenPoint(window, screenPos, cam);
			}
			if (pointer.press.wasReleasedThisFrame)
			{
				if (pressedOutsideWindow && !RectTransformUtility.RectangleContainsScreenPoint(window, screenPos, cam))
				{
					CloseWithScaleAnimation();
				}
				pressedOutsideWindow = false;
			}
		}

		// The window that holds the offers (their shared parent). Null until the shop is built.
		private RectTransform WindowRect()
		{
			if (offerButtons.Count == 0 || offerButtons[0] == null)
			{
				return null;
			}
			return offerButtons[0].transform.parent as RectTransform;
		}

		// Camera to use for screen-point hit tests: null for a Screen Space - Overlay canvas, otherwise the
		// canvas's render camera. Passing the wrong one makes RectangleContainsScreenPoint mis-report.
		private Camera CanvasCamera()
		{
			Canvas canvas = GetComponentInParent<Canvas>();
			if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				return canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
			}
			return null;
		}

		// Opens the shop with a fresh set of offers.
		public void OpenShop()
		{
			for (int i = 0; i < offerButtons.Count; i++)
			{
				offerButtons[i].Init(this);
			}
			RollOffers();
			OpenWithScaleAnimation();
		}

		// Rebuilds the offer buttons in place (after a purchase elsewhere changed gold/offers).
		public void RefreshOpen()
		{
			RefreshAll();
		}

		// Buys the offer: spends gold, adds the item to the inventory, marks the offer sold. Returns
		// false (no change) when the offer is gone or the player can't afford it. Called by the
		// inventory drop target when a shop offer is dragged onto the bag.
		public bool TryBuy(ItemShopOfferButton offerButton)
		{
			if (offerButton == null)
			{
				return false;
			}
			TowerItem item = offerButton.Offer;
			if (item == null)
			{
				return false;
			}
			if (!TrySpendGold(buyCost))
			{
				return false;
			}
			ItemInventory.Instance.Add(item);
			// One purchase per offer: mark this slot sold.
			int index = offerButtons.IndexOf(offerButton);
			if (index >= 0 && index < offers.Count)
			{
				offers[index] = null;
			}
			RefreshAll();
			return true;
		}

		// Completes a purchase without placing the item: spends gold and marks the slot sold. Used when a
		// carried offer is equipped straight onto a tower (the item goes on the tower, not the inventory).
		// Returns false (no change) when the offer is gone or the player can't afford it.
		public bool TryPurchase(ItemShopOfferButton offerButton)
		{
			if (offerButton == null || offerButton.Offer == null)
			{
				return false;
			}
			if (!TrySpendGold(buyCost))
			{
				return false;
			}
			int index = offerButtons.IndexOf(offerButton);
			if (index >= 0 && index < offers.Count)
			{
				offers[index] = null;
			}
			RefreshAll();
			return true;
		}

		public void OnReroll()
		{
			if (!TrySpendGold(rerollCost))
			{
				return;
			}
			RollOffers();
		}

		private void RollOffers()
		{
			offers.Clear();
			for (int i = 0; i < offerButtons.Count; i++)
			{
				// Always (re)bind the panel here so an offer is never clickable without its panel reference.
				// Reroll rolls offers without going through OpenShop, so binding only in OpenShop left panel null.
				offerButtons[i].Init(this);
				offers.Add(ItemFactory.CreateRandom());
			}
			RefreshAll();
		}

		private void RefreshAll()
		{
			int money = MonoSingleton<GameRecord>.Instance.Money;
			for (int i = 0; i < offerButtons.Count; i++)
			{
				TowerItem offer = (i < offers.Count) ? offers[i] : null;
				offerButtons[i].Refresh(offer, buyCost, money >= buyCost);
			}
			if (rerollButton != null)
			{
				rerollButton.interactable = (money >= rerollCost);
			}
			if (rerollCostText != null)
			{
				rerollCostText.SetText("{0}", rerollCost);
			}
			if (goldText != null)
			{
				goldText.SetText("{0}", money);
			}
		}

		private bool TrySpendGold(int amount)
		{
			GameRecord record = MonoSingleton<GameRecord>.Instance;
			if (record.Money < amount)
			{
				return false;
			}
			record.DecreaseMoney(amount);
			return true;
		}

		[SerializeField]
		private List<ItemShopOfferButton> offerButtons = new List<ItemShopOfferButton>();

		[SerializeField]
		private int buyCost = 100;

		[SerializeField]
		private int rerollCost = 50;

		[SerializeField]
		private Button rerollButton;

		[SerializeField]
		private TMP_Text rerollCostText;

		[SerializeField]
		private TMP_Text goldText;

		private readonly List<TowerItem> offers = new List<TowerItem>();

		private CanvasGroup canvasGroup;

		private bool pressedOutsideWindow;
	}
}
