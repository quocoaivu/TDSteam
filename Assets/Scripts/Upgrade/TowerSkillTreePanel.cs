using System.Collections.Generic;
using Data;
using Gameplay;
using Parameter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrade
{
	// Out-of-match screen where the player spends TowerSkillPoint to permanently unlock a tower's
	// skill-tree nodes. One shared prefab serves every tower: on Init(towerID) it builds the nodes
	// and connecting lines at runtime by cloning the inactive templates, reading each node's id,
	// name, cost, prerequisites and position from the CSV (TowerSkillTreeSpec). Rebuilds only when
	// the tower changes.
	public class TowerSkillTreePanel : GameplayDialogHandler
	{
		// Opens the panel for a specific tower's tree.
		public void Init(int towerID)
		{
			currentTowerID = towerID;
			BuildTreeIfNeeded(towerID);
			if (resetButton != null)
			{
				resetButton.onClick.RemoveListener(OnResetClicked); // tránh trùng listener mỗi lần mở
				resetButton.onClick.AddListener(OnResetClicked);
			}
			RefreshAll(false);
			OpenWithScaleAnimation();
		}

		// Respec: xóa hết node đã mở của tower hiện tại và hoàn lại toàn bộ điểm đã tiêu.
		public void OnResetClicked()
		{
			List<int> unlocked = TowerSkillTreeStore.Instance.GetUnlockedNodes(currentTowerID);
			if (unlocked.Count == 0)
			{
				return;
			}
			int refund = 0;
			for (int i = 0; i < unlocked.Count; i++)
			{
				int cost = TowerSkillTreeSpec.Instance.GetCost(currentTowerID, unlocked[i]);
				if (cost > 0)
				{
					refund += cost;
				}
			}
			TowerSkillTreeStore.Instance.ResetTower(currentTowerID);
			TowerSkillPointStore.Instance.AddSkillPoint(refund, true);
			RefreshAll(false);
		}

		public void OnNodeClicked(SkillTreeNodeButton nodeButton)
		{
			if (TryUnlockNode(nodeButton.NodeID))
			{
				RefreshAll(true);
			}
		}

		// Rebuilds the node buttons + lines for a tower, but only when the tower actually changed
		// (re-opening the same tower reuses the already-built objects).
		private void BuildTreeIfNeeded(int towerID)
		{
			if (builtTowerID == towerID && nodeButtons.Count > 0)
			{
				return;
			}
			ClearBuilt();
			BuildTree(towerID);
			builtTowerID = towerID;
		}

		// Clones the templates once per node/edge of the tower's tree, positioning each node from CSV.
		private void BuildTree(int towerID)
		{
			List<TowerSkillNode> nodes = TowerSkillTreeSpec.Instance.GetNodes(towerID);
			Dictionary<int, RectTransform> rectById = new Dictionary<int, RectTransform>();

			for (int i = 0; i < nodes.Count; i++)
			{
				TowerSkillNode node = nodes[i];
				SkillTreeNodeButton button = Object.Instantiate(nodeTemplate, nodeContainer);
				RectTransform rect = (RectTransform)button.transform;
				rect.anchoredPosition = new Vector2(node.posX, node.posY);
				button.Setup(node.nodeID, node.name);
				button.Init(this);
				button.gameObject.SetActive(true);
				nodeButtons.Add(button);
				rectById[node.nodeID] = rect;
			}

			// Một đường cho mỗi cạnh prereq -> node, tham chiếu thẳng RectTransform 2 node.
			for (int i = 0; i < nodes.Count; i++)
			{
				int nodeID = nodes[i].nodeID;
				List<int> prereqs = TowerSkillTreeSpec.Instance.GetPrerequisites(towerID, nodeID);
				for (int p = 0; p < prereqs.Count; p++)
				{
					int from = prereqs[p];
					if (!rectById.ContainsKey(from) || !rectById.ContainsKey(nodeID))
					{
						continue;
					}
					SkillTreeLine line = Object.Instantiate(lineTemplate, lineContainer);
					line.Setup(rectById[from], rectById[nodeID], from, nodeID);
					line.gameObject.SetActive(true);
					lines.Add(line);
				}
			}
		}

		private void ClearBuilt()
		{
			for (int i = 0; i < nodeButtons.Count; i++)
			{
				if (nodeButtons[i] != null)
				{
					Object.Destroy(nodeButtons[i].gameObject);
				}
			}
			for (int i = 0; i < lines.Count; i++)
			{
				if (lines[i] != null)
				{
					Object.Destroy(lines[i].gameObject);
				}
			}
			nodeButtons.Clear();
			lines.Clear();
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

		// animate=false khi mở panel (set tức thì), animate=true khi vừa unlock (chạy tween đường nối).
		private void RefreshAll(bool animate)
		{
			for (int i = 0; i < nodeButtons.Count; i++)
			{
				int nodeID = nodeButtons[i].NodeID;
				bool revealed = IsNodeRevealed(nodeID);
				nodeButtons[i].gameObject.SetActive(revealed);
				if (revealed)
				{
					int cost = TowerSkillTreeSpec.Instance.GetCost(currentTowerID, nodeID);
					nodeButtons[i].Refresh(GetNodeState(nodeID), cost);
				}
			}
			RefreshLines(animate);
			if (skillPointText != null)
			{
				skillPointText.SetText("{0}", TowerSkillPointStore.Instance.GetCurrentSkillPoint());
			}
		}

		// Mỗi đường = cạnh prereq -> node. Đường chỉ hiện khi node nguồn (prereq) đã mở;
		// trước đó ẩn cùng với node con chưa lộ.
		private void RefreshLines(bool animate)
		{
			for (int i = 0; i < lines.Count; i++)
			{
				SkillTreeLine line = lines[i];
				bool prereqDone = TowerSkillTreeStore.Instance.IsNodeUnlocked(currentTowerID, line.FromNodeID);
				line.gameObject.SetActive(prereqDone);
				if (!prereqDone)
				{
					continue;
				}
				bool nodeDone = TowerSkillTreeStore.Instance.IsNodeUnlocked(currentTowerID, line.ToNodeID);
				line.ApplyState(nodeDone ? SkillTreeLineState.Unlocked : SkillTreeLineState.Ready, animate);
			}
		}

		// Node hiện khi là node gốc (không prereq) hoặc đã có ít nhất 1 prereq được mở.
		private bool IsNodeRevealed(int nodeID)
		{
			List<int> prereqs = TowerSkillTreeSpec.Instance.GetPrerequisites(currentTowerID, nodeID);
			if (prereqs.Count == 0)
			{
				return true; // node gốc
			}
			for (int i = 0; i < prereqs.Count; i++)
			{
				if (TowerSkillTreeStore.Instance.IsNodeUnlocked(currentTowerID, prereqs[i]))
				{
					return true;
				}
			}
			return false;
		}

		[Header("Templates (inactive) + containers")]
		[SerializeField]
		private SkillTreeNodeButton nodeTemplate;

		[SerializeField]
		private SkillTreeLine lineTemplate;

		[SerializeField]
		private RectTransform nodeContainer;

		[SerializeField]
		private RectTransform lineContainer;

		[Space]
		[Header("Player currency")]
		[SerializeField]
		private TMP_Text skillPointText;

		[SerializeField]
		private Button resetButton;

		private readonly List<SkillTreeNodeButton> nodeButtons = new List<SkillTreeNodeButton>();

		private readonly List<SkillTreeLine> lines = new List<SkillTreeLine>();

		private int currentTowerID;

		private int builtTowerID = -1;
	}
}
