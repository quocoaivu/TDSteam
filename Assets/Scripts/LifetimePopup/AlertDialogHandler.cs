using System;
using UnityEngine;
using UnityEngine.UI;

namespace LifetimePopup
{
	public class AlertDialogHandler : GeneralDialogHandler
	{
        [SerializeField]
        private Text notiText;

        [SerializeField]
        private GameObject buttonGroup;

        [SerializeField]
        private GameObject goToStoreButton;

        [SerializeField]
        private GameObject freeResourcesButton;

        public GameObject confirmBtnObj;

        public Text confirmText;

        private Action confirmCallback;

        private Action closeCallback;

        public void Init(string content, bool isShowButtonFreeResources, bool isShowButtonGoToStore)
		{
			Open();
			SetContent(content);
			base.transform.SetAsLastSibling();
			freeResourcesButton.SetActive(isShowButtonFreeResources);
			goToStoreButton.SetActive(isShowButtonGoToStore);
			buttonGroup.SetActive(isShowButtonFreeResources || isShowButtonGoToStore);
		}

		public void Init(string content, string confirmText, Action confirmCallback, Action closeCallback = null)
		{
			Open();
			SetContent(content);
			base.transform.SetAsLastSibling();
			freeResourcesButton.SetActive(false);
			goToStoreButton.SetActive(false);
			this.confirmCallback = confirmCallback;
			this.closeCallback = closeCallback;
			if (!string.IsNullOrEmpty(confirmText))
			{
				confirmBtnObj.SetActive(true);
				this.confirmText.text = confirmText;
			}
		}

		private void SetContent(string content)
		{
			notiText.text = content.Replace('@', '\n').Replace('#', '-');
		}

		private void ClearContent()
		{
			notiText.text = string.Empty;
			confirmBtnObj.SetActive(false);
			confirmCallback = null;
			closeCallback = null;
		}

		public void OpenGemPackTab()
		{
			base.CustomInvoke(new Action(DoOpen), Time.deltaTime);
		}

		private void DoOpen()
		{
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.InitSelectedTab(0);
			MonoSingleton<LifespanSurface>.Instance.StorePopupController.TabsGroupController.HighlightButton(0);
		}

		public void OnClickConfirmButton()
		{
			if (confirmCallback != null)
			{
				confirmCallback();
			}
			Close();
		}

		public override void Open()
		{
			base.Open();
		}

		public override void Close()
		{
			if (closeCallback != null)
			{
				closeCallback();
			}
			base.Close();
			ClearContent();
		}


	}
}
