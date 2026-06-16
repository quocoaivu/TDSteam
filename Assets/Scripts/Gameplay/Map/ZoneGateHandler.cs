using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class ZoneGateHandler : BaseMonoBehaviour
	{
		public void Awake()
		{
			Init();
		}

		public void Init()
		{
			if (isInited)
			{
				return;
			}
			isInited = true;
			gates = new List<GameObject>();
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					gates.Add(transform.gameObject);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		[SerializeField]
		private bool gateForRunningEnemies;

		[SerializeField]
		private bool gateForFlyingEnemies;

		[NonSerialized]
		public List<GameObject> gates;

		private bool isInited;
	}
}
