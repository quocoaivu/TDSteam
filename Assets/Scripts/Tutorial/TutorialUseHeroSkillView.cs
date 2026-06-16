using System;
using UnityEngine;

namespace Tutorial
{
	public class TutorialUseHeroSkillView : MonoBehaviour
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
