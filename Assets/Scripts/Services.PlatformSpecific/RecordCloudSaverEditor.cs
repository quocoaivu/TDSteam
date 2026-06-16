using System;
using System.Collections.Generic;
using System.Diagnostics;
using LifetimePopup;
using Parameter;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class RecordCloudSaverEditor : MonoBehaviour, IRecordCloudSaver
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnDataBackupCompletedEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnDataRestoreCompletedEvent;

		public void AutoBackUpData()
		{
			UnityEngine.Debug.Log("Auto Backup data!");
		}

		public void BackupData()
		{
			UnityEngine.Debug.Log("Backup data!");
			string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(132);
			MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			if (OnDataBackupCompletedEvent != null)
			{
				OnDataBackupCompletedEvent();
			}
		}

		public void ClaimPremiumUserInfor(string userID, string userName, string userEmail, string userPhoneNumber)
		{
			throw new NotImplementedException();
		}

		public void RestoreData()
		{
			UnityEngine.Debug.Log("Restore data!");
			string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(135);
			MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			if (OnDataRestoreCompletedEvent != null)
			{
				OnDataRestoreCompletedEvent();
			}
		}

		public void RetrieveData(string dbRef, Action<IRecordSnapshot> callback)
		{
			throw new NotImplementedException();
		}

		public void RetrieveDataWithMainThreadCallback(string dbRef, Action<IRecordSnapshot> callback)
		{
			callback(new RecordSnapshotEditor());
		}

		public void UpdateData(Dictionary<string, object> updateList, string dbRefPath = null)
		{
		}

		public void WriteData(object data, string dbRefPath)
		{
		}

		public void WriteDataWithMainThreadCallback(object data, string dbRefPath, Action<IRecordSnapshot> callback)
		{
		}

		public void WriteGroupInfoTransaction(string groupInfoPath, bool isUserPremium, int tier = -1)
		{
		}

		public void WriteNewGroupInfoTransaction(int newGroupId, bool isUserPremium, int tier = -1)
		{
		}
	}
}
