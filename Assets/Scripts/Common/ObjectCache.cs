using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectCache : MonoBehaviour
{
    private static ObjectCache _instance;

    [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _instance = null;
    }
    private static List<GameObject> tempList = new List<GameObject>();

    private Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();

    private Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();

    public ObjectCache.StartupPoolMode startupPoolMode;

    public ObjectCache.StartupPool[] startupPools;

    private bool startupPoolsCreated;

    public enum StartupPoolMode
    {
        Awake,
        Start,
        CallManually
    }

    [Serializable]
    public class StartupPool
    {
        public int size;

        public GameObject prefab;
    }

    private void Awake()
	{
		ObjectCache._instance = this;
		if (startupPoolMode == ObjectCache.StartupPoolMode.Awake)
		{
			ObjectCache.CreateStartupPools();
		}
	}

	private void Start()
	{
		if (startupPoolMode == ObjectCache.StartupPoolMode.Start)
		{
			ObjectCache.CreateStartupPools();
		}
	}

	public static void CreateStartupPools()
	{
		if (!ObjectCache.instance.startupPoolsCreated)
		{
			ObjectCache.instance.startupPoolsCreated = true;
			ObjectCache.StartupPool[] array = ObjectCache.instance.startupPools;
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					ObjectCache.CreatePool(array[i].prefab, array[i].size);
				}
			}
		}
	}

	public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
	{
		ObjectCache.CreatePool(prefab.gameObject, initialPoolSize);
	}

	public static void CreatePool(GameObject prefab, int initialPoolSize)
	{
		if (prefab != null && !ObjectCache.instance.pooledObjects.ContainsKey(prefab))
		{
			List<GameObject> list = new List<GameObject>();
			ObjectCache.instance.pooledObjects.Add(prefab, list);
			if (initialPoolSize > 0)
			{
				bool activeSelf = prefab.activeSelf;
				prefab.SetActive(false);
				Transform transform = ObjectCache.instance.transform;
				while (list.Count < initialPoolSize)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
					gameObject.transform.SetParent(transform, false);
					list.Add(gameObject);
				}
				prefab.SetActive(activeSelf);
			}
		}
	}

	public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectCache.Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectCache.Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
	{
		return ObjectCache.Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab, Vector3 position) where T : Component
	{
		return ObjectCache.Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab, Transform parent) where T : Component
	{
		return ObjectCache.Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab) where T : Component
	{
		return ObjectCache.Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}

	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		if (prefab == null)
		{
			return null;
		}
		List<GameObject> list;
		if (ObjectCache.instance.pooledObjects.TryGetValue(prefab, out list))
		{
			GameObject gameObject = null;
			Transform transform;
			if (list.Count > 0)
			{
				while (gameObject == null && list.Count > 0)
				{
					gameObject = list[0];
					list.RemoveAt(0);
				}
				if (gameObject != null)
				{
					transform = gameObject.transform;
					transform.SetParent(parent, false);
					transform.localPosition = position;
					transform.localRotation = rotation;
					transform.localScale = prefab.transform.localScale;
					gameObject.SetActive(true);
					ObjectCache.instance.spawnedObjects.Add(gameObject, prefab);
					return gameObject;
				}
			}
			gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			transform = gameObject.transform;
			transform.SetParent(parent, false);
			transform.localPosition = position;
			transform.localRotation = rotation;
			transform.localScale = prefab.transform.localScale;
			ObjectCache.instance.spawnedObjects.Add(gameObject, prefab);
			return gameObject;
		}
		ObjectCache.CreatePool(prefab, 1);
		return ObjectCache.Spawn(prefab, parent, position, rotation);
	}

	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
	{
		return ObjectCache.Spawn(prefab, parent, position, Quaternion.identity);
	}

	public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return ObjectCache.Spawn(prefab, null, position, rotation);
	}

	public static GameObject Spawn(GameObject prefab, Transform parent)
	{
		return ObjectCache.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	public static GameObject Spawn(GameObject prefab, Vector3 position)
	{
		return ObjectCache.Spawn(prefab, null, position, Quaternion.identity);
	}

	public static GameObject Spawn(GameObject prefab)
	{
		return ObjectCache.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}

	public static void Recycle<T>(T obj) where T : Component
	{
		ObjectCache.Recycle(obj.gameObject);
	}

	public static void Recycle(GameObject obj)
	{
		GameObject prefab;
		if (ObjectCache.instance.spawnedObjects.TryGetValue(obj, out prefab))
		{
			ObjectCache.Recycle(obj, prefab);
		}
		else
		{
			UnityEngine.Object.Destroy(obj);
		}
	}

	private static void Recycle(GameObject obj, GameObject prefab)
	{
		ObjectCache.instance.pooledObjects[prefab].Add(obj);
		ObjectCache.instance.spawnedObjects.Remove(obj);
		obj.transform.SetParent(ObjectCache.instance.transform, false);
		obj.SetActive(false);
	}

	public static void RecycleAll<T>(T prefab) where T : Component
	{
		ObjectCache.RecycleAll(prefab.gameObject);
	}

	public static void RecycleAll(GameObject prefab)
	{
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in ObjectCache.instance.spawnedObjects)
		{
			if (keyValuePair.Value == prefab)
			{
				ObjectCache.tempList.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < ObjectCache.tempList.Count; i++)
		{
			ObjectCache.Recycle(ObjectCache.tempList[i]);
		}
		ObjectCache.tempList.Clear();
	}

	public static void RecycleAll()
	{
		ObjectCache.tempList.AddRange(ObjectCache.instance.spawnedObjects.Keys);
		for (int i = 0; i < ObjectCache.tempList.Count; i++)
		{
			ObjectCache.Recycle(ObjectCache.tempList[i]);
		}
		ObjectCache.tempList.Clear();
	}

	public static bool IsSpawned(GameObject obj)
	{
		return ObjectCache.instance.spawnedObjects.ContainsKey(obj);
	}

	public static int CountPooled<T>(T prefab) where T : Component
	{
		return ObjectCache.CountPooled(prefab.gameObject);
	}

	public static int CountPooled(GameObject prefab)
	{
		List<GameObject> list;
		if (ObjectCache.instance.pooledObjects.TryGetValue(prefab, out list))
		{
			return list.Count;
		}
		return 0;
	}

	public static int CountSpawned<T>(T prefab) where T : Component
	{
		return ObjectCache.CountSpawned(prefab.gameObject);
	}

	public static int CountSpawned(GameObject prefab)
	{
		int num = 0;
		foreach (GameObject y in ObjectCache.instance.spawnedObjects.Values)
		{
			if (prefab == y)
			{
				num++;
			}
		}
		return num;
	}

	public static int CountAllPooled()
	{
		int num = 0;
		foreach (List<GameObject> list in ObjectCache.instance.pooledObjects.Values)
		{
			num += list.Count;
		}
		return num;
	}

	public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		List<GameObject> collection;
		if (ObjectCache.instance.pooledObjects.TryGetValue(prefab, out collection))
		{
			list.AddRange(collection);
		}
		return list;
	}

	public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		List<GameObject> list2;
		if (ObjectCache.instance.pooledObjects.TryGetValue(prefab.gameObject, out list2))
		{
			for (int i = 0; i < list2.Count; i++)
			{
				list.Add(list2[i].GetComponent<T>());
			}
		}
		return list;
	}

	public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in ObjectCache.instance.spawnedObjects)
		{
			if (keyValuePair.Value == prefab)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		GameObject gameObject = prefab.gameObject;
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in ObjectCache.instance.spawnedObjects)
		{
			if (keyValuePair.Value == gameObject)
			{
				list.Add(keyValuePair.Key.GetComponent<T>());
			}
		}
		return list;
	}

	public static void DestroyPooled(GameObject prefab)
	{
		List<GameObject> list;
		if (ObjectCache.instance.pooledObjects.TryGetValue(prefab, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				UnityEngine.Object.Destroy(list[i]);
			}
			list.Clear();
		}
	}

	public static void DestroyPooled<T>(T prefab) where T : Component
	{
		ObjectCache.DestroyPooled(prefab.gameObject);
	}

	public static void DestroyAll(GameObject prefab)
	{
		ObjectCache.RecycleAll(prefab);
		ObjectCache.DestroyPooled(prefab);
	}

	public static void DestroyAll<T>(T prefab) where T : Component
	{
		ObjectCache.DestroyAll(prefab.gameObject);
	}

	public static ObjectCache instance
	{
		get
		{
			if (ObjectCache._instance != null)
			{
				return ObjectCache._instance;
			}
			ObjectCache._instance = UnityEngine.Object.FindAnyObjectByType<ObjectCache>();
			if (ObjectCache._instance != null)
			{
				return ObjectCache._instance;
			}
			ObjectCache._instance = new GameObject("ObjectCache")
			{
				transform = 
				{
					localPosition = Vector3.zero,
					localRotation = Quaternion.identity,
					localScale = Vector3.one
				}
			}.AddComponent<ObjectCache>();
			return ObjectCache._instance;
		}
	}
}
