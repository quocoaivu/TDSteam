using System;
using UnityEngine;

namespace Gameplay
{
	public class GameHasteHandler : MonoBehaviour
	{
        private float gameSpeed;

        [SerializeField]

        private HasteSwitchHandler speedButtonController;
        public float GameSpeed
		{
			get
			{
				return gameSpeed;
			}
			set
			{
				gameSpeed = value;
				Time.timeScale = gameSpeed;
			}
		}

		public void SetSpeed(float speed)
		{
			GameSpeed = speed;
		}

		public void SetNormalSpeed()
		{
			GameSpeed = 1f;
			Time.fixedDeltaTime = 0.02f / GameSpeed;
			MonoSingleton<GameRecord>.Instance.IsPause = false;
			AudioListener.pause = false;
			speedButtonController.Init(GameSpeed);
		}

		public void PauseGame()
		{
			Time.timeScale = 0.001f;
			Time.fixedDeltaTime = 0.0001f;
			MonoSingleton<GameRecord>.Instance.IsPause = true;
		}

		public void UnPauseGame()
		{
			Time.timeScale = GameSpeed;
			Time.fixedDeltaTime = 0.02f / GameSpeed;
			MonoSingleton<GameRecord>.Instance.IsPause = false;
		}
	}
}
