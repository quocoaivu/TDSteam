namespace Items
{
	// An item panel (shop / inventory / tower equip) that can rebuild its contents in place. The carry
	// controller refreshes the panel an item was picked up from after the item is placed elsewhere.
	public interface IItemPanel
	{
		void RefreshOpen();
	}
}
