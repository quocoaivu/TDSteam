using System;
using UnityEngine;

public class MoreGameSwitchHandler : SwitchHandler
{
	public override void OnClick()
	{
		Application.OpenURL(MarketingSetup.moreGameLink);
	}
}
