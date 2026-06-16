using System;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class SelectHintSwitchHandler : SwitchHandler
	{
		private void Start()
		{
			SetTipName();
		}

		private void SetTipName()
		{
			tipName.text = Singleton<GameplayTipsSynopsis>.Instance.GetName(tipID);
		}

		public override void OnClick()
		{
			base.OnClick();
			PrimerDialogHandler.Instance.GuideTipsController.currentTipIDSelected = tipID;
			PrimerDialogHandler.Instance.GuideTipsController.RefreshTipInformation();
		}

		[SerializeField]
		private int tipID;

		[SerializeField]
		private Text tipName;
	}
}
