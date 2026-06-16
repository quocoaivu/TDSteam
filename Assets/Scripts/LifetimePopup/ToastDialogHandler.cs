using System;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace LifetimePopup
{
	public class ToastDialogHandler : BaseMonoBehaviour
	{
        [SerializeField]
        private float popupLifeTime;

        [SerializeField]
        private Text text;

        public void Init(string value)
		{
			Open();
			text.text = value;
			base.CustomCancelInvoke(new Action(Close));
			base.CustomInvoke(new Action(Close), popupLifeTime);
		}

		private void Open()
		{
			base.gameObject.SetActive(true);
		}

		private void Close()
		{
			base.gameObject.SetActive(false);
			text.text = string.Empty;
		}
	}
}
