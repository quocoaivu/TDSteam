using System;
using Data;
using Gameplay;
using LifetimePopup;
using Parameter;
using UnityEngine;

namespace UserProfile
{
	public class ConfirmRenameDialogHandler : GameplayDialogHandler
	{
        [SerializeField]
        private ChangeTagDialogHandler changeNamePopupController;

        public void Init()
		{
			OpenWithScaleAnimation();
		}

		public void ConfirmRename()
		{
			int renameCost = UserProfileStore.Instance.GetRenameCost();
			int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
			if (currentGem >= renameCost)
			{
				changeNamePopupController.Rename();
				PlayerCurrencyStore.Instance.ChangeGem(-renameCost, true);
				CloseWithScaleAnimation();
			}
			else
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(20);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, true, true);
			}
		}
	}
}
