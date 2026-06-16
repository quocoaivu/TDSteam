using System;
using Services.PlatformSpecific;

namespace Gameplay
{
	public class TipInformationButton : SwitchHandler
	{
		public int TipId { get; set; }

		public void ShowButton(float buttonLifeTime)
		{
			base.gameObject.SetActive(true);
			base.CustomInvoke(new Action(HideButton), buttonLifeTime);
			SendEventShowButton();
			UISfxDirector.Instance.PlayNewTipButton();
		}

		private void SendEventShowButton()
		{
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_ShowTipsButton("New Tips", TipId);
		}

		public override void OnClick()
		{
			base.OnClick();
			ShowCard();
			HideButton();
		}

		public void ShowCard()
		{
			MonoSingleton<UIRootHandler>.Instance.GameplayTipPopup.Init(TipId);
		}

		private void HideButton()
		{
			base.gameObject.SetActive(false);
		}
	}
}
