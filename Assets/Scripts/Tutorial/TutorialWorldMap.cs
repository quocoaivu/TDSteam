using System;
using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialWorldMap : TutorialBase
	{
        private string tutorialID = TutorialStore.TUTORIAL_ID_WORLD_MAP;

        // Map_1 nằm trong theme prefab load runtime (Addressable) nên không gán qua Inspector
        // được — phải tìm theo tên. Cache lại sau lần tìm đầu (Destroy thì == null, tự tìm lại).
        private TutorialSelectSecondZone tutorialSelectSecondMap;

        protected override void SaveTutorialPassed()
		{
			TutorialStore.Instance.SetTutorialStatus(tutorialID, true);
		}

		protected override bool ShouldShowTutorial()
		{
			int mapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked();
			return !TutorialStore.Instance.GetTutorialStatus(tutorialID) && mapIDUnlocked >= 1 && mapIDUnlocked < 2;
		}

		// Đánh dấu đã qua tutorial khi người chơi đã mở map >= 2 (vượt mốc tutorial).
		// Tách khỏi ShouldShowTutorial để IsShowingTutorial() không ghi đĩa khi chỉ truy vấn.
		public void SyncPassedIfObsolete()
		{
			if (MapProgressStore.Instance.GetMapIDUnlocked() >= 2 && !TutorialStore.Instance.GetTutorialStatus(tutorialID))
			{
				SaveTutorialPassed();
			}
		}

		public void TryInvokeTutorialSelectSecondMap()
		{
			if (tutorialSelectSecondMap == null)
			{
				GameObject gameObject = GameObject.Find("Map_1");
				if (gameObject)
				{
					tutorialSelectSecondMap = gameObject.GetComponent<TutorialSelectSecondZone>();
				}
			}
			if (tutorialSelectSecondMap != null)
			{
				tutorialSelectSecondMap.CheckCondition();
			}
		}

		public bool IsShowingTutorial()
		{
			return ShouldShowTutorial();
		}
	}
}
