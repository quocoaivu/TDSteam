using System;
using System.Collections.Generic;
using UnityEngine;

namespace DailyTrial
{
	public class QuestClusterHandler : MonoBehaviour
	{
        [SerializeField]
        private List<QuestHandler> listMission = new List<QuestHandler>();

        public void InitAllMissionsState()
		{
			foreach (QuestHandler missionController in listMission)
			{
				missionController.InitState();
			}
		}
	}
}
