using System;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectCacheExtensions
{
	public static void CreatePool<T>(this T prefab) where T : Component
	{
		ObjectCache.CreatePool<T>(prefab, 0);
	}

	public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
	{
		ObjectCache.CreatePool<T>(prefab, initialPoolSize);
	}

	public static void CreatePool(this GameObject prefab)
	{
		ObjectCache.CreatePool(prefab, 0);
	}

	public static void CreatePool(this GameObject prefab, int initialPoolSize)
	{
		ObjectCache.CreatePool(prefab, initialPoolSize);
	}

	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectCache.Spawn<T>(prefab, parent, position, rotation);
	}

	public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectCache.Spawn<T>(prefab, null, position, rotation);
	}

	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
	{
		return ObjectCache.Spawn<T>(prefab, parent, position, Quaternion.identity);
	}

	public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
	{
		return ObjectCache.Spawn<T>(prefab, null, position, Quaternion.identity);
	}

	public static T Spawn<T>(this T prefab, Transform parent) where T : Component
	{
		return ObjectCache.Spawn<T>(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	public static T Spawn<T>(this T prefab) where T : Component
	{
		return ObjectCache.Spawn<T>(prefab, null, Vector3.zero, Quaternion.identity);
	}

	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		return ObjectCache.Spawn(prefab, parent, position, rotation);
	}

	public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return ObjectCache.Spawn(prefab, null, position, rotation);
	}

	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
	{
		return ObjectCache.Spawn(prefab, parent, position, Quaternion.identity);
	}

	public static GameObject Spawn(this GameObject prefab, Vector3 position)
	{
		return ObjectCache.Spawn(prefab, null, position, Quaternion.identity);
	}

	public static GameObject Spawn(this GameObject prefab, Transform parent)
	{
		return ObjectCache.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	public static GameObject Spawn(this GameObject prefab)
	{
		return ObjectCache.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}

	public static void Recycle<T>(this T obj) where T : Component
	{
		ObjectCache.Recycle<T>(obj);
	}

	public static void Recycle(this GameObject obj)
	{
		ObjectCache.Recycle(obj);
	}

	public static void RecycleAll<T>(this T prefab) where T : Component
	{
		ObjectCache.RecycleAll<T>(prefab);
	}

	public static void RecycleAll(this GameObject prefab)
	{
		ObjectCache.RecycleAll(prefab);
	}

	public static int CountPooled<T>(this T prefab) where T : Component
	{
		return ObjectCache.CountPooled<T>(prefab);
	}

	public static int CountPooled(this GameObject prefab)
	{
		return ObjectCache.CountPooled(prefab);
	}

	public static int CountSpawned<T>(this T prefab) where T : Component
	{
		return ObjectCache.CountSpawned<T>(prefab);
	}

	public static int CountSpawned(this GameObject prefab)
	{
		return ObjectCache.CountSpawned(prefab);
	}

	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return ObjectCache.GetSpawned(prefab, list, appendList);
	}

	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
	{
		return ObjectCache.GetSpawned(prefab, list, false);
	}

	public static List<GameObject> GetSpawned(this GameObject prefab)
	{
		return ObjectCache.GetSpawned(prefab, null, false);
	}

	public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return ObjectCache.GetSpawned<T>(prefab, list, appendList);
	}

	public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
	{
		return ObjectCache.GetSpawned<T>(prefab, list, false);
	}

	public static List<T> GetSpawned<T>(this T prefab) where T : Component
	{
		return ObjectCache.GetSpawned<T>(prefab, null, false);
	}

	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return ObjectCache.GetPooled(prefab, list, appendList);
	}

	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
	{
		return ObjectCache.GetPooled(prefab, list, false);
	}

	public static List<GameObject> GetPooled(this GameObject prefab)
	{
		return ObjectCache.GetPooled(prefab, null, false);
	}

	public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return ObjectCache.GetPooled<T>(prefab, list, appendList);
	}

	public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
	{
		return ObjectCache.GetPooled<T>(prefab, list, false);
	}

	public static List<T> GetPooled<T>(this T prefab) where T : Component
	{
		return ObjectCache.GetPooled<T>(prefab, null, false);
	}

	public static void DestroyPooled(this GameObject prefab)
	{
		ObjectCache.DestroyPooled(prefab);
	}

	public static void DestroyPooled<T>(this T prefab) where T : Component
	{
		ObjectCache.DestroyPooled(prefab.gameObject);
	}

	public static void DestroyAll(this GameObject prefab)
	{
		ObjectCache.DestroyAll(prefab);
	}

	public static void DestroyAll<T>(this T prefab) where T : Component
	{
		ObjectCache.DestroyAll(prefab.gameObject);
	}
}
