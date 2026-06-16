using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class WarlordW3_TurnAndSummonEnemies_P1 : WarlordW3BasePhase
	{
		public WarlordW3_TurnAndSummonEnemies_P1(RuleEnemyWarlordW3 logicEnemy) : base(logicEnemy)
		{
			if (GameKit.IsValidEnemy(enemyModel))
			{
				enemyModel.SetSpecialStateDuration(999999f);
				enemyModel.GetFsmController().GetCurrentState().SetTransition(EntityPhaseEnum.EnemySpecialState);
				logicEnemy.summonRoutineCountdown--;
				logicEnemy.StartCoroutine(TurnAndSummonSequence());
			}
		}

		private IEnumerator TurnAndSummonSequence()
		{
			float castDuration = 0.9f;
			float transformDuration = 0.8f;
			if (UnityEngine.Random.Range(0f, 1f) < 0.7f && HaveTowerBarrackAlly())
			{
				logicEnemy.animator.Play(RuleEnemyWarlordW3.animSpecialAttack);
				yield return new WaitForSeconds(castDuration);
				List<Vector3> spawnPos = new List<Vector3>();
				List<CharacterEntity> listAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
				for (int i = listAlly.Count - 1; i >= 0; i--)
				{
					if (listAlly[i] is MinionEntity)
					{
						CharacterEntity characterModel = listAlly[i];
						Vector3 position = characterModel.transform.position;
						spawnPos.Add(position);
						characterModel.Dead();
						characterModel.ReturnPool(0f);
						ObjectCache.Spawn(logicEnemy.behitTurnAllyPrefab, position);
						ObjectCache.Spawn(logicEnemy.monsterTransformationPrefab, position);
					}
				}
				yield return new WaitForSeconds(transformDuration);
				for (int j = 0; j < spawnPos.Count; j++)
				{
					EnemyData enemy = GameKit.SummonEnemy(21, 0);
					enemy.transform.position = spawnPos[j];
					enemy.monsterPathData = new CreepPathRecord(spawnPos[j], delegate()
					{
						GameSignalCenter.Instance.Trigger(GameSignalKind.OnEnemyMoveToEndPoint, enemy);
					});
				}
				yield return new WaitForSeconds(1f);
			}
			CreepPathAnchor pathAnchor = logicEnemy.GetRandomPosOnRoad(0.45f, 0.65f);
			float chance = UnityEngine.Random.Range(0f, 100f);
			int monsterId;
			int monsterQuantity;
			float disBwMonster;
			if (chance < 33f)
			{
				monsterId = 21;
				monsterQuantity = UnityEngine.Random.Range(4, 8);
				disBwMonster = 0.55f;
			}
			else if (chance < 66f)
			{
				monsterId = 20;
				monsterQuantity = UnityEngine.Random.Range(2, 4);
				disBwMonster = 1.2f;
			}
			else
			{
				monsterId = 18;
				monsterQuantity = UnityEngine.Random.Range(6, 12);
				disBwMonster = 0.4f;
			}
			logicEnemy.animator.Play(RuleEnemyWarlordW3.animSpecialAttack);
			yield return new WaitForSeconds(castDuration);
			for (int k = 0; k < monsterQuantity; k++)
			{
				EnemyData enemy = GameKit.SummonEnemy(monsterId, 0);
				Vector3 pos = pathAnchor.pos;
				ObjectCache.Spawn(logicEnemy.summonEffectPrefab, pos);
				enemy.transform.position = pos;
				enemy.monsterPathData = new CreepPathRecord(pathAnchor, delegate()
				{
					GameSignalCenter.Instance.Trigger(GameSignalKind.OnEnemyMoveToEndPoint, enemy);
				});
				LineDirector.Current.MoveMonsterAnchor(pathAnchor, disBwMonster);
			}
			logicEnemy.curState = new WarlordW3_TravelCloseToGoal_P2(logicEnemy);
			yield break;
		}

		private bool HaveTowerBarrackAlly()
		{
			List<CharacterEntity> listActiveAlly = MonoSingleton<GameRecord>.Instance.ListActiveAlly;
			for (int i = 0; i < listActiveAlly.Count; i++)
			{
				if (listActiveAlly[i] is MinionEntity)
				{
					return true;
				}
			}
			return false;
		}
	}
}
