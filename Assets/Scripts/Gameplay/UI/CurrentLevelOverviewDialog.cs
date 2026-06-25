using System;
using Data;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class CurrentLevelOverviewDialog : BaseMonoBehaviour
	{
        private TurretEntity towerModel;

        [Header("UI Text")]
        [SerializeField]
        private Text nameText;

        [Header("UI Text")]
        [SerializeField]
        private Text typeText;

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

        [Space]
        [SerializeField]
        private Image iconDamage;

        [SerializeField]
        private Sprite physicsDamageIcon;

        [SerializeField]
        private Sprite magicDamageIcon;

        private int currentLevelDamage_min;


        private int currentLevelDamage_max;
        public void Init()
		{
			Open();
			if (MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.towerModel)
			{
				towerModel = MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.towerModel;
			}
			nameText.text = Singleton<TurretSynopsis>.Instance.GetTowerName(towerModel.Id);
			typeText.text = Singleton<TurretSynopsis>.Instance.GetTowerType(towerModel.Id);
			currentLevelDamage_min = towerModel.OriginalParameter.damage;
			currentLevelDamage_max = towerModel.OriginalParameter.damage;
			damageText.text = currentLevelDamage_min.ToString();
			if (TowerParameterManager.Instance.isPhysicsAttack(towerModel.Id))
			{
				iconDamage.sprite = physicsDamageIcon;
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
			}
			healthText.text = towerModel.OriginalParameter.unit_health.ToString();
			armorText.text = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(towerModel.OriginalParameter.unit_armor);
			float reloadMs = towerModel.OriginalParameter.attackSpeed > 0 ? 1000f / towerModel.OriginalParameter.attackSpeed : 0f;
			reloadText.text = AbilityRankDescriber.Instance.GetAttackSpeedDescriptionByValue((int)reloadMs);
			int rangePixels = (int)(towerModel.OriginalParameter.range * GameRecord.PIXEL_PER_UNIT);
			attackRangeText.text = AbilityRankDescriber.Instance.GetAttackRangeDescriptionByValue(rangePixels);
			goldProduceText.text = towerModel.OriginalParameter.goldProduce.ToString();
			timeProduceText.text = (reloadMs / 1000f).ToString("F2") + "s";
			autoCollectGoldText.text = ((float)towerModel.OriginalParameter.autoCollectTime / 1000f).ToString() + "s";
			HideAll();
			if (towerModel.Id == 1)
			{
				ShowBarrackTowerAbility();
			}
			else if (towerModel.Id == 4)
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

		public void Open()
		{
			base.gameObject.SetActive(true);
		}

		public void Close()
		{
			base.gameObject.SetActive(false);
		}
	}
}
