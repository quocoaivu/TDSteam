using System;
using Common;
using UnityEngine;

public abstract class UnifiedSwitch : MonoBehaviour
{
	public int SortingOrder
	{
		get
		{
			return sortingOrder;
		}
	}

	protected void SetMouseDown()
	{
		isHolding = true;
		if (!UnifiedSwitch.hasButtonHold)
		{
			UnifiedSwitch.hasButtonHold = true;
			UnifiedSwitch.firstHoldFrame = true;
			UnifiedSwitch.currentHoldButtonOrder = sortingOrder;
		}
		else
		{
			UnifiedSwitch.currentHoldButtonOrder = Mathf.Max(sortingOrder, UnifiedSwitch.currentHoldButtonOrder);
		}
	}

	protected void SetMouseUpAsButton()
	{
		if (UnifiedSwitch.currentHoldButtonOrder == sortingOrder)
		{
			onClick.Dispatch();
			UnifiedSwitch.hasButtonHold = false;
			isHolding = false;
		}
	}

	protected void SetMouseUp()
	{
		isHolding = false;
		UnifiedSwitch.hasButtonHold = false;
	}

	public void Update()
	{
		if (clearFirstHoldFrame)
		{
			UnifiedSwitch.firstHoldFrame = false;
			clearFirstHoldFrame = false;
		}
	}

	public void LateUpdate()
	{
		if (UnifiedSwitch.firstHoldFrame)
		{
			SSRTrace.Log("UnifiedSwitchCollider.Update: firstHoldFrame");
			if (sortingOrder == UnifiedSwitch.currentHoldButtonOrder && isHolding)
			{
				UnifiedSwitch.firstHoldFrame = false;
				onMouseDown.Dispatch();
			}
			clearFirstHoldFrame = true;
		}
	}

	[SerializeField]
	private int sortingOrder;

	[SerializeField]
	private OrderedUnityEvent onClick = new OrderedUnityEvent();

	[SerializeField]
	private OrderedUnityEvent onMouseDown = new OrderedUnityEvent();

	private bool isHolding;

	private bool clearFirstHoldFrame;

	private static bool hasButtonHold;

	private static bool firstHoldFrame;

	private static int currentHoldButtonOrder;
}
