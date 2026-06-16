using System;
using MetaGame;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class InZoneBeginSurgeSwitch : ControlSurgeSwitchHandler
	{
        [SerializeField]
        private float offset = 40f;

        [SerializeField]
        private Image imageCooldown;

        [SerializeField]
        private Image indicator;

        [SerializeField]
        private GameObject flyEnemiesImage;

        private Vector3 StartWavePosition;

        private Vector3 OriginPos;

        private InZoneBeginSurgeButtonsDirector inMapStartWaveButtonsManager;


        private RectTransform rectTransform;
        private void Awake()
		{
			rectTransform = base.GetComponent<RectTransform>();
		}

		public override void Update()
		{
			base.Update();
			indicator.transform.right = base.transform.position - StartWavePosition;
		}

		public void Initialize(Vector3 position, float canvasHeight)
		{
			StartWavePosition = position;
			OriginPos = position * 100f;
			float num = Camera.main.orthographicSize * 2f;
			float num2 = num * (float)Screen.width / (float)Screen.height;
			OriginPos.x = Mathf.Clamp(OriginPos.x, -(num2 * 100f / 2f) + offset, num2 * 100f / 2f - offset);
			OriginPos.y = Mathf.Clamp(OriginPos.y, -(num * 100f / 2f) + offset, num * 100f / 2f - offset);
			if (OriginPos.y > canvasHeight / 2f - 100f)
			{
				OriginPos.x = Mathf.Clamp(OriginPos.x, -350f, 430f);
			}
			if (OriginPos.y < -(canvasHeight / 2f - 100f) && OriginPos.x > 245f)
			{
				OriginPos.y = -(canvasHeight / 2f) + 170f;
			}
			if (OriginPos.y < -(canvasHeight / 2f - 100f) && OriginPos.x < -320f)
			{
				OriginPos.y = -(canvasHeight / 2f) + 170f;
			}
			rectTransform.localPosition = OriginPos;
		}

		public void Initialize(InZoneBeginSurgeButtonsDirector inMapStartWaveButtonsManager)
		{
			this.inMapStartWaveButtonsManager = inMapStartWaveButtonsManager;
		}

		public void Show(bool isFlyEnemy)
		{
			base.gameObject.SetActive(true);
			flyEnemiesImage.SetActive(isFlyEnemy);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		public void UpdateCountDownTime(float currentValue, float maxValue)
		{
			imageCooldown.fillAmount = currentValue / maxValue;
		}

		private void StartWave()
		{
			inMapStartWaveButtonsManager.BonusMoney(base.transform.position);
			GameplayDirector.Instance.StartWave();
			Hide();
			MonoSingleton<AllyPool>.Instance.RestoreHealthForAllAllies();
			MonoSingleton<GameplayUIHeroDirector>.Instance.RestoreAllCooldownSkill();
			UISfxDirector.Instance.PlayCallEnemy();
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode == GameFormat.DailyTrialMode)
				{
					if (ZoneRuleSpec.Instance.HaveEventNewTower(MonoSingleton<GameRecord>.Instance.CurrentWave))
					{
						MonoSingleton<UIRootHandler>.Instance.TowerInformationUIManager.InitTowerInformationPanel();
					}
				}
			}
			else
			{
				SendEventCallEnemies_Active();
			}
		}

		private void SendEventCallEnemies_Active()
		{
			int mapID = MonoSingleton<GameRecord>.Instance.MapID;
			int currentWave = MonoSingleton<GameRecord>.Instance.CurrentWave;
			string callType = "Active";
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_CallEarlyEnemies(mapID + 1, currentWave, callType);
		}

		public override void OnClick()
		{
			base.OnClick();
			if (buttonStatus == GameplaySwitchHandler.ButtonStatus.Available)
			{
				OnClickAvailable();
			}
			else if (buttonStatus == GameplaySwitchHandler.ButtonStatus.Confirm)
			{
				OnConfirm();
			}
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnClickButton, new TappedObjectRecord(TappedObjectKind.NextWaveBtn));
		}

		protected override void OnClickAvailable()
		{
			base.OnClickAvailable();
			inMapStartWaveButtonsManager.DisableConfirmOtherButtons(this);
			MonoSingleton<UIRootHandler>.Instance.incomingWavePopupController.Init(base.transform.localPosition, rectTransform.sizeDelta);
			inMapStartWaveButtonsManager.DispatchEvent_ClickButton();
		}

		protected override void OnConfirm()
		{
			base.OnClick();
			StartWave();
			MonoSingleton<UIRootHandler>.Instance.incomingWavePopupController.Close();
			inMapStartWaveButtonsManager.DisableConfirmAllButton();
			inMapStartWaveButtonsManager.DispatchEvent_ConfirmButton();
		}

		protected override void OnClickOutsideDown()
		{
			base.OnClickOutsideDown();
			MonoSingleton<UIRootHandler>.Instance.incomingWavePopupController.Close();
			DisableConfirm();
		}

		protected override void OnClickOutsideUp()
		{
			base.OnClickOutsideUp();
		}
	}
}
