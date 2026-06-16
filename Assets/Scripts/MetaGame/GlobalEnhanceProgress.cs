using System;
using Common;
using UnityEngine;

namespace MetaGame
{
	[CreateAssetMenu(fileName = "GlobalEnhanceProgress", menuName = "SSR/PlayerData/GlobalEnhanceProgress")]
	public class GlobalEnhanceProgress : WritableScriptableObject<GlobalEnhanceProgressRecord>
	{
		public void ResetData()
		{
			base.CopyDefaultDataToCurrentData();
			currentData.StarsSpent = 0;
		}
	}
}
