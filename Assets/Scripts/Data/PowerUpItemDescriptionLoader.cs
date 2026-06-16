using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class PowerUpItemDescriptionLoader : CommonDescriptionLoader
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string filePath = "Parameters/Description/powerup_item_description_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<PowerUpItemDescription>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					PowerUpItemDes powerUpItemParameter = default(PowerUpItemDes);
					powerUpItemParameter.id = (int)list[i]["item_id"];
					powerUpItemParameter.name = (string)list[i]["item_name"];
					// CSV dÃ¹ng '$' lÃ m kÃ½ tá»± xuá»‘ng dÃ²ng trong pháº§n mÃ´ táº£.
					powerUpItemParameter.description = ((string)list[i]["item_description"]).Replace('$', '\n');
					Singleton<PowerUpItemDescription>.Instance.SetPowerUpItemParameter(powerUpItemParameter);
				}
			}
			catch (Exception)
			{
				PowerUpItemDescriptionLoader.ShowError(filePath);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
