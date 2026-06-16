using System;
using System.Collections.Generic;

[Serializable]
public class PlayerRecord_Zone
{
    public int mapIDUnlocked;

    public int lastMapIDPlayed;

    public int lastMapModeChoose;

    public List<PlayerRecord_Zone_Unique> listDataMap;

    public PlayerRecord_Zone()
	{
	}

	public PlayerRecord_Zone(int mapIDUnlocked, int lastMapIDPlayed, int lastMapModeChoose, List<PlayerRecord_Zone_Unique> listDataMap)
	{
		this.mapIDUnlocked = mapIDUnlocked;
		this.lastMapIDPlayed = lastMapIDPlayed;
		this.lastMapModeChoose = lastMapModeChoose;
		this.listDataMap = listDataMap;
	}
}
