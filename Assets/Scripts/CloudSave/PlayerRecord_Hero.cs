using System;
using System.Collections.Generic;

[Serializable]
public class PlayerRecord_Hero
{
    public List<PlayerRecord_Hero_Unique> listHeroData;

    public PlayerRecord_Hero()
	{
	}

	public PlayerRecord_Hero(List<PlayerRecord_Hero_Unique> listHeroData)
	{
		this.listHeroData = listHeroData;
	}

}
