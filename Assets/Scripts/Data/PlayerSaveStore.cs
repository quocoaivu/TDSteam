using System;
using UnityEngine;

namespace Data
{
	public class PlayerSaveStore
	{
        private string KEY_DEFAULT_DATA_0 = "setting";

        private string KEY_SOUND = "sound";

        private string KEY_MUSIC = "music";

        private string KEY_VIBRATE = "vibrate";

        private string KEY_NOTIFIATION = "notification";

        private string KEY_LANGUAGE = "language_";

        public const string KEY_SAVED_DATETIME = "savedDateTime";

        private string KEY_RATE = "rate_game";

        private string PLAY_COUNT = "play_count";

        private bool isFirstTimeSession = true;

        private static PlayerSaveStore instance;
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
        }

        public PlayerSaveStore()
		{
			isFirstTimeSession = true;
			IncreasePlayCount();
			UnityEngine.Debug.Log("current play count = " + GetPlayCount());
		}

		public static PlayerSaveStore Instance
		{
			get
			{
				if (PlayerSaveStore.instance == null)
				{
					PlayerSaveStore.instance = new PlayerSaveStore();
				}
				return PlayerSaveStore.instance;
			}
		}

		public void WriteDefaultSetting()
		{
			if (PlayerPrefs.GetInt(KEY_DEFAULT_DATA_0) == 0)
			{
				WriteSound(true);
				WriteMusic(true);
				WriteVibration(true);
				WriteNotification(true);
				WriteDateTime();
				WriteDefaultLanguage();
				PlayerPrefs.SetInt(KEY_DEFAULT_DATA_0, 1);
			}
		}

		public void WriteDateTime()
		{
			PlayerPrefs.SetString("savedDateTime", DateTime.Today.ToString());
		}

		public void WriteSound(bool sound)
		{
			PlayerPrefs.SetInt(KEY_SOUND, (!sound) ? 0 : 1);
		}

		public bool ReadSound()
		{
			if (PlayerPrefs.HasKey(KEY_SOUND))
			{
				int @int = PlayerPrefs.GetInt(KEY_SOUND);
				return @int > 0;
			}
			return true;
		}

		public void WriteMusic(bool music)
		{
			PlayerPrefs.SetInt(KEY_MUSIC, (!music) ? 0 : 1);
		}

		public bool ReadMusic()
		{
			if (PlayerPrefs.HasKey(KEY_MUSIC))
			{
				int @int = PlayerPrefs.GetInt(KEY_MUSIC);
				return @int > 0;
			}
			return true;
		}

		public void WriteVibration(bool vibration)
		{
			PlayerPrefs.SetInt(KEY_VIBRATE, (!vibration) ? 0 : 1);
		}

		public bool ReadVibration()
		{
			if (PlayerPrefs.HasKey(KEY_VIBRATE))
			{
				int @int = PlayerPrefs.GetInt(KEY_VIBRATE);
				return @int > 0;
			}
			return true;
		}

		public void WriteNotification(bool isAllowPushNoti)
		{
			PlayerPrefs.SetInt(KEY_NOTIFIATION, (!isAllowPushNoti) ? 0 : 1);
		}

		public bool ReadNotification()
		{
			if (PlayerPrefs.HasKey(KEY_NOTIFIATION))
			{
				int @int = PlayerPrefs.GetInt(KEY_NOTIFIATION);
				return @int > 0;
			}
			return true;
		}

		private void WriteDefaultLanguage()
		{
			UnityEngine.Debug.Log("System language = " + Application.systemLanguage);
			SystemLanguage systemLanguage = Application.systemLanguage;
			switch (systemLanguage)
			{
			case SystemLanguage.Japanese:
				WriteLanguage("lg_japanese");
				break;
			case SystemLanguage.Korean:
				WriteLanguage("lg_korean");
				break;
			default:
				if (systemLanguage != SystemLanguage.French)
				{
					if (systemLanguage != SystemLanguage.German)
					{
						if (systemLanguage != SystemLanguage.Chinese)
						{
							if (systemLanguage != SystemLanguage.English)
							{
								if (systemLanguage != SystemLanguage.Spanish)
								{
									if (systemLanguage != SystemLanguage.Vietnamese)
									{
										WriteLanguage("lg_en");
									}
									else
									{
										WriteLanguage("lg_vi");
									}
								}
								else
								{
									WriteLanguage("lg_spanish");
								}
							}
							else
							{
								WriteLanguage("lg_en");
							}
						}
						else
						{
							WriteLanguage("lg_chinese");
						}
					}
					else
					{
						WriteLanguage("lg_german");
					}
				}
				else
				{
					WriteLanguage("lg_french");
				}
				break;
			case SystemLanguage.Polish:
				WriteLanguage("lg_polish");
				break;
			case SystemLanguage.Portuguese:
				WriteLanguage("lg_brazil");
				break;
			case SystemLanguage.Russian:
				WriteLanguage("lg_russian");
				break;
			}
			ConfigRegistry.Instance.multiLanguageDataReader.ReloadParameters();
		}

		public void WriteLanguage(string value)
		{
			PlayerPrefs.SetString(KEY_LANGUAGE, value);
			PlayerPrefs.Save();
		}

		public string ReadLanguage()
		{
			string result = "lg_en";
			if (PlayerPrefs.HasKey(KEY_LANGUAGE))
			{
				result = PlayerPrefs.GetString(KEY_LANGUAGE);
			}
			return result;
		}

		public bool IsFirstTimeSession()
		{
			bool result = false;
			if (isFirstTimeSession)
			{
				result = true;
				isFirstTimeSession = false;
			}
			return result;
		}

		private void IncreasePlayCount()
		{
			int num = PlayerPrefs.GetInt(PLAY_COUNT, 0);
			num++;
			PlayerPrefs.SetInt(PLAY_COUNT, num);
			PlayerPrefs.Save();
		}

		public int GetPlayCount()
		{
			return PlayerPrefs.GetInt(PLAY_COUNT, 0);
		}

		public void SetDataRated()
		{
			PlayerPrefs.SetInt(KEY_RATE, 1);
		}

		public bool IsUserRated()
		{
			return PlayerPrefs.GetInt(KEY_RATE, 0) == 1;
		}
	}
}
