using System;

namespace Services.PlatformSpecific
{
	public interface ISocialServices
	{
		void LogIn();

		void LogOut();

		void LikeFanpage();

		void InviteFriend();

		void ShareFanpage();

		void ShareLinkGame(StageTag sceneName, int currentMapID);

		void SharePromotionImage(int imageID);

		void ShareScreenShot();

		void InviteToGroup();

		int GetFreeResources(string rewardID);

		void SendEvent_GetFreeResourcesComplete(FreeResourcesKind freeResourcesType);
	}
}
