using System;
using Data;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroLevelOverview : BaseMonoBehaviour
	{
		public void Init(int heroID, int heroLevel)
		{
			currentHeroID = heroID;
			currentHeroLevel = heroLevel;
			InitHeroStat();
			InitHeroAvatar();
			InitExpBar();
		}

		private void InitHeroAvatar()
		{
			heroAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesAvatar/avatar_hero_{0}", currentHeroID));
			heroname.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesName/name_hero_{0}", currentHeroID));
		}

		private void InitHeroStat()
		{
			levelText.text = string.Empty + (currentHeroLevel + 1);
			inforHealth.Init(currentHeroID, currentHeroLevel);
			inforAttackDamage.Init(currentHeroID, currentHeroLevel);
			inforArmor.Init(currentHeroID, currentHeroLevel);
			inforSpeed.Init(currentHeroID, currentHeroLevel);
		}

		private void InitExpBar()
		{
			int currentExp = HeroStore.Instance.GetCurrentExp(currentHeroID);
			if (currentHeroLevel < 9)
			{
				CalculateEXPBar(currentExp, HeroParameterManager.Instance.GetEXPForNextLevel(currentHeroID, currentHeroLevel));
			}
			if (currentHeroLevel == 9)
			{
				DisplayEXPBar(140f);
			}
		}

		public void DisplayLevelUpHero()
		{
			int num = HeroStore.Instance.GetCurrentHeroLevel(currentHeroID);
			int currentExp = HeroStore.Instance.GetCurrentExp(currentHeroID);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"current level = ",
				num,
				" exp dÆ° ra = ",
				currentExp
			}));
			if (num < 9)
			{
				CalculateEXPBar(currentExp, HeroParameterManager.Instance.GetEXPForNextLevel(currentHeroID, num));
			}
			if (num == 9)
			{
				DisplayEXPBar(140f);
			}
			HeroBarracksDialogHandler.Instance.RefreshHeroInformation();
			HeroBarracksDialogHandler.Instance.ShowLevelUpEffect();
		}

		public void CalculateEXPBar(int currentEXP, int currentLevelExp)
		{
			float num = (float)currentLevelExp / (float)(maxExpBarValue - minExpBarValue);
			float currentEXPLength = (float)currentEXP / num;
			DisplayEXPBar(currentEXPLength);
		}

		private void DisplayEXPBar(float currentEXPLength)
		{
			expBarSize.Set(currentEXPLength + (float)minExpBarValue, expBar.sizeDelta.y);
			expBar.sizeDelta = expBarSize;
		}

		[Space]
		[Header("Hero Level")]
		[SerializeField]
		private Text levelText;

		[Space]
		[Header("Hero Avatar")]
		[SerializeField]
		private Image heroAvatar;

		[SerializeField]
		private Image heroname;

		[Space]
		[Header("List Information Items ")]
		[SerializeField]
		private HeroOverviewItem inforHealth;

		[SerializeField]
		private HeroOverviewItem inforAttackDamage;

		[SerializeField]
		private HeroOverviewItem inforArmor;

		[SerializeField]
		private HeroOverviewItem inforSpeed;

		[Space]
		[Header("Exp bar")]
		[SerializeField]
		private RectTransform expBar;

		private Vector2 expBarSize;

		[SerializeField]
		private int minExpBarValue;

		[SerializeField]
		private int maxExpBarValue;

		private int currentHeroID;

		private int currentHeroLevel;
	}
}
