using System;
using UnityEngine;

namespace Gameplay
{
	public class UnitHealthBarPool : MonoSingleton<UnitHealthBarPool>
	{
		private void Awake()
		{
			PushPrototypeToPool();
		}

		private void PushPrototypeToPool()
		{
			GameObject gameObject = Instantiate(unitHealthBarPrefab);
			gameObject.transform.position = transform.position;
			gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(gameObject, 0);
			Common.GameObjectPool.Despawn(gameObject);
		}

		public TrooperVitalityView Get()
		{
			GameObject gameObject = Common.GameObjectPool.Spawn("Health Bar(Clone)", default(Vector3), default(Quaternion));
			TrooperVitalityView component = gameObject.GetComponent<TrooperVitalityView>();
			component.gameObject.SetActive(true);
			component.gameObject.transform.SetParent(transform, false);
			return component;
		}

		public void Despawn(TrooperVitalityView unitHealthView)
		{
			unitHealthView.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(unitHealthView.gameObject);
		}

		[SerializeField]
		private GameObject unitHealthBarPrefab;
	}
}
