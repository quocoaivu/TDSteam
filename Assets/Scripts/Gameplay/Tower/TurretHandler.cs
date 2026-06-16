using System;
using GameCore;

namespace Gameplay
{
	public abstract class TurretHandler : BaseMonoBehaviour
	{
		public TurretEntity TowerModel
		{
			get
			{
				return towerModel;
			}
			set
			{
				towerModel = value;
			}
		}

		public virtual void Initialize()
		{
		}

		public virtual void OnAppear()
		{
		}

		public virtual void OnBuildFinished()
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

		private TurretEntity towerModel;
	}
}
