using System;
using System.Diagnostics;
using UnityEngine;

namespace Gameplay
{
	public class MinionsDirector : MonoSingleton<MinionsDirector>
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> onChooseAllies;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<TurretEntity, Vector2> onAlliesMoveToAssignedPosition;

		public void UnChooseTower(TurretEntity towerModel)
		{
			if (towerModel)
			{
				towerModel.UnChooseTower();
			}
		}

		public void MoveAlliesToAssignedPosition(TurretEntity towerModel, Vector3 targetPosition)
		{
			if (onAlliesMoveToAssignedPosition != null)
			{
				onAlliesMoveToAssignedPosition(towerModel, targetPosition);
			}
		}
	}
}
