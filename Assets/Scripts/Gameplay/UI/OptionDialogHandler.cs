using System;
using DG.Tweening;
using Gameplay.Setting;
using MetaGame;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class OptionDialogHandler : GameplayDialogHandler
	{
		public void InitConfirmGroup(CancelGameKind cancelGameType)
		{
			settingPanel.gameObject.SetActive(false);
			confirmGroup.gameObject.SetActive(true);
			confirmGroup.Init(cancelGameType);
		}

		public void CloseConfirmGroup()
		{
			confirmGroup.gameObject.SetActive(false);
		}

		public override void Open()
		{
			base.Open();
			base.gameObject.SetActive(true);
			settingPanel.gameObject.SetActive(true);
			CloseConfirmGroup();
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(settingPanel.transform.DOLocalMoveY(-10f, timeToOpen, false));
			sequence.OnComplete(new TweenCallback(LateAnimationOpen));
			if (FormatDirector.Instance.gameMode == GameFormat.TournamentMode)
			{
				restartButtonController.SetUnClickable();
			}
			else
			{
				restartButtonController.SetClickable();
			}
			GameplayDirector.Instance.gameSpeedController.PauseGame();
		}

		private void LateAnimationOpen()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(settingPanel.transform.DOLocalMoveY(0f, 0.1f, false));
		}

		public override void Close()
		{
			base.Close();
			CloseConfirmGroup();
			base.transform.DOKill(false);
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(settingPanel.transform.DOLocalMoveY(800f, timeToClose, false)).OnComplete(new TweenCallback(LateAnimationClose));
		}

		private void LateAnimationClose()
		{
			base.gameObject.SetActive(false);
			GameplayDirector.Instance.gameSpeedController.UnPauseGame();
		}

		public UnityEvent OnOpen;

		public UnityEvent OnClose;

		[SerializeField]
		private GameObject settingPanel;

		[SerializeField]
		private ConfirmCluster confirmGroup;

		[SerializeField]
		private RestartSwitchHandler restartButtonController;
	}
}
