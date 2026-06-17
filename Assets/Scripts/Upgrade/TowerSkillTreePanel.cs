using System.Collections.Generic;
using Data;
using Gameplay;
using Parameter;
using TMPro;
using UnityEngine;

namespace Upgrade
{
	// Out-of-match screen where the player spends TowerSkillPoint to permanently unlock a tower's
	// skill-tree nodes. Mirrors GlobalUpgradePopupController. Shows one tower's tree at a time;
	// node buttons are pre-placed in the prefab and registered in `nodeButtons`.
	public class TowerSkillTreePanel : GameplayDialogHandler
	{
		// Opens the panel for a specific tower's tree.
		public void Init(int towerID)
		{
			currentTowerID = towerID;
			for (int i = 0; i < nodeButtons.Count; i++)
			{
				nodeButtons[i].Init(this);
			}
			RefreshAll();
			OpenWithScaleAnimation();
		}

		public void OnNodeClicked(SkillTreeNodeButton nodeButton)
		{
			if (TryUnlockNode(nodeButton.NodeID))
			{
				RefreshAll();
			}
		}

		// Validates prerequisites + affordability, then spends points and records the unlock.
		private bool TryUnlockNode(int nodeID)
		{
			if (GetNodeState(nodeID) != SkillTreeNodeState.Available)
			{
				return false;
			}
			int cost = TowerSkillTreeSpec.Instance.GetCost(currentTowerID, nodeID);
			if (!TowerSkillPointStore.Instance.TrySpend(cost))
			{
				return false;
			}
			TowerSkillTreeStore.Instance.UnlockNode(currentTowerID, nodeID);
			return true;
		}

		private SkillTreeNodeState GetNodeState(int nodeID)
		{
			if (TowerSkillTreeStore.Instance.IsNodeUnlocked(currentTowerID, nodeID))
			{
				return SkillTreeNodeState.Unlocked;
			}
			if (!ArePrerequisitesMet(nodeID))
			{
				return SkillTreeNodeState.Locked;
			}
			int cost = TowerSkillTreeSpec.Instance.GetCost(currentTowerID, nodeID);
			if (TowerSkillPointStore.Instance.GetCurrentSkillPoint() >= cost)
			{
				return SkillTreeNodeState.Available;
			}
			return SkillTreeNodeState.Unaffordable;
		}

		private bool ArePrerequisitesMet(int nodeID)
		{
			List<int> prereqs = TowerSkillTreeSpec.Instance.GetPrerequisites(currentTowerID, nodeID);
			for (int i = 0; i < prereqs.Count; i++)
			{
				if (!TowerSkillTreeStore.Instance.IsNodeUnlocked(currentTowerID, prereqs[i]))
				{
					return false;
				}
			}
			return true;
		}

		private void RefreshAll()
		{
			for (int i = 0; i < nodeButtons.Count; i++)
			{
				int nodeID = nodeButtons[i].NodeID;
				int cost = TowerSkillTreeSpec.Instance.GetCost(currentTowerID, nodeID);
				nodeButtons[i].Refresh(GetNodeState(nodeID), cost);
			}
			if (skillPointText != null)
			{
				skillPointText.SetText("{0}", TowerSkillPointStore.Instance.GetCurrentSkillPoint());
			}
		}

		[SerializeField]
		private List<SkillTreeNodeButton> nodeButtons = new List<SkillTreeNodeButton>();

		[Space]
		[Header("Player currency")]
		[SerializeField]
		private TMP_Text skillPointText;

		private int currentTowerID;
	}
}
