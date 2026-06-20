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
			// Item-preview mode only follows the tower; skip base.Update so the dialog's click-outside logic
			// doesn't close the preview when the player taps a tower to equip.
			if (isItemPreview)
			{
				if (base.gameObject.activeSelf && target)
				{
					UpdatePositionFollowTower();
				}
				return;
			}
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

		// Lightweight preview shown while the player carries a shop/inventory item and hovers this tower:
		// reveals the item equip panel + range so they see where the item lands, WITHOUT the full popup
		// machinery (no click-collider, open sound, camera focus, or IsAnyPopupOpen). That keeps the world tap
		// that equips the item working and stops the hover from flickering. Rebinds to whatever tower is
		// passed, so it doubles as the "moved to another tower" update. Pair with HideItemPreview.
		public void ShowItemPreview(TurretEntity tower)
		{
			if (tower == null)
			{
				return;
			}
			isItemPreview = true;
			towerModel = tower;
			target = tower.transform;
			CacheCurrentTowerController();
			ResetUpgradeChrome();
			if (itemPanel != null)
			{
				itemPanel.Init(tower);
			}
			offset = listOffsetTower[tower.Id];
			base.gameObject.SetActive(true);
			// Preview is look-only: don't let its buttons/background eat the tap (would block the equip click
			// or sell the tower by accident).
			PreviewCanvasGroup().blocksRaycasts = false;
			UpdatePositionFollowTower();
			tween.Kill(false);
			if (Content != null)
			{
				Content.transform.localScale = Vector3.one;
			}
			ShowRange(tower);
		}

		private CanvasGroup PreviewCanvasGroup()
		{
			if (previewCanvasGroup == null)
			{
				previewCanvasGroup = GetComponent<CanvasGroup>();
				if (previewCanvasGroup == null)
				{
					previewCanvasGroup = base.gameObject.AddComponent<CanvasGroup>();
				}
			}
			return previewCanvasGroup;
		}

		public void HideItemPreview()
		{
			if (!isItemPreview)
			{
				return;
			}
			isItemPreview = false;
			GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>().HideRange();
			// The info chrome ShowItemPreview opened lives in separate objects (not children of this popup), so
			// deactivating this GameObject alone won't hide them — close them explicitly.
			CloseUpgradeChrome();
			base.transform.position = PoolPos;
			base.gameObject.SetActive(false);
			towerModel = null;
			target = null;
		}

		public void Init(TurretEntity towerModel, Transform target)
		{
			isItemPreview = false;
			// Restore interactivity in case the last show was a (look-only) item preview.
			PreviewCanvasGroup().blocksRaycasts = true;
			this.towerModel = towerModel;
			this.target = target;
			CacheCurrentTowerController();
			ResetUpgradeChrome();
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

		// Resets the in-match upgrade/level/ultimate chrome. That chrome lives under the optional, retired
		// ContentHolder, so every reference is null-guarded: the popup still works as just the item panel +
		// range if ContentHolder (and its children) is deleted from the scene.
		private void ResetUpgradeChrome()
		{
			SetUpgrateButtonFolowTower();
			if (groupControllTowerButtons != null)
			{
				groupControllTowerButtons.DisableConfirmAllButton();
			}
			if (currentLevelInformationPopup != null)
			{
				currentLevelInformationPopup.Init();
			}
			if (nextLevelInfomationPopoup != null)
			{
				nextLevelInfomationPopoup.Close();
			}
			if (ultimateInforGroup != null)
			{
				ultimateInforGroup.HideList();
			}
		}

		// Hides the info chrome ResetUpgradeChrome shows (current/next level, ultimate). These are separate
		// objects, so both Close() and HideItemPreview() must call this to fully tear the popup down.
		private void CloseUpgradeChrome()
		{
			if (nextLevelInfomationPopoup != null)
			{
				nextLevelInfomationPopoup.Close();
			}
			if (currentLevelInformationPopup != null)
			{
				currentLevelInformationPopup.Close();
			}
			if (ultimateInforGroup != null)
			{
				ultimateInforGroup.HideList();
			}
		}

		private void SetUpgrateButtonFolowTower()
		{
			// Level-up / ultimate-branch / mastery upgrades are retired: tower power now comes from the
			// permanent skill tree bought between matches. So the in-match upgrade buttons are always
			// hidden. Null-guarded because these controllers live under the deletable ContentHolder.
			HideIfPresent(upgradeButtonController);
			if (ultimateUpgradeButtonController != null)
			{
				for (int i = 0; i < ultimateUpgradeButtonController.Length; i++)
				{
					HideIfPresent(ultimateUpgradeButtonController[i]);
				}
			}
			HideIfPresent(upgradeUltimate0ButtonController);
			HideIfPresent(upgradeUltimate1ButtonController);
			HideIfPresent(ultimateInforButtonController);
		}

		private static void HideIfPresent(MonoBehaviour controller)
		{
			if (controller != null)
			{
				controller.gameObject.SetActive(false);
			}
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
			if (buttonChangePosition != null)
			{
				buttonChangePosition.SetActive(towerModel.Id == 1);
			}
			tween.Kill(false);
			if (Content != null)
			{
				Content.transform.localScale = 0.5f * Vector3.one;
				tween = Content.transform.DOScale(1f, timeToOpen).SetEase(Ease.OutBack).OnComplete(new TweenCallback(OnOpenComplete));
			}
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
			CloseUpgradeChrome();
			tween.Kill(false);
			if (Content != null)
			{
				tween = Content.transform.DOScale(0f, timeToClose).SetEase(Ease.InBack).OnComplete(new TweenCallback(OnCloseComplete));
			}
			else
			{
				// No ContentHolder to animate (deleted): close immediately.
				OnCloseComplete();
			}
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

		private bool isItemPreview;

		private CanvasGroup previewCanvasGroup;
	}
}
