using System.Collections.Generic;
using System.Globalization;
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

        [Header("Header")]
        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text typeText;

        [SerializeField]
        private Image iconDamage;

        [SerializeField]
        private Sprite physicsDamageIcon;

        [SerializeField]
        private Sprite magicDamageIcon;

        [Header("Stat list")]
        [SerializeField]
        private Transform statContainer;

        [SerializeField]
        private StatRowView statRowTemplate;

        private List<StatRowView> spawnedRows = new List<StatRowView>();

		public void Init()
		{
			Open();
			if (MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.towerModel)
			{
				towerModel = MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.towerModel;
			}
			nameText.text = Singleton<TurretSynopsis>.Instance.GetTowerName(towerModel.Id);
			typeText.text = Singleton<TurretSynopsis>.Instance.GetTowerType(towerModel.Id);
			iconDamage.sprite = TowerParameterManager.Instance.isPhysicsAttack(towerModel.Id) ? physicsDamageIcon : magicDamageIcon;
			PopulateStats(towerModel.OriginalParameter, towerModel.Id);
		}

		// Builds the stat list for this tower. Each tower type shows the stats that matter
		// for it (mirrors the per-tower stat schema in tower_parameter.txt). Economy is shared.
		private void PopulateStats(TurretSpec spec, int id)
		{
			ClearRows();
			switch (id)
			{
			case 1: // Knights (barracks)
				AddRow("Damage", spec.damage.ToString());
				AddRow("Attack Speed", PerSecond(spec.attackSpeed));
				AddRow("Attack Range", Units(spec.unit_attackRange / GameRecord.PIXEL_PER_UNIT));
				AddRow("Move Speed", spec.unit_moveSpeed.ToString());
				AddRow("HP", spec.unit_health.ToString());
				AddRow("HP Regen", spec.unit_hpRegen + "/s");
				AddRow("Armor", spec.unit_armor + "%");
				AddRow("Shield", spec.unit_shield.ToString());
				AddRow("Max Units", spec.unit_maxUnits.ToString());
				AddRow("Respawn Time", Seconds(spec.unit_respawnTime / 1000f));
				AddRow("Deploy Range", Units(spec.unit_deployRange / GameRecord.PIXEL_PER_UNIT));
				break;
			case 2: // Stone God (AoE)
				AddRow("Damage", spec.damage.ToString());
				AddRow("Attack Speed", PerSecond(spec.attackSpeed));
				AddRow("Range", Units(spec.range));
				AddRow("AoE Radius", Units(spec.aoeRadius));
				AddRow("Projectile Speed", spec.projectileSpeed.ToString("0.#", CultureInfo.InvariantCulture));
				AddRow("Damage Falloff", spec.damageFalloff + "%");
				AddRow("Min Range", Units(spec.minRange));
				AddRow("Splash Targeting", Bool(spec.splashTargeting));
				AddRow("Target Priority", spec.targetPriority.ToString());
				break;
			case 3: // Magic Dragon
				AddRow("Magic Damage", spec.damage.ToString());
				AddRow("Attack Speed", PerSecond(spec.attackSpeed));
				AddRow("Range", Units(spec.range));
				AddRow("Projectile Speed", spec.projectileSpeed.ToString("0.#", CultureInfo.InvariantCulture));
				AddRow("Element", spec.magicElement.ToString());
				AddRow("Magic Penetration", spec.magicPenetration + "%");
				AddRow("Target Air", Bool(spec.canTargetAir));
				AddRow("Target Priority", spec.targetPriority.ToString());
				break;
			case 4: // Supporter
				AddRow("Gold / Tick", spec.goldProduce.ToString());
				AddRow("Gold Interval", Seconds(spec.goldInterval));
				AddRow("Gold On Kill", spec.goldOnKill.ToString());
				AddRow("Gold Multiplier", spec.goldMultiplier.ToString("0.0#", CultureInfo.InvariantCulture) + "x");
				AddRow("Interest Rate", spec.interestRate + "%");
				AddRow("Aura Radius", Units(spec.auraRadius));
				AddRow("Damage Amp", spec.damageAmp + "%");
				AddRow("Atk Speed Bonus", spec.attackSpeedBonus + "%");
				AddRow("Range Bonus", spec.rangeBonus + "%");
				AddRow("Cooldown Reduction", spec.cooldownReduction + "%");
				break;
			default: // Archer and other single-target projectile towers
				AddRow("Damage", spec.damage.ToString());
				AddRow("Attack Speed", PerSecond(spec.attackSpeed));
				AddRow("Range", Units(spec.range));
				AddRow("Projectile Speed", spec.projectileSpeed.ToString("0.#", CultureInfo.InvariantCulture));
				if (spec.pierceCount > 0) AddRow("Pierce", spec.pierceCount.ToString());
				if (spec.critChance > 0) AddRow("Crit Chance", spec.critChance + "%");
				AddRow("Target Air", Bool(spec.canTargetAir));
				AddRow("Target Priority", spec.targetPriority.ToString());
				break;
			}
			AddRow("Build Cost", spec.buildCost.ToString());
			AddRow("Sell Value", spec.sellValue.ToString());
		}

		private void AddRow(string label, string value)
		{
			StatRowView row = Object.Instantiate(statRowTemplate, statContainer);
			row.gameObject.SetActive(true);
			row.Set(label, value);
			spawnedRows.Add(row);
		}

		private void ClearRows()
		{
			for (int i = 0; i < spawnedRows.Count; i++)
			{
				if (spawnedRows[i] != null)
				{
					Object.Destroy(spawnedRows[i].gameObject);
				}
			}
			spawnedRows.Clear();
		}

		private static string PerSecond(float attackSpeed)
		{
			return attackSpeed.ToString("0.0#", CultureInfo.InvariantCulture) + "/s";
		}

		private static string Units(float units)
		{
			return units.ToString("0.00", CultureInfo.InvariantCulture);
		}

		private static string Seconds(float seconds)
		{
			return seconds.ToString("0.0#", CultureInfo.InvariantCulture) + "s";
		}

		private static string Bool(bool value)
		{
			return value ? "Yes" : "No";
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
