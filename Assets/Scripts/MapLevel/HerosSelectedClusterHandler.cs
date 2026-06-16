using System;
using Data;
using UnityEngine;

namespace MapLevel
{
	public class HerosSelectedClusterHandler : MonoBehaviour
	{
		public int MaxHeroCanBring
		{
			get
			{
				return maxHeroCanBring;
			}
			private set
			{
				maxHeroCanBring = value;
			}
		}

		private void Awake()
		{
			listHeroesIDSelected = new int[listHeroesSlot.Length];
		}

		public bool IsFullSlot()
		{
			bool result = true;
			for (int i = 0; i < MaxHeroCanBring; i++)
			{
				if (listHeroesIDSelected[i] < 0)
				{
					result = false;
				}
			}
			return result;
		}

		public void InitHeroesSlot()
		{
			MaxHeroCanBring = 3;
			for (int i = 0; i < listHeroesSlot.Length; i++)
			{
				if (i <= maxHeroCanBring - 1)
				{
					listHeroesSlot[i].Show();
				}
				else
				{
					listHeroesSlot[i].Hide();
				}
			}
			InitDefaultValue();
		}

		private void InitDefaultValue()
		{
			int[] listHeroIDSaved = HeroPrepareStore.Instance.GetListHeroIDSaved();
			for (int i = 0; i < listHeroesIDSelected.Length; i++)
			{
				listHeroesIDSelected[i] = listHeroIDSaved[i];
			}
			foreach (HeroSocketHandler heroSlotController in listHeroesSlot)
			{
				heroSlotController.SetDefaultValue();
				heroSlotController.Init(heroesInputGroupController);
			}
			for (int k = 0; k < MaxHeroCanBring; k++)
			{
				UnChooseHero(k);
			}
		}

		public void UnChooseHero(int slot)
		{
			listHeroesIDSelected[slot] = -1;
		}

		public void ChooseHero(int heroID)
		{
			if (GameKit.IsSubscriptionActive(SubscriptionType.doubleAttack))
			{
				GameKit.cachedHavingBooster = true;
				GameKit.cachedBoosterMultiplier = 2f;
			}
			else if (GameKit.IsSubscriptionActive(SubscriptionType.fiftyPercentAtkBoost))
			{
				GameKit.cachedHavingBooster = true;
				GameKit.cachedBoosterMultiplier = 1.5f;
			}
			int availableSlot = GetAvailableSlot();
			listHeroesSlot[availableSlot].InitHeroSlot(availableSlot, heroID);
			listHeroesIDSelected[availableSlot] = heroID;
			if (mapLevelSelectPopupController)
			{
				mapLevelSelectPopupController.OnChooseHero();
			}
		}

		private int GetAvailableSlot()
		{
			int result = -1;
			for (int i = 0; i < listHeroesSlot.Length; i++)
			{
				if (!listHeroesSlot[i].IsInitValue)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public bool IsChooseAtLeastOneHero()
		{
			bool result = false;
			foreach (int num in listHeroesIDSelected)
			{
				if (num >= 0)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		[SerializeField]
		private ZoneLevelSelectDialogHandler mapLevelSelectPopupController;

		[SerializeField]
		private HerosInputClusterHandler heroesInputGroupController;

		[SerializeField]
		private HeroSocketHandler[] listHeroesSlot;

		private int maxHeroCanBring;

		public int[] listHeroesIDSelected;
	}
}
