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

		// Turns the ability off (called when its skill item is unequipped). Most abilities only act while
		// unlock is true and gate on it in their TryToCast... methods, so clearing the flag is enough.
		// Abilities that apply a persistent tower buff on unlock must override this to also remove it.
		public virtual void LockUltimate()
		{
			unlock = false;
		}

		// Called once per tower attack (from TurretStrikeHandler via TurretMasteryHandler). Abilities that
		// proc on each shot (e.g. Multi-Shot, Freezing Arrow) override this to roll their TryToCast...;
		// passive abilities (e.g. Assassinate, which only adds a buff on unlock) leave it as a no-op.
		public virtual void OnTowerAttack()
		{
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
