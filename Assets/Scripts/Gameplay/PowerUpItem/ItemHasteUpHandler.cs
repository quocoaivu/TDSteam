using System;
using System.Collections;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	public class ItemHasteUpHandler : BaseMonoBehaviour
	{
		private void Awake()
		{
			powerUpItem = base.GetComponent<PowerUpEntry>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemSpec>.Instance.GetCustomValue(powerUpItemID);
			attackSpeedIncreasePercentage = customValue[0];
			aoeRange = (float)customValue[1] / GameRecord.PIXEL_PER_UNIT;
			activationTime = (float)Singleton<PowerUpItemSpec>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemSpec>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.BUFF_SPEED_AURA);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.BUFF_SPEED_ON_TOWER);
		}

		public void CastSkill()
		{
			base.StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
			yield return null;
			UnityEngine.Debug.Log(">>>>>Cast skill speed up!");
			GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseItem, new SignalTriggerRecord(SignalTriggerKind.UseItem, 4, 1, true));
			VisualEffectInstance speedAura = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.BUFF_SPEED_AURA);
			speedAura.transform.position = targetPosition;
			speedAura.gameObject.SetActive(true);
			speedAura.GetComponent<HasteUpAuraHandler>().Init(aoeRange, activationTime, attackSpeedIncreasePercentage);
			yield break;
		}

		private Vector2 getTargetVector()
		{
			Vector2 screenPosition = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
			Vector3 vector = Camera.main.ScreenToWorldPoint(screenPosition);
			return new Vector2(vector.x, vector.y);
		}

		private int powerUpItemID;

		private PowerUpEntry powerUpItem;

		private float activationTime;

		private float cooldownTime;

		private int attackSpeedIncreasePercentage;

		private float aoeRange;
	}
}
