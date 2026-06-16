using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class AutoOpenSwitchHandler : SwitchHandler
	{
		private void Awake()
		{
			GetAllComponents();
		}

		private void Update()
		{
			if (MonoSingleton<GameRecord>.Instance.isAvailableOpenChestTurn())
			{
				tweenAnimation.DOPlay();
			}
			else
			{
				tweenAnimation.DOPause();
			}
		}

		private void GetAllComponents()
		{
			button = base.GetComponent<Button>();
		}

		public override void OnClick()
		{
			base.OnClick();
			chestGroupController.AutoOpenChest();
			UpdateState();
			MonoSingleton<UIRootHandler>.Instance.endGamePopupController.EndGameRewardPopupController.UpdateContinueButtonStatus();
		}

		private void UpdateState()
		{
			if (chestGroupController.isAvailableChestToOpen())
			{
				button.interactable = true;
			}
			else
			{
				button.interactable = false;
			}
		}

		[SerializeField]
		private CrateClusterHandler chestGroupController;

		[SerializeField]
		private DOTweenAnimation tweenAnimation;

		private Button button;
	}
}
