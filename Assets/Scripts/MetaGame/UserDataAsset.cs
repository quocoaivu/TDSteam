using System;
using Common;
using UnityEngine;

namespace MetaGame
{
	public class UserDataAsset : ScriptableObject
	{
		public static UserDataAsset Instance
		{
			get
			{
				if (UserDataAsset.instance == null)
				{
					UserDataAsset.instance = UserDataAsset.LoadAsset();
					if (UserDataAsset.instance == null)
					{
						UserDataAsset.instance = UserDataAsset.CreateAsset();
					}
					UserDataAsset.instance.Initialize();
				}
				return UserDataAsset.instance;
			}
		}

		private void Initialize()
		{
		}

		private static UserDataAsset CreateAsset()
		{
			return null;
		}

		private static UserDataAsset LoadAsset()
		{
			string path = ApplicationDefines.ApplicationDataResourcePath + "/UserDataScriptableObject_ASSET";
			return Common.AssetLoader.Load<UserDataAsset>(path);
		}

		public GlobalEnhanceProgress GlobalUpgradeProgress
		{
			get
			{
				return globalUpgradeProgress;
			}
			private set
			{
				globalUpgradeProgress = value;
			}
		}

		private const string assetName = "UserDataScriptableObject_ASSET";

		private static UserDataAsset instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
		[SerializeField]
		private GlobalEnhanceProgress globalUpgradeProgress;
	}
}
