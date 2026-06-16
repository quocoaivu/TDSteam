using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	public class ControlSurgeSwitchHandler : GameplaySwitchHandler
	{
        [SerializeField]
        protected GameObject confirmImage;


        protected GameplaySwitchHandler.ButtonStatus buttonStatus;
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
			if (pointer.press.wasPressedThisFrame && !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPosition, Camera.main))
			{
				OnClickOutsideDown();
			}
			if (pointer.press.wasReleasedThisFrame && !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPosition, Camera.main))
			{
				OnClickOutsideUp();
			}
		}

		protected virtual void OnClickOutsideUp()
		{
		}

		protected virtual void OnClickOutsideDown()
		{
		}

		public virtual void Init(bool _isAllowedToUse, Sprite spriteNormal, Sprite lockImage)
		{
		}

		protected virtual void OnClickAvailable()
		{
			buttonStatus = GameplaySwitchHandler.ButtonStatus.Confirm;
			confirmImage.SetActive(true);
		}

		protected virtual void OnConfirm()
		{
			confirmImage.SetActive(false);
		}

		public virtual void DisableConfirm()
		{
			if (buttonStatus == GameplaySwitchHandler.ButtonStatus.Confirm)
			{
				buttonStatus = GameplaySwitchHandler.ButtonStatus.Available;
				confirmImage.SetActive(false);
			}
		}

		public virtual void UpdateBuyState()
		{
		}
	}
}
