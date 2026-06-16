using UnityEngine;

namespace Common
{
	public static class AssetLoader
	{
        private static IAssetLoader backend;

        public static IAssetLoader Backend
		{
			get
			{
				if (backend == null)
				{
					backend = new ResourcesLoader();
				}
				return backend;
			}
			set
			{
				backend = value;
			}
		}

		public static T Load<T>(string key) where T : Object
		{
			return Backend.Load<T>(key);
		}

		public static void Release(Object asset)
		{
			Backend.Release(asset);
		}
	}
}
