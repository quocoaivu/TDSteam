using System;
using UnityEngine;

namespace MainMenu
{
	public class AskToExitDialogHandler : MonoBehaviour
	{
		public void Init()
		{
			Open();
		}

		public void QuitGame()
		{
			Application.Quit();
		}

		public void Open()
		{
			base.gameObject.SetActive(true);
			isOpen = true;
		}

		public void Close()
		{
			base.gameObject.SetActive(false);
			isOpen = false;
		}

		[HideInInspector]
		public bool isOpen;
	}
}
