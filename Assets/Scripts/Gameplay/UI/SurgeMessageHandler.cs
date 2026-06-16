using System;
using MetaGame;
using GameCore;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class SurgeMessageHandler : BaseMonoBehaviour
	{
		private void Start()
		{
			SetWaveMessage();
		}

		public void SetWaveMessage()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						currentWave = GameplayDirector.Instance.endlessModeManager.TotalWavesPassed;
						waveMessage.text = currentWave.ToString();
					}
				}
				else
				{
					currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
					totalWave = MonoSingleton<GameRecord>.Instance.TotalWave;
					waveMessage.text = string.Format("{0}/{1}", currentWave, totalWave);
				}
			}
			else
			{
				currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
				totalWave = MonoSingleton<GameRecord>.Instance.TotalWave;
				waveMessage.text = string.Format("{0}/{1}", currentWave, totalWave);
			}
			onUpdateWaveMessage.Dispatch();
		}

		[SerializeField]
		private OrderedUnityEvent onUpdateWaveMessage;

		[SerializeField]
		private Text waveMessage;

		private int currentWave;

		private int totalWave;
	}
}
