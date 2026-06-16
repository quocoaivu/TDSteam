using System;

namespace DailyReward
{
	public class DailyPrizeSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			PriorityDialogDirector.Instance.CreatePopup(PriorityDialogDirector.Instance.dailyRewardPopupPrefab, DialogPriorityEnum.Normal);
		}
	}
}
