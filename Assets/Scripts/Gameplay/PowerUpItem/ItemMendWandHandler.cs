using System;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	public class ItemMendWandHandler : BaseMonoBehaviour
	{
		private void Awake()
		{
			powerUpItem = base.GetComponent<PowerUpEntry>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemSpec>.Instance.GetCustomValue(powerUpItemID);
			activationTime = (float)Singleton<PowerUpItemSpec>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			hpPerSecond = customValue[1];
			cooldownTime = (float)Singleton<PowerUpItemSpec>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			healingRange = (float)customValue[0] / GameRecord.PIXEL_PER_UNIT;
			timeTracking = 1f;
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.HEALING_WAND);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_HEAL_2);
		}

		public void CastHealingWand()
		{
			CreateHealingWand(getTargetVector(), activationTime);
			GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseItem, new SignalTriggerRecord(SignalTriggerKind.UseItem, 2, 1, true));
		}

		private void CreateHealingWand(Vector2 targetPosition, float activationTime)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.HEALING_WAND);
			effect.transform.position = targetPosition;
			effect.Init(activationTime);
			effect.GetComponent<MendWandHandler>().Init(activationTime, hpPerSecond, healingRange, timeTracking);
		}

		private Vector2 getTargetVector()
		{
			Vector2 screenPosition = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
			Vector3 vector = Camera.main.ScreenToWorldPoint(screenPosition);
			return new Vector2(vector.x, vector.y);
		}

		private int powerUpItemID;

		private PowerUpEntry powerUpItem;

		private int hpPerSecond;

		private float activationTime;

		private float healingRange;

		private float timeTracking;

		private float cooldownTime;
	}
}
