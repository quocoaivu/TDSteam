using System;
using Assets.SimpleAndroidNotifications;
using MetaGame;
using UnityEngine;

namespace Services.PlatformSpecific.Android
{
	public class AlertAndroid : MonoBehaviour, IAlert
	{
		public void FirebaseInit()
		{
		}

		private void OnDestroy()
		{
		}

		public void PushNotify(string content, int delayTimeBySecond)
		{
			if (Setup.Instance.AllowPushNoti)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"User allow push notify: Editor Notify with  content: ",
					content,
					" delay time: ",
					delayTimeBySecond
				}));
				NotificationParams notificationParams = new NotificationParams
				{
					Id = NotificationIdHandler.GetNotificationId(),
					Delay = TimeSpan.FromSeconds((double)delayTimeBySecond),
					Title = "Kingdom Defense",
					Message = content,
					Ticker = content,
					Sound = true,
					Vibrate = true,
					Vibration = new int[]
					{
						500,
						500,
						500,
						500,
						500,
						500
					},
					Light = true,
					LightOnMs = 1000,
					LightOffMs = 1000,
					LightColor = Color.green,
					SmallIcon = NotificationIcon.Event,
					SmallIconColor = new Color(0f, 0.5f, 0f),
					LargeIcon = "anp_licon",
					ExecuteMode = NotificationExecuteMode.Inexact,
					CallbackData = "notification created at " + DateTime.Now
				};
				NotificationManager.SendCustom(notificationParams);
			}
			else
			{
				UnityEngine.Debug.Log("User not allow push notify");
			}
		}

		public void CancelAllNotify()
		{
			UnityEngine.Debug.Log("Cancel all notifications");
			NotificationManager.CancelAll();
		}

		private string topic = "KingdomDefense";
	}
}
