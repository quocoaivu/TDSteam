using System;
using System.Collections;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	public class ItemVenomHandler : BaseMonoBehaviour
	{
		private void Awake()
		{
			powerUpItem = base.GetComponent<PowerUpEntry>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemSpec>.Instance.GetCustomValue(powerUpItemID);
			burnDamage = customValue[0];
			aoeRange = (float)customValue[1] / GameRecord.PIXEL_PER_UNIT;
			activationTime = (float)Singleton<PowerUpItemSpec>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemSpec>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.POISON_AREA);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_POISON1);
		}

		public void CastSkill()
		{
			base.StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
			GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseItem, new SignalTriggerRecord(SignalTriggerKind.UseItem, 5, 1, true));
			yield return null;
			UnityEngine.Debug.Log("Cast skill poison!");
			VisualEffectInstance poisonArea = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.POISON_AREA);
			poisonArea.transform.position = targetPosition;
			poisonArea.gameObject.SetActive(true);
			poisonArea.GetComponent<VenomAreaHandler>().Init(aoeRange, activationTime, burnDamage);
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

		private int burnDamage;

		private float aoeRange;

		private float cooldownTime;
	}
}
