using System;
using Data;
using MetaGame;
using GameCore;
using Services.PlatformSpecific;

namespace Gameplay
{
	public class GameRuleHandler : BaseMonoBehaviour
	{
		private void Start()
		{
			GameRecord gd = MonoSingleton<GameRecord>.Instance;
			gd.OnMoneyChange += Instance_OnMoneyChange;
			gd.OnHealthChange += Instance_OnHealthChange;
			// Paint initial money/health: they may have been set before this subscribed.
			UpdateMoney();
			UpdateHealth();
		}

		private void Instance_OnMoneyChange()
		{
			UpdateMoney();
		}

		private void Instance_OnHealthChange()
		{
			UpdateHealth();
		}

		private void Update()
		{
			if (MonoSingleton<GameRecord>.Instance.IsGameOver) return;
			// Money and health are event-driven (OnMoneyChange / OnHealthChange).
			// Victory still polls: it depends on TotalEnemy + ListActiveEnemy with no single change event.
			UpdateVictory();
		}

		private void OnDestroy()
		{
			GameRecord gd = MonoSingleton<GameRecord>.InstanceIfExists;
			if (gd != null)
			{
				gd.OnMoneyChange -= Instance_OnMoneyChange;
				gd.OnHealthChange -= Instance_OnHealthChange;
			}
		}

		private void UpdateHealth()
		{
			MonoSingleton<UIRootHandler>.Instance.playerHealthController.SetHealthMessage();
		}

		private void UpdateMoney()
		{
			MonoSingleton<UIRootHandler>.Instance.moneyController.SetMoneyMessage();
		}

		private void UpdateVictory()
		{
			var data = MonoSingleton<GameRecord>.Instance;
			if (!data.IsLastEnemyInBattle) return;
			// ListActiveEnemy.Count is the authoritative "field empty" signal;
			// TotalEnemy is a secondary cross-check (kept correct by the enemy decrement paths).
			if (data.TotalEnemy > 0) return;
			if (data.ListActiveEnemy.Count > 0) return;
			if (data.CurrentHealth <= 0) return;
			if (data.IsVictory) return;
			data.IsVictory = true;
			HandleVictoryByMode();
		}

		private void HandleVictoryByMode()
		{
			switch (FormatDirector.Instance.gameMode)
			{
				case GameFormat.CampaignMode:
					base.CustomInvoke(new Action(OnCampaignVictory), RESULT_POPUP_DELAY);
					break;
				case GameFormat.DailyTrialMode:
					base.CustomInvoke(new Action(OnDailyTrialResult_Victory), RESULT_POPUP_DELAY);
					break;
				case GameFormat.TournamentMode:
					OnTournament_EndGame();
					break;
			}
		}

		private void HandleDefeatByMode()
		{
			switch (FormatDirector.Instance.gameMode)
			{
				case GameFormat.CampaignMode:
					OnCampaign_Defeat();
					break;
				case GameFormat.DailyTrialMode:
					OnDailyTrialResult_Defeat();
					break;
				case GameFormat.TournamentMode:
					OnTournament_EndGame();
					break;
			}
		}

		private const float RESULT_POPUP_DELAY = 2f;

		public void IncreaseHealth(int health)
		{
			if (health <= 0) return;
			if (MonoSingleton<GameRecord>.Instance.CurrentHealth <= 0) return;
			MonoSingleton<GameRecord>.Instance.CurrentHealth += health;
		}

		public void DecreaseHealth(int health)
		{
			var data = MonoSingleton<GameRecord>.Instance;
			if (data.CurrentHealth <= 0) return;
			data.CurrentHealth -= health;
			if (data.CurrentHealth <= 0)
			{
				HandleDefeatByMode();
			}
		}

		private void OnCampaign_Defeat()
		{
			AdsSettings.showInterstitialCombination();
			// TODO: re-enable end-game video reward (watch ad for extra life).
			// When enabling: delete `Defeated()` below and uncomment the block.
			// Related controllers: EndGameClipHandler, WatchClipSwitchHandler, SkipSwitchHandler.
			Defeated();
			/*
			if (ClipPlayerDirector.Instance.CheckIfVideoExits())
			{
				GameplayDirector.Instance.gameSpeedController.PauseGame();
				if (!SingletonMonoBehaviour<GameRecord>.Instance.PlayedVideoEndGame)
				{
					SingletonMonoBehaviour<UIRootHandler>.Instance.endGameVideoController.Init();
				}
				else
				{
					GameplayDirector.Instance.gameSpeedController.UnPauseGame();
					Defeated();
				}
			}
			else
			{
				Defeated();
			}
			*/
		}

		public void Defeated()
		{
			MonoSingleton<GameRecord>.Instance.CurrentHealth = 0;
			MonoSingleton<UIRootHandler>.Instance.endGamePopupController.Init(BattleStanding.Defeat);
			// playCount is total attempts (win + lose), so count this defeat toward it too.
			MapProgressStore.Instance.IncreaseMapPlaycount(MonoSingleton<GameRecord>.Instance.MapID);
			MapProgressStore.Instance.IncreaseMapPlaycount(MonoSingleton<GameRecord>.Instance.MapID, BattleStanding.Defeat);
			UISfxDirector.Instance.PlayDefeat();
		}

		private void OnCampaignVictory()
		{
			MonoSingleton<UIRootHandler>.Instance.endGamePopupController.Init(BattleStanding.Victory);
			MapProgressStore.Instance.IncreaseMapIdUnlock(MonoSingleton<GameRecord>.Instance.MapID);
			MapProgressStore.Instance.IncreaseModeResult(MonoSingleton<GameRecord>.Instance.MapID);
			MapProgressStore.Instance.IncreaseMapPlaycount(MonoSingleton<GameRecord>.Instance.MapID);
			MapProgressStore.Instance.IncreaseMapPlaycount(MonoSingleton<GameRecord>.Instance.MapID, BattleStanding.Victory);
			int actuallyGemAmount = MonoSingleton<GameRecord>.Instance.GetActuallyGemAmount();
			PlayerCurrencyStore.Instance.ChangeGem(actuallyGemAmount, false);
			AwardSkillPoint();
			UISfxDirector.Instance.PlayVictory();
		}

		// Skill Point (meta currency for the tower skill tree) earned on a campaign win, scaled by the
		// star rating (1-3) using the same health->star rule as the end-game popup.
		private void AwardSkillPoint()
		{
			var data = MonoSingleton<GameRecord>.Instance;
			int star = Parameter.StarSpec.Instance.GetStar(100 * data.CurrentHealth / data.TotalHealth);
			TowerSkillPointStore.Instance.AddSkillPoint(star, false);
		}

		public void EndGame()
		{
			MonoSingleton<GameRecord>.Instance.IsGameOver = true;
			GameplayDirector.Instance.gameSpeedController.SetNormalSpeed();
			// TODO: lÃ m láº¡i hiá»‡u á»©ng Ä‘áº©y UI ra khi end game (DOTween)
			//NativeSpecificServicesSource.Services.DataCloudSaver.AutoBackUpData();
		}

		private void OnDailyTrialResult_Victory()
		{
			MonoSingleton<GameRecord>.Instance.IsGameOver = true;
			GameplayDirector.Instance.gameSpeedController.SetNormalSpeed();
			MonoSingleton<UIRootHandler>.Instance.dailyTrialResultPopupController.Init(BattleStanding.Victory);
			SendEvent_EndGameDailyTrial(MonoSingleton<GameRecord>.Instance.CurrentWave);
		}

		private void OnDailyTrialResult_Defeat()
		{
			MonoSingleton<GameRecord>.Instance.IsGameOver = true;
			GameplayDirector.Instance.gameSpeedController.SetNormalSpeed();
			MonoSingleton<UIRootHandler>.Instance.dailyTrialResultPopupController.Init(BattleStanding.Defeat);
			SendEvent_EndGameDailyTrial(Math.Max(0, MonoSingleton<GameRecord>.Instance.CurrentWave - 1));
		}

		private void SendEvent_EndGameDailyTrial(int currentWavePassed)
		{
			int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
			int playCount = 0;
			int mapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked();
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_EndGameDailyTrial(currentDayIndex, currentWavePassed, playCount, mapIDUnlocked);
		}

		// Tournament is a time-trial: ranking is based on battle time, not win/lose.
		// Called from both victory and defeat paths intentionally â€” the popup shows
		// leaderboard, never a "you win/lose" banner.
		private void OnTournament_EndGame()
		{
			MonoSingleton<GameRecord>.Instance.IsGameOver = true;
			GameplayDirector.Instance.gameSpeedController.SetNormalSpeed();
			MonoSingleton<UIRootHandler>.Instance.tournamentResultPopupController.Init();
		}
	}
}
