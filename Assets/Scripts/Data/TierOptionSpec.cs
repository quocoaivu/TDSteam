using System;
using System.Collections.Generic;

namespace Data
{
	public class TierOptionSpec
	{
        private List<int> value;
        
		public TierOptionSpec(List<int> value)
		{
			this.value = value;
		}

		public List<int> Value
		{
			get
			{
				return value;
			}
			private set
			{
				this.value = value;
			}
		}
	}
}
