using System;
using Data;

namespace MetaGame
{
	public class Setup
	{
		public Setup()
		{
			isSoundOn = PlayerSaveStore.Instance.ReadSound();
			isMusicOn = PlayerSaveStore.Instance.ReadMusic();
			isVibrationOn = PlayerSaveStore.Instance.ReadVibration();
			isAllowPushNoti = PlayerSaveStore.Instance.ReadNotification();
		}

		public int LineCount { get; set; }

		public int EarlyCallMoney { get; set; }

		public int LifePercent2Star { get; set; }

		public int LifePercent3Star { get; set; }

		public int FirstTimeGemTakenPercentage { get; set; }

		public int SecondTimeGemTakenPercentage { get; set; }

		public int ThirdTimeGemTakenPercentage { get; set; }

		public static Setup Instance
		{
			get
			{
				if (Setup.instance == null)
				{
					Setup.instance = new Setup();
				}
				return Setup.instance;
			}
		}

		public bool Sound
		{
			get
			{
				return isSoundOn;
			}
			set
			{
				isSoundOn = value;
				PlayerSaveStore.Instance.WriteSound(value);
			}
		}

		public bool Music
		{
			get
			{
				return isMusicOn;
			}
			set
			{
				isMusicOn = value;
				PlayerSaveStore.Instance.WriteMusic(isMusicOn);
			}
		}

		public bool Vibration
		{
			get
			{
				return isVibrationOn;
			}
			set
			{
				isVibrationOn = value;
				PlayerSaveStore.Instance.WriteVibration(isVibrationOn);
			}
		}

		public bool AllowPushNoti
		{
			get
			{
				return isAllowPushNoti;
			}
			set
			{
				isAllowPushNoti = value;
				PlayerSaveStore.Instance.WriteNotification(isAllowPushNoti);
			}
		}

		public string LanguageID
		{
			get
			{
				return PlayerSaveStore.Instance.ReadLanguage();
			}
			set
			{
				PlayerSaveStore.Instance.WriteLanguage(value);
				GameKit.SetCurrentLanguage(value);
			}
		}

		public const string LANGUAGE_KEY_ENGLISH = "lg_en";

		public const string LANGUAGE_KEY_VIETNAMESE = "lg_vi";

		public const string LANGUAGE_KEY_FRENCH = "lg_french";

		public const string LANGUAGE_KEY_SPANISH = "lg_spanish";

		public const string LANGUAGE_KEY_RUSSIAN = "lg_russian";

		public const string LANGUAGE_KEY_BRAZIL = "lg_brazil";

		public const string LANGUAGE_KEY_GERMAN = "lg_german";

		public const string LANGUAGE_KEY_POLISH = "lg_polish";

		public const string LANGUAGE_KEY_KOREAN = "lg_korean";

		public const string LANGUAGE_KEY_CHINESE = "lg_chinese";

		public const string LANGUAGE_KEY_JAPANESE = "lg_japanese";

		private bool isSoundOn;

		private bool isMusicOn;

		private bool isVibrationOn;

		private bool isAllowPushNoti;

		public int currentTowerRegionIDSelected = -1;

		private static Setup instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
