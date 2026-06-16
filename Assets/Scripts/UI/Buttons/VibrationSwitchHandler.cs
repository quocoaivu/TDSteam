using System;
using MetaGame;
using Services.PlatformSpecific;
using UnityEngine;

public class VibrationSwitchHandler : SwitchHandler
{
	private void OnEnable()
	{
		isOn = Setup.Instance.Vibration;
		ViewButton();
	}

	public override void OnClick()
	{
		base.OnClick();
		Setup.Instance.Vibration = !Setup.Instance.Vibration;
		isOn = Setup.Instance.Vibration;
		ViewButton();
		SendEventSetting();
	}

	private void SendEventSetting()
	{
		int vibrate;
		if (isOn)
		{
			vibrate = 1;
		}
		else
		{
			vibrate = 0;
		}
		NativeSpecificServicesSource.Services.Analytics.SendEvent_UserSetting_Vibrate(vibrate);
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
