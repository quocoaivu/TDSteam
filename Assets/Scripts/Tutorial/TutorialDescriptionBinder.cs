using System;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
	public class TutorialDescriptionBinder : BaseMonoBehaviour
	{
		private void Start()
		{
			SetTutorialDescription();
		}

		private void SetTutorialDescription()
		{
			if (tutorialDescription)
			{
				tutorialDescription.text = Singleton<TutorialDescriptionLookup>.Instance.GetDescription(tutorialID).Replace('@', '\n').Replace('#', '-');
			}
			if (textMesh)
			{
				textMesh.text = Singleton<TutorialDescriptionLookup>.Instance.GetDescription(tutorialID).Replace('@', '\n').Replace('#', '-');
			}
		}

		[SerializeField]
		private string tutorialID;

		[SerializeField]
		private Text tutorialDescription;

		[SerializeField]
		private TextMesh textMesh;
	}
}
