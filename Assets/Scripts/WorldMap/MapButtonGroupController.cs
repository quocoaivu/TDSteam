using System.Collections.Generic;
using Data;
using UnityEngine;

namespace WorldMap
{
	public class MapButtonGroupController : MonoBehaviour
	{

        [SerializeField]
        private List<ZoneSwitchHandler> listButtons = new List<ZoneSwitchHandler>();

        public void RefreshListMapButtons(int themeID)
		{
			int mapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked();
			if (themeID != 0)
			{
				if (themeID != 1)
				{
					if (themeID == 2)
					{
						UnlockToMap(mapIDUnlocked - 12);
					}
				}
				else
				{
					UnlockToMap(mapIDUnlocked - 6);
				}
			}
			else
			{
				UnlockToMap(mapIDUnlocked);
			}
		}

		private void UnlockToMap(int currentMapIDUnlocked)
		{
			for (int i = 0; i < listButtons.Count; i++)
			{
				if (i <= currentMapIDUnlocked)
				{
					listButtons[i].ViewUnLock();
				}
				else
				{
					listButtons[i].ViewLock();
				}
			}
		}
	}
}
