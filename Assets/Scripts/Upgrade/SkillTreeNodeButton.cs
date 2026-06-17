using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrade
{
	// One node button in a tower's permanent skill tree (Phase 5 meta UI). Place one per node in the
	// tree prefab, set its nodeID, and add it to TowerSkillTreePanel.nodeButtons. The panel drives
	// Refresh and handles the actual purchase when the button is clicked.
	public class SkillTreeNodeButton : MonoBehaviour
	{
		public int NodeID
		{
			get
			{
				return nodeID;
			}
		}

		private void Awake()
		{
			if (button != null)
			{
				button.onClick.AddListener(OnClick);
			}
		}

		public void Init(TowerSkillTreePanel panel)
		{
			this.panel = panel;
		}

		public void Refresh(SkillTreeNodeState state, int cost)
		{
			if (lockedOverlay != null)
			{
				lockedOverlay.SetActive(state == SkillTreeNodeState.Locked);
			}
			if (unlockedOverlay != null)
			{
				unlockedOverlay.SetActive(state == SkillTreeNodeState.Unlocked);
			}
			if (costText != null)
			{
				// Hide the price once the node is owned; otherwise show it (yellow when affordable).
				costText.gameObject.SetActive(state != SkillTreeNodeState.Unlocked);
				costText.SetText("{0}", cost);
				costText.color = (state == SkillTreeNodeState.Available) ? affordableColor : unavailableColor;
			}
			if (button != null)
			{
				button.interactable = (state == SkillTreeNodeState.Available);
			}
		}

		private void OnClick()
		{
			if (panel != null)
			{
				panel.OnNodeClicked(this);
			}
		}

		[SerializeField]
		private int nodeID;

		[SerializeField]
		private Button button;

		[SerializeField]
		private TMP_Text costText;

		[SerializeField]
		private GameObject lockedOverlay;

		[SerializeField]
		private GameObject unlockedOverlay;

		[SerializeField]
		private Color affordableColor = Color.yellow;

		[SerializeField]
		private Color unavailableColor = Color.white;

		private TowerSkillTreePanel panel;
	}
}
