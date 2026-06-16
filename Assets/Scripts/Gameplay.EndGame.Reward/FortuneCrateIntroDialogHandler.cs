using System;
using Parameter;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class FortuneCrateIntroDialogHandler : MonoBehaviour
	{
		public void Init()
		{
			Open();
			SetContent();
		}

		private void SetContent()
		{
			if (currentStep <= 2)
			{
				description.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(descriptionsKey[currentStep]).Replace('@', '\n').Replace('#', '-');
				foreach (GameObject gameObject in turns)
				{
					gameObject.SetActive(false);
				}
				turns[currentStep].SetActive(true);
			}
		}

		public void OnClickButtonNext()
		{
			currentStep++;
			SetContent();
			if (currentStep >= 3)
			{
				OnShowDescriptionComplete.Dispatch();
			}
		}

		private void Open()
		{
			base.gameObject.SetActive(true);
		}

		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private OrderedUnityEvent OnShowDescriptionComplete;

		[SerializeField]
		private int[] descriptionsKey;

		[SerializeField]
		private Text description;

		[SerializeField]
		private GameObject[] turns;

		private int currentStep;
	}
}
