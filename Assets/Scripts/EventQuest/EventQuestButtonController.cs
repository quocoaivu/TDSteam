using System;
using UnityEngine;
using WorldMap;

public class EventQuestButtonController : SwitchHandler
{
    public GameObject notifyObj;
    
	private void Start()
	{
		bool active = false;
		if (SignalQuestSystem.Instance.UnclaimedRewardList.Count > 0)
		{
			active = true;
		}
		notifyObj.SetActive(active);
	}

	public override void OnClick()
	{
		base.OnClick();
		OpenEventQuestPanel();
		notifyObj.SetActive(false);
	}

	private void OpenEventQuestPanel()
	{
		MonoSingleton<UIRootHandler>.Instance.eventQuestPopup.Init();
	}

}
