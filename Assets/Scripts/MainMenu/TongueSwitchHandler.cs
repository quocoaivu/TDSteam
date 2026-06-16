using System;
using MetaGame;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
	public class TongueSwitchHandler : SwitchHandler
	{
		private void Awake()
		{
			imageButton = base.GetComponent<Image>();
		}

		private void Start()
		{
			UpdateButtonImage();
		}

		public void UpdateButtonImage()
		{
			imageButton.sprite = Common.AssetLoader.Load<Sprite>(string.Format("CountryFlags/flag_{0}", Setup.Instance.LanguageID));
		}

		public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<UIRootHandler>.Instance.LanguageChoosePanelController.Init();
		}

		private Image imageButton;
	}
}
