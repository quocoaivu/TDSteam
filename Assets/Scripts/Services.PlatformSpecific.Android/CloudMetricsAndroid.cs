using System;
//using Firebase.Analytics;
using UnityEngine;

public class CloudMetricsAndroid : MonoBehaviour
{
	public void FirebaseInit()
	{
		UnityEngine.Debug.Log("Enabling data collection.");
		//FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
	}

	public void LogEvent(string eventName)
	{
		//FirebaseAnalytics.LogEvent(eventName);
	}

	public void LogEventBeginCheckout(decimal value, string currency)
	{
		//FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventBeginCheckout, new Parameter[]
		//{
		//	new Parameter(FirebaseAnalytics.ParameterValue, value.ToString()),
		//	new Parameter(FirebaseAnalytics.ParameterCurrency, currency)
		//});
	}
}
