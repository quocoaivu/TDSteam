using System;
using System.Collections;
using System.Diagnostics;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class TurretStrikeSingleMarkSharedHandler : TurretStrikeSingleMarkHandler
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public override event Action<ProjectileEntity, EnemyData> OnFireBullet;

		public ProjectileEntity Bullet
		{
			get
			{
				return bullet;
			}
			set
			{
				bullet = value;
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			// Bullet prefab is pre-pooled in TowerPool.InitTowerPool for towers with this controller,
			// so it's warm before the first shot â€" no lazy InitBulletsFromTower needed here.
		}

		public override void StopAttack()
		{
		}

		protected override void OnStartAttack()
		{
			base.StartCoroutine(Fire());
		}

		private IEnumerator Fire()
		{
			for (int i = 0; i < turnCount; i++)
			{
				yield return new WaitForSeconds(turnInterval);
				CreateTurn();
			}
			yield break;
		}

		private void CreateTurn()
		{
			int count = bulletParametersInOneTurn.Count;
			for (int i = 0; i < count; i++)
			{
				base.StartCoroutine(CreateBullet(bulletParametersInOneTurn[i]));
			}
		}

		private IEnumerator CreateBullet(ProjectileSpec bulletParameter)
		{
			yield return new WaitForSeconds(bulletParameter.delayTime - 0.2f);
			bulletParameter.OnCreateBullet.Invoke();
			yield return new WaitForSeconds(0.2f);
			TurretSpec originalParameter = base.TowerModel.OriginalParameter;
			// Towers no longer level up in-run: the bullet is pooled at the canonical (top-tier) level in
			// TowerPool.InitTowerPool, so fetch it at the same level. OriginalParameter.level is 0 here and
			// would ask for an unpooled "bullet_{id}_0", causing a NullReferenceException on the first shot.
			int bulletLevel = MonoSingleton<TowerPool>.Instance.CanonicalLevel(originalParameter.id);
			Bullet = MonoSingleton<BulletPool>.Instance.GetForTower(originalParameter.id, bulletLevel);
			SharedStrikeDamage commonAttackDamage = new SharedStrikeDamage();
			int buffedDmg = base.BuffedDamage;
			if (originalParameter.damageType == Parameter.DamageType.Magic)
			{
				commonAttackDamage.physicsDamage = 0;
				commonAttackDamage.magicDamage = buffedDmg;
			}
			else
			{
				commonAttackDamage.physicsDamage = buffedDmg;
				commonAttackDamage.magicDamage = 0;
			}
			commonAttackDamage.criticalStrikeChance = (int)base.BuffedCritChance;
			// Assassin's Mark (and similar) items add % to the crit damage multiplier: value 50 = +0.5x.
			float itemCritDamagePercent;
			base.TowerModel.BuffsHolder.TryGetBuffValue(BuffKeysToTurret.CritDamageIncrementCommon, out itemCritDamagePercent);
			commonAttackDamage.critMultiplier = originalParameter.critMultiplier + itemCritDamagePercent / 100f;
			commonAttackDamage.ignoreArmorChance = originalParameter.ignoreArmorChance;
			// Spell Piercer (and similar) items add % magic-armor penetration on top of base magicPenetration.
			float itemMagicPen;
			base.TowerModel.BuffsHolder.TryGetBuffValue(BuffKeysToTurret.MagicPenIncrementCommon, out itemMagicPen);
			commonAttackDamage.magicPenetration = originalParameter.magicPenetration + (int)itemMagicPen;
			commonAttackDamage.projectileSpeed = originalParameter.projectileSpeed;
			// Piercing Bolt (and similar) items add extra pass-through targets on top of base pierceCount.
			float itemPierceCount;
			base.TowerModel.BuffsHolder.TryGetBuffValue(BuffKeysToTurret.PierceCountIncrementCommon, out itemPierceCount);
			commonAttackDamage.pierceCount = originalParameter.pierceCount + (int)itemPierceCount;
			commonAttackDamage.bulletDirection = bulletParameter.bulletDirection;
			// Seismic Core (and similar) items grow the splash radius by % of the tower's base aoeRadius.
			float itemAoePercent;
			base.TowerModel.BuffsHolder.TryGetBuffValue(BuffKeysToTurret.AoeRadiusIncrementCommon, out itemAoePercent);
			commonAttackDamage.aoeRange = originalParameter.aoeRadius * (1f + itemAoePercent / 100f);
			commonAttackDamage.aoeDamageFalloff = originalParameter.damageFalloff;
			commonAttackDamage.maxRange = originalParameter.range;
			if (towerSkillScaleDamageByRange)
			{
				commonAttackDamage.physicsDamage = towerSkillScaleDamageByRange.GetScaledDamage(commonAttackDamage.physicsDamage, commonAttackDamage.maxRange, base.Target);
				commonAttackDamage.magicDamage = towerSkillScaleDamageByRange.GetScaledDamage(commonAttackDamage.magicDamage, commonAttackDamage.maxRange, base.Target);
			}
			// Falcon Sight (and similar) items add % bonus damage when the locked target is an air enemy.
			if (base.Target != null && base.Target.IsAir)
			{
				float airBonusPercent;
				base.TowerModel.BuffsHolder.TryGetBuffValue(BuffKeysToTurret.AirDamageIncrementCommon, out airBonusPercent);
				if (airBonusPercent > 0)
				{
					float airMultiplier = 1f + airBonusPercent / 100f;
					commonAttackDamage.physicsDamage = (int)(commonAttackDamage.physicsDamage * airMultiplier);
					commonAttackDamage.magicDamage = (int)(commonAttackDamage.magicDamage * airMultiplier);
				}
			}
			// Frost Arrow (and similar) items add slow-on-hit % on top of the tower's base slowPercent.
			float itemSlowPercent;
			base.TowerModel.BuffsHolder.TryGetBuffValue(BuffKeysToTurret.SlowOnHitIncrementCommon, out itemSlowPercent);
			float totalSlowPercent = originalParameter.slowPercent + itemSlowPercent;
			// Venom Tip (and similar) items add poison-on-hit DPS on top of the tower's base poisonDPS.
			float itemPoisonDPS;
			base.TowerModel.BuffsHolder.TryGetBuffValue(BuffKeysToTurret.PoisonDpsIncrementCommon, out itemPoisonDPS);
			float totalPoisonDPS = originalParameter.poisonDPS + itemPoisonDPS;
			// Map explicit debuff fields to the existing OnHitStatusApplier system.
			if (totalPoisonDPS > 0)
			{
				effectAttack.buffKey = DamageVfxType.Poison.ToString();
				effectAttack.debuffEffectValue = (int)totalPoisonDPS;
				effectAttack.debuffEffectDuration = originalParameter.poisonDuration > 0 ? originalParameter.poisonDuration : 3f;
				effectAttack.debuffChance = 100;
			}
			else if (totalSlowPercent > 0)
			{
				effectAttack.buffKey = DamageVfxType.Slow.ToString();
				effectAttack.debuffEffectValue = (int)totalSlowPercent;
				effectAttack.debuffEffectDuration = originalParameter.slowDuration > 0 ? originalParameter.slowDuration : 2f;
				effectAttack.debuffChance = 100;
			}
			else
			{
				effectAttack.buffKey = string.Empty;
				effectAttack.debuffChance = 0;
			}
			Bullet.gameObject.SetActive(true);
			Bullet.transform.position = bulletParameter.gunBarrel.position;
			Bullet.transform.eulerAngles = base.TowerModel.gun.transform.eulerAngles;
			Bullet.InitFromTower(base.TowerModel, commonAttackDamage, effectAttack, base.Target);
			if (OnFireBullet != null)
			{
				OnFireBullet(Bullet, base.Target);
			}
			yield break;
		}

		[SerializeField]
		[Range(1f, 5f)]
		private int turnCount = 1;

		[SerializeField]
		private float turnInterval;

		[SerializeField]
		private TurretAbilityScaleDamageByRange towerSkillScaleDamageByRange;

		private ProjectileEntity bullet;

		private OnHitStatusApplier effectAttack;
	}
}
