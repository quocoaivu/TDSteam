using System;
using System.Runtime.Serialization;

namespace Data
{
	[Serializable]
	public class ZoneRecord
	{
		public int playCount;

		public int starEarned;

		[OptionalField]
		public int modePassed;

		[OptionalField]
		public int playCount_victory;

		[OptionalField]
		public int playCount_defeat;
	}
}
