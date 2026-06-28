using System;
using System.Collections;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret3Mastery1Ability0 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
			sqDragonRadius = dragonRadius * dragonRadius;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
			TryToCastTeleportEnemiesBack();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void Update()
		{
			if (!MonoSingleton<GameRecord>.Instance.IsGameStart)
			{
				return;
			}
			if (!unlock)
			{
				return;
			}
			if (IsCooldownDone())
			{
				TryToCastTeleportEnemiesBack();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void ReadParameter(int currentSkillLevel)
		{
			maxEnemyAffected = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			cooldownTime = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			distance = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 2) / GameRecord.PIXEL_PER_UNIT;
			skillRange = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameRecord.PIXEL_PER_UNIT;
			unlock = true;
			commonAttackDamage = new SharedStrikeDamage();
			commonAttackDamage.aoeRange = skillRange;
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_SELL_TOWER_ON_ALLY);
		}

		private void TryToCastTeleportEnemiesBack()
		{
			listNearbyEnemies = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			if (listNearbyEnemies.Count > 0)
			{
				float num = float.PositiveInfinity;
				EnemyData enemyModel = null;
				for (int i = listNearbyEnemies.Count - 1; i >= 0; i--)
				{
					if (listNearbyEnemies[i].curState != EntityPhaseEnum.EnemySpecialState && !listNearbyEnemies[i].OriginalParameter.isBoss)
					{
						float sqrMagnitude = (base.transform.position - listNearbyEnemies[i].transform.position).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							enemyModel = listNearbyEnemies[i];
						}
					}
				}
				if (enemyModel != null)
				{
					base.StartCoroutine(PushbackSequence(enemyModel));
				}
				timeTracking = cooldownTime;
				towerModel.towerSoundController.PlayCastSkillSound(skillID);
			}
		}

		private IEnumerator PushbackSequence(EnemyData target)
		{
			CreepPathAnchor monsterAnchor = new CreepPathAnchor(target.monsterPathData.secondAnchor);
			ObjectCache.Spawn(spawnEffect, monsterAnchor.pos);
			yield return new WaitForSeconds(0.1f);
			GameObject dragon = ObjectCache.Spawn(dragonPrefab, monsterAnchor.pos);
			pushedbackEnemies.Clear();
			collidedFlag.Clear();
			pushCount = 0;
			float pushDistance = distance;
			while (pushDistance > 0f)
			{
				float move = dragonSpd * Time.deltaTime;
				pushDistance -= move;
				LineDirector.Current.MoveMonsterAnchor(monsterAnchor, -move);
				dragon.transform.position = monsterAnchor.pos;
				FindCollidedEnemies(pushDistance / dragonSpd, monsterAnchor.pos);
				for (int i = pushedbackEnemies.Count - 1; i >= 0; i--)
				{
					if (GameKit.IsValidEnemy(pushedbackEnemies[i]))
					{
						LineDirector.Current.RequestMove(pushedbackEnemies[i], pushedbackEnemies[i].monsterPathData, -move, false, 0f);
					}
					else
					{
						pushedbackEnemies.RemoveAt(i);
					}
				}
				yield return null;
			}
			ObjectCache.Spawn(spawnEffect, monsterAnchor.pos);
			yield return new WaitForSeconds(0.1f);
			dragon.Recycle();
			yield break;
		}

		private void FindCollidedEnemies(float pushDuration, Vector3 dragonPos)
		{
			if (pushCount >= maxEnemyAffected)
			{
				return;
			}
			List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
			for (int i = listActiveEnemy.Count - 1; i >= 0; i--)
			{
				if (!collidedFlag.Contains(listActiveEnemy[i].GetEntityId()) && (listActiveEnemy[i].transform.position - dragonPos).sqrMagnitude <= sqDragonRadius && listActiveEnemy[i].curState != EntityPhaseEnum.EnemySpecialState && !listActiveEnemy[i].OriginalParameter.isBoss)
				{
					pushedbackEnemies.Add(listActiveEnemy[i]);
					collidedFlag.Add(listActiveEnemy[i].GetEntityId());
					listActiveEnemy[i].SetSpecialStateDuration(pushDuration);
					listActiveEnemy[i].enemyFsmController.GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[0]);
					pushCount++;
				}
			}
		}

		private int towerID = 3;

		private int ultimateBranch = 1;

		private int skillID;

		public float dragonSpd = 0.4f;

		public float dragonRadius = 0.65f;

		public GameObject spawnEffect;

		public GameObject dragonPrefab;

		private float sqDragonRadius;

		private TurretEntity towerModel;

		private int maxEnemyAffected;

		private float distance;

		private float skillRange;

		private float cooldownTime;

		private float timeTracking;

		private List<EnemyData> listNearbyEnemies = new List<EnemyData>();

		private SharedStrikeDamage commonAttackDamage;

		[SerializeField]
		private VisualEffectSpawner effectCaster;

		private List<EnemyData> pushedbackEnemies = new List<EnemyData>();

		private HashSet<int> collidedFlag = new HashSet<int>();

		private int pushCount;
	}
}
