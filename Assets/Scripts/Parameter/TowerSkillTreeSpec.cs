using System;
using System.Collections.Generic;

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

		private List<TowerSkillNode> listNodes = new List<TowerSkillNode>();

		private static TowerSkillTreeSpec instance;

		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}
	}
}
