using System;
using UnityEngine;

public class RateGameSwitchHandler : SwitchHandler
{
	public override void OnClick()
	{
		Application.OpenURL(MarketingSetup.rateGameLink);
	}
}
