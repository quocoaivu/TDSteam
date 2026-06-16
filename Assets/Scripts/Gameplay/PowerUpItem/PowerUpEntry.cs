using System;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MetaGame;
using GameCore;
using Parameter;
using Services.PlatformSpecific;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class PowerUpEntry : BaseMonoBehaviour
	{
		private void Awake()
		{
			button = base.GetComponent<Button>();
		}

		public void Init(float _cooldownTime)
		{
			cooldownTime = _cooldownTime;
		}

		public void RefreshQuantity()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						int itemAmount = PowerUpItemStore.Instance.GetCurrentItemQuantity(powerUpItemID);
						bool isReachedLimit = false;
						if (MonoSingleton<GameRecord>.Instance)
						{
							isReachedLimit = MonoSingleton<GameRecord>.Instance.PowerupItemData.IsReachedLimitUse();
						}
						RefreshStatusByLimitUse(itemAmount, isReachedLimit);
					}
				}
				else
				{
					int itemAmount = MonoSingleton<PowerUpItemDialogHandler>.Instance.GetDailyItemQuantity(powerUpItemID);
					RefreshStatusByLimitGiven(itemAmount);
				}
			}
			else
			{
				int itemAmount = PowerUpItemStore.Instance.GetCurrentItemQuantity(powerUpItemID);
				RefreshStatusByQuantity(itemAmount);
			}
		}

		private void RefreshStatusByQuantity(int itemAmount)
		{
			if (itemAmount >= 1)
			{
				ViewEnable();
				buttonBuyMore.SetActive(false);
			}
			else
			{
				ViewDisable();
				buttonBuyMore.SetActive(true);
			}
			quantity.text = itemAmount.ToString();
		}

		private void RefreshStatusByLimitGiven(int itemAmount)
		{
			if (itemAmount >= 1)
			{
				ViewEnable();
			}
			else
			{
				ViewDisable();
			}
			buttonBuyMore.SetActive(false);
			quantity.text = itemAmount.ToString();
		}

		private void RefreshStatusByLimitUse(int itemAmount, bool isReachedLimit)
		{
			if (isReachedLimit)
			{
				ViewDisable();
				buttonBuyMore.SetActive(false);
			}
			else if (itemAmount >= 1)
			{
				ViewEnable();
				buttonBuyMore.SetActive(false);
			}
			else
			{
				ViewDisable();
				buttonBuyMore.SetActive(true);
			}
			quantity.text = itemAmount.ToString();
		}

		public void OnClick()
		{
            PowerUpItemUseKind powerUpItemUseType = useType;
			if (powerUpItemUseType != PowerUpItemUseKind.TapToUse)
			{
				if (powerUpItemUseType == PowerUpItemUseKind.TapNClickOutSideToUse)
				{
					MonoSingleton<PowerUpItemDialogHandler>.Instance.selectingItemID = powerUpItemID;
					MonoSingleton<PowerUpItemDialogHandler>.Instance.ClosePopup();
					MonoSingleton<PowerUpItemDialogHandler>.Instance.DisplayItemIsSelecting();
				}
			}
			else
			{
				MonoSingleton<PowerUpItemDialogHandler>.Instance.selectingItemID = powerUpItemID;
				CastItemSkill();
				MonoSingleton<PowerUpItemDialogHandler>.Instance.ClosePopup();
				MonoSingleton<PowerUpItemDialogHandler>.Instance.ResumeOldGameSpeed();
			}
		}

		public void CastItemSkill()
		{
			OnCastItemSkillEvent.Dispatch();
			DoCooldown();
			base.CustomInvoke(new Action(ResetValueAfterUse), Time.deltaTime);
			ChangeItemQuantity();
			SendEventUsePowerupItem();
			if (FormatDirector.Instance.gameMode == GameFormat.TournamentMode)
			{
				MonoSingleton<GameRecord>.Instance.PowerupItemData.IncreaseUseAmount();
			}
		}

		private void ChangeItemQuantity()
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						PowerUpItemStore.Instance.ChangeItemQuantity(powerUpItemID, -1);
					}
				}
				else
				{
					MonoSingleton<PowerUpItemDialogHandler>.Instance.ChangeItemQuantity(powerUpItemID, -1);
					MonoSingleton<PowerUpItemDialogHandler>.Instance.RefreshQuantityAllItems();
				}
			}
			else
			{
				PowerUpItemStore.Instance.ChangeItemQuantity(powerUpItemID, -1);
			}
		}

		private void SendEventUsePowerupItem()
		{
			string name = Singleton<PowerUpItemDescription>.Instance.GetName(powerUpItemID);
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_UsePowerupItem(name);
		}

		private void ResetValueAfterUse()
		{
			MonoSingleton<PowerUpItemDialogHandler>.Instance.selectingItemID = -1;
		}

		public void DoCooldown()
		{
			DOTween.To(() => 0f, delegate(float x)
			{
				imageCooldown.fillAmount = x;
			}, 1f, cooldownTime).SetEase(Ease.Linear).OnComplete(new TweenCallback(CooldownComplete));
			imageCooldown.gameObject.SetActive(true);
		}

		private void CooldownComplete()
		{
			imageCooldown.gameObject.SetActive(false);
		}

		public void ViewEnable()
		{
			ReadyToUse();
			imageIcon.color = Color.white;
		}

		public void ViewDisable()
		{
			NotReadyToUse();
			imageIcon.color = Color.gray;
		}

		public void ReadyToUse()
		{
			button.enabled = true;
		}

		public void NotReadyToUse()
		{
			button.enabled = false;
		}

		public OrderedUnityEvent OnCastItemSkillEvent;

		[SerializeField]
		private PowerUpItemUseKind useType;

		[SerializeField]
		private Text quantity;

		[SerializeField]
		private Image imageCooldown;

		public int powerUpItemID;

		private float cooldownTime;

		private string buffkey = "Slow";

		private Button button;

		[SerializeField]
		private Image imageIcon;

		[SerializeField]
		private GameObject buttonBuyMore;
	}
}
