using System;
using System.Collections.Generic;
using UnityEngine;

public class AdRewardConfig : ScriptableObject
{
	[Header("Reward Attribute")]
	public List<AdRewardEntry> listReward = new List<AdRewardEntry>();
}
