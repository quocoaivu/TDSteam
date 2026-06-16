using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class GameplayDialogHandler : DialogHandler
	{
        [Space]
        [Header("UI Visual Effect")]
        [SerializeField]
        private GameObject contentHolder;

        [SerializeField]
        private float scaleOpen = 1.1f;

        [SerializeField]
        private float scaleNormal = 1f;

        [SerializeField]
        private GameObject buttonScalerHolder;

        [SerializeField]

        private GameObject titleScaleHolder;
        public virtual void DefaultInit()
		{
		}

		public override void Open()
		{
			base.Open();
			MonoSingleton<GameRecord>.Instance.IsAnyPopupOpen = true;
			UISfxDirector.Instance.PlayOpenPopup();
		}

		public override void Close()
		{
			base.Close();
			MonoSingleton<GameRecord>.Instance.IsAnyPopupOpen = false;
			UISfxDirector.Instance.PlayClosePopup();
		}

		public virtual void OpenWithScaleAnimation()
		{
			base.Open();
			base.gameObject.SetActive(true);
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			if (contentHolder)
			{
				contentHolder.transform.localScale = Vector3.zero;
				sequence.Append(contentHolder.transform.DOScale(scaleOpen, timeToOpen));
				sequence.OnComplete(new TweenCallback(OnOpenWithScaleAnimComplete));
			}
			MonoSingleton<GameRecord>.Instance.IsAnyPopupOpen = true;
			UISfxDirector.Instance.PlayOpenPopup();
			if (buttonScalerHolder)
			{
				ScaleButton();
			}
			if (titleScaleHolder)
			{
				ScaleTitle();
			}
		}

		private void OnOpenWithScaleAnimComplete()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(contentHolder.transform.DOScale(scaleNormal, 0.2f));
		}

		public virtual void CloseWithScaleAnimation()
		{
			base.Close();
			base.transform.DOKill(false);
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			if (contentHolder)
			{
				sequence.Append(contentHolder.transform.DOScale(0f, timeToClose));
			}
			sequence.OnComplete(new TweenCallback(OnCloseWithScaleAnimComplete));
		}

		private void OnCloseWithScaleAnimComplete()
		{
			base.gameObject.SetActive(false);
			MonoSingleton<GameRecord>.Instance.IsAnyPopupOpen = false;
			UISfxDirector.Instance.PlayClosePopup();
			OnCloseAnimationComplete();
		}

		public virtual void OnCloseAnimationComplete()
		{
		}

		public virtual void CloseContentHolder()
		{
			contentHolder.SetActive(false);
		}

		private void ScaleButton()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			buttonScalerHolder.transform.localScale = Vector3.zero;
			sequence.AppendInterval(timeToOpen / 1.5f);
			sequence.Append(buttonScalerHolder.transform.DOScale(1.3f, 0.25f));
			sequence.OnComplete(new TweenCallback(OnScaleButtonComplete));
		}

		private void OnScaleButtonComplete()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(buttonScalerHolder.transform.DOScale(1f, 0.15f));
		}

		private void ScaleTitle()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			titleScaleHolder.transform.localScale = Vector3.zero;
			sequence.AppendInterval(timeToOpen);
			sequence.Append(titleScaleHolder.transform.DOScale(1.5f, 0.25f));
			sequence.OnComplete(new TweenCallback(OnScaleTitleComplete));
		}

		private void OnScaleTitleComplete()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(true);
			sequence.Append(titleScaleHolder.transform.DOScale(1f, 0.15f));
		}
	}
}
