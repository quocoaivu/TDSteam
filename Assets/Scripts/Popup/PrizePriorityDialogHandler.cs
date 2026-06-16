using System;
using Parameter;
using RewardPopup;
using TMPro;
using UnityEngine;

public class PrizePriorityDialogHandler : GameplayPriorityDialogHandler
{
	public void SetRewardData(PrizeItem[] listData, string customTitle = null)
	{
		base.transform.SetAsLastSibling();
		generalItemGroupController.InitListItems(listData);
		if (!string.IsNullOrEmpty(customTitle))
		{
			title.text = customTitle;
		}
		else
		{
			title.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(defaultLocalId).Replace('@', '\n').Replace('#', '-');
		}
	}

	[Header("Controllers")]
	public PrizeItemClusterHandler generalItemGroupController;

	public TextMeshProUGUI title;

	public int defaultLocalId = 32;
}
