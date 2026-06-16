using System;
using System.Diagnostics;
using UnityEngine;

namespace Gameplay
{
	public class ZoneHandler : MonoSingleton<ZoneHandler>
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Vector2> OnEnemyReachGate;

		public void DispatchEventEnemyReachGate(int gateID)
		{
			if (OnEnemyReachGate != null)
			{
				OnEnemyReachGate(listMapDestination[gateID].transform.position);
			}
		}

		[SerializeField]
		private ZoneDestinationHandler[] listMapDestination;
	}
}
