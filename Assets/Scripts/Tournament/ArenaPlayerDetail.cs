using System;
using System.Collections.Generic;

public class ArenaPlayerDetail
{
	public ArenaPlayerDetail(string name, List<int> heroIds, TimeSpan time, bool isYou, string country)
	{
		this.name = name;
		this.heroIds = heroIds;
		this.time = time;
		this.isYou = isYou;
		countryCode = country;
	}

	public ArenaPlayerDetail(KeyValuePair<string, TourSeasonGroupMember> entry, string uid)
	{
		name = entry.Value.name;
		heroIds = GameKit.DecodeHeroList(entry.Value.heroes);
		time = new TimeSpan(0, 0, 0, 0, entry.Value.score);
		isYou = (entry.Key == uid);
		countryCode = entry.Value.country;
	}

	public int rank;

	public string name;

	public List<int> heroIds;

	public TimeSpan time;

	public bool isYou;

	public string countryCode = "gb";
}
