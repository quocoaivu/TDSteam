using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class TextTipDescriptionLoader : CommonDescriptionLoader
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string filePath = "Parameters/Description/text_tip_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<TextHintSpec>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					TextHint textTipParameter = default(TextHint);
					textTipParameter.level = (int)list[i]["level"];
					textTipParameter.id = (int)list[i]["id"];
					textTipParameter.textTipContent = (string)list[i]["text_tip_content"];
					Singleton<TextHintSpec>.Instance.SetTextTipParameter(textTipParameter);
				}
			}
			catch (Exception)
			{
				TextTipDescriptionLoader.ShowError(filePath);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
