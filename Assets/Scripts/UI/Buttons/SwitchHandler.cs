using System;
using GameCore;
using Common;
using UnityEngine;

public class SwitchHandler : BaseMonoBehaviour
{
	[SerializeField]
    private OrderedUnityEvent onClick = new OrderedUnityEvent();


    public virtual void OnClick()
	{
		onClick.Dispatch();
		UISfxDirector.Instance.PlayClick();
	}

	public virtual void UpdateButtonStatus()
	{
	}


}
