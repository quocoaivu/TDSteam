using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Store
{
	public class TabsClusterHandler : MonoBehaviour
	{
		[SerializeField]
		private Transform[] listTabs;

		[SerializeField]
		[FormerlySerializedAs("buttonsSelecTab")]
		private SelectTabSwitchHandler[] selectTabButtons;

		public void InitSelectedTab(int tabID)
		{
			if (listTabs[tabID])
			{
				listTabs[tabID].SetAsLastSibling();
			}
		}

		public void HighlightButton(int tabID)
		{
			foreach (SelectTabSwitchHandler selectTabButtonController in selectTabButtons)
			{
				if (selectTabButtonController.TabID == tabID)
				{
					selectTabButtonController.ViewHighlight();
				}
				else
				{
					selectTabButtonController.ViewNormal();
				}
			}
		}
	}
}
