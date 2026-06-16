using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using GeneralVariable;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class EnemyPool : MonoSingleton<EnemyPool>
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action onDispatchEventTutorialHeroSkill;

        // Hardcoded enemy IDs whose spawn gate must be tracked for skills/tutorial.
        private const int ENEMY_ID_WOLFMAN = 9;
        private const int ENEMY_ID_SNAIL = 10;
        private const int ENEMY_ID_GOLDEN_SNAIL = 12;

        [SerializeField]
        private int mapIDTutorial;

        [SerializeField]
        private int waveIDTutorialHeroSkill;

        [NonSerialized]
        public int currentGateSnail;

        [NonSerialized]
        public int currentGateWolfman;

        [NonSerialized]
        public int currentGateGoldenSnail;

        private bool isDispatchEvent_TutorialHeroSkill;


        public void InitPoolEnemies()
		{
			var enemyIds = GameRecord.Instance.ListEnemyID;
			for (int i = 0; i < enemyIds.Count; i++)
			{
				PoolEnemy(enemyIds[i]);
			}
			Common.GameObjectPool.InitPool(GeneralVariable.GeneralDefine.SELECT_ENEMY_INDICATOR_PATH, 0);
		}

		public void InitAdditionEnemy(int enemyID)
		{
			PoolEnemy(enemyID);
		}

		// Load, register and pre-despawn one enemy prefab into the pool.
		private void PoolEnemy(int enemyID)
		{
			EnemyData prefab = Common.AssetLoader.Load<EnemyData>(string.Format("Enemies/enemy_{0}", enemyID));
			if (prefab == null)
			{
				UnityEngine.Debug.LogError("PoolEnemy: enemy prefab not found: enemy_" + enemyID);
				return;
			}
			EnemyData enemyModel = Instantiate(prefab);
			enemyModel.gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(enemyModel.gameObject, 0);
			Common.GameObjectPool.Despawn(enemyModel.gameObject);
		}

		public void StartWaveNormal(int wave)
		{
			List<WaveEnemyEntry> listEnemyWithWave = GameRecord.Instance.GetListEnemyWithWave(wave);
			int num = listEnemyWithWave.Count;
			for (int i = 0; i < num; i++)
			{
				bool lastEnemyInBattle = false;
				WaveEnemyEntry enemyData = listEnemyWithWave[i];
				int time = enemyData.time;
				int id = enemyData.id;
				int gate = enemyData.gate;
				// The last enemy in battle is the latest-spawning one of the final wave,
				// not the last CSV row. Use isLastInWave (max-time, set by
				// PostprocessWavesEnemyData) so victory can't fire before it spawns.
				if (enemyData.isLastInWave && wave + 1 >= GameRecord.Instance.TotalWave)
				{
					lastEnemyInBattle = true;
				}
				bool isLastInWave = enemyData.isLastInWave;
				StartCoroutine(Spawn(wave, time, id, CrossSceneData.Instance.BattleDifficulty, gate, lastEnemyInBattle, isLastInWave));
			}
		}

		private IEnumerator Spawn(int wave, int time, int id, BattleDifficulty level, int gate, bool lastEnemyInBattle, bool lastEnemyInWave)
		{
			yield return new WaitForSeconds((float)time / 1000f);
			if (GameRecord.Instance.MapID == mapIDTutorial && wave == waveIDTutorialHeroSkill && !isDispatchEvent_TutorialHeroSkill)
			{
				if (onDispatchEventTutorialHeroSkill != null)
				{
					onDispatchEventTutorialHeroSkill();
				}
				isDispatchEvent_TutorialHeroSkill = true;
			}
			if (id == ENEMY_ID_WOLFMAN)
			{
				currentGateWolfman = gate;
			}
			if (id == ENEMY_ID_SNAIL)
			{
				currentGateSnail = gate;
			}
			if (id == ENEMY_ID_GOLDEN_SNAIL)
			{
				currentGateGoldenSnail = gate;
			}
			if (EnemyDatabase.Instance.IsBoss(id))
			{
				yield return new WaitUntil(new Func<bool>(GameRecord.Instance.IsNoEnemyLeft));
			}
			EnemyData enemy = GetEnemy(id);
			enemy.gameObject.SetActive(true);
			enemy.transform.localScale = Vector3.one;
			enemy.transform.rotation = Quaternion.Euler(Vector3.zero);
			enemy.SetDataStartRun(id, (int)level, gate, UnityEngine.Random.Range(0, Setup.Instance.LineCount), 0f, -1);
			enemy.GetFsmController();
			SetLastEnemyInBattle(lastEnemyInBattle);
			switch (FormatDirector.Instance.gameMode)
			{
				case GameFormat.CampaignMode:
				case GameFormat.DailyTrialMode:
					if (!lastEnemyInBattle && lastEnemyInWave && wave < GameRecord.Instance.TotalWave - 1)
					{
						ShowNextWaveButton(wave);
					}
					break;
				case GameFormat.TournamentMode:
					if (lastEnemyInWave)
					{
						var endless = GameplayDirector.Instance.endlessModeManager;
						if (!endless.IsLastEnemyInNormalWave)
						{
							if (wave + 1 >= GameRecord.Instance.TotalWave)
							{
								wave = endless.waveLoopBegin - 1;
							}
						}
						else if (wave >= endless.waveLoopEnd)
						{
							wave = endless.waveLoopBegin - 1;
						}
						ShowNextWaveButton(wave);
					}
					break;
			}
		}

		// Show the start button for the wave after `wave`.
		private void ShowNextWaveButton(int wave)
		{
			GameplayDirector.Instance.ShowStartWaveButton(wave + 1, EnemyDatabase.Instance.getListEnemyGate(wave + 1));
		}

		private void SetLastEnemyInBattle(bool lastEnemyInBattle)
		{
			switch (FormatDirector.Instance.gameMode)
			{
				case GameFormat.CampaignMode:
				case GameFormat.DailyTrialMode:
					if (lastEnemyInBattle)
					{
						GameRecord.Instance.IsLastEnemyInBattle = true;
					}
					break;
				case GameFormat.TournamentMode:
					GameRecord.Instance.IsLastEnemyInBattle = false;
					break;
			}
			if (lastEnemyInBattle)
			{
				GameplayDirector.Instance.endlessModeManager.SetLastEnemyInNormalWave(lastEnemyInBattle);
			}
		}

		public void SpawnAdditionEnemyAtGate(int id, float fullPos, int gate, float randomPosition)
		{
			EnemyData enemyModel = GetEnemy(id);
			enemyModel.SetDataStartRun(id, (int)CrossSceneData.Instance.BattleDifficulty, gate, UnityEngine.Random.Range(0, Setup.Instance.LineCount), enemyModel.EnemyMovementController.MoveToPosition(fullPos, randomPosition), -1);
			enemyModel.GetFsmController();
		}

		public EnemyData GetEnemy(int id)
		{
			if (id < 0)
			{
				UnityEngine.Debug.LogError("Input ID < 0");
				return null;
			}
			string gameObjectName = string.Format("enemy_{0}(Clone)", id);
			GameObject gameObject = Common.GameObjectPool.Spawn(gameObjectName, default(Vector3), default(Quaternion));
			EnemyData component = gameObject.GetComponent<EnemyData>();
			component.transform.parent = transform;
			GameRecord.Instance.AddEnemyToListActiveEnemy(component);
			return component;
		}

		public void Despawn(EnemyData enemy, float delayTime)
		{
			StartCoroutine(IEDespawnEnemy(enemy, delayTime));
		}

		private IEnumerator IEDespawnEnemy(EnemyData enemy, float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			enemy.transform.DOKill(false);
			enemy.transform.position = Common.GameObjectPool.PoolPosition;
			enemy.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(enemy.gameObject);
		}
	}
}
