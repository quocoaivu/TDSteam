using System;

public class PrizeItem
{
	public PrizeItem()
	{
	}

	public PrizeItem(PrizeKind rewardType, int quantity, bool isDisplayQuantity)
	{
		this.rewardType = rewardType;
		value = quantity;
		this.isDisplayQuantity = isDisplayQuantity;
	}

	public PrizeItem(PrizeKind rewardType, int itemId, int quantity, bool isDisplayQuantity) : this(rewardType, quantity, isDisplayQuantity)
	{
		itemID = itemId;
	}

	public PrizeKind rewardType;

	public int itemID;

	public int value;

	public bool isDisplayQuantity;
}
