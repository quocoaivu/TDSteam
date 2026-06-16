using System;
using System.Collections.Generic;
using Data;
using Gameplay;
using HeroCamp.UltimateUpgrade;
using Parameter;
using Services.PlatformSpecific;
using Common;
using UnityEngine;
using WorldMap;

namespace HeroCamp
{
	public class HeroBarracksDialogHandler : GameplayDialogHandler
	{
		public HeroLevelOverview HeroLevelInformation
		{
			get
			{
				return heroLevelInformation;
			}
			set
			{
				heroLevelInformation = value;
			}
		}

		public HeroAbilityClusterHandler HeroSkillGroupController
		{
			get
			{
				return heroSkillGroupController;
			}
			set
			{
				heroSkillGroupController = value;
			}
		}

		public SelectHeroSwitchClusterHandler SelectHeroButtonGroupController
		{
			get
			{
				return selectHeroButtonGroupController;
			}
			set
			{
				selectHeroButtonGroupController = value;
			}
		}

		public EnhanceNBuyClusterHandler UpgradeNBuyGroupController
		{
			get
			{
				return upgradeNBuyGroupController;
			}
			set
			{
				upgradeNBuyGroupController = value;
			}
		}

		public HeroActionPortraitClusterHandler HeroActionAvatarGroupController
		{
			get
			{
				return heroActionAvatarGroupController;
			}
			set
			{
				heroActionAvatarGroupController = value;
			}
		}

		public MasteryEnhanceDialogHandler UltimateUpgradePopupController
		{
			get
			{
				return ultimateUpgradePopupController;
			}
			set
			{
				ultimateUpgradePopupController = value;
			}
		}

		public static HeroBarracksDialogHandler Instance
		{
			get
			{
				return HeroBarracksDialogHandler._instance;
			}
		}

		private void Awake()
		{
			HeroBarracksDialogHandler._instance = this;
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			ChooseDefaultHero(2);
			SelectHeroButtonGroupController.Init();
			OnInitEvent.Dispatch();
			SendEventOpenPanel();
		}

		private void SendEventOpenPanel()
		{
			int currentGem = PlayerCurrencyStore.Instance.GetCurrentGem();
			int maxMapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked() + 1;
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_OpenHeroCamp(currentGem, maxMapIDUnlocked);
		}

		public void ChooseDefaultHero(int heroID)
		{
			SelectHeroButtonGroupController.AutoChoseHero(heroID);
		}

		public void RefreshHeroInformation()
		{
			int currentHeroLevel = HeroStore.Instance.GetCurrentHeroLevel(currentHeroID);
			HeroLevelInformation.Init(currentHeroID, currentHeroLevel);
			HeroSkillGroupController.Init(currentHeroID, currentHeroLevel);
			UpgradeNBuyGroupController.RefreshStatus();
			HeroActionAvatarGroupController.ShowSelectedHeroActionAvatar(currentHeroID);
			TryToShowPetInfor();
			SelectHeroButtonGroupController.Init();
		}

		private void TryToShowPetInfor()
		{
			petActionAvatarGroupController.HideAll();
			if (HeroStore.Instance.IsPetUnlocked(currentHeroID))
			{
				int petID = HeroParameterManager.Instance.GetPetID(currentHeroID);
				petActionAvatarGroupController.ShowSelectedPetActionAvatar(petID);
			}
		}

		public void ShowSelectedHeroImage(Transform transform)
		{
			selectedHeroImage.gameObject.SetActive(true);
			selectedHeroImage.Init(transform);
		}

		private void HideSelectedHeroImage()
		{
			selectedHeroImage.gameObject.SetActive(false);
		}

		public void ShowUnlockEffect(int heroID)
		{
			List<SelectHeroSwitchHandler> listSelectHeroButton = SelectHeroButtonGroupController.listSelectHeroButton;
			foreach (SelectHeroSwitchHandler selectHeroButtonController in listSelectHeroButton)
			{
				if (selectHeroButtonController.HeroID == heroID)
				{
					unlockedEffect.transform.position = selectHeroButtonController.transform.position;
					TrySetTrigger(unlockedEffectAnimator, "Effect");
				}
			}
		}

		public void ShowLevelUpEffect()
		{
			TrySetTrigger(levelUpAnimator, "Effect");
		}

		// Guard SetTrigger Ä‘á»ƒ trÃ¡nh log "Parameter does not exist" khi animator controller
		// chÆ°a config parameter â€” UI effect chá»‰ skip, khÃ´ng crash.
		private static void TrySetTrigger(Animator animator, string paramName)
		{
			if (animator == null || animator.runtimeAnimatorController == null)
			{
				return;
			}
			for (int i = 0; i < animator.parameterCount; i++)
			{
				if (animator.parameters[i].name == paramName)
				{
					animator.SetTrigger(paramName);
					return;
				}
			}
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			HideSelectedHeroImage();
			if (!TutorialStore.Instance.GetTutorialStatus(TutorialStore.TUTORIAL_ID_WORLD_MAP))
			{
				MonoSingleton<GlobeZoneDirector>.Instance.WorldMapTutorial.NextStep();
			}
			//NativeSpecificServicesSource.Services.DataCloudSaver.AutoBackUpData();
		}

		[Space]
		[SerializeField]
		private OrderedUnityEvent OnInitEvent;

		[SerializeField]
		private HeroLevelOverview heroLevelInformation;

		[SerializeField]
		private HeroAbilityClusterHandler heroSkillGroupController;

		[Space]
		[Header("Select Hero Group")]
		[SerializeField]
		private SelectHeroSwitchClusterHandler selectHeroButtonGroupController;

		[SerializeField]
		private SelectedPictureHandler selectedHeroImage;

		[Space]
		[Header("Upgrade N Buy")]
		[SerializeField]
		private EnhanceNBuyClusterHandler upgradeNBuyGroupController;

		[Space]
		[Header("Action Avatars")]
		[SerializeField]
		private HeroActionPortraitClusterHandler heroActionAvatarGroupController;

		[SerializeField]
		private PetActionPortraitClusterHandler petActionAvatarGroupController;

		[Space]
		[Header("Ultimate Upgrade")]
		[SerializeField]
		private MasteryEnhanceDialogHandler ultimateUpgradePopupController;

		[NonSerialized]
		public int currentHeroID = -1;

		[Space]
		[Header("Visual Effect")]
		[SerializeField]
		private GameObject unlockedEffect;

		[SerializeField]
		private Animator unlockedEffectAnimator;

		[SerializeField]
		private Animator levelUpAnimator;

		private static HeroBarracksDialogHandler _instance;
		[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			_instance = null;
		}
	}
}
