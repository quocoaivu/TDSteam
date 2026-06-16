using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using MetaGame;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class PowerUpItemDialogHandler : MonoSingleton<PowerUpItemDialogHandler>
	{
		private void Awake()
		{
			GetListPowerUpItems();
			selectingItemID = -1;
			isOnAnimation = false;
			PowerUpItemStore.Instance.OnItemQuantityChangeEvent += Instance_OnItemQuantityChangeEvent;
		}

		private void Start()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						RefreshQuantityAllItems();
					}
				}
				else
				{
					InitDataDailyTrial();
					RefreshQuantityAllItems();
				}
			}
			else
			{
				RefreshQuantityAllItems();
			}
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnClickButton, new TapSwitchListenerRecord(GameKit.GetUniqueId(), new GameSignalCenter.ClickButtonMethod(HandleBtnClicked)));
		}

		private void OnDestroy()
		{
			if (PowerUpItemStore.Instance != null)
			{
				PowerUpItemStore.Instance.OnItemQuantityChangeEvent -= Instance_OnItemQuantityChangeEvent;
			}
		}

		public void InitDataDailyTrial()
		{
			int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
			listItemQuantityDailyTrial = DailyOrdealSpec.Instance.getListInputItem(currentDayIndex);
			UnityEngine.Debug.Log(listItemQuantityDailyTrial);
		}

		public int GetDailyItemQuantity(int itemID)
		{
			return listItemQuantityDailyTrial[itemID];
		}

		public void ChangeItemQuantity(int itemID, int amount)
		{
			listItemQuantityDailyTrial[itemID] += amount;
		}

		private void Instance_OnItemQuantityChangeEvent()
		{
			RefreshQuantityAllItems();
		}

		public void RefreshQuantityAllItems()
		{
			foreach (PowerUpEntry powerUpItem in listPowerUpItem)
			{
				powerUpItem.RefreshQuantity();
			}
		}

		public void CastItemSkill()
		{
			if (selectingItemID < 0)
			{
				return;
			}
			listPowerUpItem[selectingItemID].CastItemSkill();
			DisplayItemIsNotSelecting();
		}

		public void DisplayItemIsSelecting()
		{
			imageCancel.SetActive(true);
			currentItemIcon.sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_pw_{0}", selectingItemID));
			currentItemIcon.SetNativeSize();
		}

		public void DisplayItemIsNotSelecting()
		{
			imageCancel.SetActive(false);
			currentItemIcon.sprite = originIcon;
			currentItemIcon.SetNativeSize();
			ResumeOldGameSpeed();
		}

		private void GetListPowerUpItems()
		{
			listPowerUpItem = new List<PowerUpEntry>(base.GetComponentsInChildren<PowerUpEntry>(true));
		}

		public void OnClick()
		{
            UISfxDirector.Instance.PlayClick();
			GameSignalCenter.Instance.Trigger(GameSignalKind.OnClickButton, new TappedObjectRecord(TappedObjectKind.ItemSkillBtn));
		}

		public void HandleBtnClicked(TappedObjectRecord clickedObjData)
		{
			if (selectingItemID >= 0)
			{
				selectingItemID = -1;
				DisplayItemIsNotSelecting();
				UnityEngine.Debug.Log("bá» chá»n2");
				return;
			}
			if (clickedObjData.clickedObjType == TappedObjectKind.ItemSkillBtn)
			{
				toggle = !toggle;
				if (toggle)
				{
					OpenPopup();
				}
				else
				{
					ResumeOldGameSpeed();
					ClosePopup();
				}
			}
			else if (toggle)
			{
				toggle = false;
				ResumeOldGameSpeed();
				ClosePopup();
			}
		}

		private void TurnOnItemInteract()
		{
			foreach (PowerUpEntry powerUpItem in listPowerUpItem)
			{
				powerUpItem.ReadyToUse();
			}
		}

		private void TurnOffItemInteract()
		{
			foreach (PowerUpEntry powerUpItem in listPowerUpItem)
			{
				powerUpItem.NotReadyToUse();
			}
		}

		public void OpenPopup()
		{
            TrickyTrigger.dataTricky = 1;
            TrickyRoad.dataRoad = 1;
            if (isOnAnimation)
			{
				return;
			}
			gameOldSpeed = GameplayDirector.Instance.gameSpeedController.GameSpeed;
			isOnAnimation = true;
			popup.transform.DOLocalMove(Vector3.zero, timeToOpen, false).OnComplete(new TweenCallback(OnOpenComplete));
			popup.transform.DOScale(1f, timeToOpen);
			UISfxDirector.Instance.PlayOpenPopup();
			toggle = true;
			TurnOffItemInteract();
		}

		private void OnOpenComplete()
		{
			isOnAnimation = false;
			TurnOnItemInteract();
			RefreshQuantityAllItems();
		}

		public void ClosePopup()
		{
            TrickyTrigger.dataTricky = 0;
            TrickyRoad.dataRoad = 0;
            if (isOnAnimation)
			{
				return;
			}
			base.CustomInvoke(new Action(DoClose), timeToClose);
		}

		private void DoClose()
		{
			TurnOffItemInteract();
			UISfxDirector.Instance.PlayClosePopup();
			isOnAnimation = true;
			popup.transform.DOLocalMove(new Vector3(220f, -450f, 0f), timeToOpen, false).OnComplete(new TweenCallback(OnCloseComplete));
			popup.transform.DOScale(0f, timeToOpen);
			toggle = false;
		}

		private void OnCloseComplete()
		{
			isOnAnimation = false;
		}

		public void ResumeOldGameSpeed()
		{
		}

		[Space]
		[Header("Popup ")]
		[SerializeField]
		private GameObject popup;

		[SerializeField]
		private float timeToOpen;

		[SerializeField]
		private float timeToClose;

		private bool toggle;

		[SerializeField]
		private GameObject imageCancel;

		[Space]
		[Header("List Item")]
		public List<PowerUpEntry> listPowerUpItem = new List<PowerUpEntry>();

		[Space]
		[Header("List Item Icon")]
		[HideInInspector]
		public int selectingItemID = -1;

		[SerializeField]
		private Sprite originIcon;

		[SerializeField]
		private Image currentItemIcon;

		private bool isOnAnimation;

		private int[] listItemQuantityDailyTrial = new int[4];

		private float gameOldSpeed = 1f;

		private float slowSpeed = 0.3f;
	}
}
