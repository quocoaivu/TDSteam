using System;
using MetaGame;
using UnityEngine;

namespace MainMenu
{
	public class TongueChooseSwitchHandler : SwitchHandler
	{
		private void OnEnable()
		{
			if (Setup.Instance.LanguageID == languageID)
			{
				selectedImage.SetActive(true);
			}
			else
			{
				selectedImage.SetActive(false);
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			Setup.Instance.LanguageID = languageID;
			MonoSingleton<UIRootHandler>.Instance.LanguageChoosePanelController.Close();
			MonoSingleton<UIRootHandler>.Instance.LanguageButtonController.UpdateButtonImage();
			ConfigRegistry.Instance.multiLanguageDataReader.ReloadParameters();
		}

		[SerializeField]
		private string languageID;

		[SerializeField]
		private GameObject selectedImage;
	}
}
