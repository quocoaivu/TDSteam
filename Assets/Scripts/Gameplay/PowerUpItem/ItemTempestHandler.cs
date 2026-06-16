using System;
using System.Collections;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	public class ItemTempestHandler : BaseMonoBehaviour
	{
		private void Awake()
		{
			powerUpItem = base.GetComponent<PowerUpEntry>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemSpec>.Instance.GetCustomValue(powerUpItemID);
			pushBackDistance = (float)customValue[0] / GameRecord.PIXEL_PER_UNIT;
			aoeRange = (float)customValue[1] / GameRecord.PIXEL_PER_UNIT;
			maxEnemyAffected = customValue[2];
			activationTime = (float)Singleton<PowerUpItemSpec>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemSpec>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.STORM);
		}

		public void CastSkill()
		{
			base.StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
			GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseItem, new SignalTriggerRecord(SignalTriggerKind.UseItem, 8, 1, true));
			yield return null;
			UnityEngine.Debug.Log("Cast skill storm!");
			VisualEffectInstance storm = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.STORM);
			storm.transform.position = targetPosition;
			storm.gameObject.SetActive(true);
			storm.GetComponent<TempestHandler>().Init(aoeRange, activationTime, pushBackDistance, maxEnemyAffected);
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

		private float pushBackDistance;

		private float aoeRange;

		private int maxEnemyAffected;

		private string buffkey = "Slow";
	}
}
