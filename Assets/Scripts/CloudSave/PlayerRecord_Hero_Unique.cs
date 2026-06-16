using System;
using System.Collections.Generic;

[Serializable]
public class PlayerRecord_Hero_Unique
{
    public int id;

    public int level;

    public int exp;

    public bool isOwned;

    public bool ownedPet;

    public List<int> skillUpgraded;

    public PlayerRecord_Hero_Unique()
	{
	}

	public PlayerRecord_Hero_Unique(int id, int exp, bool ownedPet, List<int> skillUpgraded)
	{
		this.id = id;
		this.exp = exp;
		this.ownedPet = ownedPet;
		this.skillUpgraded = skillUpgraded;
	}

}
