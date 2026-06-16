using System;
using DG.Tweening;
using LifetimePopup;
using MetaGame;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class ContinueSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			if (chestGroupController.isAvailableChestToOpen() && MonoSingleton<GameRecord>.Instance.isAvailableOpenChestTurn())
			{
				chestGroupController.AutoOpenChest();
				base.CustomInvoke(new Action(DoContinue), 1f);
			}
			else
			{
				DoContinue();
			}
		}

		public void UpdateStatus()
		{
			if (!chestGroupController.isAvailableChestToOpen() && !MonoSingleton<GameRecord>.Instance.isAvailableOpenChestTurn())
			{
				ShowHighlightStatus();
			}
			else
			{
				ShowNormalStatus();
			}
		}

		private void ShowNormalStatus()
		{
			tweenAnimation.DOPause();
			Color color = image.color;
			color.a = 0.8627451f;
			image.color = color;
		}

		private void ShowHighlightStatus()
		{
			tweenAnimation.DOPlay();
			Color color = image.color;
			color.a = 1f;
			image.color = color;
		}

		private void DoContinue()
		{
			LoadingScreen.Instance.ShowLoading();
			base.CustomInvoke(new Action(DoLoad), 1f);
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
		}

		private void DoLoad()
		{
			ClipPlayerDirector.Instance.TryToShowInterstitialAds_EndGame();
			FormatDirector.Instance.gameMode = GameFormat.CampaignMode;
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.WorldMapSceneName);
			MonoSingleton<LifespanSurface>.Instance.TryOpenAskRatePopup(MonoSingleton<GameRecord>.Instance.MapID);
		}

		[SerializeField]
		private CrateClusterHandler chestGroupController;

		[SerializeField]
		private DOTweenAnimation tweenAnimation;

		[SerializeField]
		private Image image;
	}
}
