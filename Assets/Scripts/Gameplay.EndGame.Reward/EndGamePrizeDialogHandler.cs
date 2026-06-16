using System;
using System.Collections.Generic;
using Data;
using LifetimePopup;
using Parameter;
using Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class EndGamePrizeDialogHandler : GameplayDialogHandler
	{
		public SpecInOneTurn[] TurnsData
		{
			get
			{
				return turnsData;
			}
			set
			{
				turnsData = value;
			}
		}

		private void Awake()
		{
			MonoSingleton<GameRecord>.Instance.OnOpenChestTurnChange += Instance_OnOpenChestTurnChange;
		}

		private void Start()
		{
			if (tutorialOpenLuckyChest.IsTutorialDone() || !GameplayTutorialDirector.Instance.IsTutorialMap())
			{
				continueGroup.SetActive(true);
			}
			else
			{
				continueGroup.SetActive(false);
			}
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			GetDataRateByTurns();
			previewRewardGroupController.InitListPreviewItems();
			currentChestOpened = 0;
			tutorialOpenLuckyChest.CheckCondition();
			tutorialGetFreeLuckyChest.CheckCondition();
		}

		private void GetDataRateByTurns()
		{
			turnsData = new SpecInOneTurn[3];
			for (int i = 0; i < TurnsData.Length; i++)
			{
				SpecInOneTurn parameterInOneTurn = new SpecInOneTurn();
				parameterInOneTurn.turnID = i;
				parameterInOneTurn.heroRate = FortuneCrateSpec.Instance.GetChestRate(0, i);
				parameterInOneTurn.heroSellOfferRate_common = FortuneCrateSpec.Instance.GetChestRate(1, i);
				parameterInOneTurn.heroBonusExpRate_1 = FortuneCrateSpec.Instance.GetChestRate(2, i);
				parameterInOneTurn.heroBonusExpRate_2 = FortuneCrateSpec.Instance.GetChestRate(3, i);
				parameterInOneTurn.gemBonusRate_common = FortuneCrateSpec.Instance.GetChestRate(4, i);
				parameterInOneTurn.gemBonusRate_unCommon = FortuneCrateSpec.Instance.GetChestRate(5, i);
				parameterInOneTurn.gemBonusRate_rare = FortuneCrateSpec.Instance.GetChestRate(6, i);
				parameterInOneTurn.powerUpItemBonusRate_Freezing_Common = FortuneCrateSpec.Instance.GetChestRate(7, i);
				parameterInOneTurn.powerUpItemBonusRate_MeteorStrike_Common = FortuneCrateSpec.Instance.GetChestRate(8, i);
				parameterInOneTurn.powerUpItemBonusRate_HealingPotion_Common = FortuneCrateSpec.Instance.GetChestRate(9, i);
				parameterInOneTurn.powerUpItemBonusRate_GoldChest_Common = FortuneCrateSpec.Instance.GetChestRate(10, i);
				parameterInOneTurn.powerUpItemBonusRate_MeteorStrike_Rare = FortuneCrateSpec.Instance.GetChestRate(11, i);
				parameterInOneTurn.heroWukongRate = FortuneCrateSpec.Instance.GetChestRate(12, i);
				parameterInOneTurn.heroAshiRate = FortuneCrateSpec.Instance.GetChestRate(13, i);
				parameterInOneTurn.heroSellOfferRate_unCommon = FortuneCrateSpec.Instance.GetChestRate(14, i);
				TurnsData[i] = parameterInOneTurn;
			}
		}

		private void Instance_OnOpenChestTurnChange()
		{
			if (!MonoSingleton<GameRecord>.Instance.isAvailableOpenChestTurn() && MonoSingleton<GameRecord>.Instance.isAvailableOpenChestOffer())
			{
				chestOfferController.Init();
				continueButton.gameObject.SetActive(true);
			}
			if (MonoSingleton<GameRecord>.Instance.isAvailableOpenChestTurn() && chestGroupController.isAvailableChestToOpen())
			{
				autoOpenChestButton.interactable = true;
			}
			else
			{
				autoOpenChestButton.interactable = false;
			}
			// On the tutorial map (Map 1) Start() hides the continue group until the
			// lucky-chest tutorial is done, but nothing ever re-enabled it, leaving the
			// player stuck after opening a chest. Reveal it once a chest has been opened
			// (this event fires on every ChangeOpenChestTurn). Harmless on normal maps
			// where the group is already active.
			EnableContinueGroup();
		}

		public void EnableContinueGroup()
		{
			continueGroup.SetActive(true);
		}

		public void UpdateContinueButtonStatus()
		{
			continueButton.UpdateStatus();
		}

		public void RewardHandler(int chestID, int rewardID, int rewardValue)
		{
			switch (rewardID)
			{
			case 0:
			{
				List<int> listHeroIDNotOwned = HeroStore.Instance.GetListHeroIDNotOwned();
				listHeroIDNotOwned.Remove(0);
				listHeroIDNotOwned.Remove(1);
				if (listHeroIDNotOwned.Count > 0)
				{
					int num = listHeroIDNotOwned[UnityEngine.Random.Range(0, listHeroIDNotOwned.Count)];
					if (!HeroStore.Instance.IsHeroOwned(num))
					{
						HeroStore.Instance.UnlockHero(num);
					}
					chestGroupController.DisplayReward(chestID, "hero_" + num, false);
				}
				else
				{
					PlayerCurrencyStore.Instance.ChangeGem(rewardValue, false);
					chestGroupController.DisplayReward(chestID, "gem", true);
				}
				break;
			}
			case 1:
				if (!HeroStore.Instance.IsHeroOwned(0))
				{
                        MonoSingleton<LifespanSurface>.Instance.OfferPopupController.InitSingleHeroOffer(0, DealKind.OneTime);
                        chestGroupController.DisplayReward(chestID, "hero_offer", false);
					    UnityEngine.Debug.Log("hien thi goi offer hero" + 0);
				}
				else
				{
					PlayerCurrencyStore.Instance.ChangeGem(rewardValue, false);
					chestGroupController.DisplayReward(chestID, "gem", true);
				}
				break;
			case 2:
				HeroStore.Instance.LevelUp(1);
				chestGroupController.DisplayReward(chestID, "hero_1_max", false);
				break;
			case 3:
				HeroStore.Instance.LevelUp(2);
				chestGroupController.DisplayReward(chestID, "hero_2_max", false);
				break;
			case 4:
				PlayerCurrencyStore.Instance.ChangeGem(rewardValue, false);
				chestGroupController.DisplayReward(chestID, "gem", true);
				break;
			case 5:
				PlayerCurrencyStore.Instance.ChangeGem(rewardValue, false);
				chestGroupController.DisplayReward(chestID, "gem", true);
				break;
			case 6:
				PlayerCurrencyStore.Instance.ChangeGem(rewardValue, false);
				chestGroupController.DisplayReward(chestID, "gem", true);
				break;
			case 7:
				PowerUpItemStore.Instance.ChangeItemQuantity(0, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_0", true);
				break;
			case 8:
				PowerUpItemStore.Instance.ChangeItemQuantity(1, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_1", true);
				break;
			case 9:
				PowerUpItemStore.Instance.ChangeItemQuantity(2, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_2", true);
				break;
			case 10:
				PowerUpItemStore.Instance.ChangeItemQuantity(3, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_3", true);
				break;
			case 11:
				PowerUpItemStore.Instance.ChangeItemQuantity(1, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_1", true);
				break;
			case 12:
				if (!HeroStore.Instance.IsHeroOwned(0))
				{
					HeroStore.Instance.UnlockHero(0);
					chestGroupController.DisplayReward(chestID, "hero_0", false);
				}
				else
				{
					PlayerCurrencyStore.Instance.ChangeGem(rewardValue, false);
					chestGroupController.DisplayReward(chestID, "gem", true);
				}
				break;
			case 13:
				if (!HeroStore.Instance.IsHeroOwned(1))
				{
					HeroStore.Instance.UnlockHero(1);
					chestGroupController.DisplayReward(chestID, "hero_1", false);
				}
				else
				{
					PlayerCurrencyStore.Instance.ChangeGem(rewardValue, false);
					chestGroupController.DisplayReward(chestID, "gem", true);
				}
				break;
			case 14:
			{
				List<int> list = new List<int>();
				list.Add(3);
				list.Add(4);
				list.Add(5);
				list.Add(6);
				list.Add(7);
				list.Add(8);
				list.Add(9);
				if (list.Count > 0)
				{
					int num2 = list[UnityEngine.Random.Range(0, list.Count)];
					if (!HeroStore.Instance.IsHeroOwned(num2))
					{
						MonoSingleton<LifespanSurface>.Instance.OfferPopupController.InitSingleHeroOffer(num2, DealKind.OneTime);
						UnityEngine.Debug.Log("hien thi goi offer hero" + num2);
					}
					chestGroupController.DisplayReward(chestID, "hero_offer", false);
				}
				else
				{
					PlayerCurrencyStore.Instance.ChangeGem(rewardValue, false);
					chestGroupController.DisplayReward(chestID, "gem", true);
				}
				break;
			}
			}
		}

		public void ChangeChestQuantity()
		{
			currentChestOpened++;
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}

		[Space]
		[Header("Controllers")]
		[SerializeField]
		private UnlockCountClusterHandler unlockCountGroupController;

		[SerializeField]
		private CrateClusterHandler chestGroupController;

		[SerializeField]
		private CrateDealHandler chestOfferController;

		[SerializeField]
		private GlimpsePrizeClusterHandler previewRewardGroupController;

		[Space]
		[SerializeField]
		private GameObject continueGroup;

		[SerializeField]
		private ContinueSwitchHandler continueButton;

		[SerializeField]
		private Button autoOpenChestButton;

		private SpecInOneTurn[] turnsData;

		private int currentChestOpened;

		[Space]
		[Header("Tutorial")]
		[SerializeField]
		private TutorialOpenFortuneCrate tutorialOpenLuckyChest;

		[SerializeField]
		private TutorialGetFreeFortuneCrateByClip tutorialGetFreeLuckyChest;
	}
}
