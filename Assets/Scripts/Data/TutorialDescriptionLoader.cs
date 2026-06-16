using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class TutorialDescriptionLoader : CommonDescriptionLoader
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string filePath = "Parameters/Description/tutorial_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<TutorialDescriptionLookup>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					TutorialDescriptionEntry tutParameter = default(TutorialDescriptionEntry);
					tutParameter.id = (string)list[i]["tut_id"];
					// CSV dÃ¹ng '$' lÃ m kÃ½ tá»± xuá»‘ng dÃ²ng trong pháº§n mÃ´ táº£.
					tutParameter.description = ((string)list[i]["tut_description"]).Replace('$', '\n');
					Singleton<TutorialDescriptionLookup>.Instance.SetTutParameter(tutParameter);
				}
			}
			catch (Exception)
			{
				TutorialDescriptionLoader.ShowError(filePath);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
