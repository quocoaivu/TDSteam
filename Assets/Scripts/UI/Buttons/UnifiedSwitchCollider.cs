using System;
using Common;

public class UnifiedSwitchCollider : UnifiedSwitch
{
	public void OnMouseDown()
	{
		SSRTrace.Log("UnifiedSwitchCollider.OnMouseDown");
		base.SetMouseDown();
	}

	public void OnMouseUp()
	{
		base.SetMouseUp();
	}

	public void OnMouseUpAsButton()
	{
		base.SetMouseUpAsButton();
	}
}
