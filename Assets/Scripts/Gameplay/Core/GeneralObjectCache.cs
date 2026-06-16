using System;
using Common;
using UnityEngine;

namespace Gameplay
{
	public class GeneralObjectCache : MonoBehaviour
	{
		public static MultiPrototypesPool<GeneralCacheMember> Current { get; private set; }

		public void Awake()
		{
			pool = new GeneralObjectCache.Pool();
			GeneralObjectCache.Current = pool;
		}

		private GeneralObjectCache.Pool pool;

		public class Pool : MultiPrototypesPool<GeneralCacheMember>
		{
		}
	}
}
