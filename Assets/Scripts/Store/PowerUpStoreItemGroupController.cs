using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Store
{
	public class PowerUpStoreItemGroupController : MonoBehaviour
	{
		[SerializeField]
		private List<PowerUpStoreItem> listPowerUpStoreItem = new List<PowerUpStoreItem>();

		public List<PowerUpStoreItem> ListPowerUpStoreItem
		{
			get
			{
				return listPowerUpStoreItem;
			}
			set
			{
				listPowerUpStoreItem = value;
			}
		}

		private void OnEnable()
		{
			UpdateItemsQuantity();
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
			UpdateItemsQuantity();
		}

		private void UpdateItemsQuantity()
		{
			foreach (PowerUpStoreItem powerUpStoreItem in ListPowerUpStoreItem)
			{
				powerUpStoreItem.UpdateItemsQuantity();
			}
		}

		public void InitItemsInformation()
		{
			foreach (PowerUpStoreItem powerUpStoreItem in ListPowerUpStoreItem)
			{
				powerUpStoreItem.InitInfo();
			}
		}
	}
}
