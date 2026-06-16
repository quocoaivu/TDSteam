using System;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Gameplay;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class SurgeRankBarHandler : MonoBehaviour
	{
        [SerializeField]
        private RectTransform rankBar;

        [SerializeField]
        private float minValue;

        [SerializeField]
        private float maxValue;

        private int[] waveRanks;

        [SerializeField]
        private Image[] chestBox;

        private float unit;

        private float currentValueX;

        private Tweener tween;

        private void Start()
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
			currentValueX = 0f;
			foreach (Image image in chestBox)
			{
				image.material.SetFloat("_EffectAmount", 1f);
			}
		}

		public void UpdateRankBar()
		{
			float valueX = currentValueX;
			tween = DOTween.To(() => currentValueX, delegate(float x)
			{
				valueX = x;
				rankBar.sizeDelta = new Vector2(valueX, rankBar.sizeDelta.y);
			}, unit * (float)MonoSingleton<GameRecord>.Instance.CurrentWave, 0.2f).SetEase(Ease.Linear).OnComplete(new TweenCallback(OnUpdateRankBarComplete));
		}

		private void OnUpdateRankBarComplete()
		{
			currentValueX = unit * (float)MonoSingleton<GameRecord>.Instance.CurrentWave;
			for (int i = 0; i < waveRanks.Length; i++)
			{
				if (MonoSingleton<GameRecord>.Instance.CurrentWave == waveRanks[i])
				{
					chestBox[i].material.SetFloat("_EffectAmount", 0f);
				}
			}
		}
	}
}
