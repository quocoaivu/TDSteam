using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AdRewardProvider : MonoBehaviour
{

    [FormerlySerializedAs("adsReward")]
    [SerializeField]
    private AdRewardConfig adRewardConfig;
    
	public int GetRewardValue(string productID)
	{
		int result = -1;
		foreach (AdRewardEntry entry in adRewardConfig.listReward)
		{
			if (entry.rewardID.Equals(productID))
			{
				result = entry.rewardValue;
			}
		}
		return result;
	}

}
