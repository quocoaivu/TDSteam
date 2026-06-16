using System;
using DG.Tweening;
using LifetimePopup;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class FreeResourcesDialogHandler : GameplayDialogHandler
	{
        [SerializeField]
        private GameObject groupButton;

        [Space]
        [SerializeField]
        private GameplayClipSwitchHandler videoMoney;

        [SerializeField]
        private GameplayClipSwitchHandler videoLife;

        private bool videoMoneyChanged;

        private bool videoLifeChanged;

        [Space]
        [SerializeField]
        private AdRewardProvider readDataAdsReward;


        public GameplayClipSwitchHandler VideoMoney
		{
			get
			{
				return videoMoney;
			}
			private set
			{
				videoMoney = value;
			}
		}

		public GameplayClipSwitchHandler VideoLife
		{
			get
			{
				return videoLife;
			}
			private set
			{
				videoLife = value;
			}
		}

		public AdRewardProvider AdRewardProvider
		{
			get
			{
				return readDataAdsReward;
			}
			set
			{
				readDataAdsReward = value;
			}
		}

		public void Init()
		{
			VideoMoney.RefreshStatus();
			VideoLife.RefreshStatus();
			Open();
		}

		public void PlayVideo_Money()
		{
			if (ClipPlayerDirector.Instance.CheckIfVideoExits())
			{
				ClipPlayerDirector.Instance.playVideoGameplay_ForMoney();
			}
			else
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(19);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			}
		}

		public void PlayVideo_Life()
		{
			if (ClipPlayerDirector.Instance.CheckIfVideoExits())
			{
				ClipPlayerDirector.Instance.playVideoGameplay_ForLife();
			}
			else
			{
				string notiContent = Singleton<AlertSynopsis>.Instance.GetNotiContent(19);
				MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Init(notiContent, false, false);
			}
		}

		public override void Open()
		{
			base.Open();
			base.gameObject.SetActive(true);
			groupButton.gameObject.SetActive(true);
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(groupButton.transform.DOLocalMoveY(-10f, timeToOpen, false));
			sequence.OnComplete(new TweenCallback(LateAnimationOpen));
			GameplayDirector.Instance.gameSpeedController.PauseGame();
		}

		private void LateAnimationOpen()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(groupButton.transform.DOLocalMoveY(0f, 0.1f, false));
		}

		public override void Close()
		{
			base.Close();
			base.transform.DOKill(false);
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(groupButton.transform.DOLocalMoveY(800f, timeToClose, false)).OnComplete(new TweenCallback(LateAnimationClose));
		}

		private void LateAnimationClose()
		{
			base.gameObject.SetActive(false);
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
		}

		public override void Update()
		{
			base.Update();
			if (!videoLifeChanged & MonoSingleton<GameRecord>.Instance.PlayedGameplayVideo_ForLife)
			{
				VideoLife.IsPlayed = true;
				VideoLife.RefreshStatus();
				videoLifeChanged = true;
			}
			if (!videoMoneyChanged & MonoSingleton<GameRecord>.Instance.PlayedGameplayVideo_ForMoney)
			{
				VideoMoney.IsPlayed = true;
				VideoMoney.RefreshStatus();
				videoMoneyChanged = true;
			}
		}
	}
}
