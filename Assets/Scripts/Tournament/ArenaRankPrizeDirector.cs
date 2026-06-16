using System;
using System.Collections.Generic;
using RewardPopup;
using UnityEngine;
using UnityEngine.UI;

public class ArenaRankPrizeDirector : MonoBehaviour
{
	public void Init(int upperRank, int lowerRank, List<PrizeItem> rewards)
	{
		rangeOfRankText.text = string.Format("{0} {1}-{2}", GameKit.GetLocalization("RANK"), upperRank, lowerRank);
		int num = Mathf.Min(rewards.Count, rewardEntries.Count);
		for (int i = 0; i < num; i++)
		{
			rewardEntries[i].gameObject.SetActive(true);
			rewardEntries[i].Init(rewards[i]);
		}
		for (int j = num; j < rewardEntries.Count; j++)
		{
			rewardEntries[j].gameObject.SetActive(false);
		}
	}

	public Text rangeOfRankText;

	public List<RewardItemView> rewardEntries;
}
