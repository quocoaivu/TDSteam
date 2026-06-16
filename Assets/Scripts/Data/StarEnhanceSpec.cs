using System;
using System.Collections.Generic;

namespace Data
{
	public class StarEnhanceSpec
	{
        private List<int> starPerTier;

        public StarEnhanceSpec(List<int> value)
		{
			starPerTier = value;
		}

		public List<int> Value
		{
			get
			{
				return starPerTier;
			}
			private set
			{
				starPerTier = value;
			}
		}
	}
}
