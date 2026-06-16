using System;
using Data;
using MetaGame;
using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class AlertEditor : MonoBehaviour, IAlert
	{
		public void CancelAllNotify()
		{
			UnityEngine.Debug.Log("Cancel all notifications!");
		}

		public void PushNotify(string content, int delayTimeBySecond)
		{
			if (Setup.Instance.AllowPushNoti)
			{
				string userRegionCode = UserProfileStore.Instance.GetUserRegionCode();
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"User allow push notify: Editor Notify with country code: ",
					userRegionCode,
					" content: ",
					content,
					" delay time: ",
					delayTimeBySecond
				}));
			}
			else
			{
				UnityEngine.Debug.Log("User not allow push notify");
			}
		}
	}
}
