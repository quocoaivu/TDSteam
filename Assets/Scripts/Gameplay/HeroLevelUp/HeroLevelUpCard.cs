using System;
using Data;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class HeroLevelUpCard : BaseMonoBehaviour
	{
		private const int MAX_LEVEL_INDEX = 9;

		private const float MAX_LEVEL_BAR_LENGTH = 140f;

		public void Init(int _heroID)
		{
			heroID = _heroID;
			SetHeroAvatar(_heroID);
			InitExpBar();
		}

		private void SetHeroAvatar(int _heroID)
		{
			avatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesAvatar/avatar_hero_{0}", heroID));
		}

		public void InitExpBar()
		{
			heroLevelBeforeCalculate = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			heroExpBeforeCalculate = HeroStore.Instance.GetCurrentExp(heroID);
			heroLevelText.text = (heroLevelBeforeCalculate + 1).ToString();
			if (heroLevelBeforeCalculate >= MAX_LEVEL_INDEX)
			{
				DisplayEXPBar(MAX_LEVEL_BAR_LENGTH);
			}
			else
			{
				CalculateEXPBar(heroExpBeforeCalculate, HeroParameterManager.Instance.GetEXPForNextLevel(heroID, heroLevelBeforeCalculate));
			}
		}

		public void CalculateEXPBar(int currentEXP, int currentLevelExp)
		{
			float expPerUnitLength = (float)currentLevelExp / (float)(maxExpBarValue - minExpBarValue);
			if (expPerUnitLength <= 0f)
			{
				DisplayEXPBar(0f);
				return;
			}
			float currentEXPLength = (float)currentEXP / expPerUnitLength;
			DisplayEXPBar(currentEXPLength);
		}

		private void DisplayEXPBar(float currentEXPLength)
		{
			expBarSize.Set(Mathf.Min((float)maxExpBarValue, currentEXPLength + (float)minExpBarValue), expBar.sizeDelta.y);
			expBar.sizeDelta = expBarSize;
		}

		public void InitExpBarAfterCalculating()
		{
			heroLevelAfterCalculate = HeroStore.Instance.GetCurrentHeroLevel(heroID);
			heroExpAfterCalculate = HeroStore.Instance.GetCurrentExp(heroID);
			heroLevelText.text = (heroLevelAfterCalculate + 1).ToString();
			if (heroLevelAfterCalculate >= MAX_LEVEL_INDEX)
			{
				DisplayEXPBar(MAX_LEVEL_BAR_LENGTH);
			}
			else
			{
				CalculateEXPBar(heroExpAfterCalculate, HeroParameterManager.Instance.GetEXPForNextLevel(heroID, heroLevelAfterCalculate));
			}
			if (heroLevelAfterCalculate > heroLevelBeforeCalculate)
			{
				levelUpEffect.SetActive(true);
			}
		}

		[Space]
		[Header("Hero information")]
		[SerializeField]
		private RectTransform expBar;

		private Vector2 expBarSize;

		[SerializeField]
		private int minExpBarValue;

		[SerializeField]
		private int maxExpBarValue;

		[Space]
		[SerializeField]
		private Text heroLevelText;

		[Space]
		[SerializeField]
		private Image avatar;

		[Space]
		[Header("Level up effect")]
		[SerializeField]
		private GameObject levelUpEffect;

		private int heroID;

		private int heroLevelBeforeCalculate;

		private int heroLevelAfterCalculate;

		private int heroExpBeforeCalculate;

		private int heroExpAfterCalculate;
	}
}
