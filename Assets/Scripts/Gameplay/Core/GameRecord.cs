using System;
using System.Collections.Generic;
using System.Diagnostics;
using CodeStage.AntiCheat.ObscuredTypes;
using Data;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class GameRecord : MonoSingleton<GameRecord>
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<TurretEntity> OnTowerAdded;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<TurretEntity, bool> OnTowerRemoved;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnMoneyChange;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnHealthChange;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnOpenChestTurnChange;

        private List<int> listEnemyID = new List<int>();

        private List<List<WaveEnemyEntry>> listAllEnemyWave = new List<List<WaveEnemyEntry>>();

        private List<EnemyData> listActiveEnemy = new List<EnemyData>();

        private List<EnemyData> listOnScreenEnemy = new List<EnemyData>();

        private List<TurretEntity> listActiveTower = new List<TurretEntity>();

        private List<CharacterEntity> listActiveAlly = new List<CharacterEntity>();

        private List<int> listHeroesIdsSelected = new List<int>();

        private bool recordingPosition;

        private TurretEntity currentTowerSelected;

        private bool playerKnowHowToUseSkill;

        private ObscuredInt _money;

        private ObscuredInt _currentHealth;

        [SerializeField]
        private PowerupItemRecord powerupItemData;

        public static float PIXEL_PER_UNIT = 80f;

        public static float REVERSE_PIXEL_PER_UNIT = 1f / GameRecord.PIXEL_PER_UNIT;

        private bool playedGameplayVideo_ForMoney;

        private bool playedGameplayVideo_ForLife;

        private bool isPlayingVideoAds;

        private bool playedVideoEndGame;

        private bool playedVideoLucky;

        private bool playedVideoGems;

        public List<int> ListEnemyID
		{
			get
			{
				return listEnemyID;
			}
			set
			{
				listEnemyID = value;
			}
		}

		public List<EnemyData> ListActiveEnemy
		{
			get
			{
				return listActiveEnemy;
			}
			set
			{
				listActiveEnemy = value;
			}
		}

		public List<EnemyData> ListOnScreenEnemy
		{
			get
			{
				return listOnScreenEnemy;
			}
			set
			{
				listOnScreenEnemy = value;
			}
		}

		public List<TurretEntity> ListActiveTower
		{
			get
			{
				return listActiveTower;
			}
			set
			{
				listActiveTower = value;
			}
		}

		public List<CharacterEntity> ListActiveAlly
		{
			get
			{
				return listActiveAlly;
			}
			private set
			{
				listActiveAlly = value;
			}
		}

		public List<int> ListHeroesIdsSelected
		{
			get
			{
				return listHeroesIdsSelected;
			}
			set
			{
				listHeroesIdsSelected = value;
			}
		}

		public bool RecordingPosition
		{
			get
			{
				return recordingPosition;
			}
			set
			{
				recordingPosition = value;
			}
		}

		public TurretEntity CurrentTowerSelected
		{
			get
			{
				return currentTowerSelected;
			}
			set
			{
				currentTowerSelected = value;
			}
		}

		public bool PlayerKnowHowToUseSkill
		{
			get
			{
				return playerKnowHowToUseSkill;
			}
			set
			{
				playerKnowHowToUseSkill = value;
			}
		}

		public int MapID
		{
			get
			{
				return CrossSceneData.Instance.MapIDSelected;
			}
		}

		public bool IsGameStart { get; set; }

		public bool IsGameOver { get; set; }

		public bool IsAnyTutorialPopupOpen { get; set; }

		public bool IsAnyPopupOpen { get; set; }

		public int Money
		{
			get
			{
				return _money - GameKit.deltaValue;
			}
			set
			{
				_money = value + GameKit.deltaValue;
				if (OnMoneyChange != null)
				{
					OnMoneyChange();
				}
			}
		}

		public int TotalEnemy { get; set; }

		public int GameplayGem { get; set; }

		public int TotalExp { get; set; }

		public int CurrentHealth
		{
			get
			{
				return _currentHealth - GameKit.deltaValue;
			}
			set
			{
				_currentHealth = value + GameKit.deltaValue;
				if (OnHealthChange != null)
				{
					OnHealthChange();
				}
			}
		}

		public int TotalHealth { get; set; }

		public bool IsPause { get; set; }

		public int TotalWave { get; set; }

		public int CurrentWave { get; set; }

		public bool IsLastEnemyInBattle { get; set; }

		public int DeltaTimeWave { get; set; }

		public bool IsVictory { get; set; }

		public int CurrentOpenChestTurn { get; set; }

		public int CurrentOpenChestOffer { get; set; }

		public float tournamentBattleTime { get; set; }

		public int PlayCountMapInCampaign { get; set; }

		public PowerupItemRecord PowerupItemData
		{
			get
			{
				return powerupItemData;
			}
			set
			{
				powerupItemData = value;
			}
		}

		public bool PlayedGameplayVideo_ForMoney
		{
			get
			{
				return playedGameplayVideo_ForMoney;
			}
			set
			{
				playedGameplayVideo_ForMoney = value;
			}
		}

		public bool PlayedGameplayVideo_ForLife
		{
			get
			{
				return playedGameplayVideo_ForLife;
			}
			set
			{
				playedGameplayVideo_ForLife = value;
			}
		}

		public bool IsPlayingVideoAds
		{
			get
			{
				return isPlayingVideoAds;
			}
			set
			{
				isPlayingVideoAds = value;
			}
		}

		public bool PlayedVideoEndGame
		{
			get
			{
				return playedVideoEndGame;
			}
			set
			{
				playedVideoEndGame = value;
			}
		}

		public bool PlayedVideoLucky
		{
			get
			{
				return playedVideoLucky;
			}
			set
			{
				playedVideoLucky = value;
			}
		}

		public bool PlayedVideoGems
		{
			get
			{
				return playedVideoGems;
			}
			set
			{
				playedVideoGems = value;
			}
		}

		private void resetDataPlayVideo()
		{
			PlayedGameplayVideo_ForMoney = false;
			PlayedGameplayVideo_ForLife = false;
			PlayedVideoEndGame = false;
			IsPlayingVideoAds = false;
			playedVideoLucky = false;
		}

		private void Awake()
		{
			SetDefaultGameData();
			InitListHeroIDSelected();
		}

		private void Update()
		{
			if (FormatDirector.Instance.gameMode == GameFormat.TournamentMode && IsGameStart && !IsGameOver && !IsPause)
			{
				TournamentBattleTimeCount();
			}
		}

		private void TournamentBattleTimeCount()
		{
			tournamentBattleTime += Time.deltaTime;
		}

		public void SetDefaultGameData()
		{
			IsAnyPopupOpen = false;
			IsAnyTutorialPopupOpen = false;
			PlayerKnowHowToUseSkill = false;
			IsGameStart = false;
			IsGameOver = false;
			GameplayGem = 0;
			TotalExp = 0;
			IsPause = false;
			IsVictory = false;
			TotalEnemy = 0;
			CurrentWave = 0;
			IsLastEnemyInBattle = false;
			CurrentOpenChestTurn = 3;
			CurrentOpenChestOffer = 2;
			resetDataPlayVideo();
			if (PowerupItemData)
			{
				PowerupItemData.InitValue();
			}
			if (FormatDirector.Instance.gameMode == GameFormat.CampaignMode)
			{
				PlayCountMapInCampaign = MapProgressStore.Instance.GetCurrentPlayCount(MapID);
			}
			GameSignalCenter.Instance.UnsubscribeIngameEvent();
			if (SignalQuestSystem.Instance != null)
			{
				SignalQuestSystem.Instance.SaveEvent();
			}
			tournamentBattleTime = 0f;
		}

		private void InitListHeroIDSelected()
		{
			foreach (int item in CrossSceneData.Instance.ListHeroesIdsSelected)
			{
				ListHeroesIdsSelected.Add(item);
			}
		}

		public bool IsListActiveEnemyContainThis(EnemyData enemyModel)
		{
			return ListActiveEnemy.Contains(enemyModel);
		}

		public void AddEnemyToListActiveEnemy(EnemyData enemyModel)
		{
			if (!IsListActiveEnemyContainThis(enemyModel))
			{
				ListActiveEnemy.Add(enemyModel);
			}
		}

		public void RemoveEnemyFromListActiveEnemy(EnemyData enemyModel)
		{
			if (IsListActiveEnemyContainThis(enemyModel))
			{
				listActiveEnemy.Remove(enemyModel);
			}
		}

		public void SetWavesEnemyData(int wave, int no, int time, int enemyID, bool isLastEnemyInWave, int gate, int formationId, int minDifficulty)
		{
			WaveEnemyEntry item = new WaveEnemyEntry(wave, time, enemyID, isLastEnemyInWave, gate, formationId, minDifficulty);
			if (wave >= listAllEnemyWave.Count)
			{
				List<WaveEnemyEntry> list = new List<WaveEnemyEntry>();
				list.Insert(no, item);
				listAllEnemyWave.Insert(wave, list);
			}
			else
			{
				List<WaveEnemyEntry> list2 = listAllEnemyWave[wave];
				list2.Insert(no, item);
			}
			TotalWave = wave + 1;
			CreateListEnemyID(enemyID);
		}

		public void AddWavesEnemyData(int wave, int time, int enemyID, int gate, int formationId, int minDifficulty)
		{
			WaveEnemyEntry item = new WaveEnemyEntry(wave, time, enemyID, false, gate, formationId, minDifficulty);
			if (wave >= listAllEnemyWave.Count)
			{
				listAllEnemyWave.Add(new List<WaveEnemyEntry>());
			}
			listAllEnemyWave[wave].Add(item);
			TotalWave = wave + 1;
			CreateListEnemyID(enemyID);
		}

		public void PostprocessWavesEnemyData()
		{
			for (int i = 0; i < listAllEnemyWave.Count; i++)
			{
				int index = 0;
				for (int j = 1; j < listAllEnemyWave[i].Count; j++)
				{
					if (listAllEnemyWave[i][j].time > listAllEnemyWave[i][index].time)
					{
						index = j;
					}
				}
				WaveEnemyEntry value = listAllEnemyWave[i][index];
				value.isLastInWave = true;
				listAllEnemyWave[i][index] = value;
			}
		}

		public List<WaveEnemyEntry> GetListEnemyWithWave(int wave)
		{
			if (wave >= listAllEnemyWave.Count)
			{
				return null;
			}
			return listAllEnemyWave[wave];
		}

		public List<int> GetListEnemyIDWithWave(int wave)
		{
			List<int> list = new List<int>();
			foreach (WaveEnemyEntry enemyData in listAllEnemyWave[wave])
			{
				if (!list.Contains(enemyData.id))
				{
					list.Add(enemyData.id);
				}
			}
			return list;
		}

		public int GetTotalEnemyInWave(int enemyID, int wave)
		{
			int num = 0;
			foreach (WaveEnemyEntry enemyData in listAllEnemyWave[wave])
			{
				if (enemyData.id == enemyID)
				{
					num++;
				}
			}
			return num;
		}

		private void CreateListEnemyID(int id)
		{
			if (!ListEnemyID.Contains(id))
			{
				ListEnemyID.Add(id);
			}
		}

		public void ClearListEnemyID()
		{
			ListEnemyID.Clear();
		}

		public void GetInRangeEnemies(Vector2 centerPoint, float range, List<EnemyData> inRangeEnemies)
		{
			inRangeEnemies.Clear();
			float num = range * range;
			for (int i = 0; i < ListActiveEnemy.Count; i++)
			{
				EnemyData enemyModel = ListActiveEnemy[i];
				if (Vector2.SqrMagnitude(enemyModel.transform.position - (Vector3)centerPoint) <= num && !enemyModel.IsUnderground)
				{
					inRangeEnemies.Add(enemyModel);
				}
			}
		}

		public bool IsNoEnemyLeft()
		{
			return ListActiveEnemy.Count == 0;
		}

		private void SetListEnemyOnScreen()
		{
			ListOnScreenEnemy.Clear();
			if (ListActiveEnemy.Count <= 0)
			{
				return;
			}
			foreach (EnemyData enemyModel in ListActiveEnemy)
			{
				if (enemyModel.transform.position.x < 6f && enemyModel.transform.position.x > -6f && enemyModel.transform.position.y < 4f && enemyModel.transform.position.y > -4f)
				{
					ListOnScreenEnemy.Add(enemyModel);
				}
			}
		}

		public EnemyData getEnemyWithHighestHealth(bool isFlyEnemy, bool isUndergroundEnemy)
		{
			EnemyData enemyModel = null;
			SetListEnemyOnScreen();
			if (ListOnScreenEnemy.Count > 0)
			{
				enemyModel = ListOnScreenEnemy[0];
				for (int i = 0; i < ListOnScreenEnemy.Count; i++)
				{
					if (enemyModel.EnemyHealthController.CurrentHealth < ListOnScreenEnemy[i].EnemyHealthController.CurrentHealth && ListOnScreenEnemy[i].IsAir == isFlyEnemy && ListOnScreenEnemy[i].IsUnderground == isUndergroundEnemy)
					{
						enemyModel = ListOnScreenEnemy[i];
					}
				}
			}
			if (enemyModel && enemyModel.IsAir && !isFlyEnemy)
			{
				return null;
			}
			if (enemyModel && enemyModel.IsUnderground && !isUndergroundEnemy)
			{
				return null;
			}
			return enemyModel;
		}

		public void AddTowerToActiveList(TurretEntity towerModel)
		{
			ListActiveTower.Add(towerModel);
			if (OnTowerAdded != null)
			{
				OnTowerAdded(towerModel);
			}
		}

		public void RemoveTowerFromActiveList(TurretEntity towerModel, bool isSold)
		{
			ListActiveTower.Remove(towerModel);
			if (OnTowerRemoved != null)
			{
				OnTowerRemoved(towerModel, isSold);
			}
		}

		public void GetInRangeTowers(Vector2 centerPoint, float range, List<TurretEntity> towers, List<TurretEntity> inRangeTowers)
		{
			inRangeTowers.Clear();
			float num = range * range;
			for (int i = 0; i < towers.Count; i++)
			{
				TurretEntity towerModel = towers[i];
				if (Vector2.SqrMagnitude(towerModel.transform.position - (Vector3)centerPoint) <= num)
				{
					inRangeTowers.Add(towerModel);
				}
			}
		}

		public void GetNearestTowers(Vector2 centerPoint, int maxTowers, List<TurretEntity> towers, List<TurretEntity> nearestTowers, List<float> squaredDistances)
		{
			nearestTowers.Clear();
			squaredDistances.Clear();
			for (int i = 0; i < towers.Count; i++)
			{
				TurretEntity towerModel = towers[i];
				if (nearestTowers.Count < maxTowers)
				{
					nearestTowers.Add(towerModel);
					squaredDistances.Add(Vector2.SqrMagnitude(towerModel.CachedPosition - (Vector3)centerPoint));
				}
				else
				{
					float num = Vector2.SqrMagnitude(towerModel.CachedPosition - (Vector3)centerPoint);
					for (int j = 0; j < maxTowers; j++)
					{
						if (num < squaredDistances[j])
						{
							squaredDistances[j] = num;
							nearestTowers[j] = towerModel;
							break;
						}
					}
				}
			}
		}

		public void GetInRangeTowers(Vector2 centerPoint, float range, List<TurretEntity> inRangeTowers)
		{
			GetInRangeTowers(centerPoint, range, ListActiveTower, inRangeTowers);
		}

		[Obsolete]
		public List<TurretEntity> GetNearestTowers(Vector2 centerPoint, int maxTowers, List<TurretEntity> towers)
		{
			List<TurretEntity> list = new List<TurretEntity>();
			GetNearestTowers(centerPoint, maxTowers, towers, list, new List<float>());
			return list;
		}

		public bool IsInRange(GameObject source, GameObject target, float sqrMaxRange, float sqrMinRange)
		{
			float num = SqrDistance(source, target);
			bool flag = false;
			bool flag2 = false;
			if (num <= sqrMaxRange)
			{
				flag = true;
			}
			if (sqrMinRange <= num)
			{
				flag2 = true;
			}
			return flag && flag2;
		}

		public float SqrDistance(GameObject source, GameObject target)
		{
			return (source.transform.position - target.transform.position).sqrMagnitude;
		}

		public float SqrDistance(Vector3 p1, Vector3 p2)
		{
			return (p1 - p2).sqrMagnitude;
		}

		public void IncreaseMoney(ObscuredInt amount)
		{
			Money += amount;
			if (amount > 0)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.EventEarnGold, new SignalTriggerRecord(SignalTriggerKind.EarnGold, amount, false));
			}
		}

		public void DecreaseMoney(ObscuredInt amount)
		{
			Money -= amount;
		}

		public void ChangeOpenChestTurn(int amount)
		{
			CurrentOpenChestTurn += amount;
			if (OnOpenChestTurnChange != null)
			{
				OnOpenChestTurnChange();
			}
		}

		public void ChangeOpenChestOffer()
		{
			CurrentOpenChestOffer--;
		}

		public bool isAvailableOpenChestTurn()
		{
			return CurrentOpenChestTurn >= 1;
		}

		public bool isAvailableOpenChestOffer()
		{
			return CurrentOpenChestOffer >= 1;
		}

		public int GetActuallyGemAmount()
		{
			UnityEngine.Debug.Log("Tá»•ng sá»‘ gem nháº­n Ä‘c lÃ  = " + GameplayGem);
			return GameplayGem;
		}
	}
}
