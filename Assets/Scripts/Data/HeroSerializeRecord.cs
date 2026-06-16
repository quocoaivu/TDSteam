using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class HeroSerializeRecord
	{
        private Dictionary<int, HeroRecord> listHeroesData;

        private List<int> listHeroOwned;

        public Dictionary<int, HeroRecord> ListHeroesData
		{
			get
			{
				return listHeroesData;
			}
			set
			{
				listHeroesData = value;
			}
		}

		public List<int> ListHeroOwned
		{
			get
			{
				return listHeroOwned;
			}
			set
			{
				listHeroOwned = value;
			}
		}
	}
}
