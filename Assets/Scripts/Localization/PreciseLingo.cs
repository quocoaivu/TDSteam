using System;
using UnityEngine;

public class PreciseLingo
{
	private static PreciseLingo.PlatformBridge platform
	{
		get
		{
			if (PreciseLingo._platform == null)
			{
				PreciseLingo._platform = new PreciseLingo.PreciseLocaleAndroid();
			}
			return PreciseLingo._platform;
		}
	}

	public static string GetRegion()
	{
		return PreciseLingo.platform.GetRegion();
	}

	public static string GetLanguageID()
	{
		return PreciseLingo.platform.GetLanguageID();
	}

	public static string GetLanguage()
	{
		return PreciseLingo.platform.GetLanguage();
	}

	public static string GetCurrencyCode()
	{
		return PreciseLingo.platform.GetCurrencyCode();
	}

	public static string GetCurrencySymbol()
	{
		return PreciseLingo.platform.GetCurrencySymbol();
	}

	private static PreciseLingo.PlatformBridge _platform;

	private interface PlatformBridge
	{
		string GetRegion();

		string GetLanguage();

		string GetLanguageID();

		string GetCurrencyCode();

		string GetCurrencySymbol();
	}

	private class EditorBridge : PreciseLingo.PlatformBridge
	{
		public string GetRegion()
		{
			return "US";
		}

		public string GetLanguage()
		{
			return "en";
		}

		public string GetLanguageID()
		{
			return "en_US";
		}

		public string GetCurrencyCode()
		{
			return "USD";
		}

		public string GetCurrencySymbol()
		{
			return "$";
		}
	}

	private class PreciseLocaleAndroid : PreciseLingo.PlatformBridge
	{
		public string GetRegion()
		{
			return PreciseLingo.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getRegion", new object[0]);
		}

		public string GetLanguage()
		{
			return PreciseLingo.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getLanguage", new object[0]);
		}

		public string GetLanguageID()
		{
			return PreciseLingo.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getLanguageID", new object[0]);
		}

		public string GetCurrencyCode()
		{
			return PreciseLingo.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getCurrencyCode", new object[0]);
		}

		public string GetCurrencySymbol()
		{
			return PreciseLingo.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getCurrencySymbol", new object[0]);
		}

		private static AndroidJavaClass _preciseLocale = new AndroidJavaClass("com.kokosoft.preciselocale.PreciseLocale");
	}
}
