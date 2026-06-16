using System;
using System.Collections;
using System.Collections.Generic;
using LifetimePopup;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class CrateItem : SwitchHandler
	{
		public bool IsOpened
		{
			get
			{
				return isOpened;
			}
			set
			{
				isOpened = value;
			}
		}

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = base.GetComponent<Button>();
			animatorChest = base.GetComponentInChildren<Animator>();
		}

		public void Init()
		{
		}

		public override void OnClick()
		{
			base.OnClick();
			if (MonoSingleton<GameRecord>.Instance.isAvailableOpenChestTurn())
			{
				GetRewardParam();
				GetReward();
				PlayAnimChest();
				LockButton();
				MonoSingleton<UIRootHandler>.Instance.endGamePopupController.EndGameRewardPopupController.UpdateContinueButtonStatus();
			}
			else
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(17);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			}
		}

		private void GetReward()
		{
			MonoSingleton<GameRecord>.Instance.ChangeOpenChestTurn(-1);
			MonoSingleton<UIRootHandler>.Instance.endGamePopupController.EndGameRewardPopupController.ChangeChestQuantity();
		}

		private void GetRewardParam()
		{
			currentTurn = 2 - MonoSingleton<GameRecord>.Instance.CurrentOpenChestOffer;
			UnityEngine.Debug.Log("LÆ°á»£t má»Ÿ thá»© " + currentTurn);
			param = MonoSingleton<UIRootHandler>.Instance.endGamePopupController.EndGameRewardPopupController.TurnsData[currentTurn];
			UnityEngine.Debug.Log(param);
			List<int> listRate = new List<int>
			{
				param.heroRate,
				param.heroSellOfferRate_common,
				param.heroBonusExpRate_1,
				param.heroBonusExpRate_2,
				param.gemBonusRate_common,
				param.gemBonusRate_unCommon,
				param.gemBonusRate_rare,
				param.powerUpItemBonusRate_Freezing_Common,
				param.powerUpItemBonusRate_MeteorStrike_Common,
				param.powerUpItemBonusRate_HealingPotion_Common,
				param.powerUpItemBonusRate_GoldChest_Common,
				param.powerUpItemBonusRate_MeteorStrike_Rare,
				param.heroWukongRate,
				param.heroAshiRate,
				param.heroSellOfferRate_unCommon
			};
			rewardID = GetRewardByRate(listRate);
			rewardValue = FortuneCrateSpec.Instance.GetChestValue(rewardID, currentTurn);
			MonoSingleton<UIRootHandler>.Instance.endGamePopupController.EndGameRewardPopupController.RewardHandler(chestID, rewardID, rewardValue);
		}

		private int GetRewardByRate(List<int> listRate)
		{
			int result = -1;
			int num = UnityEngine.Random.Range(0, 100);
			if (num < listRate[0])
			{
				result = 0;
			}
			for (int i = 1; i < listRate.Count; i++)
			{
				if (num >= listRate[i - 1] && num <= listRate[i])
				{
					result = i;
				}
			}
			return result;
		}

		private void LockButton()
		{
			button.interactable = false;
			isOpened = true;
		}

		private void PlayAnimChest()
		{
			animatorChest.SetTrigger("Anim");
		}

		public void DisplayReward(string rewardName, bool isDisplayRewardValue)
		{
			base.StartCoroutine(DoDisplay(rewardName, isDisplayRewardValue));
		}

		private IEnumerator DoDisplay(string rewardName, bool isDisplayRewardValue)
		{
			yield return new WaitForSeconds(timeToOpen / Time.timeScale);
			rewardImage.gameObject.SetActive(true);
			rewardImage.sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_{0}", rewardName));
			rewardValueText.gameObject.SetActive(isDisplayRewardValue);
			rewardValueText.text = rewardValue.ToString();
			UISfxDirector.Instance.PlayluckyChestSound();
			yield break;
		}

		[SerializeField]
		private int chestID;

		private Button button;

		private Animator animatorChest;

		private bool isOpened;

		private int currentTurn;

		private SpecInOneTurn param;

		[SerializeField]
		private Image rewardImage;

		[SerializeField]
		private Text rewardValueText;

		private bool isDisplayRewardValue;

		private int rewardID;

		private int rewardValue;

		[SerializeField]
		private float timeToOpen;
	}
}
