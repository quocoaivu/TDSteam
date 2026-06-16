using System;
using GeneralVariable;
using UnityEngine;

namespace Gameplay
{
	public class ZoneDestinationHandler : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag == GeneralVariable.GeneralDefine.ENEMY_TAG)
			{
				UnityEngine.Debug.Log("Enemy Reach Gate!");
				MonoSingleton<ZoneHandler>.Instance.DispatchEventEnemyReachGate(gateID);
			}
		}

		[SerializeField]
		private int gateID;
	}
}
