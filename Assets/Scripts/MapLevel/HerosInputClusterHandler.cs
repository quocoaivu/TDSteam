using System;
using Data;
using UnityEngine;

namespace MapLevel
{
	public class HerosInputClusterHandler : MonoBehaviour
	{
		public HerosSelectedClusterHandler HeroesSelectedController
		{
			get
			{
				return heroesSelectedController;
			}
			set
			{
				heroesSelectedController = value;
			}
		}

		public HerosSelectClusterHandler HeroesSelectController
		{
			get
			{
				return heroesSelectController;
			}
			set
			{
				heroesSelectController = value;
			}
		}

		public AskToBuyHeroDialogHandler AskToBuyHeroDialogHandler
		{
			get
			{
				return askToBuyHeroPopupController;
			}
			set
			{
				askToBuyHeroPopupController = value;
			}
		}

		private void Start()
		{
			HeroStore.Instance.OnBuyNewHero += Instance_OnBuyNewHero;
		}

		private void OnDestroy()
		{
			if (HeroStore.Instance != null)
			{
				HeroStore.Instance.OnBuyNewHero -= Instance_OnBuyNewHero;
			}
		}

		private void Instance_OnBuyNewHero()
		{
			HeroesSelectController.UpdateButtonsStatus();
		}

		[SerializeField]
		private HerosSelectedClusterHandler heroesSelectedController;

		[SerializeField]
		private HerosSelectClusterHandler heroesSelectController;

		[SerializeField]
		private AskToBuyHeroDialogHandler askToBuyHeroPopupController;
	}
}
