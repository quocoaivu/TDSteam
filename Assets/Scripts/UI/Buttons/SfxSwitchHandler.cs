using System;
using MetaGame;
using Services.PlatformSpecific;
using UnityEngine;

public class SfxSwitchHandler : SwitchHandler
{
	private void OnEnable()
	{
		isOn = Setup.Instance.Sound;
		ViewButton();
	}

	public override void OnClick()
	{
		base.OnClick();
		Setup.Instance.Sound = !Setup.Instance.Sound;
		isOn = Setup.Instance.Sound;
		ViewButton();
		SendEventSetting();
	}

	private void SendEventSetting()
	{
		//NativeSpecificServicesSource.Services.Analytics.SendEvent_UserSetting_Sound(isOn ? 1 : 0);
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
