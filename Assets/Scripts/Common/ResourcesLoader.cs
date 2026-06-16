using UnityEngine;

namespace Common
{
	public class ResourcesLoader : IAssetLoader
	{
		public T Load<T>(string key) where T : Object
		{
			T asset = Resources.Load<T>(key);
			if (asset == null)
			{
				// Sau khi migrate sang Addressables, không asset nào còn nằm trong Resources/ (trừ TMP).
				// Nếu hit log này, có thể là: (1) thiếu prefix register trong AssetLoaderBootstrap,
				// hoặc (2) code dùng path không khớp prefix nào.
				Debug.LogWarning(
					"[ResourcesLoader] Asset không tìm thấy: '" + key + "' (Type: " + typeof(T).Name +
					"). Path này không match prefix Addressables nào và cũng không còn trong Resources/. " +
					"Kiểm tra: đã migrate folder chứa asset này chưa? Đã register prefix trong AssetLoaderBootstrap chưa?");
			}
			return asset;
		}

		public void Release(Object asset)
		{
		}
	}
}
