using Data;
using GameCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bootstrap
{
	public class GameBootstrap : MonoBehaviour
	{
		[SerializeField]
		[FormerlySerializedAs("readWriteRemoteSettingData")]
		private RemoteConfigLoader remoteConfigLoader;

		public RemoteConfigLoader RemoteConfig
		{
			get
			{
				return remoteConfigLoader;
			}
		}

		public static GameBootstrap Instance { get; private set; }

		private void Awake()
		{
			if (GameBootstrap.Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			GameBootstrap.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Application.targetFrameRate = 60;
		}

		private void Start()
		{
			if (GameUtils.CheckIfDayPassed())
			{
				FreeResourcesStore.Instance.ResetFreeResourcesDailyData();
				RemoteConfig.WriteDefaultRemoteSettingValue_Gem();
				DailyTrialStore.Instance.IncreaseDay();
				DailyRewardStore.Instance.TryIncreaseDay();
			}
			SaleBundleStore.Instance.SetLastTimePlay();
		}
	}
}
