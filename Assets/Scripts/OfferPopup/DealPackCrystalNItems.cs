using System;

[Serializable]
public class DealPackCrystalNItems
{
	public int saleRate;

	public DealKind offerType;

	public int timeCountDownHours;

	public string offerBundleID;

	public int gemAmount;

	public int[] itemsAmount;
}
