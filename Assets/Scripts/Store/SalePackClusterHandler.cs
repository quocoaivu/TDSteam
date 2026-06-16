using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Store
{
	public class SalePackClusterHandler : MonoBehaviour
	{
		[SerializeField]
		private List<SalePackItem> listSpecialItem = new List<SalePackItem>();

		[SerializeField]
		private List<SalePackItem> listSaleBundleItem = new List<SalePackItem>();

		public List<SalePackItem> boosters = new List<SalePackItem>();

		public List<SalePackItem> ListSpecialItem
		{
			get
			{
				return listSpecialItem;
			}
			set
			{
				listSpecialItem = value;
			}
		}

		public List<SalePackItem> ListSaleBundleItem
		{
			get
			{
				return listSaleBundleItem;
			}
			set
			{
				listSaleBundleItem = value;
			}
		}

		public void InitItemsInformation()
		{
			foreach (SalePackItem saleBundleItem in listSaleBundleItem)
			{
				saleBundleItem.Init();
			}
			InitSpecialItem();
			for (int i = 0; i < boosters.Count; i++)
			{
				boosters[i].Init();
			}
		}

		private void InitSpecialItem()
		{
			int currentAvailableSpecialPackIndex = SaleBundleStore.Instance.GetCurrentAvailableSpecialPackIndex();
			for (int i = 0; i < listSpecialItem.Count; i++)
			{
				if (i == currentAvailableSpecialPackIndex)
				{
					listSpecialItem[i].Init();
				}
				else
				{
					listSpecialItem[i].Hide();
				}
			}
		}

		public void RefreshItemStatus()
		{
			foreach (SalePackItem saleBundleItem in listSaleBundleItem)
			{
				saleBundleItem.RefreshStatus();
			}
			InitSpecialItem();
			foreach (SalePackItem saleBundleItem2 in listSpecialItem)
			{
				saleBundleItem2.RefreshStatus();
			}
		}
	}
}
