using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Gameplay;
using LifetimePopup;
using MetaGame;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class DailyOrdealOutcomeDialogHandler : GameplayDialogHandler
	{
        [Space]
        [SerializeField]
        private Text wavePassedAmount;

        [Space]
        [Header("Controller")]
        [SerializeField]
        private PrizeClusterHandler rewardGroupController;

        [SerializeField]
        private GameObject content;

        public void Init(BattleStanding battleStatus)
		{
			OpenWithScaleAnimation();
			rewardGroupController.Init(battleStatus);
			UpdateWavePassedAmount(battleStatus);
		}

		private void UpdateWavePassedAmount(BattleStanding battleStatus)
		{
			if (battleStatus != BattleStanding.Victory)
			{
				if (battleStatus == BattleStanding.Defeat)
				{
					int waveD = 0;
					tween = DOTween.To(() => 0, delegate(int x)
					{
						waveD = x;
						wavePassedAmount.text = waveD.ToString();
					}, MonoSingleton<GameRecord>.Instance.CurrentWave - 1, 2f).SetEase(Ease.Linear);
				}
			}
			else
			{
				int waveV = 0;
				tween = DOTween.To(() => 0, delegate(int x)
				{
					waveV = x;
					wavePassedAmount.text = waveV.ToString();
				}, MonoSingleton<GameRecord>.Instance.CurrentWave, 2f).SetEase(Ease.Linear);
			}
		}

		public void TryToContinue()
		{
			int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
			List<int> listInputHeroesID = DailyOrdealSpec.Instance.getListInputHeroesID(currentDayIndex);
			if (currentDayIndex == 0)
			{
				Continue();
			}
			else if (HeroStore.Instance.IsHeroOwned(listInputHeroesID))
			{
				Continue();
			}
			else if (currentDayIndex == 6)
			{
				// Day 6 has no buy-offer for its (unowned) heroes. The old code only hid
				// the content, leaving the player on a blank popup with no way back to the
				// World Map. Continue home like day 0 / already-owned cases do.
				Continue();
			}
			else
			{
				MonoSingleton<LifespanSurface>.Instance.OfferPopupController.InitSingleHeroOffer(listInputHeroesID[0], DealKind.OneTime);
				content.SetActive(false);
			}
		}

		public void Continue()
		{
			LoadingScreen.Instance.ShowLoading();
			base.Invoke("DoLoad", 1f);
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
			FormatDirector.Instance.gameMode = GameFormat.CampaignMode;
		}

		private void DoLoad()
		{
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.WorldMapSceneName);
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
