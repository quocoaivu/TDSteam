using UnityEngine;

namespace Gameplay
{
	// Pool for FloatingDamageText. Mirrors UnitHealthBarPool so damage numbers are
	// never Instantiated/Destroyed per hit. Place one in the Gameplay scene and
	// assign the prefab; if absent, calls are a safe no-op.
	public class FloatingDamageTextPool : MonoSingleton<FloatingDamageTextPool>
	{
		[SerializeField]
		private GameObject floatingTextPrefab;

		private string poolName;

		private bool poolReady;

		// Register lazily on first use, not in Awake: GameObjectPool.Instance may not be set
		// yet during scene-load Awake ordering (it NREs). By the first hit, gameplay is running
		// and the pool manager exists.
		private void EnsurePool()
		{
			if (poolReady)
			{
				return;
			}
			poolName = floatingTextPrefab.name + "(Clone)";
			PushPrototypeToPool();
			poolReady = true;
		}

		private void PushPrototypeToPool()
		{
			GameObject gameObject = Instantiate(floatingTextPrefab);
			gameObject.transform.position = transform.position;
			gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(gameObject, 0);
			Common.GameObjectPool.Despawn(gameObject);
		}

		public void Show(int amount, Vector3 worldPosition)
		{
			if (floatingTextPrefab == null)
			{
				return;
			}
			EnsurePool();
			GameObject gameObject = Common.GameObjectPool.Spawn(poolName, default(Vector3), default(Quaternion));
			FloatingDamageText component = gameObject.GetComponent<FloatingDamageText>();
			component.gameObject.SetActive(true);
			// Parent under this pool so spawned text is grouped in the hierarchy (Gameplay/Spawners/
			// FloatingDamageTextPool). Safe because the pool is a plain world-space object (scale 1,
			// not under a Canvas); Setup() still drives world position. worldPositionStays=true.
			gameObject.transform.SetParent(transform, true);
			component.Setup(amount, worldPosition);
		}

		public void Despawn(FloatingDamageText floatingText)
		{
			floatingText.OnReturnPool();
			floatingText.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(floatingText.gameObject);
		}
	}
}
