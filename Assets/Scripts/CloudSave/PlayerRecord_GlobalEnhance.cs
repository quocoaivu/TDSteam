using System;
using System.Collections.Generic;

[Serializable]
public class PlayerRecord_GlobalEnhance
{
	public PlayerRecord_GlobalEnhance()
	{
	}

	public PlayerRecord_GlobalEnhance(List<PlayerRecord_GlobalEnhance_Unique> listUpgradedTower)
	{
		this.listUpgradedTower = listUpgradedTower;
	}

	public List<PlayerRecord_GlobalEnhance_Unique> listUpgradedTower;
}
