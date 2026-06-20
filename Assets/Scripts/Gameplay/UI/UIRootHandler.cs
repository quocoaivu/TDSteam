using System;
using DailyTrial;
using Gameplay.EndGame;
using IncomingWave;
using LifetimePopup;
using MetaGame;
using Tutorial;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Gameplay
{
	public class UIRootHandler : MonoSingleton<UIRootHandler>
	{
		public BuyTurretDialogHandler BuyTowerPopupController
		{
			get
			{
				return buyTowerPopupController;
			}
			set
			{
				buyTowerPopupController = value;
			}
		}

		public EnhanceTurretDialogHandler UpgradeTowerPopupController
		{
			get
			{
				return upgradeTowerPopupController;
			}
			set
			{
				upgradeTowerPopupController = value;
			}
		}

		public OptionDialogHandler settingPopupController
		{
			get
			{
				if (_settingPopupController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefabSetting);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_settingPopupController = gameObject.GetComponent<OptionDialogHandler>();
					isSettingPopupExist = true;
				}
				return _settingPopupController;
			}
			set
			{
				_settingPopupController = value;
			}
		}

		public EndGameDialogHandler endGamePopupController
		{
			get
			{
				if (_endGamePopupController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefabEndgame);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_endGamePopupController = gameObject.GetComponent<EndGameDialogHandler>();
					isEndGamePopupExist = true;
				}
				return _endGamePopupController;
			}
			set
			{
				_endGamePopupController = value;
			}
		}

		public EndGameClipHandler endGameVideoController
		{
			get
			{
				if (_endGameVideoController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefabEndGameVideo);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_endGameVideoController = gameObject.GetComponent<EndGameClipHandler>();
					isEndGameVideoPopupExist = true;
				}
				return _endGameVideoController;
			}
			set
			{
				_endGameVideoController = value;
			}
		}

		public FreeResourcesDialogHandler freeResourcesPopupController
		{
			get
			{
				if (_freeResourcesPopupController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefabFreeResources);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_freeResourcesPopupController = gameObject.GetComponent<FreeResourcesDialogHandler>();
					isGameplayVideoPopupExist = true;
				}
				return _freeResourcesPopupController;
			}
			set
			{
				_freeResourcesPopupController = value;
			}
		}

		public DailyOrdealOutcomeDialogHandler dailyTrialResultPopupController
		{
			get
			{
				if (_dailyTrialResultPopupController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(dailyTrialResultPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_dailyTrialResultPopupController = gameObject.GetComponent<DailyOrdealOutcomeDialogHandler>();
					isDailyTrialResultPopupExist = true;
				}
				return _dailyTrialResultPopupController;
			}
			set
			{
				_dailyTrialResultPopupController = value;
			}
		}

		public ArenaOutcomeDialogHandler tournamentResultPopupController
		{
			get
			{
				if (_tournamentResultPopupController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(tournamentResultPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_tournamentResultPopupController = gameObject.GetComponent<ArenaOutcomeDialogHandler>();
					isTournamentResultPopupExist = true;
				}
				return _tournamentResultPopupController;
			}
			set
			{
				_tournamentResultPopupController = value;
			}
		}

		public IncomingSurgeDialogHandler incomingWavePopupController
		{
			get
			{
				if (_incomingWavePopupController == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(incomingWavePopupPrefab);
					gameObject.transform.SetParent(incomingWavePopupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_incomingWavePopupController = gameObject.GetComponent<IncomingSurgeDialogHandler>();
				}
				return _incomingWavePopupController;
			}
			set
			{
				_incomingWavePopupController = value;
			}
		}

		public InZoneBeginSurgeButtonsDirector InMapStartWaveButtonsManager
		{
			get
			{
				if (inMapStartWaveButtonsManager == null)
				{
					inMapStartWaveButtonsManager = UnityEngine.Object.FindAnyObjectByType<InZoneBeginSurgeButtonsDirector>();
				}
				return inMapStartWaveButtonsManager;
			}
		}

		public SurgeMessageHandler WaveMessageController
		{
			get
			{
				return waveMessageController;
			}
			set
			{
				waveMessageController = value;
			}
		}

		public TowerInformationUIManager TowerInformationUIManager
		{
			get
			{
				return towerInformationUIManager;
			}
			set
			{
				towerInformationUIManager = value;
			}
		}

		public EnemyDiscoveryPopup EnemyDiscoveryPopup
		{
			get
			{
				if (enemyDiscoveryPopup == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyInforPopupPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					enemyDiscoveryPopup = gameObject.GetComponent<EnemyDiscoveryPopup>();
					isEnemyDiscoveryPopupExist = true;
				}
				return enemyDiscoveryPopup;
			}
			set
			{
				enemyDiscoveryPopup = value;
			}
		}

		public GameplayHintOverviewDialog GameplayTipPopup
		{
			get
			{
				if (gameplayTipPopup == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(tipPopupPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					gameplayTipPopup = gameObject.GetComponent<GameplayHintOverviewDialog>();
					isGameplayTipsInformationPopupExist = true;
				}
				return gameplayTipPopup;
			}
			set
			{
				gameplayTipPopup = value;
			}
		}

		private void Start()
		{
			if (GameplayTutorialDirector.Instance.IsTutorialDone() || !GameplayTutorialDirector.Instance.IsTutorialMap())
			{
				buttonSpeed.SetActive(true);
			}
			else
			{
				buttonSpeed.SetActive(false);
			}
			InitWaveMessage();
			enemyInformationPopup.Init();
			allyInformationPopup.Init();
			petInformationPopup.Init();
			InitFreeResourcesButton();
		}

		private void Update()
		{
			UpdateKeyBack();
		}

		private void InitWaveMessage()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						waveMessageController = UnityEngine.Object.Instantiate<SurgeMessageHandler>(waveMessageTournamentPrefab);
						waveMessageController.transform.SetParent(waveHolderTournament);
					}
				}
				else
				{
					waveMessageController = UnityEngine.Object.Instantiate<SurgeMessageHandler>(waveMessageDailyTrialPrefab);
					waveMessageController.transform.SetParent(waveHolderDailyTrial);
				}
			}
			else
			{
				waveMessageController = UnityEngine.Object.Instantiate<SurgeMessageHandler>(waveMessageCampaignPrefab);
				waveMessageController.transform.SetParent(waveHolderCampaign);
			}
			waveMessageController.transform.localScale = Vector3.one;
			waveMessageController.transform.localPosition = Vector3.zero;
		}

		public void ShowSpeedButton()
		{
			buttonSpeed.SetActive(true);
		}

		private void InitFreeResourcesButton()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						freeResourcesButton.SetActive(false);
					}
				}
				else
				{
					freeResourcesButton.SetActive(false);
				}
			}
			else
			{
				freeResourcesButton.SetActive(false);
			}
		}

		public void RefreshStatusFreeResourcesButton()
		{
			if (freeResourcesPopupController.VideoLife.IsPlayed && freeResourcesPopupController.VideoMoney.IsPlayed)
			{
				freeResourcesButton.SetActive(false);
			}
		}

		private void UpdateKeyBack()
		{
			if (Keyboard.current != null && Keyboard.current.escapeKey.wasReleasedThisFrame)
			{
				if (MonoSingleton<GameRecord>.Instance.IsPlayingVideoAds)
				{
					return;
				}
				// Carrying an item: ESC drops it instead of opening the settings popup.
				if (Items.ItemCarryController.IsCarryingItem)
				{
					Items.ItemCarryController.Instance.Release();
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
				if (isGameplayVideoPopupExist && freeResourcesPopupController.isOpen)
				{
					freeResourcesPopupController.Close();
					return;
				}
				if (isEndGameVideoPopupExist && endGameVideoController.isOpen)
				{
					return;
				}
				if (MonoSingleton<GameRecord>.Instance.IsGameOver)
				{
					return;
				}
				if (TowerInformationUIManager.TowerInforPanel && TowerInformationUIManager.TowerInforPanel.isOpen)
				{
					TowerInformationUIManager.TowerInforPanel.CloseWithScaleAnimation();
					return;
				}
				if (isEnemyDiscoveryPopupExist && EnemyDiscoveryPopup.isOpen)
				{
					EnemyDiscoveryPopup.CloseWithScaleAnimation();
					return;
				}
				if (isGameplayTipsInformationPopupExist && GameplayTipPopup.isOpen)
				{
					GameplayTipPopup.CloseWithScaleAnimation();
					return;
				}
				if (!isSettingPopupExist || !settingPopupController.isOpen)
				{
					settingPopupController.Open();
					return;
				}
				if (isSettingPopupExist && settingPopupController.isOpen)
				{
					settingPopupController.Close();
					return;
				}
			}
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				if (LoadingScreen.Instance.IsLoading)
				{
					return;
				}
				GameRecord gameData = MonoSingleton<GameRecord>.Instance;
				if (gameData == null)
				{
					return;
				}
				if (gameData.IsGameOver)
				{
					return;
				}
				if (gameData.IsPlayingVideoAds)
				{
					return;
				}
				settingPopupController.Open();
			}
		}

		[Header("Game UI Controller")]
		[SerializeField]
		private BuyTurretDialogHandler buyTowerPopupController;

		[SerializeField]
		private EnhanceTurretDialogHandler upgradeTowerPopupController;

		public EnemyDetailPopup enemyInformationPopup;

		public MinionOverviewDialog allyInformationPopup;

		public PetOverviewDialog petInformationPopup;

		[Space]
		[Header("Game UI Elements")]
		[SerializeField]
		private Transform popupParent;

		[SerializeField]
		[FormerlySerializedAs("newTowerPopupParent")]
		private Transform towerPopupParent;

		[SerializeField]
		private Transform incomingWavePopupParent;

		[Header("Setting Popup")]
		[SerializeField]
		private GameObject prefabSetting;

		private bool isSettingPopupExist;

		private OptionDialogHandler _settingPopupController;

		[Header("Endgame Popup")]
		[SerializeField]
		private GameObject prefabEndgame;

		private bool isEndGamePopupExist;

		private EndGameDialogHandler _endGamePopupController;

		[Header("Endgame Video Popup")]
		[SerializeField]
		private GameObject prefabEndGameVideo;

		private bool isEndGameVideoPopupExist;

		private EndGameClipHandler _endGameVideoController;

		[Header("Gameplay free resources")]
		[SerializeField]
		private GameObject freeResourcesButton;

		[SerializeField]
		private GameObject prefabFreeResources;

		private bool isGameplayVideoPopupExist;

		private FreeResourcesDialogHandler _freeResourcesPopupController;

		[Header("Daily Trial Result")]
		[SerializeField]
		private GameObject dailyTrialResultPrefab;

		[HideInInspector]
		private bool isDailyTrialResultPopupExist;

		private DailyOrdealOutcomeDialogHandler _dailyTrialResultPopupController;

		[Header("Tournament Result")]
		[SerializeField]
		private GameObject tournamentResultPrefab;

		[HideInInspector]
		private bool isTournamentResultPopupExist;

		private ArenaOutcomeDialogHandler _tournamentResultPopupController;

		[Header("Incoming Wave Popup")]
		[SerializeField]
		private GameObject incomingWavePopupPrefab;

		[HideInInspector]
		private IncomingSurgeDialogHandler _incomingWavePopupController;

		private InZoneBeginSurgeButtonsDirector inMapStartWaveButtonsManager;

		[Space]
		[Header("Game UI Infomation")]
		public BountyHandler moneyController;

		public PlayerVitalityHandler playerHealthController;

		private SurgeMessageHandler waveMessageController;

		[Space]
		[Header("Wave message:")]
		[SerializeField]
		private SurgeMessageHandler waveMessageCampaignPrefab;

		[SerializeField]
		private SurgeMessageHandler waveMessageDailyTrialPrefab;

		[SerializeField]
		private SurgeMessageHandler waveMessageTournamentPrefab;

		[SerializeField]
		private Transform waveHolderCampaign;

		[SerializeField]
		private Transform waveHolderDailyTrial;

		[SerializeField]
		private Transform waveHolderTournament;

		[Space]
		[SerializeField]
		private GameObject buttonSpeed;

		[Space]
		[Header("Game new tips")]
		[SerializeField]
		[FormerlySerializedAs("newTowerInformationUIManager")]
		private TowerInformationUIManager towerInformationUIManager;

		[SerializeField]
		private Transform popupParentNewEnemy;

		[SerializeField]
		private GameObject enemyInforPopupPrefab;

		private bool isEnemyDiscoveryPopupExist;

		private EnemyDiscoveryPopup enemyDiscoveryPopup;

		[SerializeField]
		private Transform popupParentGameplayTips;

		[SerializeField]
		private GameObject tipPopupPrefab;

		private bool isGameplayTipsInformationPopupExist;

		private GameplayHintOverviewDialog gameplayTipPopup;
	}
}
