using System;
using UnityEngine;

[Serializable]
public class DailyPrizeSetupRecord
{
    [SerializeField]
    private int id;

    [SerializeField]
    private PrizeKind rewardtype;

    [SerializeField]
    private int rewardid;

    [SerializeField]
    private int rewardquantity;

    [SerializeField]
    private BonusKind bonustype;

    [SerializeField]
    private int bonusid;

    [SerializeField]
    private int bonusquantity;

    public int Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
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

	public BonusKind BONUSTYPE
	{
		get
		{
			return bonustype;
		}
		set
		{
			bonustype = value;
		}
	}

	public int Bonusid
	{
		get
		{
			return bonusid;
		}
		set
		{
			bonusid = value;
		}
	}

	public int Bonusquantity
	{
		get
		{
			return bonusquantity;
		}
		set
		{
			bonusquantity = value;
		}
	}
}
