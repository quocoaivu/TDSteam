using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class UnlockCountClusterHandler : MonoBehaviour
	{
		private void Awake()
		{
			MonoSingleton<GameRecord>.Instance.OnOpenChestTurnChange += Instance_OnOpenChestTurnChange;
		}

		private void Instance_OnOpenChestTurnChange()
		{
			UpdateListCountObject();
		}

		public void UpdateListCountObject()
		{
			int currentOpenChestTurn = MonoSingleton<GameRecord>.Instance.CurrentOpenChestTurn;
			for (int i = 0; i < listCountObject.Count; i++)
			{
				if (i <= currentOpenChestTurn - 1)
				{
					listCountObject[i].color = Color.white;
				}
				else
				{
					listCountObject[i].color = Color.gray;
				}
			}
		}

		[SerializeField]
		private List<Image> listCountObject = new List<Image>();
	}
}
