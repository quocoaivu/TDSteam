using System;
using MetaGame;

namespace Parameter
{
	public class StarSpec
	{
		public static StarSpec Instance
		{
			get
			{
				if (StarSpec.instance == null)
				{
					StarSpec.instance = new StarSpec();
				}
				return StarSpec.instance;
			}
		}

		public int GetStar(int percent)
		{
			int result = 0;
			if (percent > 0 && percent <= Setup.Instance.LifePercent2Star)
			{
				result = 1;
			}
			if (percent > Setup.Instance.LifePercent2Star && percent < Setup.Instance.LifePercent3Star)
			{
				result = 2;
			}
			else if (percent >= Setup.Instance.LifePercent3Star)
			{
				result = 3;
			}
			return result;
		}

		private static StarSpec instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
