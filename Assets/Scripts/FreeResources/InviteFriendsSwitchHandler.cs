using System;
using Bootstrap;
using Data;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace FreeResources
{
	public class InviteFriendsSwitchHandler : FreeResourcesSwitchHandler
	{
        [SerializeField]
        private Text currentGemRewardProgress;

        private int currentGem;

        private int maxGem;

        public override void InitData()
		{
			base.InitData();
			SetGemData();
			SetCurrentGemRewardProgress();
			SetDisplayByRemoteSetting();
		}

		private void SetCurrentGemRewardProgress()
		{
			currentGem = FreeResourcesStore.Instance.GetCurrentGemCollectedByInvite();
			maxGem = 100;
			currentGemRewardProgress.text = currentGem + "/" + maxGem;
		}

		private void SetGemData()
		{
			if (!oneTimeOnlyReward)
			{
				titleReceived.SetActive(false);
			}
			//gemAmount.text = "+ " + NativeSpecificServicesSource.Services.FacebookServices.GetFreeResources("reward_id_invite_friend").ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			if (currentGem < maxGem)
			{
				//NativeSpecificServicesSource.Services.FacebookServices.InviteFriend();
				InitData();
			}
			else
			{
				UnityEngine.Debug.Log("ÄÃ£ Ä‘áº¡t max sá»‘ lÆ°á»£ng nháº­n thÆ°á»Ÿng invite friends!");
			}
		}

		private void SetDisplayByRemoteSetting()
		{
			if (visualDependOnRemoteSetting)
			{
				if (Bootstrap.GameBootstrap.Instance.RemoteConfig.IsDisplayFreeGem())
				{
					gemAmount.gameObject.SetActive(true);
					notification.gameObject.SetActive(true);
					icon.sprite = sprite_gem_chest;
					icon.SetNativeSize();
				}
				else
				{
					gemAmount.gameObject.SetActive(false);
					notification.gameObject.SetActive(false);
					icon.sprite = sprite_normal;
					titleReceived.SetActive(false);
				}
			}
		}
	}
}
