using System;
using UnityEngine;

public class FacebookServicesRewardProvider : MonoBehaviour
{
    [SerializeField]
    private SocialServicesPrize facebookServicesReward;

    public int GetRewardAmount_Gem(string productID)
	{
		int result = -1;
		foreach (FacebookServicesRewardEntry fbsreward in facebookServicesReward.listReward)
		{
			if (fbsreward.rewardID.Equals(productID))
			{
				result = fbsreward.rewardAmount_Gem;
			}
		}
		return result;
	}
}
