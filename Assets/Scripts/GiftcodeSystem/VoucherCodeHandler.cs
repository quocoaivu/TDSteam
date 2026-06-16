using System;
using Data;
using LifetimePopup;
using Parameter;
using UnityEngine;
using WorldMap;

namespace GiftcodeSystem
{
	public class VoucherCodeHandler : MonoBehaviour
	{
		private void Awake()
		{
			MonoSingleton<GlobeZoneDirector>.Instance.GiftCodeManager.onGiftCodeSubmited += GiftCodeManager_onGiftCodeSubmited;
		}

		private void OnDestroy()
		{
			GlobeZoneDirector worldMapManager = MonoSingleton<GlobeZoneDirector>.InstanceIfExists;
			if (worldMapManager != null && worldMapManager.GiftCodeManager != null)
			{
				worldMapManager.GiftCodeManager.onGiftCodeSubmited -= GiftCodeManager_onGiftCodeSubmited;
			}
		}

		private void GiftCodeManager_onGiftCodeSubmited(ReceivedVoucherCodeMessage obj)
		{
			if (obj.bonus != null)
			{
				string giftCodeType = VoucherCodeStaticDefine.GetGiftCodeType(obj.bonus);
				UnityEngine.Debug.Log(giftCodeType);
				if (giftCodeType != null)
				{
					if (!(giftCodeType == "HERO"))
					{
						if (!(giftCodeType == "GEMS"))
						{
							if (giftCodeType == "HERONGEM")
							{
								ProcessGiftCodeHeroNGem(obj);
							}
						}
						else
						{
							ProcessGiftCodeGems(obj);
						}
					}
					else
					{
						ProcessGiftCodeHero(obj);
					}
				}
			}
			else
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(121);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			}
		}

		private void ProcessGiftCodeHero(ReceivedVoucherCodeMessage obj)
		{
			VoucherCodeHeroNCrystal giftCodeHeroNGem = VoucherCodeStaticDefine.GetGiftCodeHeroNGem(obj.bonus);
			int num;
			if (string.IsNullOrEmpty(giftCodeHeroNGem.heroid))
			{
				num = 0;
			}
			else
			{
				num = int.Parse(giftCodeHeroNGem.heroid);
			}
			if (!HeroStore.Instance.IsHeroOwned(num))
			{
				HeroStore.Instance.UnlockHero(num);
				PrizeItem[] listData = new PrizeItem[]
				{
					new PrizeItem
					{
						rewardType = PrizeKind.SingleHero,
						itemID = num,
						isDisplayQuantity = false
					}
				};
				MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			}
			else
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(122);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			}
		}

		private void ProcessGiftCodeGems(ReceivedVoucherCodeMessage obj)
		{
			VoucherCodeGems giftCodeGems = VoucherCodeStaticDefine.GetGiftCodeGems(obj.bonus);
			int num = int.Parse(giftCodeGems.gems);
			PlayerCurrencyStore.Instance.ChangeGem(num, true);
			PrizeItem[] listData = new PrizeItem[]
			{
				new PrizeItem
				{
					rewardType = PrizeKind.Gem,
					value = num,
					isDisplayQuantity = true
				}
			};
			MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
		}

		private void ProcessGiftCodeHeroNGem(ReceivedVoucherCodeMessage obj)
		{
			VoucherCodeHeroNCrystal giftCodeHeroNGem = VoucherCodeStaticDefine.GetGiftCodeHeroNGem(obj.bonus);
			int num = int.Parse(giftCodeHeroNGem.heroid);
			int num2 = int.Parse(giftCodeHeroNGem.gems);
			if (!HeroStore.Instance.IsHeroOwned(num))
			{
				HeroStore.Instance.UnlockHero(num);
				PlayerCurrencyStore.Instance.ChangeGem(num2, true);
				PrizeItem[] listData = new PrizeItem[]
				{
					new PrizeItem
					{
						rewardType = PrizeKind.SingleHero,
						itemID = num,
						isDisplayQuantity = false
					},
					new PrizeItem
					{
						rewardType = PrizeKind.Gem,
						value = num2,
						isDisplayQuantity = true
					}
				};
				MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData);
			}
			else
			{
				PlayerCurrencyStore.Instance.ChangeGem(num2, true);
				PrizeItem[] listData2 = new PrizeItem[]
				{
					new PrizeItem
					{
						rewardType = PrizeKind.Gem,
						value = num2,
						isDisplayQuantity = true
					}
				};
				MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(listData2);
			}
		}
	}
}
