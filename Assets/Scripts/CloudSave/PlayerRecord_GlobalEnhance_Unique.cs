using System;

[Serializable]
public class PlayerRecord_GlobalEnhance_Unique
{

    public int towerID;

    public int towerUpgradedLevel;

    public PlayerRecord_GlobalEnhance_Unique()
	{
	}

	public PlayerRecord_GlobalEnhance_Unique(int towerID, int towerUpgradedLevel)
	{
		this.towerID = towerID;
		this.towerUpgradedLevel = towerUpgradedLevel;
	}
}
