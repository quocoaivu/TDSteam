using System;
using Data;
using DG.Tweening;
using MetaGame;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class EnhanceTurretDialogHandler : GameplayDialogHandler
	{
		public override void Update()
		{
			base.Update();
			if (null != towerModel)
			{
				UpdateUpgradeButtonsState();
			}
			if (base.gameObject.activeSelf && target)
			{
				UpdatePositionFollowTower();
			}
		}

		public void Init(TurretEntity towerModel, Transform target)
		{
			this.towerModel = towerModel;
			this.target = target;
			CacheCurrentTowerController();
			SetUpgrateButtonFolowTower();
			groupControllTowerButtons.DisableConfirmAllButton();
			currentLevelInformationPopup.Init();
			nextLevelInfomationPopoup.Close();
			ultimateInforGroup.HideList();
			if (itemPanel != null)
			{
				itemPanel.Init(towerModel);
			}
			Open();
			ShowRange(towerModel);
			TryToFocusTowerPosition();
		}

		private void TryToFocusTowerPosition()
		{
			if (base.gameObject.activeSelf && target)
			{
				UpdatePositionFollowTower();
			}
			if ((rectTransform.localPosition.y > canvasHolder.sizeDelta.y / 2f - rectTransform.sizeDelta.y / 2f || rectTransform.localPosition.y < -canvasHolder.sizeDelta.y / 2f + rectTransform.sizeDelta.y / 2f) && target)
			{
				MonoSingleton<LensHandler>.Instance.PinchZoomFov.TryToMoveToBuildTowerPosition(target.position);
			}
		}

		private void ShowRange(TurretEntity _towerModel)
		{
			TurretRangeHandler component = GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>();
			component.target = _towerModel.transform;
			if (towerFindEnemyController != null)
			{
				component.SetRangeAttackMax(towerFindEnemyController.BuffedAttackRangeMax);
			}
			else
			{
				component.SetRangeAttackMax((float)_towerModel.OriginalParameter.attackRangeMax / GameRecord.PIXEL_PER_UNIT);
			}
		}

		private void CacheCurrentTowerController()
		{
			towerAttackSingleTargetController = towerModel.GetComponent<TurretStrikeSingleMarkHandler>();
			towerFindEnemyController = towerModel.GetComponent<TurretSeekEnemyHandler>();
		}

		private void UpdatePositionFollowTower()
		{
			base.transform.position = target.position + offset;
		}

		private void UpdateUpgradeButtonsState()
		{
			// No-op: in-match level/ultimate upgrades are retired; the upgrade buttons are hidden and
			// have no per-frame state to refresh.
		}

		public void OnUpgrade(int ultimateBranch)
		{
			// The shared `canUpgrade` field reflects whichever branch was painted
			// last (branch 1 at the ultimate tier), so it can't be trusted for
			// branch 0. Re-evaluate affordability for the branch actually clicked,
			// matching the level semantics used in UpdateUpgradeButtonsState
			// (branch 0 -> current level, branch 1 -> target level).
			int level = (ultimateBranch == 1)
				? TowerParameterManager.Instance.GetUpgradeTargetLevel(towerModel.OriginalParameter.level, 1)
				: towerModel.OriginalParameter.level;
			if (towerModel.GetUpgradeEnable(level) == CanEnhanceStanding.CanUpgrade)
			{
				towerModel.Upgrade(ultimateBranch);
				Close();
			}
		}

		private void SetUpgrateButtonFolowTower()
		{
			// Level-up / ultimate-branch / mastery upgrades are retired: tower power now comes from the
			// permanent skill tree bought between matches. So the in-match upgrade buttons are always
			// hidden. The popup still shows sell, range and info.
			upgradeButtonController.gameObject.SetActive(false);
			ultimateUpgradeButtonController[0].gameObject.SetActive(false);
			ultimateUpgradeButtonController[1].gameObject.SetActive(false);
			upgradeUltimate0ButtonController.gameObject.SetActive(false);
			upgradeUltimate1ButtonController.gameObject.SetActive(false);
			ultimateInforButtonController.gameObject.SetActive(false);
		}

		public void OnSell()
		{
			towerModel.Sell();
			Close();
		}

		protected override void OnClickOutsideUp()
		{
			base.OnClickOutsideUp();
			Close();
			GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>().HideRange();
		}

		public override void Open()
		{
			base.Open();
			if (MonoSingleton<UIRootHandler>.Instance.BuyTowerPopupController.isOpen)
			{
				MonoSingleton<UIRootHandler>.Instance.BuyTowerPopupController.Close();
			}
			base.gameObject.SetActive(true);
			offset = listOffsetTower[towerModel.Id];
			if (towerModel.Id == 1)
			{
				buttonChangePosition.SetActive(true);
			}
			else
			{
				buttonChangePosition.SetActive(false);
			}
			tween.Kill(false);
			Content.transform.localScale = 0.5f * Vector3.one;
			tween = Content.transform.DOScale(1f, timeToOpen).SetEase(Ease.OutBack).OnComplete(new TweenCallback(OnOpenComplete));
			if (towerControllerCollider == null)
			{
				towerControllerCollider = UnityEngine.Object.Instantiate(Common.AssetLoader.Load<GameObject>("UI Gameplay/Popups/TowerControllerCollider"));
			}
			Vector3 position = target.position;
			position.z = -0.5f;
			position.y += 0.3f;
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
			nextLevelInfomationPopoup.Close();
			currentLevelInformationPopup.Close();
			ultimateInforGroup.HideList();
			tween.Kill(false);
			tween = Content.transform.DOScale(0f, timeToClose).SetEase(Ease.InBack).OnComplete(new TweenCallback(OnCloseComplete));
			if (towerControllerCollider != null)
			{
				towerControllerCollider.SetActive(false);
			}
		}

		private void OnCloseComplete()
		{
			base.transform.position = PoolPos;
			base.gameObject.SetActive(false);
		}

		[Space]
		[Header("Content Holder")]
		[SerializeField]
		private GameObject Content;

		[Space]
		[Header("List offset towers")]
		[SerializeField]
		public Vector3[] listOffsetTower;

		private Vector3 offset;

		[SerializeField]
		private GameObject buttonChangePosition;

		[Space]
		[Header("Controll tower group")]
		public TowerUpgradeButtonsGroupController groupControllTowerButtons;

		[SerializeField]
		private EnhanceSwitchHandler upgradeButtonController;

		[SerializeField]
		private EnhanceSwitchHandler[] ultimateUpgradeButtonController;

		[SerializeField]
		private TurretAbilityEnhanceSwitchHandler upgradeUltimate0ButtonController;

		[SerializeField]
		private TurretAbilityEnhanceSwitchHandler upgradeUltimate1ButtonController;

		[Space]
		[Header("Ultimate Upgrade")]
		[SerializeField]
		private UltimateInformationButtonController ultimateInforButtonController;

		public UltimateInformationGroup ultimateInforGroup;

		[Space]
		[Header("Item equip")]
		[SerializeField]
		private Items.TowerItemPanel itemPanel;

		[Space]
		[Header("Pop-ups")]
		public CurrentLevelOverviewDialog currentLevelInformationPopup;

		public NextLevelOverviewDialog nextLevelInfomationPopoup;

		private CanEnhanceStanding canUpgrade;

		[HideInInspector]
		public TurretEntity towerModel;

		private TurretStrikeSingleMarkHandler towerAttackSingleTargetController;

		private TurretSeekEnemyHandler towerFindEnemyController;

		private Transform target;

		private Vector3 PoolPos = new Vector3(2000f, 2000f, 0f);

		[Space]
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private RectTransform canvasHolder;

		private GameObject towerControllerCollider;
	}
}
