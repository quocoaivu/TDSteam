using System;

public class TourUserData
{
	public TourUserData()
	{
	}

	public TourUserData(ArenaPlayerSelfDetail userInfo)
	{
		name = userInfo.name;
		heroes = userInfo.heroesCode;
		score = userInfo.score;
		lastscore = userInfo.lastscore;
		country = userInfo.countryCode;
	}

	public int curgroupid = -1;

	public int curtier;

	public int heroes = 3;

	public int lastgroupid = -1;

	public string name;

	public int recFriendReward;

	public int recSeasonReward;

	public int score;

	public int lastscore = -1;

	public string country = "ch";

	public bool tierup;
}
