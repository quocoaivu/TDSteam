using System;
using MetaGame;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tournament
{
	public class BattleTimeTally : MonoBehaviour
	{
		private void Update()
		{
			if (FormatDirector.Instance.gameMode == GameFormat.TournamentMode && MonoSingleton<GameRecord>.Instance.IsGameStart && !MonoSingleton<GameRecord>.Instance.IsGameOver && !MonoSingleton<GameRecord>.Instance.IsPause)
			{
				UpdateTime();
			}
		}

		public void UpdateTime()
		{
			timeValue.text = GameUtils.GetFormattedTimeSpan(GameUtils.GetTimeSpanFromSecond(MonoSingleton<GameRecord>.Instance.tournamentBattleTime));
		}

		[SerializeField]
		private Text timeValue;
	}
}
