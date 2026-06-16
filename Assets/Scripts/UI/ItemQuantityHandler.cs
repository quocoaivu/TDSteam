
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemQuantityHandler : MonoBehaviour
{
	public void Init(int itemAmount)
	{
		this.itemAmount = itemAmount;
		if (itemAmount > 0)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	private void Show()
	{
		base.gameObject.SetActive(true);
		itemQuantity.text = "+ " + itemAmount.ToString();
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	[SerializeField]
	private Text itemQuantity;

	private int itemAmount;
}
