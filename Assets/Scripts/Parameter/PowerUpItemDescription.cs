using System;
using System.Collections.Generic;

namespace Parameter
{
	public class PowerUpItemDescription : Singleton<PowerUpItemDescription>
	{
		private bool CheckId(int tipID)
		{
			return tipID >= 0 && tipID < listPowerUpItemDescription.Count;
		}

		public void ClearData()
		{
			listPowerUpItemDescription.Clear();
		}

		public void SetPowerUpItemParameter(PowerUpItemDes tip)
		{
			int count = listPowerUpItemDescription.Count;
			if (count <= tip.id)
			{
				listPowerUpItemDescription.Add(tip);
			}
		}

		public string GetName(int tipID)
		{
			if (tipID < listPowerUpItemDescription.Count && tipID >= 0)
			{
				return listPowerUpItemDescription[tipID].name;
			}
			return "--";
		}

		public string GetDescription(int tipID)
		{
			if (tipID < listPowerUpItemDescription.Count && tipID >= 0)
			{
				return listPowerUpItemDescription[tipID].description;
			}
			return "--";
		}

		private List<PowerUpItemDes> listPowerUpItemDescription = new List<PowerUpItemDes>();
	}
}
