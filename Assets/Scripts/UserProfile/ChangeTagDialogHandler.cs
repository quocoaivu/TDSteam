using System;
using Data;
using Gameplay;
using LifetimePopup;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class ChangeTagDialogHandler : GameplayDialogHandler
	{

        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private ConfirmRenameDialogHandler confirmRenamePopupController;
        
		public void Init()
		{
			GameUtils.ClearInputField(inputField);
			OpenWithScaleAnimation();
		}

		public void TryConfirmChangeName()
		{
			int renameCount = UserProfileStore.Instance.GetRenameCount();
			if (CheckIfInputFieldEmpty())
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(145);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
				return;
			}
			if (renameCount == 0)
			{
				Rename();
			}
			else
			{
				confirmRenamePopupController.Init();
			}
		}

		public void Rename()
		{
			UserProfileStore.Instance.SetUserName(inputField.text);
			UserProfileStore.Instance.IncreaseRenameCount();
			CloseWithScaleAnimation();
		}

		private bool CheckIfInputFieldEmpty()
		{
			bool result = false;
			string text = inputField.text;
			if (text.Length == 0)
			{
				result = true;
			}
			return result;
		}
	}
}
