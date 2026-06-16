using System;

namespace Gameplay
{
	public class Turret4Mastery1 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			ReadParameter(ultiLevel);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentUltiLevel)
		{
		}

		private int towerID = 4;

		private int ultimateID = 1;

		private TurretEntity towerModel;
	}
}
