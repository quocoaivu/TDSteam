using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class ItemFrostHandler : BaseMonoBehaviour
	{
        private int powerUpItemID;

        private PowerUpEntry powerUpItem;

        private float activationTime;

        private float cooldownTime;

        private int slowPercent;

        private string buffkey = "Slow";

        private void Awake()
		{
			powerUpItem = base.GetComponent<PowerUpEntry>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			activationTime = (float)Singleton<PowerUpItemSpec>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemSpec>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			slowPercent = 100;
			powerUpItem.Init(cooldownTime);
		}

		public void FreezeAllEnemy()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_ITEM_FREEZE);
			base.StopAllCoroutines();
			base.StartCoroutine(DoFreeze());
		}

		private IEnumerator DoFreeze()
		{
			GameSignalCenter.Instance.Trigger(GameSignalKind.EventUseItem, new SignalTriggerRecord(SignalTriggerKind.UseItem, 0, 1, true));
			List<EnemyData> ListActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
			ChillFx fxCamera = MonoSingleton<FXPool>.Instance.GetFrostEffectOnCamera();
			fxCamera.Init(2f * activationTime);
			fxCamera.DoEffectIn(activationTime, 0.25f);
			foreach (EnemyData enemyModel in ListActiveEnemy)
			{
				enemyModel.ProcessEffect(buffkey, slowPercent, activationTime, DamageVfxType.Freezing);
			}
			yield return new WaitForSeconds(activationTime);
			fxCamera.DoEffectOut(activationTime, 0f);
			yield break;
		}
	}
}
