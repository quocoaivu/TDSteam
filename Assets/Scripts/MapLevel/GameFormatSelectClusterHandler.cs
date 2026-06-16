using System;
using Data;
using UnityEngine;

namespace MapLevel
{
	public class GameFormatSelectClusterHandler : MonoBehaviour
	{
		public void InitDefault()
		{
			ChooseDefault();
		}

		private void ChooseDefault()
		{
			int lastMapModeChoose = MapProgressStore.Instance.GetLastMapModeChoose();
			if (lastMapModeChoose == 0)
			{
				listSelectModeButtons[1].OnClick();
			}
			else
			{
				listSelectModeButtons[lastMapModeChoose - 1].OnClick();
			}
		}

		public void ShowSelectedImage(BattleDifficulty battleDifficulty)
		{
			selectedImage.transform.SetParent(selectedImageHolder[(int)battleDifficulty]);
			selectedImage.transform.localPosition = Vector3.zero;
		}

		[SerializeField]
		private FormatSelectSwitchHandler[] listSelectModeButtons;

		[SerializeField]
		private Transform[] selectedImageHolder;

		[SerializeField]
		private GameObject selectedImage;
	}
}
