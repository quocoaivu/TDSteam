using System;
using Data;
using DG.Tweening;
using LifetimePopup;
using GameCore;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class CrateDealHandler : BaseMonoBehaviour
	{
		public void Init()
		{
			Open();
			turnAmountToBuyEach = MonoSingleton<GameplayRecordLoader>.Instance.LuckyChestLoader.GetTurnAmountToBuyEach();
			int currentPlayCount = MapProgressStore.Instance.GetCurrentPlayCount(MonoSingleton<GameRecord>.Instance.MapID);
			UnityEngine.Debug.Log("current map play count = " + currentPlayCount);
			if (MonoSingleton<GameRecord>.Instance.MapID == 0)
			{
				gemAmountToBuy = 0;
				gemAmountToBuyText.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(22);
			}
			else
			{
				gemAmountToBuy = MonoSingleton<GameplayRecordLoader>.Instance.LuckyChestLoader.GetGemAmountToBuyTurn();
				gemAmountToBuyText.text = gemAmountToBuy.ToString();
			}
			turnAmountToBuyEachText.text = turnAmountToBuyEach.ToString();
			int num = 2 - MonoSingleton<GameRecord>.Instance.CurrentOpenChestOffer;
			if (num == 1)
			{
				groupWatchVideo.SetActive(false);
				titleOR.SetActive(false);
			}
		}

		public void GetMoreTurnByVideoOffer()
		{
			if (ClipPlayerDirector.Instance.CheckIfVideoExits())
			{
				ClipPlayerDirector.Instance.playVideoGameplay_ForOpenChestOffer();
				MonoSingleton<UIRootHandler>.Instance.endGamePopupController.EndGameRewardPopupController.UpdateContinueButtonStatus();
				SendEvent_FreeChestOffer(FreeCrateDealKind.WatchVideo);
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(18);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			}
			else
			{
				string notiContent2 = Singleton<AlertSynopsis>.Instance.GetNotiContent(19);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent2, false, false);
			}
		}

		public void GetMoreTurnByPayGem()
		{
			int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
			if (currentGem >= gemAmountToBuy)
			{
				MonoSingleton<GameRecord>.Instance.ChangeOpenChestTurn(turnAmountToBuyEach);
				MonoSingleton<GameRecord>.Instance.ChangeOpenChestOffer();
				PlayerCurrencyStore.Instance.ChangeGem(-gemAmountToBuy, true);
				Close();
				SendEvent_FreeChestOffer(FreeCrateDealKind.PayGem);
				MonoSingleton<UIRootHandler>.Instance.endGamePopupController.EndGameRewardPopupController.UpdateContinueButtonStatus();
			}
			else
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(20);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, true, true);
			}
		}

		private void SendEvent_FreeChestOffer(FreeCrateDealKind type)
		{
			int mapID = MonoSingleton<GameRecord>.Instance.MapID + 1;
			int offerCount = 2 - MonoSingleton<GameRecord>.Instance.CurrentOpenChestOffer;
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_FreeChestOffer(mapID, type, offerCount);
		}

		public void Open()
		{
			base.gameObject.SetActive(true);
		}

		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (!CrateDealHandler.changedVideoStatus && MonoSingleton<GameRecord>.Instance.PlayedVideoLucky)
			{
				Close();
				CrateDealHandler.changedVideoStatus = true;
			}
			else
			{
				int num = 2 - MonoSingleton<GameRecord>.Instance.CurrentOpenChestOffer;
				if (num == 0)
				{
					Color color = imageButtonBuyGem.color;
					color.a = 0.490196079f;
					imageButtonBuyGem.color = color;
					tweenAnimationBuyGemButton.DOPause();
				}
				if (num == 1)
				{
					Color color2 = imageButtonBuyGem.color;
					color2.a = 1f;
					imageButtonBuyGem.color = color2;
					tweenAnimationBuyGemButton.DOPlay();
				}
			}
            groupWatchVideo.SetActive(false);
        }

        [SerializeField]
		private GameObject groupWatchVideo;

		[SerializeField]
		private GameObject titleOR;

		[Space]
		[SerializeField]
		private Image imageButtonBuyGem;

		[SerializeField]
		private DOTweenAnimation tweenAnimationBuyGemButton;

		[Space]
		[SerializeField]
		private Text turnAmountToBuyEachText;

		[SerializeField]
		private Text gemAmountToBuyText;

		private int turnAmountToBuyEach;

		private int gemAmountToBuy;

		public static bool changedVideoStatus;
	}
}
