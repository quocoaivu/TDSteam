using System;
using UnityEngine;

public class OpenFBFanpageSwitchHandler : SwitchHandler
{
	private void OnEnable()
	{
	}

	public override void OnClick()
	{
		Application.OpenURL(MarketingSetup.fbFanpageLinkWeb);
	}
}
