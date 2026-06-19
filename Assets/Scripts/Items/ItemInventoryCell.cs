using TMPro;
using UnityEngine;

namespace Items
{
	// One cell in the inventory popup: shows a held item's name + stat bonus and lets the player drag it
	// out (to a tower / equip area). Spawned per item by ItemInventoryPanel. The serialized field names
	// (nameText, levelText) are kept so the existing ItemCell.prefab wiring still binds; levelText now
	// shows the stat line instead of a level.
	public class ItemInventoryCell : MonoBehaviour
	{
		public void Bind(TowerItem item)
		{
			if (nameText != null)
			{
				nameText.SetText(item.name);
			}
			if (levelText != null)
			{
				levelText.SetText(string.Format("+{0}% {1}", item.statValue, StatLabel(item.statType)));
			}
			if (draggable != null)
			{
				draggable.SetInventoryPayload(item);
			}
		}

		private static string StatLabel(StatType statType)
		{
			switch (statType)
			{
			case StatType.AttackSpeed:
				return "ATK SPD";
			case StatType.Crit:
				return "CRIT";
			default:
				return "DMG";
			}
		}

		[SerializeField]
		private TMP_Text nameText;

		[SerializeField]
		private TMP_Text levelText;

		[SerializeField]
		private DraggableItem draggable;
	}
}
