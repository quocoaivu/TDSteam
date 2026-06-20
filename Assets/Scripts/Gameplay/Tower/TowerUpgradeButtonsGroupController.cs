using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class TowerUpgradeButtonsGroupController : DialogSingleton
	{
		private void Awake()
		{
			MonoSingleton<GameRecord>.Instance.OnMoneyChange += Instance_OnMoneyChange;
		}

		private void Instance_OnMoneyChange()
		{
			base.CustomInvoke(new Action(RefreshStatusAll), 0.2f);
		}

		private void OnDestroy()
		{
			GameRecord gd = MonoSingleton<GameRecord>.InstanceIfExists;
			if (gd != null)
			{
				gd.OnMoneyChange -= Instance_OnMoneyChange;
			}
		}

		public void DisableConfirmOtherButtons(ControlTowerButtonController NoDisableButton)
		{
			foreach (ControlTowerButtonController controllTowerButtonController in listControllButton)
			{
				// Skip null entries: the retired upgrade buttons may have been deleted with the tower popup's
				// ContentHolder, leaving holes in this serialized list.
				if (controllTowerButtonController != null && controllTowerButtonController != NoDisableButton)
				{
					controllTowerButtonController.DisableConfirm();
				}
			}
		}

		public void DisableConfirmAllButton()
		{
			foreach (ControlTowerButtonController controllTowerButtonController in listControllButton)
			{
				if (controllTowerButtonController != null)
				{
					controllTowerButtonController.DisableConfirm();
				}
			}
		}

		protected override void OnClickOutsideUp()
		{
			base.OnClickOutsideUp();
			DisableConfirmAllButton();
		}

		public void RefreshStatusAll()
		{
			foreach (ControlTowerButtonController controllTowerButtonController in listControllButton)
			{
				if (controllTowerButtonController != null)
				{
					controllTowerButtonController.UpdateBuyState();
				}
			}
		}

		[SerializeField]
		private List<ControlTowerButtonController> listControllButton = new List<ControlTowerButtonController>();
	}
}
