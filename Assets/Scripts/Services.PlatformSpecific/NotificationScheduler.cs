using System;
using Tournament;

namespace Services.PlatformSpecific
{
	public class NotificationScheduler : MonoSingleton<NotificationScheduler>
	{
		private const int SECONDS_PER_DAY = 86400;

		private const double END_SEASON_NOTIFY_OFFSET_MINUTES = 15.0;

		private IAlert notification;

		private void Start()
		{
			// Notifications are non-critical: if the platform service isn't ready
			// (e.g. scene run without the bootstrap provider), just skip silently.
			notification = NativeSpecificServicesSource.Services?.GameNotification;
			if (notification == null)
			{
				return;
			}
			notification.CancelAllNotify();
			ScheduleComeBackNotify();
			ScheduleTournamentNotify();
		}

		private void ScheduleComeBackNotify()
		{
			string key = "NOTIFY_COME_BACK_GAME";
			string localization = GameKit.GetLocalization(key);
			notification.PushNotify(localization, 2 * SECONDS_PER_DAY);
			notification.PushNotify(localization, 3 * SECONDS_PER_DAY);
			notification.PushNotify(localization, 5 * SECONDS_PER_DAY);
		}

		private void ScheduleTournamentNotify()
		{
			DateTime d = GameKit.ReadTimeStamp(ArenaDialogHandler.NEXT_FREE_TIME_KEY);
			TimeSpan timeSpan = d - ArenaDialogHandler.GetNow();
			if (timeSpan.TotalMinutes > 1.0)
			{
				string key = "NOTIFY_FREE_TOUR";
				string localization = GameKit.GetLocalization(key);
				notification.PushNotify(localization, (int)timeSpan.TotalSeconds);
			}
			DateTime d2 = GameKit.ReadTimeStamp(ArenaDialogHandler.END_SEASON_TIME_KEY).AddMinutes(END_SEASON_NOTIFY_OFFSET_MINUTES);
			timeSpan = d2 - ArenaDialogHandler.GetNow();
			if (timeSpan.TotalMinutes > 1.0)
			{
				string key2 = "NOTIFY_END_TOUR_SEASON";
				string localization2 = GameKit.GetLocalization(key2);
				notification.PushNotify(localization2, (int)timeSpan.TotalSeconds);
			}
		}
	}
}
