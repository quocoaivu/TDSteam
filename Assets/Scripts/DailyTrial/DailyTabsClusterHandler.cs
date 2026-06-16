using System;
using System.Collections.Generic;
using UnityEngine;

namespace DailyTrial
{
	public class DailyTabsClusterHandler : MonoBehaviour
	{
        [SerializeField]
        private List<DailyTabEntry> listDailyTabs = new List<DailyTabEntry>();

        public void InitTabsData()
		{
			foreach (DailyTabEntry dailyTab in listDailyTabs)
			{
				dailyTab.Init();
			}
		}
	}
}
