using System;
using Data;
using LifetimePopup;
using GameCore;
using Services.PlatformSpecific;
using WorldMap;

namespace LinkGame
{
	public class LinkGameSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			//rewardValue = NativeSpecificServicesSource.Services.FacebookServices.GetFreeResources("reward_id_install_goe");
			bool flag = GameUtils.CheckPackageAppIsPresent(MarketingSetup.goe_packageName);
			bool flag2 = OffersStore.Instance.IsOfferProcessed(OffersStore.KEY_INSTALL_GOE);
			if (flag && !flag2)
			{
				ClaimReward();
			}
			else
			{
				InitPopupLink();
			}
		}

		private void InitPopupLink()
		{
			MonoSingleton<UIRootHandler>.Instance.linkGamePopupController.Init();
		}

		private void ClaimReward()
		{
			// Grant and display must use the same value. The old code granted 200 but
			// displayed rewardValue (always 0, since the Facebook bonus line is disabled),
			// so the reward popup showed "0 gem" despite adding 200.
			PlayerCurrencyStore.Instance.ChangeGem(REWARD_GEM, true);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = REWARD_GEM,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			OffersStore.Instance.SetOfferStatus(OffersStore.KEY_INSTALL_GOE, true);
			MonoSingleton<UIRootHandler>.Instance.RefreshLinkGameButtonStatus();
		}

		private const int REWARD_GEM = 200;
	}
}
