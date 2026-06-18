using System;
using DailyTrial;
using Data;
using Endless;
using GiftcodeSystem;
using Guide;
using HeroCamp;
using LifetimePopup;
using LinkGame;
using MapLevel;
using Tournament;
using UnityEngine;
using UnityEngine.InputSystem;
using UnlockTheme;
using Upgrade;
using UserProfile;

namespace WorldMap
{
	public class UIRootHandler : MonoSingleton<UIRootHandler>
	{

        [Space]
        [SerializeField]
        private float delayTimeToInitPopups;

        [Space]
        [SerializeField]
        private Transform popupParent;

        [Space]
        [Header("Prefab Panels")]
        [SerializeField]
        private GameObject heroCampPrefab;

        [SerializeField]
        private GameObject mapLevelSelectPrefab;

        [SerializeField]
        private GameObject upgradePrefab;

        [SerializeField]
        private GameObject towerSkillTreePrefab; // 1 prefab dùng chung, dựng node theo towerID lúc runtime

        [SerializeField]
        private GameObject settingPrefab;

        [SerializeField]
        private GameObject guidePrefab;

        [SerializeField]
        private GameObject unlockThemePrefab;

        [SerializeField]
        private GameObject endlessPrefab;

        [SerializeField]
        private GameObject tournamentPrefab;

        [SerializeField]
        private GameObject dailyTrialPrefab;

        [SerializeField]
        private GameObject eventQuestPrefab;

        [SerializeField]
        private GameObject linkGamePrefab;

        [SerializeField]
        private GameObject linkGameButton;

        [SerializeField]
        private GameObject giftCodePrefab;

        [SerializeField]
        private GameObject userProfilePrefab;

        [Space]
        [Header("Map Themes Controller")]
        [SerializeField]
        private ZoneThemesHandler mapThemesController;

        [Space]
        [Header("UI Group")]
        [SerializeField]
        private GameObject buttonGroup_normal;

        [SerializeField]
        private GameObject buttonGroup_offer;

        [SerializeField]
        private GameObject buttonGroup_challenge;

        private bool isHeroCampPopupExist;

        private HeroBarracksDialogHandler _heroCampPopupController;

        private bool isMapLevelPopupExist;

        private ZoneLevelSelectDialogHandler _mapLevelSelectPopupController;

        private bool isUpgradePopupExist;

        private GlobalUpgradePopupController _upgradePopupController;

        private TowerSkillTreePanel _towerSkillTreePanel;

        private bool isSettingPopupExist;

        private OptionDialogHandler _settingPopupController;

        private bool isGuidePopupExist;

        private PrimerDialogHandler _guidePopupController;

        private bool isUnlockThemePopupExist;

        private UnlockSkinDialogHandler _unlockThemePopupController;

        private bool isEndlessPopupExist;

        private InfiniteDialogHandler _endlessPopupController;

        private bool isTournamentPopupExist;

        private ArenaDialogHandler _tournamentPopupController;

        private bool isDailyTrialPopupExist;

        private DailyOrdealDialogHandler _dailyTrialPopupController;

        private SignalQuestDialog _eventQuestPopup;

        private bool isLinkGamePopupExist;

        private LinkGameDialogHandler _linkGamePopupController;

        private bool isGiftCodePopupExist;

        private VoucherCodeDialogHandler _giftCodePopupController;

        private bool isUserProfilePopupExist;

        private PlayerDossierDialogHandler _userProfilePopupController;

        private T GetOrCreatePopup<T>(GameObject prefab, ref T cache, ref bool exists) where T : Component
		{
			if (cache == null)
			{
				GameObject instance = UnityEngine.Object.Instantiate<GameObject>(prefab);
				instance.transform.SetParent(popupParent);
				instance.transform.localPosition = Vector3.zero;
				instance.transform.localScale = Vector3.one;
				cache = instance.GetComponent<T>();
				exists = true;
			}
			return cache;
		}

		public ZoneThemesHandler MapThemesController
		{
			get
			{
				return mapThemesController;
			}
		}

		public HeroBarracksDialogHandler heroCampPopupController
		{
			get
			{
				return GetOrCreatePopup(heroCampPrefab, ref _heroCampPopupController, ref isHeroCampPopupExist);
			}
		}

		public ZoneLevelSelectDialogHandler mapLevelSelectPopupController
		{
			get
			{
				return GetOrCreatePopup(mapLevelSelectPrefab, ref _mapLevelSelectPopupController, ref isMapLevelPopupExist);
			}
		}

		public GlobalUpgradePopupController upgradePopupController
		{
			get
			{
				return GetOrCreatePopup(upgradePrefab, ref _upgradePopupController, ref isUpgradePopupExist);
			}
		}

		// Lazily instantiate (and cache) the shared skill-tree panel. One prefab serves every tower;
		// the caller passes towerID to panel.Init, which builds that tower's nodes at runtime.
		public TowerSkillTreePanel GetTowerSkillTree()
		{
			if (_towerSkillTreePanel == null)
			{
				if (towerSkillTreePrefab == null)
				{
					return null;
				}
				GameObject instance = UnityEngine.Object.Instantiate<GameObject>(towerSkillTreePrefab);
				instance.transform.SetParent(popupParent);
				instance.transform.localPosition = Vector3.zero;
				instance.transform.localScale = Vector3.one;
				_towerSkillTreePanel = instance.GetComponent<TowerSkillTreePanel>();
			}
			return _towerSkillTreePanel;
		}

		// Closes the skill-tree panel if it is open. Used by the Esc/back handler.
		private bool TryCloseOpenTowerSkillTree()
		{
			if (_towerSkillTreePanel != null && _towerSkillTreePanel.isOpen)
			{
				_towerSkillTreePanel.CloseWithScaleAnimation();
				return true;
			}
			return false;
		}

		public OptionDialogHandler settingPopupController
		{
			get
			{
				return GetOrCreatePopup(settingPrefab, ref _settingPopupController, ref isSettingPopupExist);
			}
		}

		public PrimerDialogHandler guidePopupController
		{
			get
			{
				return GetOrCreatePopup(guidePrefab, ref _guidePopupController, ref isGuidePopupExist);
			}
		}

		public UnlockSkinDialogHandler unlockThemePopupController
		{
			get
			{
				return GetOrCreatePopup(unlockThemePrefab, ref _unlockThemePopupController, ref isUnlockThemePopupExist);
			}
		}

		public InfiniteDialogHandler endlessPopupController
		{
			get
			{
				return GetOrCreatePopup(endlessPrefab, ref _endlessPopupController, ref isEndlessPopupExist);
			}
		}

		public ArenaDialogHandler tournamentPopupController
		{
			get
			{
				return GetOrCreatePopup(tournamentPrefab, ref _tournamentPopupController, ref isTournamentPopupExist);
			}
		}

		public DailyOrdealDialogHandler dailyTrialPopupController
		{
			get
			{
				return GetOrCreatePopup(dailyTrialPrefab, ref _dailyTrialPopupController, ref isDailyTrialPopupExist);
			}
		}

		public SignalQuestDialog eventQuestPopup
		{
			get
			{
				if (_eventQuestPopup == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(eventQuestPrefab, popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_eventQuestPopup = gameObject.GetComponent<SignalQuestDialog>();
				}
				return _eventQuestPopup;
			}
		}

		public LinkGameDialogHandler linkGamePopupController
		{
			get
			{
				return GetOrCreatePopup(linkGamePrefab, ref _linkGamePopupController, ref isLinkGamePopupExist);
			}
		}

		public VoucherCodeDialogHandler giftCodePopupController
		{
			get
			{
				return GetOrCreatePopup(giftCodePrefab, ref _giftCodePopupController, ref isGiftCodePopupExist);
			}
		}

		public PlayerDossierDialogHandler userProfilePopupController
		{
			get
			{
				return GetOrCreatePopup(userProfilePrefab, ref _userProfilePopupController, ref isUserProfilePopupExist);
			}
		}

		private void Start()
		{
			InitDefaultButtonGroups();
			base.CustomInvoke(new Action(InitDefaultPopups), delayTimeToInitPopups);
			RefreshLinkGameButtonStatus();
		}

		private void Update()
		{
			UpdateKeyBack();
		}

		private void InitDefaultButtonGroups()
		{
			int mapIDUnlocked = MapProgressStore.Instance.GetMapIDUnlocked();
			if (mapIDUnlocked >= 1)
			{
				buttonGroup_normal.SetActive(true);
			}
			else
			{
				buttonGroup_normal.SetActive(false);
			}
			if (mapIDUnlocked >= 2)
			{
				buttonGroup_offer.SetActive(true);
				buttonGroup_challenge.SetActive(true);
			}
			else
			{
				buttonGroup_offer.SetActive(false);
				buttonGroup_challenge.SetActive(false);
			}
		}

		private void InitDefaultPopups()
		{
			heroCampPopupController.DefaultInit();
			upgradePopupController.DefaultInit();
			guidePopupController.DefaultInit();
		}

		private void UpdateKeyBack()
		{
			if (Keyboard.current != null && Keyboard.current.escapeKey.wasReleasedThisFrame)
			{
				if (MonoSingleton<LifespanSurface>.Instance.AskToRateDialogHandler.isOpen)
				{
					MonoSingleton<LifespanSurface>.Instance.AskToRateDialogHandler.Close();
					return;
				}
				if (MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.isOpen)
				{
					MonoSingleton<LifespanSurface>.Instance.NotifyPopupController.Close();
					return;
				}
				if (MonoSingleton<LifespanSurface>.Instance.RewardPopupController.isOpen)
				{
					MonoSingleton<LifespanSurface>.Instance.RewardPopupController.Close();
					return;
				}
				if (MonoSingleton<LifespanSurface>.Instance.FreeResourcesPopupController.isOpen)
				{
					MonoSingleton<LifespanSurface>.Instance.FreeResourcesPopupController.CloseWithScaleAnimation();
					return;
				}
				if (MonoSingleton<LifespanSurface>.Instance.StorePopupController.isOpen)
				{
					MonoSingleton<LifespanSurface>.Instance.StorePopupController.CloseWithScaleAnimation();
					return;
				}
                if (MonoSingleton<LifespanSurface>.Instance.OfferPopupController.SingleHeroOfferPopupController.isOpen)
                {
                    MonoSingleton<LifespanSurface>.Instance.OfferPopupController.SingleHeroOfferPopupController.CloseWithScaleAnimation();
                    return;
                }
                if (isHeroCampPopupExist && heroCampPopupController.UltimateUpgradePopupController.isOpen)
				{
					heroCampPopupController.UltimateUpgradePopupController.Close();
					return;
				}
				if (isHeroCampPopupExist && heroCampPopupController.isOpen)
				{
					heroCampPopupController.CloseWithScaleAnimation();
					return;
				}
				if (isMapLevelPopupExist && mapLevelSelectPopupController.isOpen)
				{
					if (mapLevelSelectPopupController.HeroesInputGroupController.AskToBuyHeroDialogHandler.isOpen)
					{
						mapLevelSelectPopupController.HeroesInputGroupController.AskToBuyHeroDialogHandler.CloseWithScaleAnimation();
						return;
					}
					mapLevelSelectPopupController.CloseWithScaleAnimation();
					return;
				}
				if (isUpgradePopupExist && upgradePopupController.isOpen)
				{
					upgradePopupController.CloseWithScaleAnimation();
					return;
				}
				if (TryCloseOpenTowerSkillTree())
				{
					return;
				}
				if (isGiftCodePopupExist && giftCodePopupController.isOpen)
				{
					giftCodePopupController.CloseWithScaleAnimation();
					return;
				}
				if (isUserProfilePopupExist && userProfilePopupController.isOpen)
				{
					if (userProfilePopupController.ChangeNamePopupController.isOpen)
					{
						userProfilePopupController.ChangeNamePopupController.CloseWithScaleAnimation();
						return;
					}
					if (userProfilePopupController.ChangeRegionPopupController.isOpen)
					{
						userProfilePopupController.ChangeRegionPopupController.CloseWithScaleAnimation();
						return;
					}
					if (userProfilePopupController.ConfirmPopup.isOpen)
					{
						userProfilePopupController.ConfirmPopup.CloseWithScaleAnimation();
						return;
					}
					userProfilePopupController.CloseWithScaleAnimation();
					return;
				}
				if (isSettingPopupExist && settingPopupController.isOpen)
				{
					settingPopupController.CloseWithScaleAnimation();
					return;
				}
				if (isGuidePopupExist && guidePopupController.isOpen)
				{
					if (guidePopupController.GuideEnemyController.isOpen)
					{
						guidePopupController.GuideEnemyController.Close();
						return;
					}
					if (guidePopupController.GuideTowerController.isOpen)
					{
						guidePopupController.GuideTowerController.Close();
						return;
					}
					if (guidePopupController.GuideTipsController.isOpen)
					{
						guidePopupController.GuideTipsController.Close();
						return;
					}
					guidePopupController.CloseWithScaleAnimation();
					return;
				}
				if (isUnlockThemePopupExist && unlockThemePopupController.isOpen)
				{
					unlockThemePopupController.CloseWithScaleAnimation();
					return;
				}
				if (isDailyTrialPopupExist && dailyTrialPopupController.isOpen)
				{
					dailyTrialPopupController.CloseWithScaleAnimation();
					return;
				}
				if (isLinkGamePopupExist && linkGamePopupController.isOpen)
				{
					linkGamePopupController.CloseWithScaleAnimation();
					return;
				}
				if (isEndlessPopupExist && endlessPopupController.isOpen)
				{
					endlessPopupController.CloseWithScaleAnimation();
					return;
				}
				if (isTournamentPopupExist && tournamentPopupController.isOpen)
				{
					tournamentPopupController.CloseWithScaleAnimation();
					return;
				}
				OpenSceneMainMenu();
			}
		}

		private void OpenSceneMainMenu()
		{
			LoadingScreen.Instance.ShowLoading();
			base.Invoke("DoLoadSceneMainMenu", 1f);
		}

		private void DoLoadSceneMainMenu()
		{
			GameSceneLoader.Instance.LoadScene(GameSceneLoader.MainMenuSceneName);
		}

		public void RefreshLinkGameButtonStatus()
		{
			// Cross-promo to install the Android build: useless off mobile. On Steam/PC
			// CheckPackageAppIsPresent always returns false, so the 200-gem reward can
			// never be claimed and the button only ever opens a Google Play market:// link.
			// Hide it on non-Android until a Steam-native version exists.
			if (UnityEngine.Application.platform != UnityEngine.RuntimePlatform.Android)
			{
				linkGameButton.SetActive(false);
				return;
			}
			if (OffersStore.Instance.IsOfferProcessed(OffersStore.KEY_INSTALL_GOE))
			{
				linkGameButton.SetActive(false);
			}
			else
			{
				linkGameButton.SetActive(true);
			}
		}
	}
}
