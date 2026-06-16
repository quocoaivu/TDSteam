using Parameter;
using UnityEngine;

namespace Data
{
	public class PowerUpItemDataLoader : MonoBehaviour
	{
		private void Start()
		{
			ReadWeaponParameter();
		}

		private void ReadWeaponParameter()
		{
			ItemSetup itemConfig = ConfigRegistry.Instance.itemConfig;
			for (int i = 0; i < itemConfig.dataArray.Length; i++)
			{
				if (itemConfig.dataArray[i].Price_to_buy > 0)
				{
					ItemSetupRecord itemConfigData = itemConfig.dataArray[i];
					PowerUpEntry weaponParameter = default(PowerUpEntry);
					weaponParameter.id = itemConfigData.Id;
					weaponParameter.name = itemConfigData.Name;
					weaponParameter.priceToBuy = itemConfigData.Price_to_buy;
					weaponParameter.cooldownTime = itemConfigData.Time_cooldown;
					weaponParameter.activationTime = itemConfigData.Activation_time;
					weaponParameter.customValues = itemConfigData.Customvalues;
					Singleton<PowerUpItemSpec>.Instance.SetWeaponParameter(weaponParameter);
				}
			}
		}
	}
}
