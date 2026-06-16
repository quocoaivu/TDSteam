using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using GeneralVariable;
using LifetimePopup;
using Newtonsoft.Json.Linq;
using MetaGame;
using Parameter;
using UnityEngine;
using UnityEngine.Networking;

namespace Data
{
	public class WaveDataLoader : MonoSingleton<WaveDataLoader>
	{
        private List<Dictionary<string, object>> data;

        [Header("Tournament data")]
        [SerializeField]
        private string[] jsonDataTournamentMapString;

        private JArray jsonDataTournamentMap;

        private float timeOutTracking;

        private bool isTrackingTimeOut;

        private bool isReceivedWaveData;
        
		private void Start()
		{
			string filePath = string.Empty;
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						ReadTournamentWaveData();
					}
				}
				else
				{
					int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
					filePath = "Parameters/MapDailyTrial/map_daily_" + currentDayIndex;
					if (PlayerPrefs.GetInt("TESTcustomCSV", 0) > 0)
					{
						filePath = "Parameters/custom";
					}
					ReadWaveData(filePath);
					LoadingScreen.Instance.LoadSceneCompleted();
				}
			}
			else
			{
				filePath = "Parameters/MapCampaign/map_" + MonoSingleton<GameRecord>.Instance.MapID;
				if (PlayerPrefs.GetInt("TESTcustomCSV", 0) > 0)
				{
					filePath = "Parameters/custom";
				}
				ReadWaveData(filePath);
				LoadingScreen.Instance.LoadSceneCompleted();
			}
		}

		private void Update()
		{
			if (isTrackingTimeOut)
			{
				if (timeOutTracking == 0f)
				{
					CheckLoadingStatus();
				}
				timeOutTracking = Mathf.MoveTowards(timeOutTracking, 0f, Time.deltaTime);
			}
		}

		private void ReadWaveData(string filePath)
		{
			int battleDifficulty = (int)CrossSceneData.Instance.BattleDifficulty;
			try
			{
				if (data == null)
				{
					data = CSVLoader.Read(filePath);
				}
				MonoSingleton<GameRecord>.Instance.TotalHealth = (int)data[0]["health"];
				MonoSingleton<GameRecord>.Instance.CurrentHealth = MonoSingleton<GameRecord>.Instance.TotalHealth;
				MonoSingleton<GameRecord>.Instance.Money = (int)data[0]["money"];
				MonoSingleton<GameRecord>.Instance.DeltaTimeWave = (int)data[0]["delta_time"];
				for (int i = 0; i < data.Count; i++)
				{
					int wave = (int)data[i]["wave"];
					int num = (int)data[i]["time"];
					int enemyID = (int)data[i]["enemy_id"];
					int num2 = (int)data[i]["gate"];
					int num3 = (int)data[i]["formation_id"];
					int num4 = (int)data[i]["min_difficulty"];
					if (battleDifficulty >= num4)
					{
						if (num3 == 0)
						{
							MonoSingleton<GameRecord>.Instance.AddWavesEnemyData(wave, num, enemyID, num2, num3, num2);
						}
						else
						{
							FormationSetupRecord formationConfigData = ConfigRegistry.Instance.formationData[num3];
							for (int j = 0; j < formationConfigData.times.Count; j++)
							{
								MonoSingleton<GameRecord>.Instance.AddWavesEnemyData(wave, num + (int)(formationConfigData.times[j] * 1000f), enemyID, num2, num3, num2);
							}
						}
					}
				}
				MonoSingleton<GameRecord>.Instance.PostprocessWavesEnemyData();
				InitPoolObjects();
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
				throw;
			}
		}

		private void ReadTournamentWaveData()
		{
			// TEST: load from local Resources while the Firebase backend is down.
			if (GeneralVariable.GeneralDefine.TOURNAMENT_USE_LOCAL_DATA)
			{
				int mapID = ZoneRuleSpec.Instance.GetCurrentSeasonMapID();
				TextAsset textAsset = Resources.Load<TextAsset>(GeneralVariable.GeneralDefine.TOURNAMENT_LOCAL_DIR + "map_tournament_" + mapID);
				ParseTournamentWaveData(textAsset.text);
				LoadingScreen.Instance.LoadSceneCompleted();
				GameplayDirector.Instance.endlessModeManager.Init();
				return;
			}
			base.StartCoroutine(GetStoragedDataTournamentMap());
		}

		private IEnumerator GetStoragedDataTournamentMap()
		{
			timeOutTracking = (float)GeneralVariable.GeneralDefine.CONNECTION_TIMEOUT;
			isTrackingTimeOut = true;
			isReceivedWaveData = false;
			int mapIDTournament = ZoneRuleSpec.Instance.GetCurrentSeasonMapID();
			UnityWebRequest www = UnityWebRequest.Get(jsonDataTournamentMapString[mapIDTournament]);
			yield return www.SendWebRequest();
			if (www.result == UnityWebRequest.Result.ConnectionError)
			{
				UnityEngine.Debug.Log(www.error);
			}
			else
			{
				UnityEngine.Debug.Log(www.downloadHandler.text);
				ParseTournamentWaveData(www.downloadHandler.text);
				LoadingScreen.Instance.LoadSceneCompleted();
				GameplayDirector.Instance.endlessModeManager.Init();
				isReceivedWaveData = true;
			}
			yield break;
		}

		private void ParseTournamentWaveData(string rawJson)
		{
			jsonDataTournamentMap = JArray.Parse(rawJson);
			int battleDifficulty = (int)CrossSceneData.Instance.BattleDifficulty;
			MonoSingleton<GameRecord>.Instance.TotalHealth = (int)jsonDataTournamentMap[0]["health"];
			MonoSingleton<GameRecord>.Instance.CurrentHealth = MonoSingleton<GameRecord>.Instance.TotalHealth;
			MonoSingleton<GameRecord>.Instance.Money = (int)jsonDataTournamentMap[0]["money"];
			MonoSingleton<GameRecord>.Instance.DeltaTimeWave = (int)jsonDataTournamentMap[0]["delta_time"];
			for (int i = 0; i < jsonDataTournamentMap.Count; i++)
			{
				int wave = (int)jsonDataTournamentMap[i]["wave"];
				int num = (int)jsonDataTournamentMap[i]["time"];
				int enemyID = (int)jsonDataTournamentMap[i]["enemy_id"];
				int num2 = (int)jsonDataTournamentMap[i]["gate"];
				int num3 = (int)jsonDataTournamentMap[i]["formation_id"];
				int num4 = (int)jsonDataTournamentMap[i]["min_difficulty"];
				if (battleDifficulty >= num4)
				{
					if (num3 == 0)
					{
						MonoSingleton<GameRecord>.Instance.AddWavesEnemyData(wave, num, enemyID, num2, num3, num2);
					}
					else
					{
						FormationSetupRecord formationConfigData = ConfigRegistry.Instance.formationData[num3];
						for (int j = 0; j < formationConfigData.times.Count; j++)
						{
							MonoSingleton<GameRecord>.Instance.AddWavesEnemyData(wave, num + (int)(formationConfigData.times[j] * 1000f), enemyID, num2, num3, num2);
						}
					}
				}
			}
			MonoSingleton<GameRecord>.Instance.PostprocessWavesEnemyData();
			InitPoolObjects();
		}

		private void CheckLoadingStatus()
		{
			if (isReceivedWaveData)
			{
				UnityEngine.Debug.Log("Time out, get data completed!");
			}
			else
			{
				UnityEngine.Debug.Log("Time out, get data failed!");
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(150);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
				LoadingScreen.Instance.ShowLoading();
				base.CustomInvoke(DoLoad, 1f);
			}
			isTrackingTimeOut = false;
			isReceivedWaveData = false;
		}

		private void DoLoad()
		{
			FormatDirector.Instance.gameMode = GameFormat.CampaignMode;
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.WorldMapSceneName);
		}

		private void InitPoolObjects()
		{
			MonoSingleton<EnemyPool>.Instance.InitPoolEnemies();
			MonoSingleton<TowerPool>.Instance.InitTowerPool();
			GameplayDirector.Instance.LoadMap();
			MonoSingleton<AllyPool>.Instance.InitPoolHeroes();
			MonoSingleton<AllyPool>.Instance.InitHeroesStartPosition();
			InZoneBeginSurgeButtonsDirector.Instance.InitListButtons();
			InZoneBeginSurgeButtonsDirector.Instance.ShowListButtonOnStart();
		}
	}
}
