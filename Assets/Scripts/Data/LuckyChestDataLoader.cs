using System;
using System.Collections.Generic;
using Gameplay;
using GameCore;
using Parameter;
using UnityEngine;

namespace Data
{
	public class LuckyChestDataLoader : BaseMonoBehaviour
	{

        [SerializeField]
        private FreeCrateDeal freeChestOffer;

        private void Start()
		{
			ReadParameter(MonoSingleton<GameRecord>.Instance.MapID);
		}

		public void ReadParameter(int mapID)
		{
			string text = "Parameters/LuckyChest/lucky_chest_param_map_" + mapID;
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					FortuneCrate parameter = default(FortuneCrate);
					parameter.id = (int)list[i]["id"];
					parameter.name = (string)list[i]["name"];
					parameter.turn = (int)list[i]["turn"];
					parameter.rate = (int)list[i]["rate_scale"];
					parameter.value = (int)list[i]["value"];
					parameter.isPreview = (int)list[i]["preview"];
					FortuneCrateSpec.Instance.SetParameter(parameter);
				}
			}
			catch (Exception)
			{
				LuckyChestDataLoader.ShowError(text);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}

		public int GetTurnAmountToBuyEach()
		{
			return freeChestOffer.param.turnAmountToBuyEach;
		}

		public int GetGemAmountToBuyTurn()
		{
			int[] gemCosts = freeChestOffer.param.gemAmountToBuyTurn;
			if (gemCosts == null || gemCosts.Length == 0)
			{
				return 0;
			}
			int num = 2 - MonoSingleton<GameRecord>.Instance.CurrentOpenChestOffer;
			num = Mathf.Clamp(num, 0, gemCosts.Length - 1);
			return gemCosts[num];
		}
	}
}
