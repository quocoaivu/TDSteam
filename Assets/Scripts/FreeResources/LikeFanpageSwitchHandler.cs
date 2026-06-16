using System;
using Bootstrap;
using Data;
using Services.PlatformSpecific;

namespace FreeResources
{
	public class LikeFanpageSwitchHandler : FreeResourcesSwitchHandler
	{
		public override void InitData()
		{
			base.InitData();
			SetGemData();
			SetDisplayByRemoteSetting();
		}

		private void SetGemData()
		{
			if (oneTimeOnlyReward)
			{
				if (FreeResourcesStore.Instance.IsUserGetReward_LikeFanpage())
				{
					titleReceived.SetActive(true);
					gemAmount.gameObject.SetActive(false);
				}
				else
				{
					titleReceived.SetActive(false);
					gemAmount.gameObject.SetActive(true);
				}
			}
			else
			{
				titleReceived.SetActive(false);
				gemAmount.gameObject.SetActive(true);
			}
			//gemAmount.text = "+ " + NativeSpecificServicesSource.Services.FacebookServices.GetFreeResources("reward_id_like_fanpage").ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			//NativeSpecificServicesSource.Services.FacebookServices.LikeFanpage();
			InitData();
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
