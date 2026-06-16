using System;
using System.Collections.Generic;

namespace Parameter
{
	public class AlertSynopsis : Singleton<AlertSynopsis>
	{
		private bool CheckId(int tipID)
		{
			return tipID >= 0 && tipID < listNoti.Count;
		}

		public void ClearData()
		{
			listNoti.Clear();
		}

		public void SetNotiParameter(Alert noti)
		{
			int count = listNoti.Count;
			if (count <= noti.notiID)
			{
				listNoti.Add(noti);
			}
		}

		public string GetNotiContent(int notiID)
		{
			if (notiID < listNoti.Count && notiID >= 0)
			{
				return listNoti[notiID].notiContent;
			}
			return "--";
		}

		private List<Alert> listNoti = new List<Alert>();
	}
}
