using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Data
{
	public class NotificationsDescriptionLoader : CommonDescriptionLoader
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string filePath = "Parameters/Description/notification_" + Setup.Instance.LanguageID;
			try
			{
				Singleton<AlertSynopsis>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVLoader.Read(filePath);
				for (int i = 0; i < list.Count; i++)
				{
					Alert notiParameter = default(Alert);
					notiParameter.notiID = (int)list[i]["noti_id"];
					notiParameter.notiContent = (string)list[i]["noti_content"];
					Singleton<AlertSynopsis>.Instance.SetNotiParameter(notiParameter);
				}
			}
			catch (Exception)
			{
				NotificationsDescriptionLoader.ShowError(filePath);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv khÃ´ng tá»“n táº¡i hoáº·c dá»¯ liá»‡u trong file khÃ´ng Ä‘Ãºng Ä‘á»‹nh dáº¡ng.");
		}
	}
}
