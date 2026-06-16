using System;
using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
	public class SelectTabSwitchHandler : SwitchHandler
	{
		[SerializeField]
		private int tabID;

		private Image image;

		private Text text;

		[SerializeField]
		private Color textColorHighlight;

		[SerializeField]
		private Color textColorNormal;

		public int TabID
		{
			get
			{
				return tabID;
			}
			set
			{
				tabID = value;
			}
		}

		private void Awake()
		{
			image = base.GetComponent<Image>();
			text = base.GetComponentInChildren<Text>();
		}

		public override void OnClick()
		{
			base.OnClick();
			SelectTab();
		}

		private void SelectTab()
		{
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.InitSelectedTab(TabID);
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.HighlightButton(TabID);
		}

		public void ViewHighlight()
		{
			//image.color = Color.white;
			//text.color = textColorHighlight;
		}

		public void ViewNormal()
		{
			//image.color = Color.gray;
			//text.color = textColorNormal;
		}
	}
}
