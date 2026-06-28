using System;
using System.Collections.Generic;
using Data;

namespace Parameter
{
	// In-memory cache of tower skill-tree node definitions loaded from
	// Resources/Parameters/tower_skilltree_parameter. Lookup-only; which nodes a player has
	// unlocked lives in Data.TowerSkillTreeStore.
	public class TowerSkillTreeSpec
	{
		public static TowerSkillTreeSpec Instance
		{
			get
			{
				if (TowerSkillTreeSpec.instance == null)
				{
					TowerSkillTreeSpec.instance = new TowerSkillTreeSpec();
				}
				return TowerSkillTreeSpec.instance;
			}
		}

		public void Clear()
		{
			listNodes.Clear();
		}

		public void SetParameter(TowerSkillNode node)
		{
			listNodes.Add(node);
		}

		public bool TryGetNode(int towerID, int nodeID, out TowerSkillNode node)
		{
			foreach (TowerSkillNode n in listNodes)
			{
				if (n.towerID == towerID && n.nodeID == nodeID)
				{
					node = n;
					return true;
				}
			}
			node = default(TowerSkillNode);
			return false;
		}

		public List<TowerSkillNode> GetNodes(int towerID)
		{
			List<TowerSkillNode> list = new List<TowerSkillNode>();
			foreach (TowerSkillNode n in listNodes)
			{
				if (n.towerID == towerID)
				{
					list.Add(n);
				}
			}
			return list;
		}

		public int GetCost(int towerID, int nodeID)
		{
			TowerSkillNode node;
			if (TryGetNode(towerID, nodeID, out node))
			{
				return node.cost;
			}
			return -1;
		}

		// Parses the node's prerequisite string ("8_2") into node ids. Empty/"_" = no prerequisite.
		public List<int> GetPrerequisites(int towerID, int nodeID)
		{
			List<int> list = new List<int>();
			TowerSkillNode node;
			if (!TryGetNode(towerID, nodeID, out node) || string.IsNullOrEmpty(node.prerequisites))
			{
				return list;
			}
			string[] array = node.prerequisites.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string s in array)
			{
				list.Add(int.Parse(s));
			}
			return list;
		}

		// Sum of skill-param deltas from this tower's UNLOCKED nodes that target a given skill's param.
		// Lets the skill tree amplify (or trade off, via negatives) a specific ability param. paramIndex
		// is 0..4 (param_0..param_4 of the skill). Returns 0 when no unlocked node targets it.
		public int GetSkillParamBonus(int towerID, int ultimateBranch, int skillID, int paramIndex)
		{
			if (paramIndex < 0 || paramIndex > 4)
			{
				return 0;
			}
			int bonus = 0;
			List<int> unlocked = TowerSkillTreeStore.Instance.GetUnlockedNodes(towerID);
			for (int i = 0; i < unlocked.Count; i++)
			{
				TowerSkillNode node;
				if (!TryGetNode(towerID, unlocked[i], out node))
				{
					continue;
				}
				if (node.skillBranch == ultimateBranch && node.skillId == skillID && node.skillParamAdd != null)
				{
					bonus += node.skillParamAdd[paramIndex];
				}
			}
			return bonus;
		}

		private List<TowerSkillNode> listNodes = new List<TowerSkillNode>();

		private static TowerSkillTreeSpec instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
