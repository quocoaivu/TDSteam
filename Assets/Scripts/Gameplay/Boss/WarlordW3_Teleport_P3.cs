using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class WarlordW3_Teleport_P3 : WarlordW3BasePhase
	{
        private float minLineProp;
        private float maxLineProp;

        public WarlordW3_Teleport_P3(RuleEnemyWarlordW3 logicEnemy, float minLineProp, float maxLineProp) : base(logicEnemy)
		{
			if (GameKit.IsValidEnemy(enemyModel))
			{
				enemyModel.SetSpecialStateDuration(999999f);
				enemyModel.GetFsmController().GetCurrentState().SetTransition(EntityPhaseEnum.EnemySpecialState);
				this.minLineProp = minLineProp;
				this.maxLineProp = maxLineProp;
				logicEnemy.StartCoroutine(TeleportSequence());
			}
		}

		private IEnumerator TeleportSequence()
		{
			float transformDur = 1.15f;
			logicEnemy.animator.Play(RuleEnemyWarlordW3.animTurnToBird);
			yield return new WaitForSeconds(transformDur);
			CreepPathAnchor teleRoadAnchor = logicEnemy.GetRandomPosOnRoad(minLineProp, maxLineProp);
			Vector3 telePos = teleRoadAnchor.pos;
			float dis = (enemyModel.transform.position - telePos).magnitude;
			float moveTime = dis / logicEnemy.teleSpeed;
			logicEnemy.animator.Play(RuleEnemyWarlordW3.animBirdRunRight);
			enemyModel.transform.localScale = new Vector3((float)((telePos.x <= enemyModel.transform.position.x) ? -1 : 1), 1f, 1f);
			enemyModel.transform.DOMove(telePos, moveTime, false);
			yield return new WaitForSeconds(moveTime + 0.1f);
			logicEnemy.animator.Play(RuleEnemyWarlordW3.animTurnToBoss);
			enemyModel.monsterPathData = new CreepPathRecord(teleRoadAnchor, delegate()
			{
				UnityEngine.Debug.LogError("22222222222 ve dich roi!!!");
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnEnemyMoveToEndPoint, enemyModel);
			});
			yield return new WaitForSeconds(transformDur);
			if (logicEnemy.summonRoutineCountdown > 0)
			{
				logicEnemy.curState = new WarlordW3_TurnAndSummonEnemies_P1(logicEnemy);
			}
			else
			{
				logicEnemy.curState = new WarlordW3_TravelToGoal_P4(logicEnemy);
			}
			yield break;
		}
	}
}
