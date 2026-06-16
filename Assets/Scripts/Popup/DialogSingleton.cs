using System;
using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogSingleton : MonoSingleton<DialogSingleton>
{
	public virtual void Update()
	{
		if (MonoSingleton<GameRecord>.Instance.IsAnyTutorialPopupOpen)
		{
			return;
		}
		UpdateHideIfClickedOutside();
	}

	private void UpdateHideIfClickedOutside()
	{
		Pointer pointer = Pointer.current;
		if (pointer == null || !base.gameObject.activeSelf)
		{
			return;
		}
		Vector2 screenPosition = pointer.position.ReadValue();
		RectTransform rectTransform = base.gameObject.GetComponent<RectTransform>();
		if (pointer.press.wasPressedThisFrame)
		{
			if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPosition, Camera.main))
			{
				OnClickOutsideDown();
			}
		}
		if (pointer.press.wasReleasedThisFrame)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPosition, Camera.main))
			{
				OnClick();
			}
			else
			{
				OnClickOutsideUp();
			}
		}
	}

	protected virtual void OnClickOutsideUp()
	{
	}

	protected virtual void OnClickOutsideDown()
	{
	}

	protected virtual void OnClick()
	{
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

	public virtual void Open()
	{
	}

	public virtual void Close()
	{
	}

	public virtual void OnClickOutSide()
	{
	}

	[HideInInspector]
	public bool isOpen;
}
