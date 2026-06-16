using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UnlockTheme
{
	public class UnlockCriterionBody : MonoBehaviour
	{
        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Toggle unlockStateToggle;


        public void InitContent(int conditionType, int themeID, bool isPassDescription)
		{
			Show();
			descriptionText.text = ThemeStore.Instance.GetDescription(conditionType, themeID);
			unlockStateToggle.isOn = isPassDescription;
		}

		private void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}
	}
}
