using System;
using Data;
using LifetimePopup;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class PowerUpItemButton : BaseMonoBehaviour
	{
		public void RefreshQuantity()
		{
			if (quantityText == null || PowerUpItemStore.Instance == null)
			{
				return;
			}
			quantity = PowerUpItemStore.Instance.GetCurrentItemQuantity(powerUpItemID);
			quantityText.text = quantity.ToString();
		}

		public void OpenPowerupItemTab()
		{
			base.CustomInvoke(new Action(DoOpen), Time.deltaTime);
		}

		private void DoOpen()
		{
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.InitSelectedTab(1);
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.HighlightButton(1);
		}

		[SerializeField]
		private int powerUpItemID;

		[SerializeField]
		private Text quantityText;

		private int quantity;
	}
}
