using System;
using System.Collections.Generic;
using UnityEngine;

namespace RewardPopup
{
	public class PrizeItemClusterHandler : MonoBehaviour
	{
		public void InitListItems(PrizeItem[] listData)
		{
			HideAllItems();
			for (int i = 0; i < listData.Length; i++)
			{
				listItems[i].Init(listData[i]);
			}
		}

		public void HideAllItems()
		{
			foreach (RewardItemView generalItem in listItems)
			{
				generalItem.Hide();
			}
		}

		[SerializeField]
		private List<RewardItemView> listItems = new List<RewardItemView>();
	}
}
