using System;
using System.Collections.Generic;
using Bootstrap;
using UnityEngine;

public class SignalQuestSystem : MonoBehaviour
{
	public static SignalQuestSystem Instance { get; private set; }

	private readonly List<RunningSignalRecord> _runningEventList = new List<RunningSignalRecord>();

	private readonly List<SignalSetupRecord> _unclaimedRewardList = new List<SignalSetupRecord>();

	private int requiredEasyEvent = 2;

	private int requiredHardEvent = 1;

	public IReadOnlyList<RunningSignalRecord> RunningEventList => _runningEventList;

	public IReadOnlyList<SignalSetupRecord> UnclaimedRewardList => _unclaimedRewardList;

	private void Awake()
	{
		if (SignalQuestSystem.Instance != null && SignalQuestSystem.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SignalQuestSystem.Instance = this;
		// Parent "Common Data" already calls DontDestroyOnLoad; this subtree persists with it.
	}

	private void Start()
	{
		// Init in Start so ConfigRegistry / GameBootstrap singletons are ready.
		if (SignalQuestSystem.Instance == this)
		{
			Init();
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			return;
		}
		if (GameKit.GetNow().DayOfYear != GameKit.GetDayOfYearUpdateEvent())
		{
			Init();
		}
	}

	public void Init()
	{
		_runningEventList.Clear();
		_unclaimedRewardList.Clear();
		DateTime now = GameKit.GetNow();

		List<int> listUnclaimedReward = GameKit.GetListUnclaimedReward();
		for (int i = 0; i < listUnclaimedReward.Count; i++)
		{
			if (ConfigRegistry.Instance.eventIdToEventData.TryGetValue(listUnclaimedReward[i], out SignalSetupRecord rewardConfig))
			{
				_unclaimedRewardList.Add(rewardConfig);
			}
		}

		bool needSave = false;
		int easyCount = 0;
		int hardCount = 0;
		byte[] listRunningEvent = GameKit.GetListRunningEvent();
		for (int l = 0; l < listRunningEvent.Length; l++)
		{
			if (!ConfigRegistry.Instance.eventIdToEventData.TryGetValue((int)listRunningEvent[l], out SignalSetupRecord runningConfig))
			{
				continue;
			}
			RunningSignalRecord runningEventData = new RunningSignalRecord();
			runningEventData.configData = runningConfig;
			GameKit.GetRunningEventProgress(runningEventData);
			if ((now - runningEventData.endTime).TotalSeconds > 0.0)
			{
				needSave = true;
			}
			else
			{
				_runningEventList.Add(runningEventData);
				if (runningEventData.configData.EVENTQUESTTYPE == SignalQuestKind.Easy)
				{
					easyCount++;
				}
				if (runningEventData.configData.EVENTQUESTTYPE == SignalQuestKind.Hard)
				{
					hardCount++;
				}
			}
		}

		if (now.DayOfYear != GameKit.GetDayOfYearUpdateEvent())
		{
			GameKit.SetDayOfYearUpdateEvent(now.DayOfYear);
			List<RunningSignalRecord> holidayEvents = GetHolidayEventIdsFromRemoteConfig();
			for (int m = 0; m < holidayEvents.Count; m++)
			{
				if (!IsEventRunning(holidayEvents[m].configData.Eventid) && (holidayEvents[m].endTime - now).TotalSeconds > 0.0)
				{
					_runningEventList.Add(holidayEvents[m]);
					needSave = true;
				}
			}
			while (easyCount < requiredEasyEvent)
			{
				RunningSignalRecord newEvent = GetNewRandomEvent(SignalQuestKind.Easy);
				if (newEvent == null)
				{
					break;
				}
				_runningEventList.Add(newEvent);
				needSave = true;
				easyCount++;
			}
			while (hardCount < requiredHardEvent)
			{
				RunningSignalRecord newEvent = GetNewRandomEvent(SignalQuestKind.Hard);
				if (newEvent == null)
				{
					break;
				}
				_runningEventList.Add(newEvent);
				needSave = true;
				hardCount++;
			}
		}

		GameSignalCenter.Instance.UnsubscribeEventQuestEvent();
		for (int n = _runningEventList.Count - 1; n >= 0; n--)
		{
			_runningEventList[n].SubscribeTrigger();
		}
		if (needSave)
		{
			SaveEvent();
		}
	}

	public void OnEventComplete(RunningSignalRecord eventData)
	{
		_unclaimedRewardList.Add(eventData.configData);
		for (int i = _runningEventList.Count - 1; i >= 0; i--)
		{
			if (_runningEventList[i] == eventData)
			{
				_runningEventList.RemoveAt(i);
				break;
			}
		}
		SaveEvent();
	}

	public void SaveEvent()
	{
		GameKit.SaveListRunningEvent(_runningEventList);
		GameKit.SaveListUnclaimedReward(_unclaimedRewardList);
	}

	public void ClaimEventReward(int unclaimedIndex)
	{
		if (unclaimedIndex >= _unclaimedRewardList.Count)
		{
			UnityEngine.Debug.LogError("try to claim event out of list, error id " + unclaimedIndex);
			return;
		}
		_unclaimedRewardList.RemoveAt(unclaimedIndex);
		GameKit.SaveListUnclaimedReward(_unclaimedRewardList);
	}

	public bool IsEventRunning(int eventId)
	{
		for (int i = _runningEventList.Count - 1; i >= 0; i--)
		{
			if (_runningEventList[i].configData.Eventid == eventId)
			{
				return true;
			}
		}
		return false;
	}

	private DateTime GetMoment0(DateTime day)
	{
		day = day.AddHours((double)(-(double)day.Hour));
		day = day.AddMinutes((double)(-(double)day.Minute));
		day = day.AddSeconds((double)(-(double)day.Second));
		return day;
	}

	private RunningSignalRecord GetNewRandomEvent(SignalQuestKind eventType)
	{
		if (!ConfigRegistry.Instance.eventTypeToEventData.TryGetValue(eventType, out List<SignalSetupRecord> candidates) || candidates.Count == 0)
		{
			return null;
		}
		int count = candidates.Count;
		int num = UnityEngine.Random.Range(0, count);
		// Thá»­ tá»‘i Ä‘a count láº§n Ä‘á»ƒ tÃ¬m event chÆ°a cháº¡y; trÃ¡nh láº·p vÃ´ háº¡n khi táº¥t cáº£ Ä‘Ã£ cháº¡y.
		for (int tried = 0; tried < count; tried++)
		{
			SignalSetupRecord candidate = candidates[num];
			if (!IsEventRunning(candidate.Eventid))
			{
				return new RunningSignalRecord(candidate, GetMoment0(GameKit.GetNow()).AddDays((double)candidate.Durationinday));
			}
			num = (num + 1) % count;
		}
		return null;
	}

	public List<RunningSignalRecord> GetHolidayEventIdsFromRemoteConfig()
	{
		List<RunningSignalRecord> list = new List<RunningSignalRecord>();
		int holidayEventId = Bootstrap.GameBootstrap.Instance.RemoteConfig.GetHolidayEventId();
		if (holidayEventId > 0 && !IsEventRunning(holidayEventId)
			&& ConfigRegistry.Instance.eventIdToEventData.TryGetValue(holidayEventId, out SignalSetupRecord eventConfigData))
		{
			DateTime day = GameKit.FromUnixTimeToDateTime(Bootstrap.GameBootstrap.Instance.RemoteConfig.GetHolidayStartDay());
			DateTime dateTime = GetMoment0(day).AddDays((double)eventConfigData.Durationinday);
			if ((dateTime - GameKit.GetNow()).TotalMinutes > 0.0)
			{
				list.Add(new RunningSignalRecord(eventConfigData, dateTime));
			}
		}
		return list;
	}
}
