using System;
using System.Collections.Generic;

namespace Parameter
{
	public class GameplayTipsSynopsis : Singleton<GameplayTipsSynopsis>
	{
		private bool CheckId(int tipID)
		{
			return tipID >= 0 && tipID < listTip.Count;
		}

		public void ClearData()
		{
			listTip.Clear();
		}

		public void SetGameplayTipParameter(GameplayHint tip)
		{
			int count = listTip.Count;
			if (count <= tip.id)
			{
				listTip.Add(tip);
			}
		}

		public string GetName(int tipID)
		{
			if (tipID < listTip.Count && tipID >= 0)
			{
				return listTip[tipID].name;
			}
			return "--";
		}

		public string GetDescription(int tipID)
		{
			if (tipID < listTip.Count && tipID >= 0)
			{
				return listTip[tipID].description;
			}
			return "--";
		}

		private List<GameplayHint> listTip = new List<GameplayHint>();
	}
}
