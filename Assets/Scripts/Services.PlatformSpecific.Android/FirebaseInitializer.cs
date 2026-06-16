using System;
using System.Collections;
//using System.Threading.Tasks;
//using Firebase;
using UnityEngine;

namespace Services.PlatformSpecific.Android
{
	public class FirebaseInitializer : MonoBehaviour
	{
		private void Start()
		{
			base.StartCoroutine(_InitializeFirebase());
		}

		private void InitializeFirebase()
		{
			dataCloudSaverAndroid.FirebaseInit();
			userProfileAndroid.FirebaseInit();
			firebaseAnalyticsAndroid.FirebaseInit();
			notificationAndroid.FirebaseInit();
		}

		private IEnumerator _InitializeFirebase()
		{
			yield return null;
			yield return null;
			//FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(delegate(Task<DependencyStatus> task)
			//{
			//	dependencyStatus = task.Result;
			//	if (dependencyStatus == DependencyStatus.Available)
			//	{
			//		InitializeFirebase();
			//		UnityEngine.Debug.Log("Inits Firebase services!");
			//	}
			//	else
			//	{
			//		UnityEngine.Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
			//	}
			//});
			yield break;
		}

		[SerializeField]
		private RecordCloudSaverAndroid dataCloudSaverAndroid;

		[SerializeField]
		private PlayerDossierAndroid userProfileAndroid;

		[SerializeField]
		private CloudMetricsAndroid firebaseAnalyticsAndroid;

		[SerializeField]
		private AlertAndroid notificationAndroid;

		//private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
	}
}
