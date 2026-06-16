using System;

namespace Gameplay
{
	// Centralizes the pooled-object naming convention so the "{id}_{level}" strings
	// live in one place instead of scattered string.Format calls across the pools.
	public static class PoolNames
	{
		public static string Tower(int id, int level)
		{
			return string.Format("tower_{0}_{1}", id, level);
		}

		public static string Bullet(int towerId, int towerLevel)
		{
			return string.Format("bullet_{0}_{1}", towerId, towerLevel);
		}

		public static string HeroBullet(int heroId, int bulletIndex)
		{
			return string.Format("hero_{0}_bullet_{1}", heroId, bulletIndex);
		}

		// Pools are keyed by the Unity "(Clone)" instance name.
		public static string Clone(string name)
		{
			return string.Format("{0}(Clone)", name);
		}
	}
}
