namespace Parameter
{
	// One node in a tower's permanent skill tree. The stat fields are deltas added onto the tower's
	// base spec when the node is unlocked (see TowerParameterManager.GetStatWithTree).
	public struct TowerSkillNode
	{
		public int towerID;
		public int nodeID;
		public string name;
		public int cost;
		// Node ids joined by '_', e.g. "8_2" (needs both). "_" or empty = root (no prerequisite).
		public string prerequisites;
		public int dmgAdd;
		public int rangeAdd;
		public int reloadReduce;
		public int critAdd;
		public int pierceAdd;
	}
}
