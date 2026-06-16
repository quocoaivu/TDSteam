using System;
using System.Collections.Generic;
using Data;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class TowerBuyButtonsGroupController : DialogSingleton
	{
		private void Awake()
		{
			MonoSingleton<GameRecord>.Instance.OnMoneyChange += Instance_OnMoneyChange;
		}

		public void InitButtonsStatusByWave()
		{
			for (int i = 0; i < listControllButton.Count; i++)
			{
				bool isAllowedToUse = false;
				GameFormat gameMode = FormatDirector.Instance.gameMode;
				if (gameMode != GameFormat.CampaignMode)
				{
					if (gameMode != GameFormat.DailyTrialMode)
					{
						if (gameMode == GameFormat.TournamentMode)
						{
							string currentSeasonID = ZoneRuleSpec.Instance.GetCurrentSeasonID();
							isAllowedToUse = ZoneRuleSpec.Instance.IsTowerAllowed_Tournament(currentSeasonID, i);
						}
					}
					else
					{
						int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
						int currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
						isAllowedToUse = ZoneRuleSpec.Instance.IsTowerAllowed_Daily(currentWave, i);
					}
				}
				else
				{
					isAllowedToUse = ZoneRuleSpec.Instance.IsTowerAllowed_Campaign(MonoSingleton<GameRecord>.Instance.MapID, i);
				}
				listControllButton[i].Init(isAllowedToUse, buttonImage[i], lockImage);
			}
			RefreshStatusAll();
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
				if (!controllTowerButtonController.Equals(NoDisableButton))
				{
					controllTowerButtonController.DisableConfirm();
				}
			}
		}

		public void DisableConfirmAllButton()
		{
			foreach (ControlTowerButtonController controllTowerButtonController in listControllButton)
			{
				controllTowerButtonController.DisableConfirm();
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
				controllTowerButtonController.UpdateBuyState();
			}
		}

		[SerializeField]
		private List<ControlTowerButtonController> listControllButton = new List<ControlTowerButtonController>();

		[SerializeField]
		private Sprite[] buttonImage;

		[SerializeField]
		private Sprite lockImage;
	}
}
