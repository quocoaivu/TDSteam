using System;
using System.Collections.Generic;
using Data;
using Gameplay;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace Upgrade
{
	public class GlobalUpgradePopupController : GameplayDialogHandler
	{

        public EnhanceOverviewHandler upgradeInformationController;

        [Header("Upgrade group")]
        public List<TowerUpgradeGroupController> upgradeGroupControllers = new List<TowerUpgradeGroupController>();

        [Space]
        [SerializeField]
        private GameObject upgradeSelectedImage;

        [SerializeField]
        private GameObject upgradedEffect;

        [SerializeField]
        private Animator upgradedEffectAnimator;

        [SerializeField]
        private GlobalUpgradeApplyButton upgradeButton;

        [SerializeField]
        private GlobalUpgradeResetButton resetButton;

        [HideInInspector]
        public int currentTowerIDSelected;

        [HideInInspector]
        public int currentTierSelected;

        [Space]
        [Header("Player currency")]
        [SerializeField]
        private Text playerStar;

        [NonSerialized]
        public int currentStar;

        private static GlobalUpgradePopupController _instance;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _instance = null;
        }
        public static GlobalUpgradePopupController Instance
		{
			get
			{
				return GlobalUpgradePopupController._instance;
			}
		}

		private void Awake()
		{
			GlobalUpgradePopupController._instance = this;
		}

		private void Start()
		{
			InitUpgradeStatus();
			HideSelectedImage();
			SetDefaultData();
			CalculateCurrentStar();
		}

		private void OnEnable()
		{
			InitUpgradeStatus();
			HideSelectedImage();
			SetDefaultData();
		}

		public void Init()
		{
			CalculateCurrentStar();
			OpenWithScaleAnimation();
			SendEventOpenPanel();
		}

		private void SendEventOpenPanel()
		{
			int totalStar = PlayerCurrencyStore.Instance.GetCurrentStar();
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_OpenGlobalUpgrade(currentStar, totalStar);
		}

		private void SetDefaultData()
		{
			currentTowerIDSelected = -1;
			currentTierSelected = -1;
			upgradeButton.gameObject.SetActive(false);
			upgradeInformationController.InitDefaultData();
		}

		private void InitUpgradeStatus()
		{
			for (int i = 0; i < upgradeGroupControllers.Count; i++)
			{
				upgradeGroupControllers[i].currentUpgradeLevel = GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(i);
				upgradeGroupControllers[i].RefreshListTier();
			}
		}

		private void HideSelectedImage()
		{
			upgradeSelectedImage.SetActive(false);
		}

		public void ShowUpgradedEffect()
		{
			upgradedEffect.transform.position = upgradeGroupControllers[currentTowerIDSelected].listTierUpgrade[currentTierSelected].transform.position;
			upgradedEffectAnimator.SetTrigger("Effect");
		}

		public void ShowSelectedUpgradeImage(Vector3 pos)
		{
			upgradeSelectedImage.SetActive(true);
			upgradeSelectedImage.transform.position = pos;
		}

		public void TryShowButtonUpgrade(bool canUpgrade)
		{
			upgradeButton.gameObject.SetActive(canUpgrade);
			upgradeButton.RefreshButtonStatus();
		}

		public void Reset()
		{
			for (int i = 0; i < upgradeGroupControllers.Count; i++)
			{
				upgradeGroupControllers[i].currentUpgradeLevel = -1;
				upgradeGroupControllers[i].RefreshListTier();
				GlobalUpgradeStore.Instance.Save(i, -1);
			}
			TryShowButtonUpgrade(false);
			HideSelectedImage();
			CalculateCurrentStar();
		}

		public void CalculateCurrentStar()
		{
			int num = PlayerCurrencyStore.Instance.GetCurrentStar();
			int num2 = 0;
			for (int i = 0; i < upgradeGroupControllers.Count; i++)
			{
				for (int j = 0; j <= GlobalUpgradeStore.Instance.GetCurrentUpgradeLevel(i); j++)
				{
					int starRequireForUpgrade = GlobalUpgradeStore.Instance.GetStarRequireForUpgrade(i, j);
					num2 += starRequireForUpgrade;
				}
			}
			currentStar = num - num2;
			playerStar.text = currentStar + "/" + num;
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			MonoSingleton<GlobeZoneDirector>.Instance.WorldMapTutorial.SetTutorialPassed();
		}

	}
}
