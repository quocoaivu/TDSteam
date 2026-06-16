using System;
using System.Collections;
using GameCore;
using UnityEngine;

namespace MainMenu
{
	public class MainMenuButtonsClusterHandler : BaseMonoBehaviour
	{
        [SerializeField]
        private GameObject[] listButtons;

        [SerializeField]
        private float delayTimeToStart;

        [SerializeField]
        private float delayTimeBetweenObjects;

        private WaitForSeconds waitToStart;

        private WaitForSeconds waitDelay;

        private void Start()
		{
			waitToStart = new WaitForSeconds(delayTimeToStart);
			waitDelay = new WaitForSeconds(delayTimeBetweenObjects);
			base.StartCoroutine(ShowGroupButtons());
		}

		private IEnumerator ShowGroupButtons()
		{
			yield return waitToStart;
			for (int i = 0; i < listButtons.Length; i++)
			{
				listButtons[i].SetActive(true);
				yield return waitDelay;
			}
			yield break;
		}
	}
}
