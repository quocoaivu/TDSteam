using Bootstrap;
using Data;
using FreeResources;
using OfferPopup;
using Store;
using UnityEngine;

namespace LifetimePopup
{
	public class LifespanSurface : MonoSingleton<LifespanSurface>
	{
        [SerializeField]
        private PrizeDialogHandler rewardPopupController;

        [SerializeField]
        private AlertDialogHandler notifyPopupController;

        [SerializeField]
        private ToastDialogHandler toastPopupController;

        [SerializeField]
        private ShopDialogHandler storePopupController;

        [SerializeField]
        private DealDialogHandler offerPopupController;

        [SerializeField]
        private FreeResourcesDialogHandler freeResourcesPopupController;

        [SerializeField]
        private AskToRateDialogHandler askToRatePopupController;

        [SerializeField]
        private LoadingProgressDialogHandler loadingProgressPopupController;

        [SerializeField]
        private AskToBuyDialogHandler askToBuyPopupController;

        public PrizeDialogHandler RewardPopupController
		{
			get
			{
				return rewardPopupController;
			}
			private set
			{
				rewardPopupController = value;
			}
		}

		public AlertDialogHandler NotifyPopupController
		{
			get
			{
				return notifyPopupController;
			}
			private set
			{
				notifyPopupController = value;
			}
		}

		public ToastDialogHandler ToastPopupController
		{
			get
			{
				return toastPopupController;
			}
			private set
			{
				toastPopupController = value;
			}
		}

		public ShopDialogHandler StorePopupController
		{
			get
			{
				return storePopupController;
			}
			private set
			{
				storePopupController = value;
			}
		}

		public DealDialogHandler OfferPopupController
		{
			get
			{
				return offerPopupController;
			}
			private set
			{
				offerPopupController = value;
			}
		}

		public FreeResourcesDialogHandler FreeResourcesPopupController
		{
			get
			{
				return freeResourcesPopupController;
			}
			private set
			{
				freeResourcesPopupController = value;
			}
		}

		public AskToRateDialogHandler AskToRateDialogHandler
		{
			get
			{
				return askToRatePopupController;
			}
			private set
			{
				askToRatePopupController = value;
			}
		}

		public LoadingProgressDialogHandler LoadingProgressPopupController
		{
			get
			{
				return loadingProgressPopupController;
			}
			private set
			{
				loadingProgressPopupController = value;
			}
		}

		public AskToBuyDialogHandler AskToBuyDialogHandler
		{
			get
			{
				return askToBuyPopupController;
			}
			private set
			{
				askToBuyPopupController = value;
			}
		}

		public bool IsLoadingProgressActive()
		{
			return LoadingProgressPopupController.IsGameObjectActive();
		}

		public void TryOpenAskRatePopup(int currentMapID)
		{
			int chanceToShowAskRating = Bootstrap.GameBootstrap.Instance.RemoteConfig.GetChanceToShowAskRating();
			if ((currentMapID == 2 || currentMapID == 4) && !PlayerSaveStore.Instance.IsUserRated() && MapProgressStore.Instance.GetStarEarnedByMap(currentMapID) == 3 && UnityEngine.Random.Range(0, 100) < chanceToShowAskRating)
			{
				PriorityDialogDirector.Instance.CreatePopup(PriorityDialogDirector.Instance.ratePopupPrefab, DialogPriorityEnum.Normal);
				return;
			}
		}
	}
}
