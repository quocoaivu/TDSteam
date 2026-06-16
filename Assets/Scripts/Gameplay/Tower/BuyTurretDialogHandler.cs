using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class BuyTurretDialogHandler : GameplayDialogHandler
	{
        private Transform target;

        private Vector3 outSide = new Vector3(2000f, 2000f, 0f);

        [Space]
        [Header("Content Holder")]
        [SerializeField]
        private GameObject Content;

        [Space]
        [Header("Controll tower group")]
        public TowerBuyButtonsGroupController groupControllTowerButtons;

        [Space]
        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private RectTransform canvasHolder;


        private GameObject towerControllerCollider;
        public override void Update()
		{
			base.Update();
			if (base.gameObject.activeSelf && target)
			{
				UpdatePositionFollowBuildRegion();
			}
		}

		public void Init(Transform target)
		{
			this.target = target;
			Open();
			groupControllTowerButtons.InitButtonsStatusByWave();
			TryToFocusTowerPosition();
		}

		private void TryToFocusTowerPosition()
		{
			if (base.gameObject.activeSelf && target)
			{
				UpdatePositionFollowBuildRegion();
			}
			if ((rectTransform.localPosition.y > canvasHolder.sizeDelta.y / 2f - rectTransform.sizeDelta.y / 2f || rectTransform.localPosition.y < -canvasHolder.sizeDelta.y / 2f + rectTransform.sizeDelta.y / 2f) && target)
			{
				MonoSingleton<LensHandler>.Instance.PinchZoomFov.TryToMoveToBuildTowerPosition(target.position);
			}
		}

		private void UpdatePositionFollowBuildRegion()
		{
			base.transform.position = target.position;
		}

		protected override void OnClickOutsideUp()
		{
			base.OnClickOutsideUp();
			Close();
		}

		public override void Open()
		{
			base.Open();
			if (MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.isOpen)
			{
				MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.Close();
			}
			base.gameObject.SetActive(true);
			groupControllTowerButtons.DisableConfirmAllButton();
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Close();
			GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>().HideRange();
			tween.Kill(false);
			Content.transform.localScale = 0.5f * Vector3.one;
			tween = Content.transform.DOScale(1f, timeToOpen).SetEase(Ease.OutBack).OnComplete(new TweenCallback(OnOpenComplete));
			if (towerControllerCollider == null)
			{
				towerControllerCollider = UnityEngine.Object.Instantiate(Common.AssetLoader.Load<GameObject>("UI Gameplay/Popups/TowerControllerCollider"));
			}
			Vector3 position = target.position;
			position.z = -0.5f;
			towerControllerCollider.transform.position = position;
			towerControllerCollider.SetActive(true);
		}

		private void OnOpenComplete()
		{
		}

		public override void Close()
		{
			base.Close();
			target = null;
			groupControllTowerButtons.DisableConfirmAllButton();
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Close();
			if (!MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.isOpen)
			{
				GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>().HideRange();
			}
			tween.Kill(false);
			tween = Content.transform.DOScale(0.1f, timeToClose).SetEase(Ease.InBack).OnComplete(new TweenCallback(OnCloseComplete));
			if (towerControllerCollider != null)
			{
				towerControllerCollider.SetActive(false);
			}
		}

		private void OnCloseComplete()
		{
			base.transform.position = outSide;
			base.gameObject.SetActive(false);
		}
	}
}
