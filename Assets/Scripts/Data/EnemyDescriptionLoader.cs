using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class EnemyDescriptionLoader : CommonDescriptionLoader
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string filePath = "Parameters/Description/enemy_description_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<EnemyBioConfig>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					EnemyLocalizationData enemyDescription = default(EnemyLocalizationData);
					enemyDescription.id = (int)list[i]["id"];
					enemyDescription.name = (string)list[i]["name"];
					enemyDescription.level = (int)list[i]["level"];
					enemyDescription.description = (string)list[i]["description"];
					enemyDescription.specialAbility = (string)list[i]["special_ability"];
					Singleton<EnemyBioConfig>.Instance.SetEnemyDescription(enemyDescription);
				}
			}
			catch (Exception)
			{
				EnemyDescriptionLoader.ShowError(filePath);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
