using System;
using GameCore;
using UnityEngine;

public class GeneralDialogHandler : BaseMonoBehaviour
{
	public virtual void OnClick()
	{
	}

	public virtual void Open()
	{
		isOpen = true;
		base.gameObject.SetActive(true);
		UISfxDirector.Instance.PlayOpenPopup();
	}

	public virtual void Close()
	{
		isOpen = false;
		base.gameObject.SetActive(false);
		UISfxDirector.Instance.PlayClosePopup();
	}

	public virtual void Toggle()
	{
		if (isOpen)
		{
			Close();
		}
		else
		{
			Open();
		}
	}

	[HideInInspector]
	public bool isOpen;
}
