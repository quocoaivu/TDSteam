using UnityEngine;

namespace Common
{
	public static class AssetLoaderBootstrap
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Init()
		{
			CompositeAssetLoader composite = new CompositeAssetLoader(
				new ResourcesLoader(),
				new AddressablesLoader()
			);

			// Đăng ký các prefix đã được migrate sang Addressables ở đây.
			// Path bắt đầu bằng prefix sẽ đi qua AddressablesLoader, còn lại đi ResourcesLoader.
			composite.RegisterAddressablesPrefix("CountryFlags/");
			composite.RegisterAddressablesPrefix("HeroesName/");
			composite.RegisterAddressablesPrefix("HeroesAvatar/");
			composite.RegisterAddressablesPrefix("HeroesMiniAvatar/");
			composite.RegisterAddressablesPrefix("Gameplay-HeroIcon/");
			composite.RegisterAddressablesPrefix("UI Mainmenu/");
			composite.RegisterAddressablesPrefix("EventQuest/");
			composite.RegisterAddressablesPrefix("NewTip/");
			composite.RegisterAddressablesPrefix("NewEnemy/");
            composite.RegisterAddressablesPrefix("NewTower/");
            composite.RegisterAddressablesPrefix("Pet/");
            composite.RegisterAddressablesPrefix("LuckyChest/");
			composite.RegisterAddressablesPrefix("TowerUltimateUpgradeIcon/");
			composite.RegisterAddressablesPrefix("UI Gameplay/");
			composite.RegisterAddressablesPrefix("CountryFlags2/");
			composite.RegisterAddressablesPrefix("UI Worldmap/");
			composite.RegisterAddressablesPrefix("Preview/");
			composite.RegisterAddressablesPrefix("HeroCamp/");
			composite.RegisterAddressablesPrefix("Allies/");
			composite.RegisterAddressablesPrefix("Heroes/");
			composite.RegisterAddressablesPrefix("Enemies/");
			composite.RegisterAddressablesPrefix("Towers/");
			composite.RegisterAddressablesPrefix("Maps/");
			composite.RegisterAddressablesPrefix("Bullets/");
			composite.RegisterAddressablesPrefix("FXs/");

			AssetLoader.Backend = composite;
		}
	}
}
