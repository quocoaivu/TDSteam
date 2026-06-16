using System;
using UnityEngine;

namespace Gameplay
{
	public class EnemyAnimationSpeedModifier : EnemyBrain
	{
        private EnemyMovement enemyMovementController;

        private Animator animator;
        public override void Update()
		{
			base.Update();
			CorrectAnimationSpeed();
		}

		public override void Initialize()
		{
			base.Initialize();
			CacheComponents();
		}

		private void CacheComponents()
		{
			enemyMovementController = base.EnemyModel.EnemyMovementController;
			animator = base.GetComponent<Animator>();
		}

		private void CorrectAnimationSpeed()
		{
			if (base.EnemyModel.curState != EntityPhaseEnum.EnemyDie)
			{
				animator.speed = enemyMovementController.Speed / enemyMovementController.OriginSpeed;
				animator.speed = Mathf.Clamp(animator.speed, 0f, 2f);
			}
		}

		public void SetNormalSpeed()
		{
			animator.speed = 1f;
		}
	}
}
