using System;
using GameCore;

namespace Gameplay
{
	public class TurretMasteryShared : BaseMonoBehaviour
	{
		public virtual void UnlockUltimate(int ultiLevel)
		{
			MonoSingleton<TurretControlSfxHandler>.Instance.PlayUpgradeUltimate();
		}

		public virtual void InitTowerModel(TurretEntity towerModel)
		{
			unlock = false;
		}

		public virtual void OnReturnPool()
		{
			unlock = false;
		}

		public bool unlock;
	}
}
