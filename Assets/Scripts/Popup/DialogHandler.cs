using System;
using DG.Tweening;
using Gameplay;
using GameCore;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogHandler : BaseMonoBehaviour, IDialog
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

	public virtual void Open()
	{
		isOpen = true;
	}

	public virtual void Close()
	{
		isOpen = false;
	}

	[Space]
	[Header("Tween open n close")]
	public Tweener tween;

	public float timeToOpen = 0.2f;

	public float timeToClose = 0.1f;

	[HideInInspector]
	public bool isOpen;
}
