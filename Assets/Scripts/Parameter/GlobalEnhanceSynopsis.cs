using System;
using System.Collections.Generic;

namespace Parameter
{
	public class GlobalEnhanceSynopsis : Singleton<GlobalEnhanceSynopsis>
	{
		public void ClearData()
		{
			listUpgradeDescription.Clear();
		}

		public void SetGlobalUpgradeParameters(GlobalEnhanceBrief gud)
		{
			int count = listUpgradeDescription.Count;
			if (count <= gud.id)
			{
				listUpgradeDescription.Add(gud);
			}
		}

		public string GetTitle(int guID)
		{
			if (guID < listUpgradeDescription.Count && guID >= 0)
			{
				return listUpgradeDescription[guID].title;
			}
			return "--";
		}

		public string GetDescription(int guID)
		{
			if (guID < listUpgradeDescription.Count && guID >= 0)
			{
				return listUpgradeDescription[guID].description;
			}
			return "--";
		}

		private List<GlobalEnhanceBrief> listUpgradeDescription = new List<GlobalEnhanceBrief>();
	}
}
