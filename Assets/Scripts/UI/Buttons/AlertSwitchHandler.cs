using System;
using MetaGame;
using UnityEngine;

public class AlertSwitchHandler : SwitchHandler
{
	private void OnEnable()
	{
		isOn = Setup.Instance.AllowPushNoti;
		ViewButton();
	}

	public override void OnClick()
	{
		base.OnClick();
		Setup.Instance.AllowPushNoti = !Setup.Instance.AllowPushNoti;
		isOn = Setup.Instance.AllowPushNoti;
		ViewButton();
	}

	private void ViewButton()
	{
		if (isOn)
		{
			imageOff.SetActive(false);
		}
		else
		{
			imageOff.SetActive(true);
		}
	}

	[SerializeField]
	private GameObject imageOff;

	private bool isOn;
}
