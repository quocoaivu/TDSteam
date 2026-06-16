using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace Gameplay.EndGame.Reward
{
	public class CrateClusterHandler : BaseMonoBehaviour
	{
		public void AutoOpenChest()
		{
			int currentOpenChestTurn = MonoSingleton<GameRecord>.Instance.CurrentOpenChestTurn;
			for (int i = 0; i < currentOpenChestTurn; i++)
			{
				foreach (CrateItem chestItem in listChestItems)
				{
					if (!chestItem.IsOpened)
					{
						chestItem.OnClick();
						break;
					}
				}
			}
		}

		public bool isAvailableChestToOpen()
		{
			bool result = false;
			foreach (CrateItem chestItem in listChestItems)
			{
				if (!chestItem.IsOpened)
				{
					result = true;
				}
			}
			return result;
		}

		public void DisplayReward(int chestID, string rewardName, bool isDisplayRewardValue)
		{
			listChestItems[chestID].DisplayReward(rewardName, isDisplayRewardValue);
		}

		[SerializeField]
		private List<CrateItem> listChestItems = new List<CrateItem>();
	}
}
