using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
	public class CompositeAssetLoader : IAssetLoader
	{
        private readonly IAssetLoader fallback;

        private readonly IAssetLoader addressables;

        private readonly List<string> addressablesPrefixes = new List<string>();

        public CompositeAssetLoader(IAssetLoader fallback, IAssetLoader addressables)
		{
			this.fallback = fallback;
			this.addressables = addressables;
		}

		public void RegisterAddressablesPrefix(string prefix)
		{
			addressablesPrefixes.Add(prefix);
		}

		public T Load<T>(string key) where T : UnityEngine.Object
		{
			if (UsesAddressables(key))
			{
				return addressables.Load<T>(key);
			}
			return fallback.Load<T>(key);
		}

		public void Release(UnityEngine.Object asset)
		{
			addressables.Release(asset);
			fallback.Release(asset);
		}

		private bool UsesAddressables(string key)
		{
			for (int i = 0; i < addressablesPrefixes.Count; i++)
			{
				if (key.StartsWith(addressablesPrefixes[i], StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}
	}
}
