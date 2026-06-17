namespace Upgrade
{
	// Display/interaction state of one tower skill-tree node in the meta upgrade screen.
	public enum SkillTreeNodeState
	{
		Locked,        // prerequisites not yet unlocked
		Unaffordable,  // prerequisites met but not enough skill points
		Available,     // prerequisites met and affordable -> can buy
		Unlocked       // already owned
	}
}
