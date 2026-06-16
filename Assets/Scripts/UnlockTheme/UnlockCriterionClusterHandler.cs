using System;
using UnityEngine;

namespace UnlockTheme
{
	public class UnlockCriterionClusterHandler : MonoBehaviour
	{
        [SerializeField]
        private UnlockCriterionBody[] listUnlockConditionContents;

        public void InitConditionContent(int index, int conditionType, int themeID, bool isPassedCondition)
		{
			listUnlockConditionContents[index].InitContent(conditionType, themeID, isPassedCondition);
		}

		public void HideAll()
		{
			foreach (UnlockCriterionBody unlockConditionContent in listUnlockConditionContents)
			{
				unlockConditionContent.Hide();
			}
		}
	}
}
