using System;
using UnityEngine;

[Serializable]
public class SignalSetupRecord
{
    [SerializeField]
    private int eventid;

    [SerializeField]
    private SignalIconKind eventicontype;

    [SerializeField]
    private int durationinday;

    [SerializeField]
    private SignalQuestKind eventquesttype;

    [SerializeField]
    private SignalTriggerKind eventtriggertype;

    [SerializeField]
    private int[] triggervalue = new int[0];

    [SerializeField]
    private CompareValueFormat comparevaluemode;

    [SerializeField]
    private int targetquantity;

    [SerializeField]
    private string eventtitlekey;

    [SerializeField]
    private PrizeKind rewardtype;

    [SerializeField]
    private int rewardid;

    [SerializeField]
    private int rewardquantity;

    [SerializeField]
    private string textextradata;

    public int Eventid
	{
		get
		{
			return eventid;
		}
		set
		{
			eventid = value;
		}
	}

	public SignalIconKind EVENTICONTYPE
	{
		get
		{
			return eventicontype;
		}
		set
		{
			eventicontype = value;
		}
	}

	public int Durationinday
	{
		get
		{
			return durationinday;
		}
		set
		{
			durationinday = value;
		}
	}

	public SignalQuestKind EVENTQUESTTYPE
	{
		get
		{
			return eventquesttype;
		}
		set
		{
			eventquesttype = value;
		}
	}

	public SignalTriggerKind EVENTTRIGGERTYPE
	{
		get
		{
			return eventtriggertype;
		}
		set
		{
			eventtriggertype = value;
		}
	}

	public int[] Triggervalue
	{
		get
		{
			return triggervalue;
		}
		set
		{
			triggervalue = value;
		}
	}

	public CompareValueFormat COMPAREVALUEMODE
	{
		get
		{
			return comparevaluemode;
		}
		set
		{
			comparevaluemode = value;
		}
	}

	public int Targetquantity
	{
		get
		{
			return targetquantity;
		}
		set
		{
			targetquantity = value;
		}
	}

	public string Eventtitlekey
	{
		get
		{
			return eventtitlekey;
		}
		set
		{
			eventtitlekey = value;
		}
	}

	public PrizeKind REWARDTYPE
	{
		get
		{
			return rewardtype;
		}
		set
		{
			rewardtype = value;
		}
	}

	public int Rewardid
	{
		get
		{
			return rewardid;
		}
		set
		{
			rewardid = value;
		}
	}

	public int Rewardquantity
	{
		get
		{
			return rewardquantity;
		}
		set
		{
			rewardquantity = value;
		}
	}

	public string Textextradata
	{
		get
		{
			return textextradata;
		}
		set
		{
			textextradata = value;
		}
	}
}
