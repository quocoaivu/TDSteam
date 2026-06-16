using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class SalePackRecord
	{
        private List<SerializePackItem> listSpecialBundleData;

        public string lastTimePlayed;

        public List<SerializePackItem> ListSpecialBundleData
		{
			get
			{
				return listSpecialBundleData;
			}
			set
			{
				listSpecialBundleData = value;
			}
		}
	}
}
