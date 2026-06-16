using System;
using UnityEngine;

namespace GiftcodeSystem
{
	public class ConfirmSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			Confirm();
		}

		private void Confirm()
		{
			giftCodePopupController.TryToSendGiftCode();
		}

		[SerializeField]
		private VoucherCodeDialogHandler giftCodePopupController;
	}
}
