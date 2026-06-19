namespace Items
{
	// A UI region that can accept a dragged item (inventory panel, tower equip area). Implemented on a
	// component sitting on (or above) a raycast-target graphic so DraggableItem can find it on release.
	public interface IItemDropTarget
	{
		// Handle a drop. Return true if the item was accepted/consumed; false to leave it where it was.
		bool OnItemDropped(DraggableItem dragged);
	}
}
