using System;

public class RunningSignalRecord
{
    public SignalSetupRecord configData;

    public TailoredObscuredInt curProgress = new TailoredObscuredInt();

    public DateTime endTime;

    private DateTime lastSaveTime;

    private int subscribeId;

    public RunningSignalRecord()
	{
		lastSaveTime = GameKit.GetNow().AddDays(-1.0);
	}

	public RunningSignalRecord(SignalSetupRecord configData, DateTime endTime)
	{
		this.configData = configData;
		this.endTime = endTime;
		lastSaveTime = GameKit.GetNow().AddDays(-1.0);
	}

	public void OnTriggerEvent(SignalTriggerRecord triggerData)
	{
		if (IsValidTrigger(triggerData))
		{
			curProgress.Value = curProgress.Value + triggerData.addedQuantity;
			if (curProgress.Value >= configData.Targetquantity)
			{
				GameSignalCenter.Instance.Unsubscribe(subscribeId);
				SignalQuestSystem.Instance.OnEventComplete(this);
			}
			else if (triggerData.forceSaveProgress || (GameKit.GetNow() - lastSaveTime).TotalSeconds > 4.0)
			{
				GameKit.SaveRunningEventProgress(this);
				lastSaveTime = GameKit.GetNow();
			}
		}
	}

	public bool IsValidTrigger(SignalTriggerRecord triggerData)
	{
		if (triggerData.triggerType != configData.EVENTTRIGGERTYPE)
		{
			return false;
		}
		if ((GameKit.GetNow() - endTime.AddDays((double)(-(double)configData.Durationinday))).TotalSeconds < 0.0)
		{
			return false;
		}
		if ((endTime - GameKit.GetNow()).TotalSeconds < 0.0)
		{
			return false;
		}
		CompareValueFormat comparevaluemode = configData.COMPAREVALUEMODE;
		if (comparevaluemode == CompareValueFormat.None)
		{
			return true;
		}
		if (comparevaluemode != CompareValueFormat.OneToOne)
		{
			if (comparevaluemode == CompareValueFormat.OneAmong)
			{
				for (int i = configData.Triggervalue.Length - 1; i >= 0; i--)
				{
					if (configData.Triggervalue[i] == triggerData.triggerValue)
					{
						return true;
					}
				}
			}
			return false;
		}
		return triggerData.triggerValue == configData.Triggervalue[0];
	}

	public void SubscribeTrigger()
	{
		subscribeId = GameKit.GetUniqueId();
		SignalTriggerListenerRecord data = new SignalTriggerListenerRecord(subscribeId, new GameSignalCenter.EventTriggerMethod(OnTriggerEvent));
		switch (configData.EVENTTRIGGERTYPE)
		{
		case SignalTriggerKind.KillMonster:
			GameSignalCenter.Instance.Subscribe(GameSignalKind.EventKillMonster, data);
			break;
		case SignalTriggerKind.WinCampaign:
		case SignalTriggerKind.LoseCampaign:
			GameSignalCenter.Instance.Subscribe(GameSignalKind.EventCampaign, data);
			break;
		case SignalTriggerKind.UseItem:
			GameSignalCenter.Instance.Subscribe(GameSignalKind.EventUseItem, data);
			break;
		case SignalTriggerKind.UseHeroWinCampaign:
			GameSignalCenter.Instance.Subscribe(GameSignalKind.EventUseHero, data);
			break;
		case SignalTriggerKind.PlayTournament:
			GameSignalCenter.Instance.Subscribe(GameSignalKind.EventPlayTournament, data);
			break;
		case SignalTriggerKind.InviteFriend:
			GameSignalCenter.Instance.Subscribe(GameSignalKind.EventInviteFriend, data);
			break;
		case SignalTriggerKind.EarnGold:
			GameSignalCenter.Instance.Subscribe(GameSignalKind.EventEarnGold, data);
			break;
		case SignalTriggerKind.UseGem:
			GameSignalCenter.Instance.Subscribe(GameSignalKind.EventUseGem, data);
			break;
		}
	}

	public bool IsTargetReached()
	{
		return curProgress.Value >= configData.Targetquantity;
	}
}
