using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class TurretMotionHandler : TurretHandler
	{
		public override void Initialize()
		{
			base.Initialize();
			towerFindEnemyController = base.TowerModel.towerFindEnemyController;
		}

		public void TurnAround()
		{
			List<EnemyData> targets = towerFindEnemyController.Targets;
			if (targets.Count > 0)
			{
				target = targets[0];
			}
			localScale.x = (float)(-(float)GetDirection(target.gameObject));
			foreach (GameObject gameObject in listSprite)
			{
				gameObject.transform.localScale = localScale;
			}
		}

		private int GetDirection(GameObject target)
		{
			float num = target.transform.position.x - base.gameObject.transform.position.x;
			if (num > 0f)
			{
				num = 1f;
			}
			else
			{
				num = -1f;
			}
			return (int)num;
		}

		public void PlayAnimAttack(int index)
		{
			Animator animator = listAnim[index];
			if (animator.HasState(0, AttackHash))
			{
				animator.Play(AttackHash);
			}
		}

		private static readonly int AttackHash = Animator.StringToHash("Attack");

		[SerializeField]
		private List<Animator> listAnim = new List<Animator>();

		[SerializeField]
		private List<GameObject> listSprite = new List<GameObject>();

		private EnemyData target;

		private TurretSeekEnemyHandler towerFindEnemyController;

		private Vector3 localScale = Vector3.one;
	}
}
