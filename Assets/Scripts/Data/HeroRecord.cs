using System;
using System.Runtime.Serialization;

namespace Data
{
	[Serializable]
	public class HeroRecord
	{
		public int level;

		public int totalExp;

		[OptionalField]
		public int[] skillPoints;

		[OptionalField]
		public bool havePet;
	}
}
