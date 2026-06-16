using System;
using UnityEngine;

// Anti-cheat clock. Combines Android SystemClock.elapsedRealtime (monotonic, unaffected
// by user changing device time) with a persisted PlayerPrefs anchor so time can never
// move backward across sessions. Within a single boot, uptime is fully trusted and
// system time is ignored — this also blocks "skip cooldown" forward-cheats. After a
// reboot we fall back to system time but never earlier than the last saved value.
public class UnbiasedTime : MonoBehaviour
{
	private const string PREF_LAST_TICKS = "UnbiasedTime_LastTicks";
	private const string PREF_LAST_UPTIME_MS = "UnbiasedTime_LastUptimeMs";
	private const float SAVE_INTERVAL_SECONDS = 30f;

	private static UnbiasedTime instance;

	public static UnbiasedTime Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("UnbiasedTimeSingleton");
				instance = go.AddComponent<UnbiasedTime>();
				UnityEngine.Object.DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	private bool _isAndroid;
	private DateTime _anchorTime;     // unbiased "now" at the moment of anchoring
	private long _anchorUptimeMs;     // SystemClock.elapsedRealtime when anchor was taken

	[HideInInspector] public long timeOffset;

	private void Awake()
	{
		_isAndroid = Application.platform == RuntimePlatform.Android;
		UpdateTimeOffset();
		InvokeRepeating(nameof(SaveState), SAVE_INTERVAL_SECONDS, SAVE_INTERVAL_SECONDS);
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause) SaveState();
		else UpdateTimeOffset();
	}

	private void OnApplicationQuit()
	{
		SaveState();
	}

	public DateTime Now()
	{
		long currentUptimeMs = GetElapsedRealtimeMs();
		long elapsedSinceAnchorMs = currentUptimeMs - _anchorUptimeMs;

		// Defensive: uptime should never go backward within a session.
		if (elapsedSinceAnchorMs < 0)
		{
			UpdateTimeOffset();
			return _anchorTime;
		}

		return _anchorTime.AddMilliseconds(elapsedSinceAnchorMs);
	}

	public void UpdateTimeOffset()
	{
		long currentUptimeMs = GetElapsedRealtimeMs();
		DateTime systemNow = DateTime.Now;

		long lastTicks = ParseLong(PlayerPrefs.GetString(PREF_LAST_TICKS, "0"));
		long lastUptimeMs = ParseLong(PlayerPrefs.GetString(PREF_LAST_UPTIME_MS, "-1"));

		if (lastTicks == 0L)
		{
			// First launch ever — nothing else to trust, accept system time.
			_anchorTime = systemNow;
		}
		else
		{
			DateTime lastTime = new DateTime(lastTicks);

			if (lastUptimeMs >= 0L && currentUptimeMs >= lastUptimeMs)
			{
				// Same boot session: uptime is authoritative, ignore system clock entirely.
				long elapsedMs = currentUptimeMs - lastUptimeMs;
				_anchorTime = lastTime.AddMilliseconds(elapsedMs);
			}
			else
			{
				// Device rebooted (or no prior uptime saved). Use system time but never
				// allow it to be earlier than the last value we observed.
				_anchorTime = systemNow > lastTime ? systemNow : lastTime;
			}
		}

		_anchorUptimeMs = currentUptimeMs;
		timeOffset = (long)(systemNow - _anchorTime).TotalSeconds;
		SaveState();
	}

	public bool IsUsingSystemTime()
	{
		// True when we have no monotonic uptime source (iOS / Editor).
		return !_isAndroid;
	}

	private long GetElapsedRealtimeMs()
	{
		if (_isAndroid)
		{
			try
			{
				using (AndroidJavaClass systemClock = new AndroidJavaClass("android.os.SystemClock"))
				{
					return systemClock.CallStatic<long>("elapsedRealtime");
				}
			}
			catch (Exception)
			{
				// JNI failure — fall through to realtimeSinceStartup.
			}
		}
		return (long)(Time.realtimeSinceStartup * 1000f);
	}

	private void SaveState()
	{
		DateTime now = Now();
		PlayerPrefs.SetString(PREF_LAST_TICKS, now.Ticks.ToString());
		PlayerPrefs.SetString(PREF_LAST_UPTIME_MS, GetElapsedRealtimeMs().ToString());
		PlayerPrefs.Save();
	}

	private static long ParseLong(string s)
	{
		long v;
		return long.TryParse(s, out v) ? v : 0L;
	}
}
