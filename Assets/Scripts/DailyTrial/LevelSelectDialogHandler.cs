using System;
using Data;
using Gameplay;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class LevelSelectDialogHandler : GameplayDialogHandler
	{
        [Space]
        [Header("Controller")]
        [SerializeField]
        private DailyTabEntry inputHero;

        [SerializeField]
        private QuestClusterHandler missionGroupController;

        [Space]
        [SerializeField]
        private Text title;

        [SerializeField]
        private GameObject offerButton;


        private int currentDay;
        public void Init()
		{
			currentDay = DailyTrialStore.Instance.GetCurrentDayIndex();
			title.text = DailyOrdealSpec.Instance.GetTitle(currentDay);
			OpenWithScaleAnimation();
			InitDailyHeroInfor();
			missionGroupController.InitAllMissionsState();
			InitOfferButton();
		}

		private void InitOfferButton()
		{
			offerButton.SetActive(false);
		}

		private void InitDailyHeroInfor()
		{
			inputHero.Day = currentDay;
			inputHero.Init();
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
