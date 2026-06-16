using System;
using GameCore;

namespace Gameplay
{
	public abstract class MinionHandler : BaseMonoBehaviour
	{

        private MinionEntity allyModel;

        public MinionEntity MinionEntity
		{
			get
			{
				return allyModel;
			}
			set
			{
				allyModel = value;
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
	}
}
