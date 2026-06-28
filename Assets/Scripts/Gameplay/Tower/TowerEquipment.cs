using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Gameplay
{
	// Holds the items equipped on one tower (up to SLOT_COUNT) and applies their stats through the
	// tower's BuffHolder. Stats are recomputed from the equipped list on every change and written once
	// per stat type to a shared buff key, so removing an item is always exact (no drift). In-run only.
	// Added to the tower at runtime by TurretEntity (no prefab wiring needed).
	[DisallowMultipleComponent]
	public class TowerEquipment : MonoBehaviour
	{
		public const int SLOT_COUNT = 4;

		// At most this many skill-unlock items per tower (the "pick 2 of 4 skills" rule). Plain stat items
		// are not limited beyond SLOT_COUNT.
		public const int MAX_SKILL_ITEMS = 2;

		// Permanent buffs use a large duration by repo convention (BuffHolder ticks duration down).
		private const float PERMANENT_DURATION = 999999f;

		public IReadOnlyList<TowerItem> Equipped
		{
			get
			{
				return equipped;
			}
		}

		public void Initialize(TurretEntity tower, BuffHolder buffHolder)
		{
			this.tower = tower;
			this.buffHolder = buffHolder;
		}

		// Why an item can't be equipped here, for player feedback. None = it fits.
		public enum EquipBlock
		{
			None,
			WrongTower,
			AlreadyEquipped,
			TooManySkills,
			NoFreeSlot
		}

		// Reason this item can't go on this tower (None = it fits). Checked in priority order so the message
		// is the most relevant one (wrong tower before full, etc.).
		public EquipBlock GetEquipBlock(TowerItem item)
		{
			if (item == null || tower == null || item.towerID != tower.Id)
			{
				return EquipBlock.WrongTower;
			}
			if (equipped.Contains(item))
			{
				return EquipBlock.AlreadyEquipped;
			}
			if (item.IsSkillItem && CountSkillItems() >= MAX_SKILL_ITEMS)
			{
				return EquipBlock.TooManySkills;
			}
			if (equipped.Count >= SLOT_COUNT)
			{
				return EquipBlock.NoFreeSlot;
			}
			return EquipBlock.None;
		}

		// True when the item fits this tower, there is a free slot, and it is not already equipped here.
		public bool CanEquip(TowerItem item)
		{
			return GetEquipBlock(item) == EquipBlock.None;
		}

		public bool Equip(TowerItem item)
		{
			if (!CanEquip(item))
			{
				return false;
			}
			equipped.Add(item);
			for (int i = 0; i < item.statTypes.Length; i++)
			{
				RecomputeStat(item.statTypes[i]);
			}
			if (item.IsSkillItem)
			{
				ActivateSkill(item);
			}
			return true;
		}

		public void Unequip(TowerItem item)
		{
			if (!equipped.Remove(item))
			{
				return;
			}
			for (int i = 0; i < item.statTypes.Length; i++)
			{
				RecomputeStat(item.statTypes[i]);
			}
			if (item.IsSkillItem)
			{
				DeactivateSkill(item);
			}
		}

		// True when the inventory has room for every equipped item, so selling won't lose any.
		public bool CanReturnAllToInventory()
		{
			return ItemInventory.Instance.FreeSlots >= equipped.Count;
		}

		// Sell / despawn while items are equipped: hand them back to the inventory so nothing is lost.
		public void ReturnAllToInventory()
		{
			for (int i = 0; i < equipped.Count; i++)
			{
				ItemInventory.Instance.Add(equipped[i]);
			}
			ClearAll();
		}

		// Drops every item without returning it (fresh build from pool). Also clears the stat buffs.
		public void ClearAll()
		{
			for (int i = 0; i < equipped.Count; i++)
			{
				if (equipped[i].IsSkillItem)
				{
					DeactivateSkill(equipped[i]);
				}
			}
			equipped.Clear();
			foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
			{
				RemoveStatBuff(statType);
			}
		}

		private int CountSkillItems()
		{
			int count = 0;
			for (int i = 0; i < equipped.Count; i++)
			{
				if (equipped[i].IsSkillItem)
				{
					count++;
				}
			}
			return count;
		}

		// Bridges a skill-item to the tower's ability controller: equipping unlocks the ability at the
		// item's rarity (tier), unequipping locks it back off.
		private void ActivateSkill(TowerItem item)
		{
			if (tower != null && tower.towerUltimateController != null)
			{
				tower.towerUltimateController.ActivateSkill(item.skillBranch, item.skillId, item.rarity);
			}
		}

		private void DeactivateSkill(TowerItem item)
		{
			if (tower != null && tower.towerUltimateController != null)
			{
				tower.towerUltimateController.DeactivateSkill(item.skillBranch, item.skillId);
			}
		}

		// Sums statValue of every equipped item that contributes to this stat type and writes it once to
		// the shared key. Remove-then-add so the key holds exactly the new total.
		private void RecomputeStat(StatType statType)
		{
			int total = 0;
			for (int i = 0; i < equipped.Count; i++)
			{
				StatType[] types = equipped[i].statTypes;
				int[] values = equipped[i].statValues;
				for (int j = 0; j < types.Length; j++)
				{
					if (types[j] == statType)
					{
						total += values[j];
					}
				}
			}
			// Clear first so the re-added buff holds exactly the new total (AddBuff's ChooseMax rule
			// can't lower an existing value, e.g. after an unequip).
			RemoveStatBuff(statType);
			if (total <= 0)
			{
				return;
			}
			// Attack-speed buff value is a percentage of base reload (100 = no change), so item
			// bonuses sit on top of a 100 base. Damage and crit keys are plain additive percentages.
			float value = (statType == StatType.AttackSpeed) ? (100 + total) : total;
			buffHolder.AddBuff(KeyFor(statType), new BuffStatus(true, value, PERMANENT_DURATION),
				BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
		}

		private void RemoveStatBuff(StatType statType)
		{
			buffHolder.RemoveBuffs(KeyFor(statType));
		}

		private static string KeyFor(StatType statType)
		{
			switch (statType)
			{
			case StatType.AttackSpeed:
				return BuffKeysToTurret.INCREASE_ATTACK_SPEED_BY_PERCENTAGE;
			case StatType.Crit:
				return BuffKeysToTurret.CritIncrementCommon;
			case StatType.Range:
				return BuffKeysToTurret.AttackRangeIncrementCommon;
			case StatType.Health:
				return BuffKeysToTurret.ITEM_MINION_HEALTH_PERCENT;
			case StatType.Armor:
				return BuffKeysToTurret.ITEM_MINION_ARMOR_FLAT;
			case StatType.GoldProduce:
				return BuffKeysToTurret.ITEM_GOLD_PRODUCE_FLAT;
			case StatType.Slow:
				return BuffKeysToTurret.SlowOnHitIncrementCommon;
			case StatType.Pierce:
				return BuffKeysToTurret.PierceCountIncrementCommon;
			case StatType.Poison:
				return BuffKeysToTurret.PoisonDpsIncrementCommon;
			case StatType.AirDamage:
				return BuffKeysToTurret.AirDamageIncrementCommon;
			case StatType.CritDamage:
				return BuffKeysToTurret.CritDamageIncrementCommon;
			case StatType.AoeRadius:
				return BuffKeysToTurret.AoeRadiusIncrementCommon;
			case StatType.MagicPen:
				return BuffKeysToTurret.MagicPenIncrementCommon;
			case StatType.HpRegen:
				return BuffKeysToTurret.ITEM_MINION_HP_REGEN_FLAT;
			case StatType.AuraDamage:
				return BuffKeysToTurret.ITEM_AURA_DAMAGE_AMP_FLAT;
			default:
				return BuffKeysToTurret.DamageIncrementCommon;
			}
		}

		private readonly List<TowerItem> equipped = new List<TowerItem>();

		private TurretEntity tower;

		private BuffHolder buffHolder;
	}
}
