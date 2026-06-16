using System;
using System.Collections.Generic;
using Data;
using LifetimePopup;
using Services.PlatformSpecific;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignalQuestEntry : MonoBehaviour
{
    public Image iconEvent;

    public TextMeshProUGUI textEventTitle;

    public Text textProgress;

    public GameObject progressBar;

    public Text timeRemainText;

    public GameObject inactiveBtn;

    public GameObject claimBtn;

    public TextMeshProUGUI rewardText;

    private float countdownUpdateTime;

    private bool isEventCompleted;

    private SignalSetupRecord curEventData;

    private RunningSignalRecord runningEventData;

    private SignalQuestDialog eventPopup;

    public void Init(SignalSetupRecord completedEventData, SignalQuestDialog eventPopup)
	{
		isEventCompleted = true;
		curEventData = completedEventData;
		this.eventPopup = eventPopup;
		Init();
	}

	public void Init(RunningSignalRecord runningEventData, SignalQuestDialog eventPopup)
	{
		isEventCompleted = false;
		curEventData = runningEventData.configData;
		this.runningEventData = runningEventData;
		this.eventPopup = eventPopup;
		Init();
	}

	private void Init()
	{
		iconEvent.sprite = Common.AssetLoader.Load<Sprite>("EventQuest/icon/EventIcon" + (int)curEventData.EVENTICONTYPE);
		textEventTitle.text = string.Format(GameKit.GetLocalization(curEventData.Eventtitlekey), curEventData.Targetquantity, curEventData.Textextradata);
		UnityEngine.Debug.LogFormat(">>>>>>>> event {0}, complete: {1}", new object[]
		{
			textEventTitle.text,
			isEventCompleted
		});
		if (isEventCompleted)
		{
			textProgress.text = string.Format("{0}/{0}", curEventData.Targetquantity);
			progressBar.transform.localScale = Vector3.one;
			timeRemainText.text = string.Empty;
			inactiveBtn.SetActive(false);
			claimBtn.SetActive(true);
		}
		else
		{
			DateTime d = runningEventData.endTime.AddDays((double)(-(double)runningEventData.configData.Durationinday));
			if ((d - GameKit.GetNow()).TotalMinutes > 0.0)
			{
				textProgress.text = "??/??";
				progressBar.transform.localScale = new Vector3(0f, 1f, 1f);
				countdownUpdateTime = 1E+09f;
				timeRemainText.text = "Upcoming event";
			}
			else
			{
				textProgress.text = string.Format("{0}/{1}", runningEventData.curProgress.Value, curEventData.Targetquantity);
				progressBar.transform.localScale = new Vector3((float)runningEventData.curProgress.Value * 1f / (float)curEventData.Targetquantity, 1f, 1f);
				countdownUpdateTime = 0f;
			}
			inactiveBtn.SetActive(true);
			claimBtn.SetActive(false);
		}
		string arg = string.Empty;
		if (curEventData.REWARDTYPE == PrizeKind.Gem)
		{
			arg = GameKit.ConvertIconToText(TdGlyphKey.Gem);
		}
		else if (curEventData.REWARDTYPE == PrizeKind.Item)
		{
			arg = GameKit.ConvertIconToText((TdGlyphKey)curEventData.Rewardid);
		}
		rewardText.text = string.Format("{0}\n<size=180%>{1} {2}", (!isEventCompleted) ? "REWARD" : "CLAIM", curEventData.Rewardquantity, arg);
	}

	private void Update()
	{
		if (!isEventCompleted)
		{
			if (runningEventData == null)
			{
				isEventCompleted = true;
				UnityEngine.Debug.LogFormat(">>>>>>>>>>>> evetn {0} have null runningData", new object[]
				{
					textEventTitle.text
				});
				return;
			}
			countdownUpdateTime -= Time.deltaTime;
			if (countdownUpdateTime <= 0f)
			{
				countdownUpdateTime = 1.1f;
				TimeSpan timeSpan = runningEventData.endTime - GameKit.GetNow();
				if (timeSpan.TotalSeconds < 0.0)
				{
					countdownUpdateTime = 1E+08f;
					timeRemainText.text = string.Empty;
				}
				else
				{
					timeRemainText.text = string.Format("{0} {1}d{2}h{3}m{4}s", new object[]
					{
						GameKit.GetLocalization("TIME_LEFT"),
						timeSpan.Days,
						timeSpan.Hours,
						timeSpan.Minutes,
						timeSpan.Seconds
					});
				}
			}
		}
	}

	public void OnClaimRewardBtnClicked()
	{
		List<PrizeItem> list = new List<PrizeItem>();
		if (curEventData.REWARDTYPE == PrizeKind.Gem)
		{
			list.Add(new PrizeItem(PrizeKind.Gem, curEventData.Rewardquantity, true));
			PlayerCurrencyStore.Instance.ChangeGem(curEventData.Rewardquantity, true);
		}
		else if (curEventData.REWARDTYPE == PrizeKind.Item)
		{
			list.Add(new PrizeItem(PrizeKind.Item, curEventData.Rewardid, curEventData.Rewardquantity, true));
			PowerUpItemStore.Instance.ChangeItemQuantity(curEventData.Rewardquantity, curEventData.Rewardid);
		}
		MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Init(list.ToArray());
        NativeSpecificServicesSource.Services.Analytics.SendEvent_CompleteEvent(curEventData.Eventid);
        eventPopup.UpdateEventList(this);
	}
}
