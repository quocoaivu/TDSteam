using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Threading.Tasks;
using Data;
//using Facebook.MiniJSON;
//using Facebook.Unity;
//using Firebase;
//using Firebase.Auth;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using LifetimePopup;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class PlayerDossierAndroid : MonoBehaviour, IPlayerDossier
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnLogStatusChangeEvent;

		public bool IsSignInAnonymous
		{
			get
			{
				//if (auth == null)
				//{
				//	return false;
				//}
				//if (auth.CurrentUser == null)
				//{
				//	return false;
				//}
				//IEnumerable<IUserInfo> providerData = auth.CurrentUser.ProviderData;
				//foreach (IUserInfo userInfo in providerData)
				//{
				//	if (!string.Equals(userInfo.ProviderId, "firebase"))
				//	{
				//		return false;
				//	}
				//}
				return true;
			}
		}

		public void Start()
		{
			TryInitializeFacebook();
			TryInitPlaygamePlatform();
		}

		private void OnDestroy()
		{
			//if (auth != null)
			//{
			//	auth.StateChanged -= AuthStateChanged;
			//	auth = null;
			//}
		}

		private void TryInitializeFacebook()
		{
			//if (!FB.IsInitialized)
			//{
			//	FB.Init(new InitDelegate(InitCallback), new HideUnityDelegate(OnHideUnity), null);
			//}
			//else
			//{
			//	FB.ActivateApp();
			//}
		}

		private void InitCallback()
		{
			//if (FB.IsInitialized)
			//{
			//	FB.ActivateApp();
			//}
			//else
			//{
			//	UnityEngine.Debug.Log("Failed to Initialize the Facebook SDK");
			//}
		}

		private void OnHideUnity(bool isGameShown)
		{
			if (!isGameShown)
			{
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}

		private void SendEvent_LogStatusChange()
		{
			if (OnLogStatusChangeEvent != null)
			{
				OnLogStatusChangeEvent();
			}
		}

		private void TryInitPlaygamePlatform()
		{
			//PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().RequestIdToken().RequestEmail().AddOauthScope("email").Build();
			//PlayGamesPlatform.InitializeInstance(configuration);
			//PlayGamesPlatform.DebugLogEnabled = true;
			//PlayGamesPlatform.Activate();
		}

		public void FirebaseInit()
		{
			//auth = FirebaseAuth.DefaultInstance;
			//UnityEngine.Debug.Log("auth setup " + auth);
			//auth.StateChanged += AuthStateChanged;
			//auth.IdTokenChanged += Auth_IdTokenChanged;
			//if (auth.CurrentUser != null)
			//{
			//	usedUserID = auth.CurrentUser.UserId;
			//	UnityEngine.Debug.Log("Init Firebase with userID" + usedUserID);
			//	isPlayingAsGuest = false;
			//}
			//else
			//{
			//	isPlayingAsGuest = true;
			//}
		}

		private void Auth_IdTokenChanged(object sender, EventArgs e)
		{
			//if (auth.CurrentUser != null)
			//{
			//	auth.CurrentUser.TokenAsync(true).ContinueWith(delegate(Task<string> task)
			//	{
			//		tokenID = task.Result;
			//	});
			//}
		}

		private void AuthStateChanged(object sender, EventArgs eventArgs)
		{
			//if (auth.CurrentUser != null)
			//{
			//	if (IsSignInAnonymous)
			//	{
			//		usedUserID = string.Empty;
			//	}
			//	isPlayingAsGuest = string.IsNullOrEmpty(usedUserID);
			//	isNewUser = (usedUserID != auth.CurrentUser.UserId);
			//	usedUserID = auth.CurrentUser.UserId;
			//	if (IsSignInAnonymous)
			//	{
			//		usedUserID = string.Empty;
			//	}
			//}
			//else
			//{
			//	usedUserID = string.Empty;
			//	isNewUser = true;
			//	isPlayingAsGuest = true;
			//	loginState = SignInPhase.None;
			//}
			//SendEvent_LogStatusChange();
		}

		private void LoginFirebaseWithGoogleCredential()
		{
			//loginState = SignInPhase.GoogleLogin;
			//if (IsAuthenticatedWithFacebook)
			//{
			//	LinkingAccount();
			//	UnityEngine.Debug.Log("Da dang nhap FB truoc do, linking!");
			//}
			//else
			//{
			//	UnityEngine.Debug.Log("Chua dang nhap FB truoc do, new user!");
			//	Credential credential = GoogleAuthProvider.GetCredential(tokenID, null);
			//	auth.SignInWithCredentialAsync(credential).ContinueWith(delegate(Task<FirebaseUser> task)
			//	{
			//		if (task.IsCompleted)
			//		{
			//			UnityEngine.Debug.Log("Log in Firebase with GG userID" + auth.CurrentUser.UserId);
			//			SaveUserID();
			//			SaveUserRegionCode();
			//			SendEvent_LogStatusChange();
			//			UnityEngine.Debug.Log("End Progress");
			//			SingletonMonoBehaviour<LifespanSurface>.Instance.LoadingProgressPopupController.Close();
			//		}
			//	});
			//}
		}

		private void LoginFirebaseWithFacebookCredential()
		{
			//loginState = SignInPhase.FacebookLogin;
			//if (IsAuthenticatedWithGoogle)
			//{
			//	LinkingAccount();
			//	UnityEngine.Debug.Log("Da dang nhap GG truoc do, linking!");
			//}
			//else
			//{
			//	UnityEngine.Debug.Log("Chua dang nhap GG truoc do, new user!");
			//	Credential credential = FacebookAuthProvider.GetCredential(tokenID);
			//	auth.SignInWithCredentialAsync(credential).ContinueWith(delegate(Task<FirebaseUser> task)
			//	{
			//		if (task.IsCompleted)
			//		{
			//			UnityEngine.Debug.Log("Log in Firebase with FB userID" + auth.CurrentUser.UserId);
			//			SaveUserID();
			//			SaveUserRegionCode();
			//			SendEvent_LogStatusChange();
			//			UnityEngine.Debug.Log("End Progress");
			//			SingletonMonoBehaviour<LifespanSurface>.Instance.LoadingProgressPopupController.Close();
			//		}
			//	});
			//}
		}

		private void SaveUserID()
		{
			//UserProfileStore.Instance.SetUserID(auth.CurrentUser.UserId);
		}

		private void SaveUserRegionCode()
		{
			string text = PreciseLingo.GetRegion();
			text = text.ToLower();
			UnityEngine.Debug.Log("Region code  = " + text);
			UserProfileStore.Instance.SetRegionCode(text);
		}

		public void FirebaseSignOut()
		{
			//if (auth != null)
			//{
			//	auth.SignOut();
			//}
			//usedUserID = string.Empty;
			//isPlayingAsGuest = true;
			//UserProfileStore.Instance.ClearUserID();
		}

		public void BackupData()
		{
			NativeSpecificServicesSource.Services.DataCloudSaver.BackupData();
		}

		public void RestoreData()
		{
			NativeSpecificServicesSource.Services.DataCloudSaver.RestoreData();
		}

		public void LogIn_Facebook()
		{
			//SingletonMonoBehaviour<LifespanSurface>.Instance.LoadingProgressPopupController.Open();
			//if (!FB.IsLoggedIn)
			//{
			//	FB.LogInWithReadPermissions(listPermissionLogin, new FacebookDelegate<ILoginResult>(LogInFacebookCallBack));
			//}
			//else
			//{
			//	DoLoginFirebaseWithFB();
			//}
		}

		//private void LogInFacebookCallBack(ILoginResult result)
		//{
		//	if (FB.IsLoggedIn)
		//	{
		//		DoLoginFirebaseWithFB();
		//	}
		//	else
		//	{
		//		UnityEngine.Debug.Log("333");
		//	}
		//}

		private void DoLoginFirebaseWithFB()
		{
			//AccessToken currentAccessToken = AccessToken.CurrentAccessToken;
			//string userId = currentAccessToken.UserId;
			//tokenID = currentAccessToken.TokenString;
			//UnityEngine.Debug.Log(string.Concat(new object[]
			//{
			//	"Sign in FB success with userID = ",
			//	userId,
			//	" token = ",
			//	currentAccessToken
			//}));
			//FB.API("/me?fields=name", HttpMethod.GET, new FacebookDelegate<IGraphResult>(GetUserNameCallback), null);
			//LoginFirebaseWithFacebookCredential();
		}

		//private void GetUserNameCallback(IResult result)
		//{
		//	if (result.Error == null)
		//	{
		//		UserProfileStore.Instance.SetUserName(result.ResultDictionary["name"].ToString());
		//	}
		//}

		public bool IsLoggedIn_Facebook()
		{
			return IsAuthenticatedWithFacebook;
		}

		public void LogOut_Facebook()
		{
			//if (IsLoggedIn_Facebook())
			//{
			//	FB.LogOut();
			//}
			//loginState = SignInPhase.None;
			//SendEvent_LogStatusChange();
		}

		public bool IsAuthenticatedWithFacebook
		{
			get
			{
				//if (auth == null)
				//{
				//	UnityEngine.Debug.Log("auth object null");
				//	return false;
				//}
				//if (auth.CurrentUser == null)
				//{
				//	return false;
				//}
				//IEnumerable<IUserInfo> providerData = auth.CurrentUser.ProviderData;
				//foreach (IUserInfo userInfo in providerData)
				//{
				//	if (string.Equals(userInfo.ProviderId, "facebook.com"))
				//	{
				//		return true;
				//	}
				//}
				return false;
			}
		}

		public string GetUidOfUser()
		{
			//if (FB.IsLoggedIn)
			//{
			//	return AccessToken.CurrentAccessToken.UserId;
			//}
			return null;
		}

		public void GetUidsOfUserFriends(Action<List<string>> callback)
		{
			//List<string> rtn = new List<string>();
			//string query = "/me/friends";
			//FB.API(query, HttpMethod.GET, delegate(IGraphResult result)
			//{
			//	Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(result.RawResult);
			//	List<object> list = (List<object>)dictionary["data"];
			//	foreach (object obj in list)
			//	{
			//		rtn.Add(((Dictionary<string, object>)obj)["id"] as string);
			//	}
			//	callback(rtn);
			//}, null);
		}

		public void InviteFriend_Facebook()
		{
			NativeSpecificServicesSource.Services.FacebookServices.InviteFriend();
		}

		public void LogIn_Google()
		{
			MonoSingleton<LifespanSurface>.Instance.LoadingProgressPopupController.Open();
			Social.localUser.Authenticate(delegate(bool success)
			{
				if (success)
				{
					string id = Social.localUser.id;
					//tokenID = PlayGamesPlatform.Instance.GetIdToken();
					UnityEngine.Debug.Log("Sign in google success with userID = " + id + " token = " + tokenID);
					LoginFirebaseWithGoogleCredential();
				}
				else
				{
					UnityEngine.Debug.Log("Sign in google fail!");
				}
			});
		}

		public bool IsLoggedIn_Google()
		{
			return IsAuthenticatedWithGoogle;
		}

		public void LogOut_Google()
		{
			if (IsLoggedIn_Google())
			{
				//PlayGamesPlatform.Instance.SignOut();
			}
			loginState = SignInPhase.None;
			SendEvent_LogStatusChange();
		}

		public bool IsAuthenticatedWithGoogle
		{
			get
			{
				//if (auth.CurrentUser == null)
				//{
				//	return false;
				//}
				//IEnumerable<IUserInfo> providerData = auth.CurrentUser.ProviderData;
				//foreach (IUserInfo userInfo in providerData)
				//{
				//	if (string.Equals(userInfo.ProviderId, "google.com"))
				//	{
				//		return true;
				//	}
				//}
				return false;
			}
		}

		private void LinkingAccount()
		{
			//SignInPhase loginState = loginState;
			//if (loginState != SignInPhase.GoogleLogin)
			//{
			//	if (loginState == SignInPhase.FacebookLogin)
			//	{
			//		if (IsLoggedIn_Google())
			//		{
			//			tokenID = AccessToken.CurrentAccessToken.TokenString;
			//			Credential credential = FacebookAuthProvider.GetCredential(tokenID);
			//			auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(delegate(Task<FirebaseUser> task)
			//			{
			//				if (task.IsCanceled)
			//				{
			//					UnityEngine.Debug.LogError("LinkWithCredentialAsync was canceled.");
			//					return;
			//				}
			//				if (task.IsFaulted)
			//				{
			//					UnityEngine.Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
			//					string key = "TITLE_DATABASE_EXIST";
			//					string localization = GameKit.GetLocalization(key);
			//					SingletonMonoBehaviour<LifespanSurface>.Instance.NotifyPopupController.Init(localization, false, false);
			//					return;
			//				}
			//				SendEvent_LogStatusChange();
			//				loginState = SignInPhase.None;
			//				UnityEngine.Debug.Log("Linking FB with existing GG account!");
			//			});
			//			UnityEngine.Debug.Log("End Progress");
			//			SingletonMonoBehaviour<LifespanSurface>.Instance.LoadingProgressPopupController.Close();
			//		}
			//	}
			//}
			//else if (IsLoggedIn_Facebook())
			//{
			//	tokenID = PlayGamesPlatform.Instance.GetIdToken();
			//	Credential credential2 = GoogleAuthProvider.GetCredential(tokenID, null);
			//	auth.CurrentUser.LinkWithCredentialAsync(credential2).ContinueWith(delegate(Task<FirebaseUser> task)
			//	{
			//		if (task.IsCanceled)
			//		{
			//			UnityEngine.Debug.LogError("LinkWithCredentialAsync was canceled.");
			//			return;
			//		}
			//		if (task.IsFaulted)
			//		{
			//			UnityEngine.Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
			//			string key = "TITLE_DATABASE_EXIST";
			//			string localization = GameKit.GetLocalization(key);
			//			SingletonMonoBehaviour<LifespanSurface>.Instance.NotifyPopupController.Init(localization, false, false);
			//			return;
			//		}
			//		SendEvent_LogStatusChange();
			//		loginState = SignInPhase.None;
			//		UnityEngine.Debug.Log("Linking GG with existing FB account!");
			//	});
			//	UnityEngine.Debug.Log("End Progress");
			//	SingletonMonoBehaviour<LifespanSurface>.Instance.LoadingProgressPopupController.Close();
			//}
		}

		public string GetFirebaseUserID()
		{
			string result = string.Empty;
			//if (auth.CurrentUser != null)
			//{
			//	result = auth.CurrentUser.UserId;
			//}
			return result;
		}

		public string GetFireseBaseUserName()
		{
			string text = string.Empty;
			//if (auth.CurrentUser != null)
			//{
			//	text = auth.CurrentUser.DisplayName;
			//}
			//if (string.IsNullOrEmpty(text) && Social.localUser != null)
			//{
			//	text = ((PlayGamesLocalUser)Social.localUser).userName;
			//}
			return text;
		}

		public string GetFireseBaseUserEmail()
		{
			string text = string.Empty;
			//if (auth.CurrentUser != null)
			//{
			//	text = auth.CurrentUser.Email;
			//}
			//if (string.IsNullOrEmpty(text) && Social.localUser != null)
			//{
			//	text = ((PlayGamesLocalUser)Social.localUser).Email;
			//}
			return text;
		}

		public string GetFireseBaseUserPhoneNumber()
		{
			string result = string.Empty;
			//if (auth.CurrentUser != null)
			//{
			//	result = auth.CurrentUser.PhoneNumber;
			//}
			return result;
		}

		public void TakePremiumUserInfor()
		{
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			text = GetFirebaseUserID();
			text2 = GetFireseBaseUserName();
			text3 = GetFireseBaseUserEmail();
			text4 = GetFireseBaseUserPhoneNumber();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			UnityEngine.Debug.Log(string.Concat(new string[]
			{
				"Claim user infor //id = ",
				text,
				" //name = ",
				text2,
				" //email = ",
				text3,
				" //phonenumber = ",
				text4
			}));
			NativeSpecificServicesSource.Services.DataCloudSaver.ClaimPremiumUserInfor(text, text2, text3, text4);
		}

		public Sprite GetUserAvatar()
		{
			//if (FB.IsLoggedIn)
			//{
			//	FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, new FacebookDelegate<IGraphResult>(GetPicture), null);
			//}
			return currentUserAvatar;
		}

		//private void GetPicture(IGraphResult result)
		//{
		//	if (result.Error == null && result.Texture != null)
		//	{
		//		currentUserAvatar = Sprite.Create(result.Texture, new Rect(0f, 0f, 128f, 128f), default(Vector2));
		//	}
		//}

		private List<string> listPermissionLogin = new List<string>
		{
			"public_profile",
			"email",
			"user_friends"
		};

		private const string FIREBASE_PROVIDER_ID = "firebase";

		private const string GOOGLE_PROVIDER_ID = "google.com";

		private const string FACEBOOK_PROVIDER_ID = "facebook.com";

		//private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

		//private FirebaseAuth auth;

		private SignInPhase loginState;

		private bool isNewUser;

		private string tokenID = string.Empty;

		private string usedUserID;

		private bool isPlayingAsGuest;

		private Sprite currentUserAvatar;
	}
}
