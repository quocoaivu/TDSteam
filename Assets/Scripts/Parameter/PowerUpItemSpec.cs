using System;
using System.Collections.Generic;

namespace Parameter
{
	public class PowerUpItemSpec : Singleton<PowerUpItemSpec>
	{
		private bool CheckId(int puItemId)
		{
			return puItemId >= 0 && puItemId < listWeapon.Count;
		}

		public void SetWeaponParameter(PowerUpEntry powerUpItem)
		{
			int count = listWeapon.Count;
			if (count <= powerUpItem.id)
			{
				listWeapon.Add(powerUpItem);
			}
		}

		public int GetPrice(int puItemId)
		{
			if (puItemId < listWeapon.Count && puItemId >= 0)
			{
				return listWeapon[puItemId].priceToBuy;
			}
			return 0;
		}

		public int GetCooldownTime(int puItemId)
		{
			if (puItemId < listWeapon.Count && puItemId >= 0)
			{
				return listWeapon[puItemId].cooldownTime;
			}
			return 0;
		}

		public int GetWeaponActivationTime(int puItemId)
		{
			if (puItemId < listWeapon.Count && puItemId >= 0)
			{
				return listWeapon[puItemId].activationTime;
			}
			return 0;
		}

		public int[] GetCustomValue(int puItemId)
		{
			if (puItemId < listWeapon.Count && puItemId >= 0)
			{
				return listWeapon[puItemId].customValues;
			}
			return null;
		}

		public PowerUpEntry GetWeapon(int puItemId)
		{
			return listWeapon[puItemId];
		}

		private List<PowerUpEntry> listWeapon = new List<PowerUpEntry>();
	}
}
