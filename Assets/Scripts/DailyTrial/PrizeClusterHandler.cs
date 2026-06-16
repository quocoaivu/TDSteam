using System;
using System.Collections;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Gameplay;
using Parameter;
using UnityEngine;

namespace DailyTrial
{
	public class PrizeClusterHandler : MonoBehaviour
	{

        [SerializeField]
        private GameObject continueButton;

        [SerializeField]
        private RectTransform rankBar;

        [SerializeField]
        private float minValue;

        [SerializeField]
        private float maxValue;

        private int[] waveRanks;

        private int[] remants;

        private bool[] isAbleToTakeReward;

        [SerializeField]
        private PrizeHandler[] listRewardController;

        private int currentWavePassed;

        private float unit;

        private Tweener tween;

        public void Init(BattleStanding battleStatus)
		{
			int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
			int[] listWaveRank = DailyOrdealSpec.Instance.GetListWaveRank(currentDayIndex);
			waveRanks = new int[]
			{
				listWaveRank[0],
				listWaveRank[1],
				listWaveRank[2]
			};
			unit = maxValue / (float)MonoSingleton<GameRecord>.Instance.TotalWave;
			rankBar.sizeDelta = new Vector2(0f, rankBar.sizeDelta.y);
			if (battleStatus != BattleStanding.Victory)
			{
				if (battleStatus == BattleStanding.Defeat)
				{
					UpdateRankBarResult(MonoSingleton<GameRecord>.Instance.CurrentWave - 1);
				}
			}
			else
			{
				UpdateRankBarResult(MonoSingleton<GameRecord>.Instance.CurrentWave);
			}
		}

		public void UpdateRankBar_Victory()
		{
			float valueX = 0f;
			tween = DOTween.To(() => 0f, delegate(float x)
			{
				valueX = x;
				rankBar.sizeDelta = new Vector2(valueX, rankBar.sizeDelta.y);
			}, unit * (float)MonoSingleton<GameRecord>.Instance.CurrentWave, 2f).SetEase(Ease.Linear).OnComplete(new TweenCallback(OnUpdateRankBarComplete_Victory));
		}

		private void OnUpdateRankBarComplete_Victory()
		{
			continueButton.SetActive(true);
			for (int i = 0; i < waveRanks.Length; i++)
			{
				if (MonoSingleton<GameRecord>.Instance.CurrentWave >= waveRanks[i])
				{
					listRewardController[i].InitRewardInfor();
				}
			}
		}

		public void UpdateRankBar_Defeat()
		{
			float valueX = 0f;
			tween = DOTween.To(() => 0f, delegate(float x)
			{
				valueX = x;
				rankBar.sizeDelta = new Vector2(valueX, rankBar.sizeDelta.y);
			}, unit * (float)(MonoSingleton<GameRecord>.Instance.CurrentWave - 1), 2f).SetEase(Ease.Linear).OnComplete(new TweenCallback(OnUpdateRankBarComplete_Defeat));
		}

		private void OnUpdateRankBarComplete_Defeat()
		{
			continueButton.SetActive(true);
			for (int i = 0; i < waveRanks.Length; i++)
			{
				if (MonoSingleton<GameRecord>.Instance.CurrentWave >= waveRanks[i] + 1)
				{
					listRewardController[i].InitRewardInfor();
				}
			}
		}

		private void UpdateRankBarResult(int wavePassed)
		{
			currentWavePassed = wavePassed;
			isAbleToTakeReward = new bool[3];
			remants = new int[3];
			for (int i = 0; i < waveRanks.Length; i++)
			{
				if (wavePassed >= waveRanks[i])
				{
					isAbleToTakeReward[i] = true;
				}
			}
			if (wavePassed >= 0 && wavePassed < 3)
			{
				remants[0] = wavePassed;
				remants[1] = -1;
				remants[2] = -1;
			}
			if (wavePassed >= 3 && wavePassed < 6)
			{
				remants[0] = 3;
				remants[1] = wavePassed - 3;
				remants[2] = -1;
			}
			if (wavePassed >= 6 && wavePassed <= 9)
			{
				remants[0] = 3;
				remants[1] = 3;
				remants[2] = wavePassed - 6;
			}
			base.StartCoroutine(ProcessBarRank0());
		}

		private IEnumerator ProcessBarRank0()
		{
			yield return null;
			if (remants[0] > 0)
			{
				float valueX = 0f;
				tween = DOTween.To(() => 0f, delegate(float x)
				{
					valueX = x;
					rankBar.sizeDelta = new Vector2(valueX, rankBar.sizeDelta.y);
				}, unit * (float)remants[0], 1f).SetEase(Ease.Linear).OnComplete(new TweenCallback(ProcessRewardRank0));
			}
			yield break;
		}

		private void ProcessRewardRank0()
		{
			if (isAbleToTakeReward[0])
			{
				listRewardController[0].InitRewardInfor();
			}
			if (remants[1] <= 0)
			{
				continueButton.SetActive(true);
			}
			base.StartCoroutine(ProcessBarRank1());
		}

		private IEnumerator ProcessBarRank1()
		{
			yield return new WaitForSeconds(0.3f);
			if (remants[1] > 0)
			{
				float valueX = rankBar.sizeDelta.x;
				tween = DOTween.To(() => rankBar.sizeDelta.x, delegate(float x)
				{
					valueX = x;
					rankBar.sizeDelta = new Vector2(valueX, rankBar.sizeDelta.y);
				}, unit * (float)(remants[0] + remants[1]), 0.75f).SetEase(Ease.Linear).OnComplete(new TweenCallback(ProcessRewardRank1));
			}
			yield break;
		}

		private void ProcessRewardRank1()
		{
			if (isAbleToTakeReward[1])
			{
				listRewardController[1].InitRewardInfor();
			}
			if (remants[2] <= 0)
			{
				continueButton.SetActive(true);
			}
			base.StartCoroutine(ProcessBarRank2());
		}

		private IEnumerator ProcessBarRank2()
		{
			yield return new WaitForSeconds(0.3f);
			if (remants[2] > 0)
			{
				float valueX = rankBar.sizeDelta.x;
				tween = DOTween.To(() => rankBar.sizeDelta.x, delegate(float x)
				{
					valueX = x;
					rankBar.sizeDelta = new Vector2(valueX, rankBar.sizeDelta.y);
				}, unit * (float)(remants[0] + remants[1] + remants[2]), 0.5f).SetEase(Ease.Linear).OnComplete(new TweenCallback(ProcessRewardRank2));
			}
			yield break;
		}

		private void ProcessRewardRank2()
		{
			if (isAbleToTakeReward[2])
			{
				listRewardController[2].InitRewardInfor();
			}
			continueButton.SetActive(true);
		}
	}
}
