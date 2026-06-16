using System;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class EnemyDiscoveryButton : SwitchHandler
	{
		public int EnemyId
		{
			get
			{
				return enemyId;
			}
		}

		public void Init(int enemyID)
		{
			enemyId = enemyID;
			SetAvatar(enemyID);
		}

		private void SetAvatar(int enemyID)
		{
			enemyFullAvatar = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Enemies/p_enemy_{0}", enemyID));
			avatar.sprite = enemyFullAvatar;
			avatar.SetNativeSize();
		}

		public void ShowButton(float buttonLifeTime)
		{
			base.gameObject.SetActive(true);
			base.CustomInvoke(new Action(HideButton), buttonLifeTime);
			SendEventShowButton();
			UISfxDirector.Instance.PlayNewEnemyButton();
		}

		private void SendEventShowButton()
		{
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_ShowTipsButton("New Enemy", EnemyId);
		}

		public override void OnClick()
		{
			base.OnClick();
			ShowCard();
			HideButton();
		}

		private void ShowCard()
		{
			MonoSingleton<UIRootHandler>.Instance.EnemyDiscoveryPopup.Init(EnemyId);
		}

		private void HideButton()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private Image avatar;

		private Sprite enemyFullAvatar;

		private int enemyId;
	}
}
