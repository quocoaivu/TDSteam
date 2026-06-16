using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
	[Serializable]
	public class OrderedUnityEvent
	{
        [SerializeField]
        private List<UnityEvent> listeners = new List<UnityEvent>();

        private List<UnityEvent> Listeners
		{
			get
			{
				return listeners;
			}
			set
			{
				listeners = value;
			}
		}

		public int UnityEventsCount
		{
			get
			{
				return listeners.Count;
			}
		}

		public int FinalListenersCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < listeners.Count; i++)
				{
					num += listeners[i].GetPersistentEventCount();
				}
				return num;
			}
		}

		public void Dispatch()
		{
			for (int i = 0; i < Listeners.Count; i++)
			{
				UnityEvent unityEvent = Listeners[i];
				if (unityEvent != null)
				{
					unityEvent.Invoke();
				}
			}
		}
	}
}
