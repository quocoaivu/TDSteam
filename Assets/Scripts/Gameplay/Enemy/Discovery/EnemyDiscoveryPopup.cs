using System;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class EnemyDiscoveryPopup : GameplayDialogHandler
	{
		public void Init(int enemyID)
		{
			this.enemyID = enemyID;
			base.OpenWithScaleAnimation();
			GameplayDirector.Instance.gameSpeedController.PauseGame();
			imageAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("Preview/Enemies/FullAvatars/fa_enemy_{0}", enemyID));
			enemyName.text = Singleton<EnemyBioConfig>.Instance.GetEnemyName(enemyID);
			enemyDescription.text = Singleton<EnemyBioConfig>.Instance.GetEnemyDescription(enemyID).Replace('@', '\n').Replace('#', '-');
			enemySpecialAbility.text = Singleton<EnemyBioConfig>.Instance.GetEnemySpecialAbility(enemyID).Replace('@', '\n').Replace('#', '-');
			SendEventOpenPopup();
		}

		private void SendEventOpenPopup()
		{
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_OpenTipsPopup("New Enemy", enemyID);
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
		}

		[Space]
		[SerializeField]
		private Image imageAvatar;

		[SerializeField]
		private Text enemyName;

		[SerializeField]
		private Text enemyDescription;

		[SerializeField]
		private Text enemySpecialAbility;

		private int enemyID;
	}
}
