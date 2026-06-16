using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class TempestHandler : BaseMonoBehaviour
	{
		private void Update()
		{
			if (!isReady)
			{
				return;
			}
			if (timeTracking == 0f)
			{
				TryToCastSkill();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			if (isReady)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(base.transform.position, aoeRange);
			}
		}

		public void Init(float aoeRange, float activationTime, float pushBackDistance, int maxEnemyAffected)
		{
			this.aoeRange = aoeRange;
			this.activationTime = activationTime;
			this.pushBackDistance = pushBackDistance;
			this.maxEnemyAffected = maxEnemyAffected;
			currentTarget = null;
			base.CustomInvoke(new Action(GetReady), appearAnimDuration);
			base.CustomInvoke(new Action(EndOfLifeTime), activationTime + appearAnimDuration);
		}

		private void TryToCastSkill()
		{
			base.StartCoroutine(PushbackSequence());
			timeTracking = trackingDuration;
		}

		private IEnumerator PushbackSequence()
		{
			int lineIndex = 0;
			Vector3 offSet = Vector3.zero;
			CreepPathAnchor monsterAnchor = new CreepPathAnchor();
			LineDirector.Current.FindNearestLine(base.gameObject.transform.position, out lineIndex, out offSet, out monsterAnchor);
			pushedbackEnemies.Clear();
			collidedFlag.Clear();
			pushCount = 0;
			float pushDistance = pushBackDistance;
			while (pushDistance > 0f)
			{
				float move = stormSpeed * Time.deltaTime;
				pushDistance -= move;
				LineDirector.Current.MoveMonsterAnchor(monsterAnchor, -move);
				base.gameObject.transform.position = monsterAnchor.pos;
				FindCollidedEnemies(pushDistance / stormSpeed, monsterAnchor.pos);
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
			if (pushDistance <= 0f)
			{
				ForceEndOfLifeTime();
			}
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
				if (!collidedFlag.Contains(listActiveEnemy[i].GetEntityId()) && (listActiveEnemy[i].transform.position - dragonPos).sqrMagnitude <= aoeRange && listActiveEnemy[i].curState != EntityPhaseEnum.EnemySpecialState && !listActiveEnemy[i].OriginalParameter.isBoss)
				{
					pushedbackEnemies.Add(listActiveEnemy[i]);
					collidedFlag.Add(listActiveEnemy[i].GetEntityId());
					listActiveEnemy[i].SetSpecialStateDuration(pushDuration);
					listActiveEnemy[i].enemyFsmController.GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[0]);
					pushCount++;
				}
			}
		}

		private void GetReady()
		{
			isReady = true;
		}

		private void ForceEndOfLifeTime()
		{
			base.CustomCancelInvoke(new Action(EndOfLifeTime));
			EndOfLifeTime();
		}

		private void EndOfLifeTime()
		{
			isReady = false;
			isCastingSkill = false;
			currentTarget = null;
			lastTarget = null;
			base.CustomInvoke(new Action(ReturnPool), disAppearAnimDuration);
		}

		private void ReturnPool()
		{
			MonoSingleton<TowerPool>.Instance.Despawn(base.gameObject);
		}

		[SerializeField]
		private float appearAnimDuration;

		[SerializeField]
		private float disAppearAnimDuration;

		private float aoeRange;

		private float activationTime;

		private float pushBackDistance;

		private int maxEnemyAffected;

		private bool isReady;

		private bool isCastingSkill;

		private float timeTracking;

		private float trackingDuration = 1f;

		private EnemyData lastTarget;

		private EnemyData currentTarget;

		[SerializeField]
		private float stormSpeed;

		private List<EnemyData> pushedbackEnemies = new List<EnemyData>();

		private HashSet<int> collidedFlag = new HashSet<int>();

		private int pushCount;
	}
}
