using System;
using UnityEngine;

namespace RewardPopup
{
	public class SingleItemClusterHandler : MonoBehaviour
	{
		public ItemQuantityHandler GemQuantity
		{
			get
			{
				return gemQuantity;
			}
			set
			{
				gemQuantity = value;
			}
		}

		public ItemQuantityHandler LifeQuantity
		{
			get
			{
				return lifeQuantity;
			}
			set
			{
				lifeQuantity = value;
			}
		}

		public ItemQuantityHandler MoneyQuantity
		{
			get
			{
				return moneyQuantity;
			}
			set
			{
				moneyQuantity = value;
			}
		}

		public void InitValue(PrizeKind rewardType, int value)
		{
			this.rewardType = rewardType;
			this.value = value;
			HideAllItems();
		}

		public void Show()
		{
			PrizeKind rewardType = this.rewardType;
			if (rewardType != PrizeKind.Gem)
			{
				if (rewardType != PrizeKind.Life)
				{
					if (rewardType == PrizeKind.Money)
					{
						MoneyQuantity.Init(value);
					}
				}
				else
				{
					LifeQuantity.Init(value);
				}
			}
			else
			{
				GemQuantity.Init(value);
			}
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
			HideAllItems();
		}

		private void HideAllItems()
		{
			gemQuantity.Hide();
			lifeQuantity.Hide();
			moneyQuantity.Hide();
		}

		[SerializeField]
		private ItemQuantityHandler gemQuantity;

		[SerializeField]
		private ItemQuantityHandler lifeQuantity;

		[SerializeField]
		private ItemQuantityHandler moneyQuantity;

		private PrizeKind rewardType;

		private int value;
	}
}
