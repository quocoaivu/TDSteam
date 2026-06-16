using System;
using MetaGame;
using UnityEngine;

namespace MainMenu
{
	public class PolicyDialogHandler : MonoBehaviour
	{
		public void Init()
		{
			Open();
			InitPolicyContent();
		}

		private void InitPolicyContent()
		{
			string languageID = Setup.Instance.LanguageID;
			policy_general.SetActive(false);
			policy_korean.SetActive(false);
			policy_japanese.SetActive(false);
			if (languageID.Equals("lg_korean"))
			{
				policy_korean.SetActive(true);
			}
			else if (languageID.Equals("lg_japanese"))
			{
				policy_japanese.SetActive(true);
			}
			else
			{
				policy_general.SetActive(true);
			}
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

		[SerializeField]
		private GameObject policy_general;

		[SerializeField]
		private GameObject policy_korean;

		[SerializeField]
		private GameObject policy_japanese;
	}
}
