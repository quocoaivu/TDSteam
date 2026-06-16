using System;
using System.Collections;
using GameCore;
using Parameter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	public class ItemBackup : BaseMonoBehaviour
	{
		private void Awake()
		{
			powerUpItem = base.GetComponent<PowerUpEntry>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemSpec>.Instance.GetCustomValue(powerUpItemID);
			heroParameter.attack_physics_min = customValue[0];
			heroParameter.attack_physics_max = customValue[1];
			heroParameter.attack_range_min = customValue[2];
			heroParameter.attack_range_average = customValue[3];
			heroParameter.attack_range_max = customValue[4];
			heroParameter.health = customValue[5];
			heroParameter.armor_physics = customValue[6];
			heroParameter.speed = customValue[7];
			heroParameter.attack_cooldown = customValue[8];
			reinforcementAmount = customValue[9];
			duration = (float)Singleton<PowerUpItemSpec>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemSpec>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			UnityEngine.Debug.Log(heroParameter);
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<AllyPool>.Instance.PushAlliesToPool(allyID, allyID, 0);
			if (heroParameter.attack_range_max > 100)
			{
				MonoSingleton<BulletPool>.Instance.InitBulletsFromTower(allyID, allyID);
			}
		}

		public void CastSkill()
		{
			base.StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
			yield return null;
			UnityEngine.Debug.Log("Call reforcement!");
			if (allyID == 1000)
			{
				UnityEngine.Debug.Log("_____>>> trigger use ground reinforce");
				GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseItem, new SignalTriggerRecord(SignalTriggerKind.UseItem, 6, 1, true));
			}
			else if (allyID == 1001)
			{
				UnityEngine.Debug.Log("_____>>> trigger use ssky reinforce");
				GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseItem, new SignalTriggerRecord(SignalTriggerKind.UseItem, 7, 1, true));
			}
			int cloneIndex = 0;
			for (int i = 0; i < reinforcementAmount; i++)
			{
				MinionEntity allyModel = MonoSingleton<AllyPool>.Instance.GetAlly(allyID, allyID);
				if (reinforcementAmount == 1)
				{
					allyModel.transform.position = targetPosition;
				}
				if (reinforcementAmount > 1)
				{
					if (cloneIndex == 0)
					{
						allyModel.transform.position = targetPosition - vectorXUnit;
					}
					else
					{
						allyModel.transform.position = targetPosition + vectorXUnit;
					}
				}
				allyModel.InitFromHero(heroParameter, parameterScale, duration);
				allyModel.gameObject.SetActive(true);
				if (heroParameter.attack_range_max > 100)
				{
					allyModel.MinionStrikeHandler.rangeAttack = true;
				}
				cloneIndex++;
			}
			yield break;
		}

		private Vector2 getTargetVector()
		{
			Vector2 screenPosition = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
			Vector3 vector = Camera.main.ScreenToWorldPoint(screenPosition);
			return new Vector2(vector.x, vector.y);
		}

		[SerializeField]
		private int allyID;

		private int powerUpItemID;

		private PowerUpEntry powerUpItem;

		private int reinforcementAmount;

		private float duration;

		private float cooldownTime;

		private string buffkey = "Slow";

		private HeroSpec heroParameter = default(HeroSpec);

		private float parameterScale = 1f;

		private Vector2 vectorXUnit = new Vector2(0.3f, 0f);
	}
}
