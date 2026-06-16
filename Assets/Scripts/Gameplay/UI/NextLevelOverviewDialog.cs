using System;
using Data;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class NextLevelOverviewDialog : BaseMonoBehaviour
	{
		private void Update()
		{
			if (isActive)
			{
				SetPositionFolowTower();
			}
		}

		public void Init(int ultiNo, int towerID, int towerLevel, Transform _target)
		{
			Open();
			if (MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.towerModel)
			{
				towerModel = MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.towerModel;
			}
			target = _target;
			this.towerID = towerID;
			int num = TowerParameterManager.Instance.GetUpgradeTargetLevel(towerLevel, ultiNo);
			towerParameter = TowerParameterManager.Instance.GetTowerParameter(towerID, num);
			nameText.text = Singleton<TurretSynopsis>.Instance.GetTowerName(towerID);
			descriptionText.text = Singleton<TurretSynopsis>.Instance.GetTowerShortDescription(towerID, num);
			nextLevelDamage_min = ((towerParameter.damage_Physics_min <= 0) ? towerParameter.damage_Magic_min : towerParameter.damage_Physics_min);
			nextLevelDamage_max = ((towerParameter.damage_Physics_max <= 0) ? towerParameter.damage_Magic_max : towerParameter.damage_Physics_max);
			damageText.text = nextLevelDamage_min.ToString() + "-" + nextLevelDamage_max.ToString();
			if (TowerParameterManager.Instance.isPhysicsAttack(towerID))
			{
				iconDamage.sprite = physicsDamageIcon;
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
			}
			reloadText.text = AbilityRankDescriber.Instance.GetAttackSpeedDescriptionByValue(towerParameter.reload);
			attackRangeText.text = AbilityRankDescriber.Instance.GetAttackRangeDescriptionByValue(towerParameter.attackRangeMax);
			healthText.text = towerParameter.unit_health.ToString();
			armorText.text = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(towerParameter.unit_armor_physics);
			goldProduceText.text = towerParameter.goldProduce.ToString();
			timeProduceText.text = ((float)towerParameter.reload / 1000f).ToString() + "s";
			autoCollectGoldText.text = ((float)towerParameter.autoCollectTime / 1000f).ToString() + "s";
			HideAll();
			if (towerID == 1)
			{
				ShowBarrackTowerAbility();
			}
			else if (towerID == 4)
			{
				ShowSupportTowerAbility();
			}
			else
			{
				ShowNormalTowerAbility();
			}
		}

		public void HideAll()
		{
			damageHolder.gameObject.SetActive(false);
			reloadHolder.gameObject.SetActive(false);
			attackRangeHolder.gameObject.SetActive(false);
			healthHolder.gameObject.SetActive(false);
			armorHolder.gameObject.SetActive(false);
			goldProduceHolder.gameObject.SetActive(false);
			timeProduceHolder.gameObject.SetActive(false);
			autoCollectGoldHolder.gameObject.SetActive(false);
		}

		private void ShowNormalTowerAbility()
		{
			damageHolder.gameObject.SetActive(true);
			reloadHolder.gameObject.SetActive(true);
			attackRangeHolder.gameObject.SetActive(true);
		}

		private void ShowBarrackTowerAbility()
		{
			damageHolder.gameObject.SetActive(true);
			healthHolder.gameObject.SetActive(true);
			armorHolder.gameObject.SetActive(true);
		}

		private void ShowSupportTowerAbility()
		{
			goldProduceHolder.gameObject.SetActive(true);
			timeProduceHolder.gameObject.SetActive(true);
			autoCollectGoldHolder.gameObject.SetActive(true);
		}

		private void SetPositionFolowTower()
		{
			if (target.position.x > Camera.main.transform.position.x)
			{
				offset.Set(leftVisiblePosX, MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.listOffsetTower[towerID].y, 0f);
				base.transform.position = target.position + offset;
			}
			else
			{
				offset.Set(rightVisiblePosX, MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.listOffsetTower[towerID].y, 0f);
				base.transform.position = target.position + offset;
			}
		}

		private void Open()
		{
			base.gameObject.SetActive(true);
			isActive = true;
		}

		public void Close()
		{
			isActive = false;
			towerModel = null;
			base.transform.position = PoolPos;
			base.gameObject.SetActive(false);
		}

		private TurretEntity towerModel;

		private int towerID;

		private TurretSpec towerParameter;

		private Transform target;

		[Header("UI Text")]
		[SerializeField]
		private Text nameText;

		[Header("UI Text")]
		[SerializeField]
		private Text descriptionText;

		[Header("UI Text")]
		[SerializeField]
		private Text damageText;

		[Header("UI Text")]
		[SerializeField]
		private Text healthText;

		[Header("UI Text")]
		[SerializeField]
		private Text armorText;

		[Header("UI Text")]
		[SerializeField]
		private Text reloadText;

		[Header("UI Text")]
		[SerializeField]
		private Text attackRangeText;

		[Header("UI Text")]
		[SerializeField]
		private Text goldProduceText;

		[Header("UI Text")]
		[SerializeField]
		private Text timeProduceText;

		[Header("UI Text")]
		[SerializeField]
		private Text autoCollectGoldText;

		[Header("Holder")]
		[SerializeField]
		private GameObject damageHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject healthHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject armorHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject reloadHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject attackRangeHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject goldProduceHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject timeProduceHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject autoCollectGoldHolder;

		private int nextLevelDamage_min;

		private int nextLevelDamage_max;

		private int nextLevelAtkSpeed;

		private int nextLevelAttackRange;

		private bool isActive;

		[SerializeField]
		private Vector3 offset;

		private Vector3 PoolPos = new Vector3(1000f, 200f, 0f);

		[SerializeField]
		private float leftVisiblePosX;

		[SerializeField]
		private float rightVisiblePosX;

		[Space]
		[SerializeField]
		private Image iconDamage;

		[SerializeField]
		private Sprite physicsDamageIcon;

		[SerializeField]
		private Sprite magicDamageIcon;
	}
}
