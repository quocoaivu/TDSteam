using System;
using System.Collections.Generic;
using Data;
using Services.PlatformSpecific;
using UnityEngine;
using WorldMap;

namespace UserProfile
{
	public class SelectSectorSwitchHandler : SwitchHandler
	{
        [SerializeField]
        private string countryCode;

        public override void OnClick()
		{
			base.OnClick();
			UserProfileStore.Instance.SetRegionCode(countryCode);
			MonoSingleton<UIRootHandler>.Instance.userProfilePopupController.ChangeRegionPopupController.CloseWithScaleAnimation();
			string userID = UserProfileStore.Instance.GetUserID();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(string.Format("Tournament/Users/{0}/country", userID), countryCode);
			if (GameKit.tourUserSelfInfo != null)
			{
				dictionary.Add(string.Format("Tournament/Curseasongroups/{0}/{1}/country", GameKit.tourUserSelfInfo.curgroupid, userID), countryCode);
			}
			NativeSpecificServicesSource.Services.DataCloudSaver.UpdateData(dictionary, null);
		}
	}
}
