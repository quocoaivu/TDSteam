using System;
using UnityEngine;

namespace Tutorial
{
	public class TutorialBuildTowerView : MonoBehaviour
	{
		public void Show()
		{
			tutorialContent.SetActive(true);
		}

		public void Hide()
		{
			tutorialContent.SetActive(false);
		}

		[SerializeField]
		private GameObject tutorialContent;
	}
}
