using System;
using System.Collections.Generic;
using Data;
using MetaGame;
using Parameter;
using Services.PlatformSpecific;
using Common;
using UnityEngine;

namespace Gameplay
{
	public class GameplayDirector : MonoBehaviour
	{
		public static GameplayDirector Instance { get; private set; }

        [Header("Events")]
        [SerializeField]
        private OrderedUnityEvent OnStartEvent;

        [Space]
        [SerializeField]
        private OrderedUnityEvent OnEnemyDieEvent;

        [Space]
        public HerosDirector heroesManager;

        [SerializeField]
        private GameObject prefabTowerRange;

        private GameObject _currentTowerRange;

        [NonSerialized]
        public GameHasteHandler gameSpeedController;

        [NonSerialized]
        public GameRuleHandler gameLogicController;

        [NonSerialized]
        public InfiniteFormatDirector endlessModeManager;

        private int customWave;

        public void Awake()
		{
			GameplayDirector.Instance = this;
			GetAllComponents();
			SetDefaultGameData();
		}

		private void GetAllComponents()
		{
			gameSpeedController = base.GetComponentInChildren<GameHasteHandler>();
			gameLogicController = base.GetComponentInChildren<GameRuleHandler>();
			endlessModeManager = base.GetComponentInChildren<InfiniteFormatDirector>();
		}

		public void Start()
		{
			OnStartEvent.Dispatch();
			gameSpeedController.SetNormalSpeed();
			SendEventStartGame();
		}

		private void OnDestroy()
		{
			if (GameplayDirector.Instance == this)
			{
				GameplayDirector.Instance = null;
			}
		}

		private void SetDefaultGameData()
		{
			MonoSingleton<GameRecord>.Instance.SetDefaultGameData();
			customWave = -1;
		}

		public void LoadMap()
		{
			string path = string.Empty;
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						int currentSeasonMapID = ZoneRuleSpec.Instance.GetCurrentSeasonMapID();
						path = string.Format("Maps/map_tournament_{0}", currentSeasonMapID);
					}
				}
				else
				{
					int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
					int map_id = DailyOrdealSpec.Instance.GetParameter(currentDayIndex).map_id;
					path = string.Format("Maps/map_daily_{0}", map_id);
				}
			}
			else
			{
				path = string.Format("Maps/map_campaign_{0}", MonoSingleton<GameRecord>.Instance.MapID);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(Common.AssetLoader.Load<GameObject>(path));
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.position = Vector3.zero;
		}

		private void SendEventStartGame()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode == GameFormat.CampaignMode)
			{
				int mapID = MonoSingleton<GameRecord>.Instance.MapID;
				List<int> listHeroesIdsSelected = MonoSingleton<GameRecord>.Instance.ListHeroesIdsSelected;
				string heroName = Singleton<HeroSynopsis>.Instance.GetHeroName(listHeroesIdsSelected[0]);
				int hero1Level = HeroStore.Instance.GetCurrentHeroLevel(listHeroesIdsSelected[0]) + 1;
				string hero2Name = "Empty";
				int hero2Level = -1;
				if (listHeroesIdsSelected.Count > 1)
				{
					hero2Name = Singleton<HeroSynopsis>.Instance.GetHeroName(listHeroesIdsSelected[1]);
					hero2Level = HeroStore.Instance.GetCurrentHeroLevel(listHeroesIdsSelected[1]) + 1;
				}
				string hero3Name = "Empty";
				int hero3Level = -1;
				if (listHeroesIdsSelected.Count > 2)
				{
					hero3Name = Singleton<HeroSynopsis>.Instance.GetHeroName(listHeroesIdsSelected[2]);
					hero3Level = HeroStore.Instance.GetCurrentHeroLevel(listHeroesIdsSelected[2]) + 1;
				}
				int currentItemQuantity = PowerUpItemStore.Instance.GetCurrentItemQuantity(0);
				int currentItemQuantity2 = PowerUpItemStore.Instance.GetCurrentItemQuantity(1);
				int currentItemQuantity3 = PowerUpItemStore.Instance.GetCurrentItemQuantity(2);
				int currentItemQuantity4 = PowerUpItemStore.Instance.GetCurrentItemQuantity(3);
				//NativeSpecificServicesSource.Services.Analytics.SendEvent_StartGame(mapID + 1, heroName, hero1Level, hero2Name, hero2Level, hero3Name, hero3Level, currentItemQuantity, currentItemQuantity2, currentItemQuantity3, currentItemQuantity4);
			}
		}

		public void OnEnemyDie()
		{
			OnEnemyDieEvent.Dispatch();
		}

		public void ReloadCurrentScene()
		{
			GameSceneLoader.Instance.ReloadGameplayScene();
		}

		public GameObject CurrentTowerRange
		{
			get
			{
				if (_currentTowerRange == null)
				{
					_currentTowerRange = UnityEngine.Object.Instantiate<GameObject>(prefabTowerRange);
				}
				return _currentTowerRange;
			}
			set
			{
				_currentTowerRange = value;
			}
		}

		public void StartWave()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						StartWave_TournamentMode();
					}
				}
				else
				{
					StartWave_CampaignMode();
				}
			}
			else
			{
				StartWave_CampaignMode();
			}
			MonoSingleton<UIRootHandler>.Instance.InMapStartWaveButtonsManager.Hide();
			MonoSingleton<GameRecord>.Instance.IsGameStart = true;
			MonoSingleton<UIRootHandler>.Instance.WaveMessageController.SetWaveMessage();
		}

		private void StartWave_CampaignMode()
		{
			if (MonoSingleton<GameRecord>.Instance.CurrentWave < MonoSingleton<GameRecord>.Instance.TotalWave)
			{
				MonoSingleton<GameRecord>.Instance.CurrentWave++;
				if (MonoSingleton<GameRecord>.Instance.CurrentWave < customWave)
				{
					MonoSingleton<GameRecord>.Instance.CurrentWave = customWave;
				}
				MonoSingleton<EnemyPool>.Instance.StartWaveNormal(MonoSingleton<GameRecord>.Instance.CurrentWave - 1);
			}
		}

		private void StartWave_TournamentMode()
		{
			endlessModeManager.IncreaseWavePassed();
			if (MonoSingleton<GameRecord>.Instance.CurrentWave < MonoSingleton<GameRecord>.Instance.TotalWave)
			{
				MonoSingleton<GameRecord>.Instance.CurrentWave++;
				MonoSingleton<EnemyPool>.Instance.StartWaveNormal(MonoSingleton<GameRecord>.Instance.CurrentWave - 1);
			}
			if (!endlessModeManager.IsLastEnemyInNormalWave)
			{
				return;
			}
			endlessModeManager.FirstTimeIncreaseLoopAmount();
			MonoSingleton<EnemyPool>.Instance.StartWaveNormal(endlessModeManager.CurrentWaveEndless);
			endlessModeManager.IncreaseCurrentWaveEndless();
		}

		public void ShowStartWaveButton(int wave, List<int> listEnemyGate)
		{
			MonoSingleton<UIRootHandler>.Instance.InMapStartWaveButtonsManager.Show(wave, listEnemyGate, (float)MonoSingleton<GameRecord>.Instance.DeltaTimeWave / 1000f);
		}

		public void SetCustomWaveForTest(int customWave)
		{
			this.customWave = customWave;
		}

		public void GetEndingVideoReward()
		{
			int endGameVideoReward = MonoSingleton<GameplayRecordLoader>.Instance.EndGameVideoProvider.GetEndGameVideoReward();
			gameLogicController.IncreaseHealth(endGameVideoReward);
		}
	}
}
