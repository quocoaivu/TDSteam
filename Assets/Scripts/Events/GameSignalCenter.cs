using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class GameSignalCenter
{
    public delegate void SimpleSubscribeMethod();

    public delegate void EnemySubscribeMethod(EnemyData enemy);

    public delegate void SelectCharacterMethod(int characterId);

    public delegate void AllySubscribeMethod(MinionEntity ally);

    public delegate void ClickButtonMethod(TappedObjectRecord clickedObjData);

    public delegate void DamageInfoMethod(SharedStrikeDamage damageInfo);

    public delegate void EventTriggerMethod(SignalTriggerRecord data);

    public static GameSignalCenter Instance
	{
		get
		{
			if (GameSignalCenter._instance == null)
			{
				GameSignalCenter._instance = new GameSignalCenter();
				GameSignalCenter._instance.Initialization();
			}
			return GameSignalCenter._instance;
		}
	}

	public void Initialization()
	{
		string[] names = Enum.GetNames(typeof(GameSignalKind));
		for (int i = 0; i < names.Length; i++)
		{
			GameSignalKind gameEventType = (GameSignalKind)Enum.Parse(typeof(GameSignalKind), names[i]);
			gameEventList.Add(gameEventType);
			subscriptions.Add(gameEventType, new List<IListenerRecord>());
		}
	}

	public void Subscribe(GameSignalKind gameEvent, IListenerRecord data)
	{
		if (data.GetType() != eventTypeToSubcriberType[(int)((int)gameEvent / 1000)])
		{
			return;
		}
		subscriptions[gameEvent].Add(data);
	}

	public void UnsubscribeAll()
	{
		UnityEngine.Debug.Log("__Unsubscribe all events");
		for (int i = 0; i < gameEventList.Count; i++)
		{
			subscriptions[gameEventList[i]].Clear();
		}
	}

	public void UnsubscribeIngameEvent()
	{
		UnityEngine.Debug.Log("___ unsubscribe all ingame events");
		for (int i = 0; i < ingameEvents.Count; i++)
		{
			subscriptions[ingameEvents[i]].Clear();
		}
	}

	public void UnsubscribeEventQuestEvent()
	{
		for (int i = 0; i < eventTriggerEvents.Count; i++)
		{
			subscriptions[eventTriggerEvents[i]].Clear();
		}
	}

	public void Unsubscribe(int subscriberId, GameSignalKind gameEvent)
	{
		List<IListenerRecord> list = subscriptions[gameEvent];
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i].subscriberId == subscriberId)
			{
				list.RemoveAt(i);
			}
		}
	}

	public void Unsubscribe(int subscriberId)
	{
		for (int i = gameEventList.Count - 1; i >= 0; i--)
		{
			Unsubscribe(subscriberId, gameEventList[i]);
		}
	}

	public void Unsubscribe(GameSignalKind gameEvent)
	{
		subscriptions[gameEvent].Clear();
	}

	public void Trigger(GameSignalKind gameEvent, object gameEventData)
	{
		List<IListenerRecord> list = subscriptions[gameEvent];
		for (int i = list.Count - 1; i >= 0; i--)
		{
			list[i].OnEventTrigger(gameEventData);
		}
	}

	private Dictionary<int, Type> eventTypeToSubcriberType = new Dictionary<int, Type>
	{
		{
			0,
			typeof(SimpleListenerRecord)
		},
		{
			1,
			typeof(EnemyEventData)
		},
		{
			2,
			typeof(SelectPersonaListenerRecord)
		},
		{
			3,
			typeof(MinionListenerRecord)
		},
		{
			4,
			typeof(TapSwitchListenerRecord)
		},
		{
			5,
			typeof(DamageDetailListenerRecord)
		},
		{
			6,
			typeof(SignalTriggerListenerRecord)
		}
	};

	private List<GameSignalKind> gameEventList = new List<GameSignalKind>();

	private Dictionary<GameSignalKind, List<IListenerRecord>> subscriptions = new Dictionary<GameSignalKind, List<IListenerRecord>>();

	private static GameSignalCenter _instance;

	[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStatics()
	{
		_instance = null;
	}
	private List<GameSignalKind> ingameEvents = new List<GameSignalKind>
	{
		GameSignalKind.OnTournamentMapRuleReceived,
		GameSignalKind.OnTournamentPriceConstantsReceived,
		GameSignalKind.OnEnemyMoveToEndPoint,
		GameSignalKind.OnSelectAlly,
		GameSignalKind.OnSelectEnemy,
		GameSignalKind.OnSelectHero,
		GameSignalKind.OnSelectPet,
		GameSignalKind.OnClickButton,
		GameSignalKind.OnAfterCalculateMagicDamage,
		GameSignalKind.OnAfterCalculatePhysicsDamage,
		GameSignalKind.OnBeforeCalculatePhysicsDamage,
		GameSignalKind.OnCompletePurchase
	};

	private List<GameSignalKind> eventTriggerEvents = new List<GameSignalKind>
	{
		GameSignalKind.EventKillMonster,
		GameSignalKind.EventCampaign,
		GameSignalKind.EventUseItem,
		GameSignalKind.EventUseHero,
		GameSignalKind.EventPlayTournament,
		GameSignalKind.EventInviteFriend,
		GameSignalKind.EventEarnGold,
		GameSignalKind.EventUseGem
	};
}
