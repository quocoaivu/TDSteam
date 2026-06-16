using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class HeroDescriptionLoader : CommonDescriptionLoader
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string filePath = "Parameters/Description/hero_description_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<HeroSynopsis>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					HeroBrief heroParameter = default(HeroBrief);
					heroParameter.id = (int)list[i]["id"];
					heroParameter.name = (string)list[i]["name"];
					heroParameter.skillID = (int)list[i]["skill_id"];
					// CSV dÃ¹ng '$' lÃ m kÃ½ tá»± xuá»‘ng dÃ²ng trong pháº§n mÃ´ táº£.
					heroParameter.shortDescription = ((string)list[i]["short_description"]).Replace('$', '\n');
					heroParameter.fullDescription = ((string)list[i]["full_description"]).Replace('$', '\n');
					heroParameter.skillName = ((string)list[i]["skill_name"]).Replace('$', '\n');
					heroParameter.skillType = ((string)list[i]["skill_type"]).Replace('$', '\n');
					heroParameter.skillDescription = ((string)list[i]["skill_description"]).Replace('$', '\n');
					heroParameter.skillUnlock = ((string)list[i]["skill_unlock"]).Replace('$', '\n');
					Singleton<HeroSynopsis>.Instance.SetHeroParameter(heroParameter);
				}
			}
			catch (Exception)
			{
				HeroDescriptionLoader.ShowError(filePath);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
