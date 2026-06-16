using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaAllPrizeLeagueEntry : MonoBehaviour
{
	public void Init(int leagueIndex)
	{
		leagueTitleText.text = GameKit.GetLocalization("LEAGUE_" + leagueIndex);
		for (int i = leagueIcons.Count - 1; i >= 0; i--)
		{
			leagueIcons[i].SetActive(false);
		}
		leagueIcons[leagueIndex].SetActive(true);
		List<ArenaPrizeSetupRecord> leagueAllPrize = GameKit.GetLeagueAllPrize(leagueIndex);
		int count = leagueAllPrize.Count;
		for (int j = 0; j < count; j++)
		{
			prizeBlocks[count - j - 1].Init(leagueAllPrize[j].Rankrangeupper, leagueAllPrize[j].Rankrangelower, GameKit.GetTournamentRewardList(leagueAllPrize[j]));
		}
	}

	public Text leagueTitleText;

	public List<GameObject> leagueIcons;

	public List<ArenaRankPrizeDirector> prizeBlocks;
}
