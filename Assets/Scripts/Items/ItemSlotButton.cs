using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
	// One ability slot on the tower popup. Shows the item that can be equipped into this slot (if the
	// player owns one) and equips it on click. slotIndex maps to TurretMasteryHandler.listTowerUltimate
	// (== skillID). Place one per ability the tower's canonical prefab carries (2) and register it in
	// TowerItemPanel.slotButtons. Mirrors SkillTreeNodeButton from the skill-tree UI.
	public class ItemSlotButton : MonoBehaviour
	{
		public int SlotIndex
		{
			get
			{
				return slotIndex;
			}
		}

		public TowerItem OwnedItem
		{
			get
			{
				return ownedItem;
			}
		}

		private void Awake()
		{
			if (button != null)
			{
				button.onClick.AddListener(OnClick);
			}
		}

		public void Init(TowerItemPanel panel)
		{
			this.panel = panel;
		}

		// ownedItem == null -> no compatible item in inventory; equipped -> already active on the tower.
		public void Refresh(TowerItem ownedItem, bool equipped)
		{
			this.ownedItem = ownedItem;
			if (nameText != null)
			{
				nameText.SetText(ownedItem != null ? ownedItem.name : "—");
			}
			if (emptyOverlay != null)
			{
				emptyOverlay.SetActive(ownedItem == null);
			}
			if (equippedOverlay != null)
			{
				equippedOverlay.SetActive(equipped);
			}
			if (button != null)
			{
				button.interactable = (ownedItem != null && !equipped);
			}
		}

		private void OnClick()
		{
			if (panel != null)
			{
				panel.OnSlotClicked(this);
			}
		}

		[SerializeField]
		private int slotIndex;

		[SerializeField]
		private Button button;

		[SerializeField]
		private TMP_Text nameText;

		[SerializeField]
		private GameObject emptyOverlay;

		[SerializeField]
		private GameObject equippedOverlay;

		private TowerItem ownedItem;

		private TowerItemPanel panel;
	}
}
