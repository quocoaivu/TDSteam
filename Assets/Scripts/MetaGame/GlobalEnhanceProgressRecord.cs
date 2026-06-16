using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MetaGame
{
	[Serializable]
	public class GlobalEnhanceProgressRecord
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnStarsSpentChanged;

		public List<TierEnhanceStanding> Tiers
		{
			get
			{
				return tiers;
			}
		}

		public int StarsSpent
		{
			get
			{
				return starsSpent;
			}
			set
			{
				starsSpent = value;
				if (GlobalEnhanceProgressRecord.OnStarsSpentChanged != null)
				{
					GlobalEnhanceProgressRecord.OnStarsSpentChanged();
				}
			}
		}

		[SerializeField]
		private List<TierEnhanceStanding> tiers;

		[SerializeField]
		private int starsSpent;
	}
}
