using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class GlobalUpgradeDescriptionLoader : CommonDescriptionLoader
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string filePath = "Parameters/Description/global_upgrade_description_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<GlobalEnhanceSynopsis>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					GlobalEnhanceBrief globalUpgradeDes = default(GlobalEnhanceBrief);
					globalUpgradeDes.id = (int)list[i]["upgrade_id"];
					globalUpgradeDes.title = (string)list[i]["upgrade_title"];
					// CSV dÃ¹ng '$' lÃ m kÃ½ tá»± xuá»‘ng dÃ²ng trong pháº§n mÃ´ táº£.
					globalUpgradeDes.description = ((string)list[i]["upgrade_description"]).Replace('$', '\n');
					Singleton<GlobalEnhanceSynopsis>.Instance.SetGlobalUpgradeParameters(globalUpgradeDes);
				}
			}
			catch (Exception)
			{
				GlobalUpgradeDescriptionLoader.ShowError(filePath);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
