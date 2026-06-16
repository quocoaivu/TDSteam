using System;
using System.Collections;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	public class ItemCometStrikeHandler : BaseMonoBehaviour
	{
		private void Awake()
		{
			powerUpItem = base.GetComponent<PowerUpEntry>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemSpec>.Instance.GetCustomValue(powerUpItemID);
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(meteorPrefab.gameObject);
			aoeRange = (float)customValue[0] / GameRecord.PIXEL_PER_UNIT;
			damage = customValue[1];
			amount = customValue[2];
			delayTime = (float)customValue[3] / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemSpec>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(getTargetVector(), aoeRange);
		}

		public void CastMeteorStrike()
		{
            UnityEngine.Debug.Log("start skill meteor!");
            MonoSingleton<FXPool>.Instance.InitFX(FXPool.LIGHTNING_PROJECTILE_SHADOW);
            MonoSingleton<FXPool>.Instance.InitFX(FXPool.LIGHTNING_PROJECTILE_RANGE);
            MonoSingleton<FXPool>.Instance.InitFX(FXPool.METEOR_EXPLOSION);
            base.StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
            CastEffectSkillRange(targetPosition);
			GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseItem, new SignalTriggerRecord(SignalTriggerKind.UseItem, 1, 1, true));
            UnityEngine.Debug.Log("Cast skill meteor!");
            for (int i = 0; i < amount; i++)
			{
				yield return new WaitForSeconds(delayTime);
				CometHandler bullet = MonoSingleton<BulletPool>.Instance.GetMeteorController();
				bullet.transform.position = new Vector2(targetPosition.x, offsetHigh) + UnityEngine.Random.insideUnitCircle * aoeRange;
				bullet.Init(damage, singleAoeRange, timeStep * (offsetHigh - targetPosition.y), offsetHigh - targetPosition.y);
			}
			yield break;
		}

		private void CastEffectSkillRange(Vector2 targetPosition)
		{
            UnityEngine.Debug.Log("target skill meteor!");
            VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.LIGHTNING_PROJECTILE_RANGE);
			effect.transform.position = targetPosition;
			effect.Init(0.75f);
		}

		private Vector2 getTargetVector()
		{
			Vector2 screenPosition = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
			Vector3 vector = Camera.main.ScreenToWorldPoint(screenPosition);
			return new Vector2(vector.x, vector.y);
		}

		private int powerUpItemID;

		private PowerUpEntry powerUpItem;

		private int damage;

		private float delayTime;

		private float cooldownTime;

		private int amount;

		private float aoeRange;

		[SerializeField]
		private float singleAoeRange;

		[SerializeField]
		private CometHandler meteorPrefab;

		[SerializeField]
		private float offsetHigh;

		[SerializeField]
		private float timeStep;
	}
}
