using System;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Data
{
	public class UnlockHeroLoader : MonoBehaviour
	{
		private void Awake()
		{
			ReadHeroUnlockParameter();
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}

		private void ReadHeroUnlockParameter()
		{
			string text = "Parameters/hero_unlock_parameter";
			try
			{
				List<Dictionary<string, object>> list = CSVLoader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					HeroUnlockSpecs heroUnlockParamParameter = default(HeroUnlockSpecs);
					heroUnlockParamParameter.id = (int)list[i]["id"];
					heroUnlockParamParameter.name = (string)list[i]["name"];
					heroUnlockParamParameter.isUnlockByPlay = (int)list[i]["is_unlock_by_play"];
					heroUnlockParamParameter.mapIDToUnlock = (int)list[i]["unlock_at_map"];
					heroUnlockParamParameter.isUnlockByGem = (int)list[i]["is_unlock_by_gem"];
					heroUnlockParamParameter.gemAmountToUnlock = (int)list[i]["gem_unlock_amount"];
					heroUnlockParamParameter.isUnlockByRealMoney = (int)list[i]["is_unlock_by_real_money"];
					Singleton<UnlockHeroSpec>.Instance.SetHeroUnlockParamParameter(heroUnlockParamParameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}
	}
}
