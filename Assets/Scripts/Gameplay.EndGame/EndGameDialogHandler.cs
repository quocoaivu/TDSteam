using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Gameplay.EndGame.Reward;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay.EndGame
{
	public class EndGameDialogHandler : GameplayDialogHandler
	{
		public EndGamePrizeDialogHandler EndGameRewardPopupController
		{
			get
			{
				return endGameRewardPopupController;
			}
			set
			{
				endGameRewardPopupController = value;
			}
		}

		public BattleStanding BattleStanding
		{
			get
			{
				return battleStatus;
			}
			private set
			{
				battleStatus = value;
			}
		}

		public int NumberStar
		{
			get
			{
				return numberStar;
			}
			set
			{
				numberStar = value;
			}
		}

		public void Init(BattleStanding battleStatus)
		{
			BattleStanding = battleStatus;
			GameplayDirector.Instance.gameLogicController.EndGame();
			SendEvent_EndGame();
			base.CustomInvoke(new Action(Open), delayTimeToOpen);
			StopBackgroundMusic();
			CloseAnyPopup();
		}

		private void CloseAnyPopup()
		{
		}

		private void StopBackgroundMusic()
		{
		}

		private void Victory()
		{
            //ads
            AdsSettings.showInterstitialCombination();

            for (int i = MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected.Count - 1; i >= 0; i--)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseHero, new SignalTriggerRecord(SignalTriggerKind.UseHeroWinCampaign, MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected[i], 1, true));
			}
			victoryGroup.SetActive(true);
			heroLevelUpItemGroupController.InitData();
			PlaySoundVictory();
			base.StartCoroutine(PlayStarsEffect());
			DisplayGem();
			SendEventEndGame_Victory();
		}

		private void SendEvent_EndGame()
		{
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_EndGame();
		}

		private void SendEventEndGame_Victory()
		{
			int mapID = MonoSingleton<GameRecord>.Instance.MapID + 1;
			int starEarned = NumberStar;
			int currentPlayCount = MapProgressStore.Instance.GetCurrentPlayCount(MonoSingleton<GameRecord>.Instance.MapID);
			int actuallyGemAmount = MonoSingleton<GameRecord>.Instance.GetActuallyGemAmount();
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_EndGame(mapID, starEarned, actuallyGemAmount, currentPlayCount);
		}

		private void DisplayGem()
		{
			gemAmountText.text = MonoSingleton<GameRecord>.Instance.GetActuallyGemAmount().ToString();
		}

		private void PlaySoundVictory()
		{
		}

		private void CalculatorStarCount()
		{
			NumberStar = StarSpec.Instance.GetStar(100 * MonoSingleton<GameRecord>.Instance.CurrentHealth / MonoSingleton<GameRecord>.Instance.TotalHealth);
			UnityEngine.Debug.Log("number star = " + NumberStar);
			MapProgressStore.Instance.SaveStarEarned(MonoSingleton<GameRecord>.Instance.MapID, NumberStar);
		}

		private IEnumerator PlayStarsEffect()
		{
			CalculatorStarCount();
			yield return new WaitForSeconds(delayTImeToInvokeStars);
			for (int i = 0; i < NumberStar; i++)
			{
				listStar[i].SetActive(true);
				yield return new WaitForSeconds(timeBetweenStars);
			}
			yield break;
		}

		public void PlayStarSound()
		{
		}

		public void TryInitEndGameRewardPopup()
		{
			int currentPlayCount = MapProgressStore.Instance.GetCurrentPlayCount(MonoSingleton<GameRecord>.Instance.MapID);
			UnityEngine.Debug.Log("current map play count = " + currentPlayCount);
			if (currentPlayCount > 1)
			{
				LoadingScreen.Instance.ShowLoading();
				base.Invoke("DoLoad", 1f);
				GameplayDirector.Instance.gameSpeedController.UnPauseGame();
			}
			else
			{
				EndGameRewardPopupController.Init();
				victoryGroup.SetActive(false);
			}
		}

		private void DoLoad()
		{
			ClipPlayerDirector.Instance.TryToShowInterstitialAds_EndGame();
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.WorldMapSceneName);
			MonoSingleton<LifespanSurface>.Instance.TryOpenAskRatePopup(MonoSingleton<GameRecord>.Instance.MapID);
		}

		private void Defeat()
		{
			defeatGroup.SetActive(true);
			PlaySoundDefeat();
		}

		private void PlaySoundDefeat()
		{
		}

		public override void Open()
		{
			base.Open();
			OpenWithScaleAnimation();
			BattleStanding battleStatus = BattleStanding;
			if (battleStatus != BattleStanding.Victory)
			{
				if (battleStatus == BattleStanding.Defeat)
				{
					GameSignalCenter.Instance.Trigger(GameSignalKind.EventCampaign, new SignalTriggerRecord(SignalTriggerKind.LoseCampaign, 1, true));
					Defeat();
				}
			}
			else
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.EventCampaign, new SignalTriggerRecord(SignalTriggerKind.WinCampaign, 1, true));
				Victory();
			}
		}

		public override void Close()
		{
			base.Close();
			base.gameObject.SetActive(false);
		}

		[Space]
		[Header("Attribute")]
		[SerializeField]
		private GameObject victoryGroup;

		[SerializeField]
		private GameObject defeatGroup;

		[Space]
		[Header("Victory elements")]
		[SerializeField]
		private EndGamePrizeDialogHandler endGameRewardPopupController;

		[SerializeField]
		[FormerlySerializedAs("heroesLevelUpItemGroupController")]
		private HeroLevelUpItemGroupController heroLevelUpItemGroupController;

		[SerializeField]
		private Text gemAmountText;

		[SerializeField]
		private List<GameObject> listStar;

		[Space]
		[SerializeField]
		private float delayTimeToOpen;

		[SerializeField]
		private float delayTImeToInvokeStars;

		[SerializeField]
		private float timeBetweenStars;

		private BattleStanding battleStatus;

		private int numberStar;
	}
}
