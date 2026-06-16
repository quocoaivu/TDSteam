using System;
using UnityEngine;

namespace GiftcodeSystem
{
	public class CancelSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			Cancel();
		}

		private void Cancel()
		{
			giftCodePopupController.CancelGiftCode();
		}

		[SerializeField]
		private VoucherCodeDialogHandler giftCodePopupController;
	}
}
