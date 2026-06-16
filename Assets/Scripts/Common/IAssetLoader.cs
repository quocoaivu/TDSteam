using UnityEngine;

namespace Common
{
	public interface IAssetLoader
	{
		T Load<T>(string key) where T : Object;

		void Release(Object asset);
	}
}
