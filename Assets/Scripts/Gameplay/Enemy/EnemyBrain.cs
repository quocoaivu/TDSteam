using System;
using GameCore;

namespace Gameplay
{
	public abstract class EnemyBrain : BaseMonoBehaviour
	{
		private EnemyData enemyModel;

        public EnemyData EnemyModel
		{
			get
			{
				return enemyModel;
			}
			set
			{
				enemyModel = value;
			}
		}

		public virtual void Initialize()
		{
		}

		public virtual void OnAppear()
		{
		}

		public virtual void OnReturnPool()
		{
		}

		public virtual void Update()
		{
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
		}

		public bool IsCurrentSpeedGreaterThanMinSpeed()
		{
			return EnemyModel.EnemyMovementController.Speed > 0.05f;
		}

		public bool IsEnemyAlive()
		{
			return EnemyModel.IsAlive;
		}
	}
}
