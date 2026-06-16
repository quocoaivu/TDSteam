using System;
using Gameplay;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class BackupRestoreConfirmPopup : GameplayDialogHandler
	{

        [SerializeField]
        private BackupRestoreConfirmButton confirmButtonController;

        [Space]
        [SerializeField]
        private Text noti;

        [SerializeField]
        private int notiIDBackup;

        [SerializeField]

        private int notiIDRestore;
        public void Init(CloudRecordInteraction cloudDataInteraction)
		{
			OpenWithScaleAnimation();
			confirmButtonController.Init(cloudDataInteraction);
			if (cloudDataInteraction != CloudRecordInteraction.Backup)
			{
				if (cloudDataInteraction == CloudRecordInteraction.Restore)
				{
					string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(notiIDRestore);
					noti.text = notiContent.Replace('@', '\n').Replace('#', '-');
				}
			}
			else
			{
				string notiContent2 = Singleton<AlertSynopsis>.Instance.GetNotiContent(notiIDBackup);
				noti.text = notiContent2.Replace('@', '\n').Replace('#', '-');
			}
		}
	}
}
