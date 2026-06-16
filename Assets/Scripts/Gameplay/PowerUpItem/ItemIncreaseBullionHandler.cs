using System;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	public class ItemIncreaseBullionHandler : BaseMonoBehaviour
	{
		private void Awake()
		{
			powerUpItem = base.GetComponent<PowerUpEntry>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemSpec>.Instance.GetCustomValue(powerUpItemID);
			goldAmount = customValue[0];
			cooldownTime = (float)Singleton<PowerUpItemSpec>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
		}

		public void IncreaseGold()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.GOLD_CHEST);
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.GOLD_CHEST);
			effect.transform.position = Vector2.zero;
			effect.Init(delaytimeFX);
			effect.GetComponent<SummonExtendFX>().Init();
			base.CustomInvoke(new Action(DoIncreaseGold), delaytimeFX);
		}

		private void DoIncreaseGold()
		{
			GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseItem, new SignalTriggerRecord(SignalTriggerKind.UseItem, 3, 1, true));
            //SingletonMonoBehaviour<GameRecord>.Instance.IncreaseMoney(goldAmount);
            MonoSingleton<GameRecord>.Instance.IncreaseMoney(999999);
        }

        private Vector2 getTargetVector()
		{
			Vector2 screenPosition = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
			Vector3 vector = Camera.main.ScreenToWorldPoint(screenPosition);
			return new Vector2(vector.x, vector.y);
		}

		[SerializeField]
		private float delaytimeFX;

		private int powerUpItemID;

		private PowerUpEntry powerUpItem;

		private int goldAmount;

		private float cooldownTime;
	}
}
