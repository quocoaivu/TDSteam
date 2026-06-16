using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class WarlordW3_Summon_P0 : WarlordW3BasePhase
	{
		public WarlordW3_Summon_P0(RuleEnemyWarlordW3 logicEnemy) : base(logicEnemy)
		{
			enemyModel.SetSpecialStateDuration(999999f);
			enemyModel.GetFsmController().GetCurrentState().SetTransition(EntityPhaseEnum.EnemySpecialState);
			logicEnemy.StartCoroutine(SpawnSequence());
		}

		private IEnumerator SpawnSequence()
		{
			WarlordW3StatueDirector.instance.BreakIce();
			yield return new WaitForSeconds(1.5f);
			WarlordW3StatueDirector.instance.iceAnimator.gameObject.SetActive(false);
			WarlordW3StatueDirector.instance.bossStatue.SetActive(false);
			enemyModel.transform.position = WarlordW3StatueDirector.instance.bossStatue.transform.position;
			yield return new WaitForSeconds(2f);
			MonoSingleton<LensHandler>.Instance.PinchZoomFov.MoveAndZoomToPosition(enemyModel.transform.position, 2f, 3.9f);
			logicEnemy.curState = new WarlordW3_Teleport_P3(logicEnemy, 0.55f, 0.7f);
			yield break;
		}
	}
}
