using System;
using Data;
using Gameplay;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.Serialization;

namespace Store
{
	public class ShopDialogHandler : GameplayDialogHandler
	{
        [Space]
        [Header("Controllers")]
        [SerializeField]
        [FormerlySerializedAs("itemPowerUpGroupController")]
        private PowerUpStoreItemGroupController powerUpStoreItemGroupController;

        [SerializeField]
        private CrystalPackClusterHandler gemPackGroupController;

        [SerializeField]
        private SalePackClusterHandler saleBundleGroupController;

        [SerializeField]
        private TabsClusterHandler tabsGroupController;

        [SerializeField]
        [FormerlySerializedAs("readDataShopItemAttribute")]
        private ShopItemLookup shopItemLookup;

        [Space]
        [Header("Gem Controller")]
        [SerializeField]
        private TotalGemDisplay gemController;

        public PowerUpStoreItemGroupController PowerUpStoreItemGroupController
		{
			get
			{
				return powerUpStoreItemGroupController;
			}
		}

		public CrystalPackClusterHandler GemPackGroupController
		{
			get
			{
				return gemPackGroupController;
			}
		}

		public SalePackClusterHandler SaleBundleGroupController
		{
			get
			{
				return saleBundleGroupController;
			}
		}

		public TabsClusterHandler TabsGroupController
		{
			get
			{
				return tabsGroupController;
			}
		}

		public ShopItemLookup ShopItemLookup
		{
			get
			{
				return shopItemLookup;
			}
		}

		private void OnEnable()
		{
			InitDefaultData();
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			InitDefaultData();
			UpdateGemStatus();
			SendEventOpenPanel();
		}

		private void SendEventOpenPanel()
		{
			int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
			int maxMapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked() + 1;
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_OpenStore(currentGem, maxMapIDUnlocked);
		}

		private void InitDefaultData()
		{
			PowerUpStoreItemGroupController.InitItemsInformation();
			GemPackGroupController.EnableScroll();
			SaleBundleGroupController.InitItemsInformation();
			SaleBundleGroupController.RefreshItemStatus();
		}

		public void ShowBuyEffect()
		{
		}

		public void UpdateGemStatus()
		{
			gemController.UpdateGemMessage();
		}

		public void PlayAnimationNotEnoughGem()
		{
			gemController.PlayAnimationNotEnoughGem();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			GemPackGroupController.DisableScroll();
		}


	}
}
