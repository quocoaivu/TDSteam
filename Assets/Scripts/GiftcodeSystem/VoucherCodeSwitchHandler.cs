using System;
using LifetimePopup;
using GameCore;
using Parameter;
using WorldMap;

namespace GiftcodeSystem
{
	public class VoucherCodeSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			TryOpenPopupGiftCode();
		}

		private void TryOpenPopupGiftCode()
		{
			if (GameUtils.IsInternetConnectionAvailable())
			{
				MonoSingleton<UIRootHandler>.Instance.giftCodePopupController.Init();
			}
			else
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(118);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, true, false);
			}
		}
	}
}
