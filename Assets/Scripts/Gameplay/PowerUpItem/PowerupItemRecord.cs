using System;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class PowerupItemRecord : MonoBehaviour
	{
		public int TotalPowerupItemUsed
		{
			get
			{
				return totalPowerupItemUsed;
			}
			private set
			{
				totalPowerupItemUsed = value;
			}
		}

		public void InitValue()
		{
			totalPowerupItemUsed = 0;
		}

		public void IncreaseUseAmount()
		{
			totalPowerupItemUsed++;
		}

		public bool IsReachedLimitUse()
		{
			bool result = false;
			int powerupItemLimit = ZoneRuleSpec.Instance.GetPowerupItemLimit();
			if (totalPowerupItemUsed >= powerupItemLimit)
			{
				result = true;
			}
			return result;
		}

		private int totalPowerupItemUsed;

		private int powerupItemLimitUse;
	}
}
