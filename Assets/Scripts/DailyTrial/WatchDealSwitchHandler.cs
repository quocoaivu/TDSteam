using System;
using System.Collections.Generic;
using Data;
using Parameter;

namespace DailyTrial
{
	public class WatchDealSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			InitOffer();
		}

		private void InitOffer()
		{
			int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
			List<int> listInputHeroesID = DailyOrdealSpec.Instance.getListInputHeroesID(currentDayIndex);
		}

		public void TurnOffButton()
		{
			base.gameObject.SetActive(false);
		}
	}
}
