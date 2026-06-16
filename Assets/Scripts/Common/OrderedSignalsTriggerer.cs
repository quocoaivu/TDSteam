using System;
using Common;
using UnityEngine;

namespace Common
{
	public class OrderedSignalsTriggerer : MonoBehaviour
	{

        [SerializeField]

        private OrderedUnityEvent events = new OrderedUnityEvent();

        [ContextMenu("Trigger")]
		public void Trigger()
		{
			events.Dispatch();
		}
	}
}
