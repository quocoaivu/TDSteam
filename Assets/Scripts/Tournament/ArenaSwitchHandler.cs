using System;
using Data;
using LifetimePopup;
using GameCore;
using Parameter;
using WorldMap;

namespace Tournament
{
	public class ArenaSwitchHandler : SwitchHandler
	{
		public override void OnClick()
		{
			base.OnClick();
			TryToOpenTournamentPopup();
		}

		private void TryToOpenTournamentPopup()
		{
			// TEST: with local tournament data there is no backend, and the real userID
			// setter (Firebase auth) is disabled so GetUserID() stays "empty" and the gate
			// below never passes. Assign a dummy id and open directly. Remove with the flag.
			if (GeneralVariable.GeneralDefine.TOURNAMENT_USE_LOCAL_DATA)
			{
				string testUserID = UserProfileStore.Instance.GetUserID();
				if (string.IsNullOrEmpty(testUserID) || testUserID == "empty")
				{
					UserProfileStore.Instance.SetUserID("local_test_user");
				}
				OpenTournamentPopup();
				return;
			}
			if (GameUtils.IsInternetConnectionAvailable())
			{
				string userID = UserProfileStore.Instance.GetUserID();
				if (string.IsNullOrEmpty(userID) || userID == "empty")
				{
					string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(133);
					MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, "OK", null, delegate()
					{
						MonoSingleton<UIRootHandler>.Instance.userProfilePopupController.Init();
					});
				}
				else
				{
					OpenTournamentPopup();
				}
			}
			else
			{
				string notiContent2 = Singleton<AlertSynopsis>.Instance.GetNotiContent(119);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent2, false, false);
			}
		}

		private void OpenTournamentPopup()
		{
			MonoSingleton<UIRootHandler>.Instance.tournamentPopupController.Init();
		}
	}
}
