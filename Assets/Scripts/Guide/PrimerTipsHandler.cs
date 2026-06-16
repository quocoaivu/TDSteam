using System;
using System.Collections.Generic;
using UnityEngine;

namespace Guide
{
	public class PrimerTipsHandler : GeneralDialogHandler
	{
		public void Init()
		{
			Open();
			base.CustomInvoke(new Action(InitDefaultData), Time.deltaTime);
		}

		public void RefreshTipInformation()
		{
			tipInformationController.InitInformation(currentTipIDSelected);
		}

		private void InitDefaultData()
		{
			if (listSelectTips.Count > 0)
			{
				listSelectTips[0].OnClick();
			}
		}

		public override void Open()
		{
			base.Open();
		}

		public override void Close()
		{
			base.Close();
		}

		[NonSerialized]
		public int currentTipIDSelected;

		[SerializeField]
		private List<SelectHintSwitchHandler> listSelectTips = new List<SelectHintSwitchHandler>();

		[SerializeField]
		private HintOverviewHandler tipInformationController;
	}
}
