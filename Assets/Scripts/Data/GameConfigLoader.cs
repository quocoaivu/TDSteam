using System;
using System.Collections.Generic;
using MetaGame;
using UnityEngine;

namespace Data
{
	public class GameConfigLoader : MonoBehaviour
	{
		private void Awake()
		{
			ReadGameConfig();
		}

		private void ReadGameConfig()
		{
			string text = "Parameters/game_config";
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					Setup.Instance.LineCount = (int)list[i]["line_count"];
					Setup.Instance.EarlyCallMoney = (int)list[i]["early_call_money"];
					Setup.Instance.LifePercent2Star = (int)list[i]["life_2_star"];
					Setup.Instance.LifePercent3Star = (int)list[i]["life_3_star"];
					Setup.Instance.FirstTimeGemTakenPercentage = (int)list[i]["gem_taken_first_time"];
					Setup.Instance.SecondTimeGemTakenPercentage = (int)list[i]["gem_taken_second_time"];
					Setup.Instance.ThirdTimeGemTakenPercentage = (int)list[i]["gem_taken_third_time"];
				}
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogError("File " + text + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
				throw;
			}
		}
	}
}
