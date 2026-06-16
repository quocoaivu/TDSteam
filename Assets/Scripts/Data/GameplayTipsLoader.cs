using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class GameplayTipsLoader : CommonDescriptionLoader
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string filePath = "Parameters/Description/gameplay_tip_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<GameplayTipsSynopsis>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					GameplayHint gameplayTipParameter = default(GameplayHint);
					gameplayTipParameter.id = (int)list[i]["tip_id"];
					gameplayTipParameter.name = (string)list[i]["tip_name"];
					// CSV dÃ¹ng '$' lÃ m kÃ½ tá»± xuá»‘ng dÃ²ng trong pháº§n mÃ´ táº£.
					gameplayTipParameter.description = ((string)list[i]["tip_description"]).Replace('$', '\n');
					Singleton<GameplayTipsSynopsis>.Instance.SetGameplayTipParameter(gameplayTipParameter);
				}
			}
			catch (Exception)
			{
				GameplayTipsLoader.ShowError(filePath);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
