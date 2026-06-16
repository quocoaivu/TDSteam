using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class HasteSwitchHandler : SwitchHandler
	{
		private void Awake()
		{
			buttonImage = base.GetComponent<Image>();
		}

		public void Init(float gameSpeed)
		{
			if (gameSpeed > 1f)
			{
				SetSpeedView();
			}
			else
			{
				SetNormalView();
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			if (GameplayDirector.Instance.gameSpeedController.GameSpeed > 1f)
			{
				GameplayDirector.Instance.gameSpeedController.SetSpeed(1f);
				SetNormalView();
			}
			else
			{
				GameplayDirector.Instance.gameSpeedController.SetSpeed(2f);
				SetSpeedView();
			}
		}

		private void SetNormalView()
		{
			buttonImage.sprite = spriteNormal;
		}

		private void SetSpeedView()
		{
			buttonImage.sprite = spriteSpeed;
		}

		[SerializeField]
		private Sprite spriteNormal;

		[SerializeField]
		private Sprite spriteSpeed;

		private Image buttonImage;
	}
}
