using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace MapLevel
{
	public class PowerUpItemClusterHandler : MonoBehaviour
	{
		private void OnEnable()
		{
			if (PowerUpItemStore.Instance != null)
			{
				PowerUpItemStore.Instance.OnItemQuantityChangeEvent += Instance_OnItemQuantityChangeEvent;
			}
		}

		private void OnDisable()
		{
			if (PowerUpItemStore.Instance != null)
			{
				PowerUpItemStore.Instance.OnItemQuantityChangeEvent -= Instance_OnItemQuantityChangeEvent;
			}
		}

		private void Instance_OnItemQuantityChangeEvent()
		{
			RefreshPowerupItems();
		}

		public void RefreshPowerupItems()
		{
			foreach (PowerUpItemButton powerUpItem in listPowerUpItem)
			{
				powerUpItem.RefreshQuantity();
			}
		}

		[SerializeField]
		private List<PowerUpItemButton> listPowerUpItem = new List<PowerUpItemButton>();
	}
}
