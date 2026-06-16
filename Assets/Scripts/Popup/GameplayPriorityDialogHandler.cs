using System;
using Gameplay;
using UnityEngine;

public class GameplayPriorityDialogHandler : GameplayDialogHandler
{
	public virtual void InitPriority(DialogPriorityEnum priority)
	{
		if (rectTrans == null)
		{
			rectTrans = base.GetComponent<RectTransform>();
		}
		if (rectTrans != null)
		{
			rectTrans.offsetMax = Vector2.zero;
			rectTrans.offsetMin = Vector2.zero;
		}
		AddPriority(priority);
		isRecyclable = true;
	}

	public void AddPriority(DialogPriorityEnum priority)
	{
		if (isInQueue)
		{
			return;
		}
		isInQueue = true;
		this.priority = priority;
		PriorityDialogDirector.Instance.AddPopup(this);
	}

	public override void OnCloseAnimationComplete()
	{
		base.OnCloseAnimationComplete();
		if (isInQueue)
		{
			isInQueue = false;
			PriorityDialogDirector.Instance.RemoveCurrentPopup(this);
		}
		if (isRecyclable)
		{
			this.Recycle<GameplayPriorityDialogHandler>();
		}
	}

	private bool isInQueue;

	[HideInInspector]
	public DialogPriorityEnum priority;

	private bool isRecyclable;

	private RectTransform rectTrans;
}
