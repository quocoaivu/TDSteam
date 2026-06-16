using System;
using System.Collections;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class EndGameClipHandler : GameplayDialogHandler
	{
        [Space]
        [SerializeField]
        private Text title;

        [SerializeField]
        private Text lifeAmount;

        [SerializeField]
        private Text countdownText;

        private int originTimeCooldown;

        private int currentTimeCooldown;

        public void Init()
		{
			OpenWithScaleAnimation();
			ShowTitle();
			GameplayDirector.Instance.gameSpeedController.PauseGame();
			originTimeCooldown = MonoSingleton<GameplayRecordLoader>.Instance.EndGameVideoProvider.GetCountdownTime();
			currentTimeCooldown = originTimeCooldown;
			StartCountDown();
		}

		private void ShowTitle()
		{
			int actuallyGemAmount = MonoSingleton<GameRecord>.Instance.GetActuallyGemAmount();
			string text = string.Format(Singleton<AlertSynopsis>.Instance.GetNotiContent(21), actuallyGemAmount);
			title.text = text.Replace('@', '\n').Replace('#', '-');
			lifeAmount.text = "+" + MonoSingleton<GameplayRecordLoader>.Instance.EndGameVideoProvider.GetEndGameVideoReward();
		}

		private IEnumerator ICountDown()
		{
			for (int i = 0; i <= originTimeCooldown; i++)
			{
				countdownText.text = currentTimeCooldown.ToString();
				currentTimeCooldown--;
				yield return new WaitForSecondsRealtime(1f);
			}
			OutOfTime();
			yield break;
		}

		public void StartCountDown()
		{
			base.StartCoroutine(ICountDown());
		}

		public void StopCountDown()
		{
			base.StopCoroutine(ICountDown());
		}

		private void OutOfTime()
		{
			UnityEngine.Debug.Log("out of time!");
			Close();
			GameplayDirector.Instance.gameLogicController.Defeated();
		}

		public override void Open()
		{
			base.Open();
			base.gameObject.SetActive(true);
		}

		public override void Close()
		{
			base.Close();
			base.gameObject.SetActive(false);
		}
	}
}
