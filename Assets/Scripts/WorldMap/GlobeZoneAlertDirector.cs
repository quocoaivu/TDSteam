using System;
using System.Collections.Generic;
using Data;
using Notify;
using UnityEngine;

namespace WorldMap
{
	public class GlobeZoneAlertDirector : MonoSingleton<GlobeZoneAlertDirector>
	{
        [SerializeField]
        private List<AlertTrooper> listNotifyUnit = new List<AlertTrooper>();

        private bool refreshScheduled;

        private void Start()
		{
			RefreshAllNotifyStatus();
			PlayerCurrencyStore.Instance.OnGemChangeEvent += OnAnyDataChanged;
			GlobalUpgradeStore.Instance.OnStarChangeEvent += OnAnyDataChanged;
			HeroStore.Instance.OnSkillPointChangeEvent += OnAnyDataChanged;
		}

		private void OnDestroy()
		{
			if (PlayerCurrencyStore.Instance != null)
			{
				PlayerCurrencyStore.Instance.OnGemChangeEvent -= OnAnyDataChanged;
			}
			if (GlobalUpgradeStore.Instance != null)
			{
				GlobalUpgradeStore.Instance.OnStarChangeEvent -= OnAnyDataChanged;
			}
			if (HeroStore.Instance != null)
			{
				HeroStore.Instance.OnSkillPointChangeEvent -= OnAnyDataChanged;
			}
		}

		private void OnAnyDataChanged()
		{
			if (refreshScheduled)
			{
				return;
			}
			refreshScheduled = true;
			base.CustomInvoke(new Action(DoRefresh), Time.deltaTime);
		}

		private void DoRefresh()
		{
			refreshScheduled = false;
			RefreshAllNotifyStatus();
		}

		public void RefreshAllNotifyStatus()
		{
			foreach (AlertTrooper notifyUnit in listNotifyUnit)
			{
				if (notifyUnit != null)
				{
					notifyUnit.CheckCondition();
				}
			}
		}
	}
}
