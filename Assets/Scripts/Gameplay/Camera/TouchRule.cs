using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchRule : MonoBehaviour
{
	public virtual void Update()
	{
		Touchscreen touchscreen = Touchscreen.current;
		if (touchscreen == null)
		{
			return;
		}
		for (int i = 0; i < touchscreen.touches.Count; i++)
		{
			TouchControl touch = touchscreen.touches[i];
			TouchPhase phase = touch.phase.ReadValue();
			if (phase == TouchPhase.None)
			{
				continue;
			}
			TouchRule.currTouch = touch.touchId.ReadValue();
			switch (phase)
			{
			case TouchPhase.Began:
				OnTouchBeganAnywhere();
				break;
			case TouchPhase.Moved:
				OnTouchMovedAnywhere();
				break;
			case TouchPhase.Stationary:
				OnTouchStayedAnywhere();
				break;
			case TouchPhase.Ended:
				OnTouchEndedAnywhere();
				break;
			}
		}
	}

	public virtual void OnTouchBeganAnywhere()
	{
	}

	public virtual void OnTouchEndedAnywhere()
	{
	}

	public virtual void OnTouchMovedAnywhere()
	{
	}

	public virtual void OnTouchStayedAnywhere()
	{
	}

	public static int currTouch;

	[HideInInspector]
	public int touch2Watch = 64;
}
