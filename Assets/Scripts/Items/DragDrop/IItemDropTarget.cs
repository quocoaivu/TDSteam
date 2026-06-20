namespace Items
{
	// A UI region that can accept the carried item (inventory panel, tower equip area). Implemented on a
	// component sitting on (or above) a raycast-target graphic so a click while carrying can be resolved
	// to it.
	public interface IItemDropTarget
	{
		// Handle a drop of the currently-carried item. Return true if it was accepted/consumed; false to
		// leave it on the cursor.
		bool OnItemDropped(ItemCarryController carrier);
	}
}
