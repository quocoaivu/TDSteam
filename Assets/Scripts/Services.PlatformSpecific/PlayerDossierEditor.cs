using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class PlayerDossierEditor : MonoBehaviour, IPlayerDossier
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnLogStatusChangeEvent;

		public void LogIn_Facebook()
		{
			PlayerPrefs.SetInt(PlayerDossierEditor.USER_LOGIN_FACEBOOK, 1);
			if (OnLogStatusChangeEvent != null)
			{
				OnLogStatusChangeEvent();
			}
			UnityEngine.Debug.Log("Log in FB!");
		}

		public void LogIn_Google()
		{
			PlayerPrefs.SetInt(PlayerDossierEditor.USER_LOGIN_GOOGLE, 1);
			if (OnLogStatusChangeEvent != null)
			{
				OnLogStatusChangeEvent();
			}
			UnityEngine.Debug.Log("Log in GG!");
		}

		public void LogOut_Google()
		{
			PlayerPrefs.SetInt(PlayerDossierEditor.USER_LOGIN_GOOGLE, 0);
			if (OnLogStatusChangeEvent != null)
			{
				OnLogStatusChangeEvent();
			}
			UnityEngine.Debug.Log("Log out GG!");
		}

		public void LogOut_Facebook()
		{
			PlayerPrefs.SetInt(PlayerDossierEditor.USER_LOGIN_FACEBOOK, 0);
			if (OnLogStatusChangeEvent != null)
			{
				OnLogStatusChangeEvent();
			}
			UnityEngine.Debug.Log("Log out FB!");
		}

		public void BackupData()
		{
			NativeSpecificServicesSource.Services.DataCloudSaver.BackupData();
		}

		public void RestoreData()
		{
			NativeSpecificServicesSource.Services.DataCloudSaver.RestoreData();
		}

		public bool IsLoggedIn_Google()
		{
			return PlayerPrefs.GetInt(PlayerDossierEditor.USER_LOGIN_GOOGLE, 0) == 1;
		}

		public bool IsLoggedIn_Facebook()
		{
			return PlayerPrefs.GetInt(PlayerDossierEditor.USER_LOGIN_FACEBOOK, 0) == 1;
		}

		public string GetFirebaseUserID()
		{
			return "userIDTest";
		}

		public Sprite GetUserAvatar()
		{
			return null;
		}

		public string GetUidOfUser()
		{
			throw new NotImplementedException();
		}

		public void GetUidsOfUserFriends(Action<List<string>> callback)
		{
			throw new NotImplementedException();
		}

		public void FirebaseSignOut()
		{
			UnityEngine.Debug.Log("Sign out firebase!");
		}

		public void InviteFriend_Facebook()
		{
		}

		public void TakePremiumUserInfor()
		{
			throw new NotImplementedException();
		}

		private static string USER_LOGIN_GOOGLE = "user_login_google";

		private static string USER_LOGIN_FACEBOOK = "user_login_facebook";

		private static string USER_NAME = "user_name";
	}
}
