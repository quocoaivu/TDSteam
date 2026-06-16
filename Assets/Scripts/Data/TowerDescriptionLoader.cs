using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class TowerDescriptionLoader : CommonDescriptionLoader
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			ReadTowerParameter();
			ReadTowerSkillParameter();
		}

		private void ReadTowerSkillParameter()
		{
			string filePath = "Parameters/Description/tower_skill_description_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<TurretSynopsis>.Instance.ClearTowerSkillData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					TurretAbilityBrief towerSkillParameter = default(TurretAbilityBrief);
					towerSkillParameter.id = (int)list[i]["id"];
					towerSkillParameter.level = (int)list[i]["level"];
					towerSkillParameter.ultimateBranch = (int)list[i]["ultimate_id"];
					towerSkillParameter.skillID = (int)list[i]["skill_id"];
					towerSkillParameter.name = (string)list[i]["name"];
					// CSV dÃ¹ng '$' lÃ m kÃ½ tá»± xuá»‘ng dÃ²ng trong pháº§n mÃ´ táº£.
					towerSkillParameter.ultimateName = ((string)list[i]["ultimate_name"]).Replace('$', '\n');
					towerSkillParameter.ultimateDescription = ((string)list[i]["ultimate_description"]).Replace('$', '\n');
					Singleton<TurretSynopsis>.Instance.SetTowerSkillParameter(towerSkillParameter);
				}
			}
			catch (Exception)
			{
				TowerDescriptionLoader.ShowError(filePath);
				throw;
			}
		}

		private void ReadTowerParameter()
		{
			string filePath = "Parameters/Description/tower_description_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<TurretSynopsis>.Instance.ClearTowerData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					TurretBrief towerParameter = default(TurretBrief);
					towerParameter.id = (int)list[i]["id"];
					towerParameter.name = (string)list[i]["name"];
					towerParameter.type = (string)list[i]["type"];
					towerParameter.level = (int)list[i]["level"];
					// CSV dÃ¹ng '$' lÃ m kÃ½ tá»± xuá»‘ng dÃ²ng trong pháº§n mÃ´ táº£.
					towerParameter.shortDescription = ((string)list[i]["short_description"]).Replace('$', '\n');
					towerParameter.fullDescription = ((string)list[i]["description"]).Replace('$', '\n');
					towerParameter.unlockDescription = ((string)list[i]["unlock_description"]).Replace('$', '\n');
					Singleton<TurretSynopsis>.Instance.SetTowerParameter(towerParameter);
				}
			}
			catch (Exception)
			{
				TowerDescriptionLoader.ShowError(filePath);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
