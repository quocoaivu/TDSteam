using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Data
{
	[Serializable]
	public class ZoneSerializeRecord
	{
        public int mapIDUnlocked;

        public int lastMapIDPlayed;

        [OptionalField]
        public int lastMapModeChoose;

        private Dictionary<int, ZoneRecord> listMapsData;

        public Dictionary<int, ZoneRecord> ListMapsData
		{
			get
			{
				return listMapsData;
			}
			set
			{
				listMapsData = value;
			}
		}
	}
}
