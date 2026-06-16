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
			if (towerModel.OriginalParameter.level < 2)
			{
				canUpgrade = towerModel.GetUpgradeEnable(towerModel.OriginalParameter.level);
				upgradeButtonController.UpdateStatusButton(canUpgrade);
			}
			// At the top base tier the player chooses an ultimate branch: button[0] -> branch 0, button[1] -> branch 1.
			if (towerModel.OriginalParameter.level == TowerParameterManager.MAX_BASE_LEVEL)
			{
				canUpgrade = towerModel.GetUpgradeEnable(towerModel.OriginalParameter.level);
				ultimateUpgradeButtonController[0].UpdateStatusButton(canUpgrade);
				canUpgrade = towerModel.GetUpgradeEnable(TowerParameterManager.Instance.GetUpgradeTargetLevel(towerModel.OriginalParameter.level, 1));
				ultimateUpgradeButtonController[1].UpdateStatusButton(canUpgrade);
			}
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
			int num = 0;
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						string currentSeasonID = ZoneRuleSpec.Instance.GetCurrentSeasonID();
						num = ZoneRuleSpec.Instance.GetMaxLevelTowerCanUpgrade_Tournament(currentSeasonID, towerModel.Id);
					}
				}
				else
				{
					int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
					int currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
					num = ZoneRuleSpec.Instance.GetMaxLevelTowerCanUpgrade_Daily(currentWave, towerModel.Id);
				}
			}
			else
			{
				num = ZoneRuleSpec.Instance.GetMaxLevelTowerCanUpgrade_Campaign(MonoSingleton<GameRecord>.Instance.MapID, towerModel.Id);
			}
			if (towerModel.OriginalParameter.level == num)
			{
			}
			if (towerModel.OriginalParameter.level < 2)
			{
				bool isAllowedToUpgrade = towerModel.OriginalParameter.level < num;
				int price = TowerParameterManager.Instance.GetPrice(towerModel.OriginalParameter.id, towerModel.OriginalParameter.level + 1);
				ultimateInforButtonController.gameObject.SetActive(false);
				upgradeButtonController.gameObject.SetActive(true);
				upgradeButtonController.Init(towerModel, isAllowedToUpgrade, price);
				ultimateUpgradeButtonController[0].gameObject.SetActive(false);
				ultimateUpgradeButtonController[1].gameObject.SetActive(false);
				upgradeUltimate0ButtonController.gameObject.SetActive(false);
				upgradeUltimate1ButtonController.gameObject.SetActive(false);
			}
			else if (towerModel.OriginalParameter.level == 2)
			{
				ultimateInforButtonController.gameObject.SetActive(false);
				upgradeButtonController.gameObject.SetActive(false);
				ultimateUpgradeButtonController[0].gameObject.SetActive(true);
				ultimateUpgradeButtonController[1].gameObject.SetActive(true);
				upgradeUltimate0ButtonController.gameObject.SetActive(false);
				upgradeUltimate1ButtonController.gameObject.SetActive(false);
				if (num == 3)
				{
					ultimateUpgradeButtonController[0].Init(towerModel, true, TowerParameterManager.Instance.GetPrice(towerModel.OriginalParameter.id, 3));
					ultimateUpgradeButtonController[1].Init(towerModel, false, TowerParameterManager.Instance.GetPrice(towerModel.OriginalParameter.id, 4));
				}
				else if (num == 4)
				{
					ultimateUpgradeButtonController[0].Init(towerModel, true, TowerParameterManager.Instance.GetPrice(towerModel.OriginalParameter.id, 3));
					ultimateUpgradeButtonController[1].Init(towerModel, true, TowerParameterManager.Instance.GetPrice(towerModel.OriginalParameter.id, 4));
				}
				else
				{
					ultimateUpgradeButtonController[0].Init(towerModel, false, 0);
					ultimateUpgradeButtonController[1].Init(towerModel, false, 0);
				}
			}
			else
			{
				ultimateInforButtonController.gameObject.SetActive(true);
				upgradeButtonController.gameObject.SetActive(false);
				ultimateUpgradeButtonController[0].gameObject.SetActive(false);
				ultimateUpgradeButtonController[1].gameObject.SetActive(false);
				upgradeUltimate0ButtonController.gameObject.SetActive(true);
				upgradeUltimate0ButtonController.Init(towerModel);
				upgradeUltimate1ButtonController.gameObject.SetActive(true);
				upgradeUltimate1ButtonController.Init(towerModel);
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
