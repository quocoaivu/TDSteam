using System;
using Gameplay;
using LifetimePopup;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace GiftcodeSystem
{
	public class VoucherCodeDialogHandler : GameplayDialogHandler
	{
		public void Init()
		{
			OpenWithScaleAnimation();
		}

		public void TryToSendGiftCode()
		{
			if (GameUtils.IsInternetConnectionAvailable())
			{
				if (inputField.text.Length == 0)
				{
					string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(120);
					MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
				}
				else
				{
					SendGiftCode();
					CloseWithScaleAnimation();
				}
			}
			else
			{
				string notiContent2 = Singleton<AlertSynopsis>.Instance.GetNotiContent(119);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent2, false, false);
			}
		}

		private void SendGiftCode()
		{
			string deviceUniqueID = GameUtils.GetDeviceUniqueID();
			string text = inputField.text;
			MonoSingleton<GlobeZoneDirector>.Instance.GiftCodeManager.SubmitGiftCode(text, deviceUniqueID);
		}

		public void CancelGiftCode()
		{
			CloseWithScaleAnimation();
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			GameUtils.ClearInputField(inputField);
		}

		[SerializeField]
		private InputField inputField;
	}
}
