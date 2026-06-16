using System;

namespace MetaGame
{
	public class FormatDirector
	{
		public static FormatDirector Instance
		{
			get
			{
				if (FormatDirector.instance == null)
				{
					FormatDirector.instance = new FormatDirector();
				}
				return FormatDirector.instance;
			}
		}

		private static FormatDirector instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
		public GameFormat gameMode;
	}
}
