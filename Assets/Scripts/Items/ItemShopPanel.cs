using System.Collections.Generic;
using DG.Tweening;
using GameCore;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Items
{
	// In-match shop on the HUD: a "tree" button that always shows on the map and blooms its item offers out
	// in a radial arc when tapped. Tapping the tree again (or clicking the map) collapses the items back into
	// the tree; the tree itself never hides. Buying an item picks it up onto the cursor (ItemCarryController)
	// and gold is spent when it's placed on a tower; reroll spends gold to regenerate the offers. It is NOT a
	// blocking popup (no backdrop, no game pause) — only the items toggle. Mirrors TowerItemPanel.
	public class ItemShopPanel : GameplayDialogHandler, IItemPanel
	{
		// While an item is on the cursor, let clicks fall through the tree so a tap reaches a tower on the map.
		// Otherwise the items are clickable while open. Self-managed via a CanvasGroup so no scene wiring needed.
		public override void Update()
		{
			if (canvasGroup == null)
			{
				canvasGroup = GetComponent<CanvasGroup>();
				if (canvasGroup == null)
				{
					canvasGroup = gameObject.AddComponent<CanvasGroup>();
				}
			}
			canvasGroup.blocksRaycasts = !ItemCarryController.IsCarryingItem;
			if (shopOpen)
			{
				UpdateCloseOnClickOutside();
			}
		}

		// Tree click toggles the items. Ignored while carrying (that click is placing the item on a tower).
		// Wire the tree Button's OnClick to this.
		public void ToggleShop()
		{
			if (ItemCarryController.IsCarryingItem)
			{
				return;
			}
			if (shopOpen)
			{
				CloseShop();
			}
			else
			{
				OpenShop();
			}
		}

		// Blooms the items out with a fresh set of offers. Kept public so a HUD button can also open it.
		public void OpenShop()
		{
			if (shopOpen)
			{
				return;
			}
			shopOpen = true;
			SetChrome(true);
			RollOffers();
			PunchTree();
			if (radialLayout != null)
			{
				radialLayout.PlayOpen();
			}
		}

		// Collapses the items back into the tree. The tree stays on screen.
		public void CloseShop()
		{
			if (!shopOpen)
			{
				return;
			}
			shopOpen = false;
			// Hide the chrome after the items finish collapsing so it doesn't pop out before them.
			if (radialLayout != null)
			{
				radialLayout.PlayClose(() => SetChrome(false));
			}
			else
			{
				SetChrome(false);
			}
		}

		private void SetChrome(bool visible)
		{
			if (shopChrome != null)
			{
				shopChrome.SetActive(visible);
			}
		}

		// Kept so the scene's existing close wiring still collapses the items (the tree is never hidden now).
		public override void CloseWithScaleAnimation()
		{
			CloseShop();
		}

		// A click released away from the tree and all item slots collapses the open shop, like tapping off a
		// menu. Skipped while carrying (those clicks place the item). Requires press AND release both off the
		// shop so the tap that opens it can't immediately close it and a drag off a slot doesn't dismiss it.
		private void UpdateCloseOnClickOutside()
		{
			if (ItemCarryController.IsCarryingItem)
			{
				pressedOutsideShop = false;
				return;
			}
			Pointer pointer = Pointer.current;
			if (pointer == null)
			{
				return;
			}
			Vector2 screenPos = pointer.position.ReadValue();
			if (pointer.press.wasPressedThisFrame)
			{
				pressedOutsideShop = !ShopContainsPoint(screenPos);
			}
			if (pointer.press.wasReleasedThisFrame)
			{
				if (pressedOutsideShop && !ShopContainsPoint(screenPos))
				{
					CloseShop();
				}
				pressedOutsideShop = false;
			}
		}

		// True when the screen point is over the tree, an item slot, or the reroll button (the shop's
		// interactive area). Clicks anywhere else collapse the shop.
		private bool ShopContainsPoint(Vector2 screenPos)
		{
			Camera cam = CanvasCamera();
			if (RectContains(transform as RectTransform, screenPos, cam))
			{
				return true;
			}
			if (rerollButton != null && RectContains(rerollButton.transform as RectTransform, screenPos, cam))
			{
				return true;
			}
			for (int i = 0; i < offerButtons.Count; i++)
			{
				RectTransform slot = (offerButtons[i] != null) ? offerButtons[i].transform as RectTransform : null;
				if (RectContains(slot, screenPos, cam))
				{
					return true;
				}
			}
			return false;
		}

		private static bool RectContains(RectTransform rect, Vector2 screenPos, Camera cam)
		{
			return rect != null && RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, cam);
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

		// Quick squash-and-stretch on the tree when the items bloom. Skipped when no target is assigned.
		private void PunchTree()
		{
			if (treePunchTarget == null)
			{
				return;
			}
			treePunchTarget.DOKill();
			treePunchTarget.localScale = Vector3.one;
			treePunchTarget.DOPunchScale(Vector3.one * 0.12f, 0.2f, 5).SetUpdate(true);
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
			// Don't spend gold if the bag has no room for the item.
			if (ItemInventory.Instance.IsFull)
			{
				return false;
			}
			if (!TrySpendGold(CostFor(item)))
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
			if (!TrySpendGold(CostFor(offerButton.Offer)))
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
				int cost = CostFor(offer);
				offerButtons[i].Refresh(offer, cost, money >= cost);
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

		// Buy price scales with rarity off the inspector base cost (buyCost = rarity 0): r1 = 1.5x, r2+ = 2.5x.
		// So with the default base of 100 the prices are 100 / 150 / 250.
		private int CostFor(TowerItem item)
		{
			if (item == null)
			{
				return buyCost;
			}
			switch (item.rarity)
			{
			case 0:
				return buyCost;
			case 1:
				return buyCost * 3 / 2;
			default:
				return buyCost * 5 / 2;
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

		// Radial bloom layout on the ItemContainer that holds the item slots.
		[SerializeField]
		private RadialShopLayout radialLayout;

		// The "tree" graphic to punch-scale when the items bloom (usually the tree Image). Optional.
		[SerializeField]
		private Transform treePunchTarget;

		// Holder for the shop chrome (reroll + gold) shown only while the shop is open, so a closed tree shows
		// nothing but the tree. Optional.
		[SerializeField]
		private GameObject shopChrome;

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

		private bool shopOpen;

		private bool pressedOutsideShop;
	}
}
