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
			// so it's warm before the first shot â€” no lazy InitBulletsFromTower needed here.
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
			Bullet = MonoSingleton<BulletPool>.Instance.GetForTower(originalParameter.id, originalParameter.level);
			SharedStrikeDamage commonAttackDamage = new SharedStrikeDamage();
			commonAttackDamage.physicsDamage = base.BuffedDamagePhysics;
			commonAttackDamage.magicDamage = base.BuffedDamageMagic;
			commonAttackDamage.instantKillChance = base.BuffedInstantKillChance;
			commonAttackDamage.criticalStrikeChance = originalParameter.criticalStrikeChance;
			commonAttackDamage.ignoreArmorChance = originalParameter.ignoreArmorChance;
			commonAttackDamage.bulletDirection = bulletParameter.bulletDirection;
			commonAttackDamage.aoeRange = (float)originalParameter.bulletAoe / GameRecord.PIXEL_PER_UNIT;
			commonAttackDamage.maxRange = (float)originalParameter.attackRangeMax / GameRecord.PIXEL_PER_UNIT;
			if (towerSkillScaleDamageByRange)
			{
				commonAttackDamage.physicsDamage = towerSkillScaleDamageByRange.GetScaledDamage(commonAttackDamage.physicsDamage, commonAttackDamage.maxRange, base.Target);
				commonAttackDamage.magicDamage = towerSkillScaleDamageByRange.GetScaledDamage(commonAttackDamage.magicDamage, commonAttackDamage.maxRange, base.Target);
			}
			effectAttack.buffKey = originalParameter.debuffKey;
			effectAttack.debuffEffectValue = originalParameter.debuffEffectValue;
			effectAttack.debuffEffectDuration = (float)originalParameter.debuffEffectDuration / 1000f;
			effectAttack.debuffChance = originalParameter.debuffChance;
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
