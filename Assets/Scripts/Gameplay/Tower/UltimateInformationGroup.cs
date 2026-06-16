using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class UltimateInformationGroup : BaseMonoBehaviour
	{
		public void InitList()
		{
			towerModel = MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.towerModel;
			if (towerModel.transform.position.x > screenLenght / 2f)
			{
				UnityEngine.Debug.Log("tower phai!");
				for (int i = 0; i < 2; i++)
				{
					listUltiInforItem[i].Init(towerModel.Id, towerModel.Level, allignLeft[i], i);
					listUltiInforItem[i].Show();
				}
			}
			else if (towerModel.transform.position.x < -screenLenght / 2f)
			{
				UnityEngine.Debug.Log("tower trai");
				for (int j = 0; j < 2; j++)
				{
					listUltiInforItem[j].Init(towerModel.Id, towerModel.Level, allignRight[j], j);
					listUltiInforItem[j].Show();
				}
			}
			else
			{
				UnityEngine.Debug.Log("tower giua!");
				for (int k = 0; k < 2; k++)
				{
					listUltiInforItem[k].Init(towerModel.Id, towerModel.Level, allignCenter[k], k);
					listUltiInforItem[k].Show();
				}
			}
		}

		public void HideList()
		{
			foreach (UltimateInformationItem ultimateInforItem in listUltiInforItem)
			{
				ultimateInforItem.Hide();
				toggle = false;
			}
		}

		public void TogglePopup()
		{
			toggle = !toggle;
			if (toggle)
			{
				InitList();
			}
			else
			{
				HideList();
			}
		}

		[SerializeField]
		private List<UltimateInformationItem> listUltiInforItem = new List<UltimateInformationItem>();

		[Space]
		[Header("Offset position for items")]
		[SerializeField]
		private List<int> allignCenter = new List<int>();

		[SerializeField]
		private List<int> allignLeft = new List<int>();

		[SerializeField]
		private List<int> allignRight = new List<int>();

		private TurretEntity towerModel;

		private bool toggle;

		private float screenLenght = 6.4f;
	}
}
