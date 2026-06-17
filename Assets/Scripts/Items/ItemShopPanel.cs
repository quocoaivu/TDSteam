using System.Collections.Generic;
using GameCore;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
	// In-match shop to buy tower items with gold. Shows N random offers; buying spends gold and adds the
	// item to the in-run inventory; reroll spends gold to regenerate the offers. Extends GameplayDialogHandler
	// like the other gameplay popups. Open it from a gameplay button -> OpenShop(). Mirrors TowerItemPanel.
	public class ItemShopPanel : GameplayDialogHandler
	{
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

		public void OnOfferClicked(ItemShopOfferButton offerButton)
		{
			TowerItem item = offerButton.Offer;
			if (item == null)
			{
				return;
			}
			if (!TrySpendGold(buyCost))
			{
				return;
			}
			ItemInventory.Instance.Add(item);
			// One purchase per offer: mark this slot sold.
			int index = offerButtons.IndexOf(offerButton);
			if (index >= 0 && index < offers.Count)
			{
				offers[index] = null;
			}
			RefreshAll();
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
	}
}
