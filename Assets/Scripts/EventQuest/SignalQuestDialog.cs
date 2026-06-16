using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class SignalQuestDialog : GameplayDialogHandler
{
    [Space]
    public SignalQuestEntry sampleEntry;

    public Text noEventText;

    public GameObject eventScrollObj;

    public RectTransform scrollContent;

    public RectTransform scrollHandle;

    public float heightOfEventEntry = 90f;

    private List<SignalQuestEntry> eventList = new List<SignalQuestEntry>();

    private bool isInited;

    public void Init()
	{
		if (!isInited)
		{
			isInited = true;
			eventList.Add(sampleEntry);
			scrollHandle.offsetMin = new Vector2(scrollHandle.offsetMin.x, 0f);
			scrollHandle.offsetMax = new Vector2(scrollHandle.offsetMax.x, 0f);
		}
		int count = SignalQuestSystem.Instance.UnclaimedRewardList.Count;
		int count2 = SignalQuestSystem.Instance.RunningEventList.Count;
		int num = count2 + count;
		for (int i = eventList.Count; i < num; i++)
		{
			SignalQuestEntry eventQuestEntry = UnityEngine.Object.Instantiate<SignalQuestEntry>(sampleEntry, sampleEntry.transform.parent);
			eventQuestEntry.transform.localPosition = sampleEntry.transform.localPosition + new Vector3(0f, (float)(-(float)i) * heightOfEventEntry, 0f);
			eventList.Add(eventQuestEntry);
		}
		for (int j = 0; j < eventList.Count; j++)
		{
			eventList[j].gameObject.SetActive(j < num);
		}
		noEventText.gameObject.SetActive(num == 0);
		eventScrollObj.SetActive(num > 0);
		if (num > 0)
		{
			for (int k = 0; k < count; k++)
			{
				eventList[k].Init(SignalQuestSystem.Instance.UnclaimedRewardList[k], this);
			}
			for (int l = 0; l < count2; l++)
			{
				eventList[count + l].Init(SignalQuestSystem.Instance.RunningEventList[l], this);
			}
			scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, (float)num * heightOfEventEntry + 100f);
		}
		OpenWithScaleAnimation();
	}

	public void UpdateEventList(SignalQuestEntry claimedEntry)
	{
		int count = SignalQuestSystem.Instance.UnclaimedRewardList.Count;
		for (int i = 0; i < count; i++)
		{
			if (eventList[i] == claimedEntry)
			{
				SignalQuestSystem.Instance.ClaimEventReward(i);
				Init();
				break;
			}
		}
	}
}
