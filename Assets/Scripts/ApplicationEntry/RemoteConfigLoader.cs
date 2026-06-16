using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Bootstrap
{
	public class RemoteConfigLoader : MonoBehaviour
	{
		// Empty attribute structs required by RemoteConfigService.FetchConfigs.
		private struct UserAttributes { }

		private struct AppAttributes { }


        private const string REMOTE_CONFIG_KEY_GEM = "free_gem_config";

        private const string CURRENT_REMOTE_SETTING_VALUE_GEM = "current_remote_setting_value";

        [Header("Fallback defaults (offline / before first fetch; dashboard overrides these)")]
        [SerializeField]
        [Tooltip("Free-gem display chance %. 100 = always show.")]
        private int defaultRemoteSettingValue_gem = 100;

        private int remoteSettingValue_gem;

        private const string REMOTE_VALUE_ADS_END_GAME = "remote_value_ads_endGame";

        private const string REMOTE_VALUE_ADS_LOADING = "remote_value_ads_loading";

        [SerializeField]
        [Tooltip("Interstitial ads chance % (used for both end-game and loading). 100 = always.")]
        private int defaultRemoteSettingValue_ads = 100;

        private int remoteSettingValue_ads;

        private const string REMOTE_VALUE_SHOW_ASK_RATING = "remote_chance_to_show_ask_rating";

        [SerializeField]
        [Tooltip("Ask-rating popup chance %. 100 = always.")]
        private int defaultRemoteSettingValue_askRating = 100;

        private int remoteSettingValue_askRating;

        private const string REMOTE_VALUE_RATING_BEHAVIOR = "remote_rating_behavior";

        [SerializeField]
        private string defaultRemoteValueRatingBehavior = RatingConduct.thanknhide.ToString();

        private string remoteValueRatingBehavior = string.Empty;

        private const string REMOTE_VALUE_HERO_BUNDLE_OPTION = "remote_sale_hero_bundle_option";

        [SerializeField]
        [Tooltip("Hero bundle layout option index (not a %). 0 = first option.")]
        private int defaultRemoteSettingValue_heroBundleOption;

        private int remoteSettingValue_heroBundleOption;

        private const string HOLIDAY_EVENT_ID_KEY = "holiday_event_id";

        private const string HOLIDAY_EVENT_START_DAY = "holiday_event_start_day";

        [SerializeField]
        [Tooltip("-1 = no holiday event.")]
        private int defaultHolidayEventId = -1;

        [SerializeField]
        private string defaultHolidayEventStartDay = "0";

        private int holidayEventId;

        private string holidayEventStartDay;

        
		private async void Awake()
		{
			try
			{
				await InitializeRemoteConfigAsync();
			}
			catch (Exception e)
			{
				// Offline / services unavailable: keep last cached values in PlayerPrefs.
				UnityEngine.Debug.LogWarning("Remote Config init failed: " + e.Message);
				return;
			}
			RemoteConfigService.Instance.FetchCompleted += OnFetchCompleted;
			RemoteConfigService.Instance.FetchConfigs(new UserAttributes(), new AppAttributes());
		}

		private void OnDestroy()
		{
			// Only unsubscribe if init got far enough to subscribe.
			if (UnityServices.State == ServicesInitializationState.Initialized)
			{
				RemoteConfigService.Instance.FetchCompleted -= OnFetchCompleted;
			}
		}

		// Remote Config needs initialized Unity Services + a signed-in player id.
		private async Task InitializeRemoteConfigAsync()
		{
			if (UnityServices.State != ServicesInitializationState.Initialized)
			{
				await UnityServices.InitializeAsync();
			}
			if (!AuthenticationService.Instance.IsSignedIn)
			{
				await AuthenticationService.Instance.SignInAnonymouslyAsync();
			}
		}

		private void OnFetchCompleted(ConfigResponse response)
		{
			WriteAllRemoteDefaults();
		}

		// Single source of truth for caching remote config into PlayerPrefs, run after each successful fetch.
		private void WriteAllRemoteDefaults()
		{
			WriteDefaultRemoteSettingValue_Gem();
			WriteDefaultRemoteSettingValue_Ads();
			WriteDefaultRemoteSettingValue_AskRating();
			WriteDefaultRemoteSettingValue_RatingBehavior();
			GetHolidayEvent();
			WriteDefaultRemoteSettingValue_HeroBundleOption();
		}

		public void WriteDefaultRemoteSettingValue_Gem()
		{
			remoteSettingValue_gem = RemoteConfigService.Instance.appConfig.GetInt(REMOTE_CONFIG_KEY_GEM, defaultRemoteSettingValue_gem);
			if (UnityEngine.Random.Range(0, 100) < remoteSettingValue_gem)
			{
				PlayerPrefs.SetInt(CURRENT_REMOTE_SETTING_VALUE_GEM, 1);
			}
			else
			{
				PlayerPrefs.SetInt(CURRENT_REMOTE_SETTING_VALUE_GEM, 0);
			}
		}

		public bool IsDisplayFreeGem()
		{
			return PlayerPrefs.GetInt(CURRENT_REMOTE_SETTING_VALUE_GEM) == 1;
		}

		public void WriteDefaultRemoteSettingValue_Ads()
		{
			remoteSettingValue_ads = RemoteConfigService.Instance.appConfig.GetInt(MarketingSetup.REMOTE_KEY_ADS_END_GAME, defaultRemoteSettingValue_ads);
			PlayerPrefs.SetInt(REMOTE_VALUE_ADS_END_GAME, remoteSettingValue_ads);
			remoteSettingValue_ads = RemoteConfigService.Instance.appConfig.GetInt(MarketingSetup.REMOTE_KEY_ADS_LOADING, defaultRemoteSettingValue_ads);
			PlayerPrefs.SetInt(REMOTE_VALUE_ADS_LOADING, remoteSettingValue_ads);
			UnityEngine.Debug.Log("Chance to show ad loading = " + PlayerPrefs.GetInt(REMOTE_VALUE_ADS_LOADING));
			UnityEngine.Debug.Log("Chance to show ad end game = " + PlayerPrefs.GetInt(REMOTE_VALUE_ADS_END_GAME));
		}

		public int ChanceToShowInterAds_EndGame()
		{
			return PlayerPrefs.GetInt(REMOTE_VALUE_ADS_END_GAME);
		}

		public int ChanceToShowInterAds_Loading()
		{
			return PlayerPrefs.GetInt(REMOTE_VALUE_ADS_LOADING);
		}

		private void WriteDefaultRemoteSettingValue_AskRating()
		{
			remoteSettingValue_askRating = RemoteConfigService.Instance.appConfig.GetInt(MarketingSetup.REMOTE_KEY_ASK_RATING, defaultRemoteSettingValue_askRating);
			PlayerPrefs.SetInt(REMOTE_VALUE_SHOW_ASK_RATING, remoteSettingValue_askRating);
			UnityEngine.Debug.Log("chance to show ask rating = " + PlayerPrefs.GetInt(REMOTE_VALUE_SHOW_ASK_RATING));
		}

		public int GetChanceToShowAskRating()
		{
			return PlayerPrefs.GetInt(REMOTE_VALUE_SHOW_ASK_RATING);
		}

		private void WriteDefaultRemoteSettingValue_RatingBehavior()
		{
			remoteValueRatingBehavior = RemoteConfigService.Instance.appConfig.GetString(MarketingSetup.REMOTE_KEY_RATING_BEHAVIOR, defaultRemoteValueRatingBehavior);
			PlayerPrefs.SetString(REMOTE_VALUE_RATING_BEHAVIOR, remoteValueRatingBehavior);
			UnityEngine.Debug.Log("rating behavior = " + PlayerPrefs.GetString(REMOTE_VALUE_RATING_BEHAVIOR));
		}

		public string GetRatingBehavior()
		{
			return PlayerPrefs.GetString(REMOTE_VALUE_RATING_BEHAVIOR);
		}

		private void GetHolidayEvent()
		{
			holidayEventId = RemoteConfigService.Instance.appConfig.GetInt(HOLIDAY_EVENT_ID_KEY, defaultHolidayEventId);
			PlayerPrefs.SetInt(HOLIDAY_EVENT_ID_KEY, holidayEventId);
			holidayEventStartDay = RemoteConfigService.Instance.appConfig.GetString(HOLIDAY_EVENT_START_DAY, defaultHolidayEventStartDay);
			PlayerPrefs.SetString(HOLIDAY_EVENT_START_DAY, holidayEventStartDay);
			UnityEngine.Debug.LogFormat("_____ set remote config holiday event {0} start day {1}", new object[]
			{
				holidayEventId,
				holidayEventStartDay
			});
		}

		public int GetHolidayEventId()
		{
			return PlayerPrefs.GetInt(HOLIDAY_EVENT_ID_KEY, defaultHolidayEventId);
		}

		public long GetHolidayStartDay()
		{
			string @string = PlayerPrefs.GetString(HOLIDAY_EVENT_START_DAY, holidayEventStartDay);
			long result = 0L;
			long.TryParse(@string, out result);
			return result;
		}

		private void WriteDefaultRemoteSettingValue_HeroBundleOption()
		{
			remoteSettingValue_heroBundleOption = RemoteConfigService.Instance.appConfig.GetInt(MarketingSetup.REMOTE_KEY_HERO_BUNDLE_OPTION, defaultRemoteSettingValue_heroBundleOption);
			PlayerPrefs.SetInt(REMOTE_VALUE_HERO_BUNDLE_OPTION, remoteSettingValue_heroBundleOption);
			UnityEngine.Debug.Log("hero bundle option = " + PlayerPrefs.GetInt(REMOTE_VALUE_HERO_BUNDLE_OPTION));
		}

		public int GetHeroBundleOption()
		{
			return PlayerPrefs.GetInt(REMOTE_VALUE_HERO_BUNDLE_OPTION);
		}


	}
}
