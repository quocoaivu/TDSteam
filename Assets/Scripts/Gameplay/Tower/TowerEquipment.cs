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
			RecomputeStat(item.statType);
			return true;
		}

		public void Unequip(TowerItem item)
		{
			if (equipped.Remove(item))
			{
				RecomputeStat(item.statType);
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
			equipped.Clear();
			RemoveStatBuff(StatType.Damage);
			RemoveStatBuff(StatType.AttackSpeed);
			RemoveStatBuff(StatType.Crit);
		}

		// Sums statValue of every equipped item of this type and writes it once to the shared key.
		// Remove-then-add so the key holds exactly the new total (AddBuff's stack rules can't lower it).
		private void RecomputeStat(StatType statType)
		{
			int total = 0;
			for (int i = 0; i < equipped.Count; i++)
			{
				if (equipped[i].statType == statType)
				{
					total += equipped[i].statValue;
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
			default:
				return BuffKeysToTurret.DamageIncrementCommon;
			}
		}

		private readonly List<TowerItem> equipped = new List<TowerItem>();

		private TurretEntity tower;

		private BuffHolder buffHolder;
	}
}
