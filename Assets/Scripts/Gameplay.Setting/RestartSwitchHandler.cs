using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Setting
{
	public class RestartSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			MonoSingleton<UIRootHandler>.Instance.settingPopupController.InitConfirmGroup(CancelGameKind.Restart);
		}

		public void SetClickable()
		{
			button.enabled = true;
			image.color = Color.white;
		}

		public void SetUnClickable()
		{
			button.enabled = false;
			image.color = Color.gray;
		}

		[SerializeField]
		private Button button;

		[SerializeField]
		private Image image;
	}
}
