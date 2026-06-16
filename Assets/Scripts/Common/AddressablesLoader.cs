// NOTE: File nÃ y yÃªu cáº§u package `com.unity.addressables` Ä‘Ã£ Ä‘Æ°á»£c install.
// Náº¿u Unity bÃ¡o lá»—i compile (cannot find Addressables namespace), nghÄ©a lÃ 
// package chÆ°a import xong. Äá»£i Unity Package Manager táº£i xong rá»“i recompile.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Common
{
	public class AddressablesLoader : IAssetLoader
	{
        private readonly Dictionary<Object, AsyncOperationHandle> handles = new Dictionary<Object, AsyncOperationHandle>();
        
		public T Load<T>(string key) where T : Object
		{
			// Normalize vá» lowercase Ä‘á»ƒ khá»›p behavior case-insensitive cá»§a Resources (Windows filesystem).
			string normalizedKey = key.ToLowerInvariant();

			// Resources.Load tráº£ null cho asset thiáº¿u, cÃ²n Addressables.LoadAssetAsync vá»«a nÃ©m vá»«a
			// Tá»° LOG InvalidKeyException qua ResourceManager (try/catch cá»§a caller khÃ´ng cháº·n Ä‘Æ°á»£c log Ä‘Ã³).
			// Kiá»ƒm tra location trÆ°á»›c (LoadResourceLocationsAsync tráº£ list rá»—ng cho key thiáº¿u, khÃ´ng
			// nÃ©m/khÃ´ng log) Ä‘á»ƒ giá»¯ há»£p Ä‘á»“ng null vÃ  trÃ¡nh lá»—i Ä‘á» trong console.
			if (!HasLocation(normalizedKey))
			{
				Debug.LogWarning("AddressablesLoader: khÃ´ng tÃ¬m tháº¥y asset cho key '" + normalizedKey + "', tráº£ null.");
				return null;
			}

			// Addressables register prefab dÆ°á»›i type GameObject. Khi caller request Component
			// (vd MinionEntity, HeroEntity, VisualEffectInstance), pháº£i load GameObject rá»“i GetComponent.
			// Resources.Load<T>() tá»± lÃ m viá»‡c nÃ y, Addressables khÃ´ng. Bridge á»Ÿ Ä‘Ã¢y.
			if (typeof(Component).IsAssignableFrom(typeof(T)))
			{
				AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>(normalizedKey);
				GameObject prefab = goHandle.WaitForCompletion();
				if (prefab == null)
				{
					return null;
				}
				T component = prefab.GetComponent<T>();
				if (component != null)
				{
					handles[component] = goHandle;
				}
				return component;
			}

			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(normalizedKey);
			T asset = handle.WaitForCompletion();
			if (asset != null)
			{
				handles[asset] = handle;
			}
			return asset;
		}

		// Tráº£ true náº¿u key cÃ³ Ã­t nháº¥t má»™t location Ä‘Ã£ Ä‘Äƒng kÃ½. DÃ¹ng Ä‘á»ƒ trÃ¡nh gá»i LoadAssetAsync
		// trÃªn key khÃ´ng tá»“n táº¡i (vá»‘n sáº½ nÃ©m VÃ€ tá»± log InvalidKeyException).
		private bool HasLocation(string key)
		{
			AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(key);
			IList<IResourceLocation> locations = locationsHandle.WaitForCompletion();
			bool exists = locations != null && locations.Count > 0;
			Addressables.Release(locationsHandle);
			return exists;
		}

		public void Release(Object asset)
		{
			if (asset == null)
			{
				return;
			}
			if (handles.TryGetValue(asset, out AsyncOperationHandle handle))
			{
				Addressables.Release(handle);
				handles.Remove(asset);
			}
		}
	}
}
