using System;
using GameCore;

namespace Gameplay
{
	public class HeroHandler : BaseMonoBehaviour
	{
        private HeroEntity heroModel;

        public HeroEntity HeroModel
		{
			get
			{
				return heroModel;
			}
			set
			{
				heroModel = value;
			}
		}

		public virtual void Initialize()
		{
		}

		public virtual void OnAppear()
		{
		}

		public virtual void OnDead()
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
