using System;
using Bootstrap;
using Data;
using Services.PlatformSpecific;
using UnityEngine;

namespace FreeResources
{
	public class ShareFanpageSwitchHandler : FreeResourcesSwitchHandler
	{
        private int currentShareAmount;

        public override void InitData()
		{
			base.InitData();
			SetGemData();
			SetShareCountData();
			SetDisplayByRemoteSetting();
		}

		private void SetGemData()
		{
			if (!oneTimeOnlyReward)
			{
				titleReceived.SetActive(false);
			}
			//gemAmount.text = "+ " + NativeSpecificServicesSource.Services.FacebookServices.GetFreeResources("reward_id_share_fanpage").ToString();
		}

		private void SetShareCountData()
		{
			currentShareAmount = FreeResourcesStore.Instance.GetCurrentSharePerDay();
			if (currentShareAmount > 0)
			{
				titleReceived.SetActive(false);
				gemAmount.gameObject.SetActive(true);
				HideCountdownTime();
			}
			else
			{
				titleReceived.SetActive(true);
				gemAmount.gameObject.SetActive(false);
				DisplayCountdownTime();
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			if (currentShareAmount > 0)
			{
				//NativeSpecificServicesSource.Services.FacebookServices.ShareFanpage();
				currentShareAmount--;
				FreeResourcesStore.Instance.SetCurrentSharePerDay(currentShareAmount);
				if (currentShareAmount == 0)
				{
					DisplayCountdownTime();
				}
				InitData();
			}
			else
			{
				UnityEngine.Debug.Log("Da het luot share trong ngay!");
			}
		}

		private void DisplayCountdownTime()
		{
			timeCountDown.gameObject.SetActive(true);
		}

		private void HideCountdownTime()
		{
			timeCountDown.gameObject.SetActive(false);
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
