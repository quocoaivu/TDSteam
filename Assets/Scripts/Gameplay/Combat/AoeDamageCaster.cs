using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class AoeDamageCaster : MonoBehaviour
	{
        [SerializeField]
        private SingleDamageCaster damageToSingleEnemy;


        private List<EnemyData> enemiesInAoeRange = new List<EnemyData>();
        public void CastDamage(GameObject targetPosition, DamageKind damageType, SharedStrikeDamage commonAttackDamage)
		{
			enemiesInAoeRange = GameKit.GetListEnemiesInRange(targetPosition, commonAttackDamage);
			CastToList(targetPosition.transform.position, damageType, commonAttackDamage, false, default(OnHitStatusApplier));
		}

		public void CastDamage(DamageKind damageType, SharedStrikeDamage commonAttackDamage)
		{
			enemiesInAoeRange = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			CastToList(base.transform.position, damageType, commonAttackDamage, false, default(OnHitStatusApplier));
		}

		public void CastDamage(DamageKind damageType, SharedStrikeDamage commonAttackDamage, OnHitStatusApplier effectAttack)
		{
			enemiesInAoeRange = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			CastToList(base.transform.position, damageType, commonAttackDamage, true, effectAttack);
		}

		// Applies damage to every enemy in the blast. With aoeDamageFalloff > 0 the base
		// damage is scaled down the further an enemy sits from the blast center; the shared
		// damage object is restored afterwards so callers see the original values.
		private void CastToList(Vector3 center, DamageKind damageType, SharedStrikeDamage commonAttackDamage, bool useEffect, OnHitStatusApplier effectAttack)
		{
			int basePhysics = commonAttackDamage.physicsDamage;
			int baseMagic = commonAttackDamage.magicDamage;
			bool useFalloff = commonAttackDamage.aoeDamageFalloff > 0 && commonAttackDamage.aoeRange > 0f;
			for (int i = 0; i < enemiesInAoeRange.Count; i++)
			{
				EnemyData enemy = enemiesInAoeRange[i];
				if (useFalloff)
				{
					float scale = GetFalloffScale(center, enemy.transform.position, commonAttackDamage.aoeRange, commonAttackDamage.aoeDamageFalloff);
					commonAttackDamage.physicsDamage = basePhysics > 0 ? Mathf.Max(1, (int)(basePhysics * scale)) : 0;
					commonAttackDamage.magicDamage = baseMagic > 0 ? Mathf.Max(1, (int)(baseMagic * scale)) : 0;
				}
				if (useEffect)
				{
					damageToSingleEnemy.CastDamage(damageType, enemy, commonAttackDamage, effectAttack);
				}
				else
				{
					damageToSingleEnemy.CastDamage(damageType, enemy, commonAttackDamage);
				}
			}
			commonAttackDamage.physicsDamage = basePhysics;
			commonAttackDamage.magicDamage = baseMagic;
		}

		// Linear falloff: full damage at the center down to (1 - falloff%) at the edge.
		private float GetFalloffScale(Vector3 center, Vector3 enemyPos, float aoeRange, int falloffPercent)
		{
			float edge = Mathf.Clamp01(Vector3.Distance(center, enemyPos) / aoeRange);
			float reduction = falloffPercent / 100f * edge;
			return Mathf.Clamp01(1f - reduction);
		}
	}
}
