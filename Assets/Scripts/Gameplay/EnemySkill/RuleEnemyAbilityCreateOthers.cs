using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class RuleEnemyAbilityCreateOthers : EnemyBrain
	{
		public override void OnAppear()
		{
			base.OnAppear();
			currentGate = base.EnemyModel.Gate;
		}

		private void Awake()
		{
			enemyMovementController = base.EnemyModel.EnemyMovementController;
			MonoSingleton<EnemyPool>.Instance.InitAdditionEnemy(enemyID);
		}

		public void CreateOtherEnemy(float delayTime)
		{
			if (dieEffect != null)
			{
				ObjectCache.Spawn(dieEffect, base.EnemyModel.transform.position);
			}
			base.CustomInvoke(new Action(DoCreate), delayTime);
		}

		private void DoCreate()
		{
			EnemyData otherEnemy = MonoSingleton<EnemyPool>.Instance.GetEnemy(enemyID);
			otherEnemy.SetDataStartRun(enemyID, (int)CrossSceneData.Instance.BattleDifficulty, currentGate, enemyMovementController.currentLine, 0f, 0);
			otherEnemy.monsterPathData = new CreepPathRecord(LineDirector.Current.GetLineIndex(base.EnemyModel.Gate, base.EnemyModel.moveLine), base.EnemyModel.transform.position, delegate()
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnEnemyMoveToEndPoint, otherEnemy);
			});
			otherEnemy.GetFsmController();
		}

		public void OnDisable()
		{
		}

		[SerializeField]
		private int enemyID;

		public GameObject dieEffect;

		private EnemyMovement enemyMovementController;

		private int currentGate = -1;

		private Vector2 poolPos = new Vector2(100f, 100f);
	}
}
