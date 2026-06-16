using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Common
{
	// String-keyed GameObject pool, drop-in replacement for the old TrashMan singleton.
	// Backed by UnityEngine.Pool.ObjectPool<GameObject>. One pool per prefab name.
	public class GameObjectPool : MonoBehaviour
	{
		public static GameObjectPool Instance;

		// Off-screen parking position for despawned pooled objects.
		public static readonly Vector3 PoolPosition = new Vector3(1000f, 100f, 0f);

		private readonly Dictionary<string, ObjectPool<GameObject>> _pools = new Dictionary<string, ObjectPool<GameObject>>();
		// Track each pool's prefab so createFunc can clone it.
		private readonly Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}
			Instance = this;
		}

		private void OnDestroy()
		{
			if (Instance == this) Instance = null;
		}

		// Load a prefab from Resources, register a pool keyed by "{prefabName}(Clone)" to match
		// the legacy naming convention. Preallocates `preallocate` instances.
		public static void InitPool(string resourcesPath, int preallocate = 0)
		{
			var prefab = Common.AssetLoader.Load<GameObject>(resourcesPath);
			if (prefab == null)
			{
				Debug.LogError("GameObjectPool.InitPool: prefab not found at " + resourcesPath);
				return;
			}
			ManagePool(prefab, preallocate);
		}

		// Register a pool for an existing prefab (already loaded / instantiated). The key is
		// `prefab.name` — caller is responsible for the "(Clone)" suffix convention.
		public static void ManagePool(GameObject prefab, int preallocate = 0)
		{
			Instance._Manage(prefab, preallocate);
		}

		public static GameObject Spawn(string poolName, Vector3 position = default, Quaternion rotation = default)
		{
			return Instance._Spawn(poolName, position, rotation);
		}

		public static void Despawn(GameObject go)
		{
			if (go == null) return;
			Instance._Despawn(go);
		}

		// --- instance methods ---

		private void _Manage(GameObject prefab, int preallocate)
		{
			// Normalize key sang dạng "{prefabName}(Clone)" — match legacy Spawn() convention.
			// Raw prefab từ Addressables/Resources có name không có "(Clone)";
			// instance đã Instantiate() sẵn (truyền vào từ BulletPool/TowerPool) đã có "(Clone)".
			string key = prefab.name;
			if (!key.EndsWith("(Clone)"))
			{
				key += "(Clone)";
			}
			if (_pools.ContainsKey(key)) return;

			_prefabs[key] = prefab;
			var pool = new ObjectPool<GameObject>(
				createFunc: () =>
				{
					var p = _prefabs[key];
					var go = Instantiate(p);
					go.name = key;
					go.transform.SetParent(transform, false);
					go.SetActive(false);
					return go;
				},
				actionOnGet: go => { /* caller activates */ },
				actionOnRelease: go =>
				{
					go.SetActive(false);
					go.transform.SetParent(transform, false);
				},
				actionOnDestroy: go => Destroy(go),
				collectionCheck: false,
				defaultCapacity: Mathf.Max(1, preallocate),
				maxSize: 10000);
			_pools[key] = pool;

			for (int i = 0; i < preallocate; i++)
			{
				pool.Release(pool.Get());
			}
		}

		private GameObject _Spawn(string key, Vector3 pos, Quaternion rot)
		{
			if (!_pools.TryGetValue(key, out var pool))
			{
				Debug.LogError("GameObjectPool.Spawn: no pool for '" + key + "'");
				return null;
			}
			var go = pool.Get();
			var t = go.transform;
			t.SetParent(null, false);
			t.position = pos;
			t.rotation = rot;
			go.SetActive(true);
			return go;
		}

		private void _Despawn(GameObject go)
		{
			if (_pools.TryGetValue(go.name, out var pool))
			{
				pool.Release(go);
			}
			else
			{
				Destroy(go);
			}
		}
	}
}
