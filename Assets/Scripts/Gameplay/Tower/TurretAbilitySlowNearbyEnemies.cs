using System;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class TurretAbilitySlowNearbyEnemies : TurretHandler
	{
		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			skillReady = false;
		}

		private void SetParameter()
		{
			param = TurretDefaultAbilitySpec.Instance.GetTowerParameter(towerID, towerLevel);
			chanceToCast = param.skillParam0;
			skillRange = (float)param.skillParam1 / GameRecord.PIXEL_PER_UNIT;
			slowPercent = param.skillParam2;
			slowTime = (float)param.skillParam3 / 1000f;
			cooldownTime = (float)param.skillParam4 / 1000f;
			effectAttack.buffKey = "Slow";
			effectAttack.debuffChance = chanceToCast;
			effectAttack.debuffEffectValue = slowPercent;
			effectAttack.debuffEffectDuration = slowTime;
			effectAttack.damageFXType = DamageVfxType.Electric;
			effectAttackSuper.buffKey = "Slow";
			effectAttackSuper.debuffChance = chanceToCast;
			effectAttackSuper.debuffEffectValue = slowPercent + (int)((float)(100 - slowPercent) * 0.65f);
			effectAttackSuper.debuffEffectDuration = slowTime;
			effectAttackSuper.damageFXType = DamageVfxType.Electric;
			effectAttackOnBoss.buffKey = "Slow";
			effectAttackOnBoss.debuffChance = chanceToCast;
			effectAttackOnBoss.debuffEffectValue = (int)((float)slowPercent * 0.25f);
			effectAttackOnBoss.debuffEffectDuration = slowTime;
			effectAttackOnBoss.damageFXType = DamageVfxType.Electric;
			skillReady = true;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_ELECTRIC);
		}

		public override void Update()
		{
			base.Update();
			if (!MonoSingleton<GameRecord>.Instance.IsGameStart)
			{
				return;
			}
			if (!skillReady)
			{
				return;
			}
			if (IsCooldownDone())
			{
				CastSlow();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void CastSlow()
		{
			SharedStrikeDamage commonAttackDamage = new SharedStrikeDamage(0, 0, skillRange);
			List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			for (int i = listEnemiesInRange.Count - 1; i >= 0; i--)
			{
				if (listEnemiesInRange[i].OriginalParameter.isBoss)
				{
					listEnemiesInRange[i].ProcessDamage(DamageKind.Range, commonAttackDamage, effectAttackOnBoss);
				}
				else if (listEnemiesInRange[i].Id == 18)
				{
					listEnemiesInRange[i].ProcessDamage(DamageKind.Range, commonAttackDamage, effectAttackSuper);
				}
				else
				{
					listEnemiesInRange[i].ProcessDamage(DamageKind.Range, commonAttackDamage, effectAttack);
				}
			}
			timeTracking = cooldownTime;
			if (listEnemiesInRange.Count > 0 != showingSlowEffect)
			{
				showingSlowEffect = (listEnemiesInRange.Count > 0);
				slowEffect.SetActive(showingSlowEffect);
			}
		}

		[SerializeField]
		private int towerID;

		[SerializeField]
		private int towerLevel;

		public GameObject slowEffect;

		private bool skillReady;

		private float skillRange;

		private int chanceToCast;

		private int slowPercent;

		private float slowTime;

		private float cooldownTime;

		private float timeTracking;

		private OnHitStatusApplier effectAttack;

		private OnHitStatusApplier effectAttackSuper;

		private OnHitStatusApplier effectAttackOnBoss;

		private TurretDefaultAbility param;

		private bool showingSlowEffect;
	}
}
