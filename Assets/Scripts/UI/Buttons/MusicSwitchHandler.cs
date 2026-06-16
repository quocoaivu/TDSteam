using System;
using MetaGame;
using Services.PlatformSpecific;
using UnityEngine;

public class MusicSwitchHandler : SwitchHandler
{
	private void OnEnable()
	{
		isOn = Setup.Instance.Music;
		ViewButton();
	}

	public override void OnClick()
	{
		base.OnClick();
		Setup.Instance.Music = !Setup.Instance.Music;
		isOn = Setup.Instance.Music;
		ViewButton();
		SendEventSetting();
	}

	private void SendEventSetting()
	{
		//NativeSpecificServicesSource.Services.Analytics.SendEvent_UserSetting_Music(isOn ? 1 : 0);
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
