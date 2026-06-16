using System;
using System.Collections.Generic;
using MetaGame;
using Parameter;
using Services.PlatformSpecific;
using Common;
using Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class InZoneBeginSurgeButtonsDirector : MonoBehaviour
	{
        private static string STARTWAVE_BUTTON_NAME = "ButtonStartWave";

        private static string BONUSMONEY_OBJECT_NAME = "BonusMoney";

        [SerializeField]
        private OrderedUnityEvent onClickCallEnemyButtonEvents;

        [SerializeField]
        private OrderedUnityEvent onConfirmCallEnemyButtonEvents;

        [Space]
        [SerializeField]
        private InZoneBeginSurgeSwitch prefabStartWaveButton;

        [SerializeField]
        private BonusBountyMotionHandler bonusMoneyAnimController;

        private List<InZoneBeginSurgeSwitch> buttons = new List<InZoneBeginSurgeSwitch>();

        [SerializeField]
        private RectTransform canvas;

        [SerializeField]
        private Transform listButtonHolder;

        private float delayTime;

        private float countTime;

        private bool isCounting;

        private int freeTime;

        private int bonusMoney;

        public static InZoneBeginSurgeButtonsDirector Instance { get; set; }

		private void Awake()
		{
			if (InZoneBeginSurgeButtonsDirector.Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			InZoneBeginSurgeButtonsDirector.Instance = this;
			MonoSingleton<FXPool>.Instance.InitExtendObject(prefabStartWaveButton.gameObject, 0);
			MonoSingleton<FXPool>.Instance.InitExtendObject(bonusMoneyAnimController.gameObject, 0);
		}

		private void Start()
		{
			if (GameplayTutorialDirector.Instance.IsTutorialDone() || !GameplayTutorialDirector.Instance.IsTutorialMap())
			{
				ShowListButtonHolder();
			}
			else
			{
				HideListButtonHolder();
			}
		}

		private void Update()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						if (isCounting)
						{
							if (countTime == 0f)
							{
								countTime = 0f;
								isCounting = false;
								GameplayDirector.Instance.StartWave();
								SendEventCallEnemies_Passive();
								Hide();
							}
							countTime = Mathf.MoveTowards(countTime, 0f, Time.deltaTime);
							UpdateCountDownButton();
						}
					}
				}
			}
			else if (isCounting)
			{
				if (countTime == 0f)
				{
					countTime = 0f;
					isCounting = false;
					GameplayDirector.Instance.StartWave();
					SendEventCallEnemies_Passive();
					Hide();
				}
				countTime = Mathf.MoveTowards(countTime, 0f, Time.deltaTime);
				UpdateCountDownButton();
			}
		}

		public void InitListButtons()
		{
			CreateListButtons();
			GetListButtons();
			InitializeButtons();
		}

		private void SendEventCallEnemies_Passive()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
				}
			}
			else
			{
				int mapID = MonoSingleton<GameRecord>.Instance.MapID;
				int currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
				string callType = "Passive";
				//NativeSpecificServicesSource.Services.Analytics.SendEvent_CallEarlyEnemies(mapID + 1, currentWave, callType);
			}
		}

		private void CreateListButtons()
		{
			List<Transform> listStartPosition = MonoSingleton<BeginSurgePositionDirector>.Instance.listStartPosition;
			for (int i = 0; i < listStartPosition.Count; i++)
			{
				GameObject objectByName = MonoSingleton<FXPool>.Instance.GetObjectByName(InZoneBeginSurgeButtonsDirector.STARTWAVE_BUTTON_NAME);
				objectByName.transform.SetParent(listButtonHolder);
				objectByName.transform.localScale = Vector3.one;
				objectByName.SetActive(true);
				objectByName.GetComponent<InZoneBeginSurgeSwitch>().Initialize(listStartPosition[i].position, canvas.rect.height);
			}
		}

		public void Show(int wave, List<int> listEnemyGate, float delayTime)
		{
			this.delayTime = delayTime;
			countTime = delayTime;
			isCounting = true;
			ShowButtons(wave, listEnemyGate);
		}

		public void Hide()
		{
			countTime = 0f;
			isCounting = false;
			HideButtons();
		}

		public void ShowListButtonHolder()
		{
			listButtonHolder.gameObject.SetActive(true);
		}

		public void HideListButtonHolder()
		{
			listButtonHolder.gameObject.SetActive(false);
		}

		public void DisableConfirmOtherButtons(ControlSurgeSwitchHandler NoDisableButton)
		{
			foreach (InZoneBeginSurgeSwitch inMapStartWaveButton in buttons)
			{
				if (!inMapStartWaveButton.Equals(NoDisableButton))
				{
					inMapStartWaveButton.DisableConfirm();
				}
			}
		}

		public void DisableConfirmAllButton()
		{
			foreach (InZoneBeginSurgeSwitch inMapStartWaveButton in buttons)
			{
				inMapStartWaveButton.DisableConfirm();
			}
		}

		public void BonusMoney(Vector3 buttonPosition)
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode == GameFormat.DailyTrialMode)
				{
					int goldBonusForCallEnemy = ZoneRuleSpec.Instance.GetGoldBonusForCallEnemy(MonoSingleton<GameRecord>.Instance.CurrentWave);
					MonoSingleton<GameRecord>.Instance.IncreaseMoney(goldBonusForCallEnemy);
					int healthBonusForCallEnemy = ZoneRuleSpec.Instance.GetHealthBonusForCallEnemy(MonoSingleton<GameRecord>.Instance.CurrentWave);
					GameplayDirector.Instance.gameLogicController.IncreaseHealth(healthBonusForCallEnemy);
				}
			}
			else
			{
				bonusMoney = Mathf.RoundToInt((float)Setup.Instance.EarlyCallMoney * countTime);
				MonoSingleton<GameRecord>.Instance.IncreaseMoney(bonusMoney);
				if (bonusMoney > 0)
				{
					GameObject objectByName = MonoSingleton<FXPool>.Instance.GetObjectByName(InZoneBeginSurgeButtonsDirector.BONUSMONEY_OBJECT_NAME);
					objectByName.transform.SetParent(base.gameObject.transform, false);
					objectByName.transform.localScale = Vector3.one;
					objectByName.SetActive(true);
					objectByName.transform.position = buttonPosition;
					objectByName.GetComponent<BonusBountyMotionHandler>().Init(bonusMoney);
				}
			}
		}

		public void ShowListButtonOnStart()
		{
			ShowButtons(0, EnemyDatabase.Instance.getListEnemyGate(0));
		}

		private void GetListButtons()
		{
			buttons = new List<InZoneBeginSurgeSwitch>(base.GetComponentsInChildren<InZoneBeginSurgeSwitch>());
		}

		private void InitializeButtons()
		{
			foreach (InZoneBeginSurgeSwitch button in buttons)
			{
				InitializeButton(button);
			}
		}

		private void InitializeButton(InZoneBeginSurgeSwitch button)
		{
			button.Initialize(this);
		}

		private void ShowButtons(int wave, List<int> listGate)
		{
			HideButtons();
			for (int i = 0; i < listGate.Count; i++)
			{
				bool isFlyEnemy = EnemyDatabase.Instance.IsFlyEnemyInGate(wave, listGate[i]);
				if (listGate[i] >= buttons.Count)
				{
					buttons[listGate[i] - buttons.Count].Show(isFlyEnemy);
				}
				else
				{
					buttons[listGate[i]].Show(isFlyEnemy);
				}
			}
			UISfxDirector.Instance.PlayBeforeCallEnemy();
		}

		private void UpdateCountDownButton()
		{
			foreach (InZoneBeginSurgeSwitch inMapStartWaveButton in buttons)
			{
				inMapStartWaveButton.UpdateCountDownTime(countTime, delayTime);
			}
		}

		private void HideButtons()
		{
			foreach (InZoneBeginSurgeSwitch inMapStartWaveButton in buttons)
			{
				inMapStartWaveButton.Hide();
			}
			MonoSingleton<UIRootHandler>.Instance.incomingWavePopupController.Close();
			DisableConfirmAllButton();
		}

		public void DispatchEvent_ClickButton()
		{
			onClickCallEnemyButtonEvents.Dispatch();
		}

		public void DispatchEvent_ConfirmButton()
		{
			onConfirmCallEnemyButtonEvents.Dispatch();
		}
	}
}
