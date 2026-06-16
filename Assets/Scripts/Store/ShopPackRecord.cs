using System;

public class ShopPackRecord
{
	public static SalePackSetupRecord GetDataSaleBundle(string productID)
	{
		SalePackSetupRecord result = null;
		SalePackSetupRecord[] dataArray = ConfigRegistry.Instance.saleBundleConfig.dataArray;
		for (int i = 0; i < dataArray.Length; i++)
		{
			if (dataArray[i].Productid.Equals(productID))
			{
				result = dataArray[i];
			}
		}
		return result;
	}
}
