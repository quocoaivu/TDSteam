using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace MetaGame
{
	public class GameplayTipDiscoveryTracker : BaseMonoBehaviour
	{
		public static GameplayTipDiscoveryTracker Instance
		{
			get
			{
				return GameplayTipDiscoveryTracker.instance;
			}
			private set
			{
				GameplayTipDiscoveryTracker.instance = value;
			}
		}

		private void Awake()
		{
			GameplayTipDiscoveryTracker.Instance = this;
			NumberOfTips = 7;
			LoadTipsFirstTime();
		}

		public bool IsTipFirstTime(int tipID)
		{
			bool flag = GameplayTipDiscoveryTracker.tipsFirstTime[tipID];
			if (flag)
			{
				GameplayTipDiscoveryTracker.UnlockTip(tipID);
			}
			return flag;
		}

		public static bool TipAppeared(int enemyId)
		{
			return !GameplayTipDiscoveryTracker.tipsFirstTime[enemyId];
		}

		private static void UnlockTip(int enemyId)
		{
			GameplayTipDiscoveryTracker.tipsFirstTime[enemyId] = false;
			PlayerPrefs.SetInt(string.Format("TipUnlocked_{0}", enemyId), 1);
		}

		private void LoadTipsFirstTime()
		{
			GameplayTipDiscoveryTracker.tipsFirstTime = new bool[NumberOfTips];
			for (int i = 0; i < NumberOfTips; i++)
			{
				GameplayTipDiscoveryTracker.tipsFirstTime[i] = (PlayerPrefs.GetInt(string.Format("TipUnlocked_{0}", i), 0) == 0);
			}
		}

		public List<bool> GetListTipsUnlockStatus()
		{
			List<bool> list = new List<bool>();
			for (int i = 0; i < GameplayTipDiscoveryTracker.tipsFirstTime.Length; i++)
			{
				if (!GameplayTipDiscoveryTracker.tipsFirstTime[i])
				{
					list.Add(true);
				}
				else
				{
					list.Add(false);
				}
			}
			return list;
		}

		private int NumberOfTips;

		private const string KeyFormat = "TipUnlocked_{0}";

		private static bool[] tipsFirstTime;

		private static GameplayTipDiscoveryTracker instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
