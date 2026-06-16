using System;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class TurretOverviewHandler : MonoBehaviour
	{
		public void Init(int _towerID, int _towerLevel)
		{
			SetBasicInformation(_towerID, _towerLevel);
			SetAbilityInformation(_towerID, _towerLevel);
			HideAll();
			if (_towerID == 1)
			{
				ShowBarrackTowerAbility();
			}
			else if (_towerID == 4)
			{
				ShowSupportTowerAbility();
			}
			else
			{
				ShowNormalTowerAbility();
			}
		}

		private void SetBasicInformation(int _towerID, int _towerLevel)
		{
			towerName.text = Singleton<TurretSynopsis>.Instance.GetTowerName(_towerID);
			description.text = Singleton<TurretSynopsis>.Instance.GetTowerDescription(_towerID, _towerLevel).Replace('@', '\n').Replace('#', '-');
			if (_towerLevel == 3 || _towerLevel == 4)
			{
				string text = Singleton<TurretSynopsis>.Instance.GetTowerUltimateDescription(_towerID, _towerLevel, 0) + "\n" + Singleton<TurretSynopsis>.Instance.GetTowerUltimateDescription(_towerID, _towerLevel, 1);
				ultimateDescription.text = text.Replace('@', '\n').Replace('#', '-');
			}
			else
			{
				ultimateDescription.text = string.Empty;
			}
		}

		private void SetAbilityInformation(int _towerID, int _towerLevel)
		{
			damage.text = TowerParameterManager.Instance.GetMinDamage(_towerID, _towerLevel) + "-" + TowerParameterManager.Instance.GetMaxDamage(_towerID, _towerLevel);
			if (TowerParameterManager.Instance.isPhysicsAttack(_towerID))
			{
				iconDamage.sprite = physicsDamageIcon;
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
			}
			attackSpeed.text = AbilityRankDescriber.Instance.GetAttackSpeedDescriptionByValue(TowerParameterManager.Instance.GetAttackSpeed(_towerID, _towerLevel));
			attackRange.text = AbilityRankDescriber.Instance.GetAttackRangeDescriptionByValue(TowerParameterManager.Instance.GetRangeMax(_towerID, _towerLevel));
			unitHealth.text = TowerParameterManager.Instance.GetUnitHealth(_towerID, _towerLevel).ToString();
			unitPhysicsArmor.text = AbilityRankDescriber.Instance.GetArmorDescriptionByValue(TowerParameterManager.Instance.GetUnitArmor(_towerID, _towerLevel));
			goldProduce.text = TowerParameterManager.Instance.GetGoldProduce(_towerID, _towerLevel).ToString();
			timeProduce.text = TowerParameterManager.Instance.GetCooldownTime(_towerID, _towerLevel).ToString() + "s";
		}

		public void HideAll()
		{
			damageHolder.gameObject.SetActive(false);
			attackSpeedHolder.gameObject.SetActive(false);
			attackRangeHolder.gameObject.SetActive(false);
			unitHealthHolder.gameObject.SetActive(false);
			unitPhysicsArmorHolder.gameObject.SetActive(false);
			goldProduceHolder.gameObject.SetActive(false);
			timeProduceHolder.gameObject.SetActive(false);
		}

		private void ShowNormalTowerAbility()
		{
			damageHolder.gameObject.SetActive(true);
			attackSpeedHolder.gameObject.SetActive(true);
			attackRangeHolder.gameObject.SetActive(true);
		}

		private void ShowBarrackTowerAbility()
		{
			damageHolder.gameObject.SetActive(true);
			unitHealthHolder.gameObject.SetActive(true);
			unitPhysicsArmorHolder.gameObject.SetActive(true);
		}

		private void ShowSupportTowerAbility()
		{
			goldProduceHolder.gameObject.SetActive(true);
			timeProduceHolder.gameObject.SetActive(true);
		}

		[Space]
		[Header("Basic Infor")]
		[SerializeField]
		private Text towerName;

		[Space]
		[Header("Basic Infor")]
		[SerializeField]
		private Text description;

		[Space]
		[Header("Basic Infor")]
		[SerializeField]
		private Text ultimateDescription;

		[Space]
		[Header("Value")]
		[SerializeField]
		private Text damage;

		[Space]
		[Header("Value")]
		[SerializeField]
		private Text attackSpeed;

		[Space]
		[Header("Value")]
		[SerializeField]
		private Text attackRange;

		[SerializeField]
		private Text unitHealth;

		[SerializeField]
		private Text unitPhysicsArmor;

		[SerializeField]
		private Text goldProduce;

		[SerializeField]
		private Text timeProduce;

		[Space]
		[Header("Holder")]
		[SerializeField]
		private GameObject damageHolder;

		[Space]
		[Header("Holder")]
		[SerializeField]
		private GameObject attackSpeedHolder;

		[Space]
		[Header("Holder")]
		[SerializeField]
		private GameObject attackRangeHolder;

		[SerializeField]
		private GameObject unitHealthHolder;

		[SerializeField]
		private GameObject unitPhysicsArmorHolder;

		[SerializeField]
		private GameObject goldProduceHolder;

		[SerializeField]
		private GameObject timeProduceHolder;

		[Space]
		[SerializeField]
		private Image iconDamage;

		[SerializeField]
		private Sprite physicsDamageIcon;

		[SerializeField]
		private Sprite magicDamageIcon;
	}
}
