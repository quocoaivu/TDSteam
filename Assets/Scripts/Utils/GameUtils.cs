using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
	public static class GameUtils
	{
		public static bool IsInternetConnectionAvailable()
		{
			return Application.internetReachability != NetworkReachability.NotReachable;
		}

		public static bool CheckIfDayPassed()
		{
			bool result = false;
			string text = GameKit.GetNow().ToString();
			DateTime dateTime = Convert.ToDateTime(PlayerPrefs.GetString("savedDateTime", text));
			PlayerPrefs.SetString("savedDateTime", text);
			if (DateTime.Today.Year > dateTime.Year)
			{
				result = true;
			}
			else if (DateTime.Today.Month > dateTime.Month)
			{
				result = true;
			}
			else if (DateTime.Today.Day > dateTime.Day)
			{
				result = true;
			}
			return result;
		}

		public static TimeSpan GetTimeSpanFromSecond(float value)
		{
			return TimeSpan.FromSeconds((double)value);
		}

		public static string GetFormattedTimeSpan(TimeSpan timeSpan)
		{
			return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}

		public static bool CheckPackageAppIsPresent(string package)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
				AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getInstalledPackages", new object[]
				{
					0
				});
				int num = androidJavaObject2.Call<int>("size", new object[0]);
				for (int i = 0; i < num; i++)
				{
					AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("get", new object[]
					{
						i
					});
					string text = androidJavaObject3.Get<string>("packageName");
					if (text.CompareTo(package) == 0)
					{
						return true;
					}
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static string GetHighLightTextByLevel(string text0, string text1, string text2, int level)
		{
			string result = string.Empty;
			switch (level)
			{
			case 0:
				result = string.Concat(new string[]
				{
					"<color=lime>",
					text0,
					"</color>/",
					text1,
					"/",
					text2
				});
				break;
			case 1:
				result = string.Concat(new string[]
				{
					"<color=lime>",
					text0,
					"</color>/",
					text1,
					"/",
					text2
				});
				break;
			case 2:
				result = string.Concat(new string[]
				{
					text0,
					"/<color=lime>",
					text1,
					"</color>/",
					text2
				});
				break;
			case 3:
				result = string.Concat(new string[]
				{
					text0,
					"/",
					text1,
					"/<color=lime>",
					text2,
					"</color>"
				});
				break;
			}
			return result;
		}

		public static string GetDeviceUniqueID()
		{
			return SystemInfo.deviceUniqueIdentifier;
		}

		public static void ClearInputField(InputField inputField)
		{
			inputField.Select();
			inputField.text = string.Empty;
		}
	}
}
