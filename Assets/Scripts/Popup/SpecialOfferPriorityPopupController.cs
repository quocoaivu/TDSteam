using System;

public class SpecialOfferPriorityPopupController : GameplayPriorityDialogHandler
{
	public override void InitPriority(DialogPriorityEnum priority)
	{
		base.InitPriority(priority);
		saleBundleItem.Init();
		saleBundleItem.RefreshStatus();
	}

	public SalePackItem saleBundleItem;
}
