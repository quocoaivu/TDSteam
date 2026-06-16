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
			for (int i = 0; i < enemiesInAoeRange.Count; i++)
			{
				damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[i], commonAttackDamage);
			}
		}

		public void CastDamage(DamageKind damageType, SharedStrikeDamage commonAttackDamage)
		{
			enemiesInAoeRange = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			for (int i = 0; i < enemiesInAoeRange.Count; i++)
			{
				damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[i], commonAttackDamage);
			}
		}

		public void CastDamage(DamageKind damageType, SharedStrikeDamage commonAttackDamage, OnHitStatusApplier effectAttack)
		{
			enemiesInAoeRange = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			for (int i = 0; i < enemiesInAoeRange.Count; i++)
			{
				damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[i], commonAttackDamage, effectAttack);
			}
		}
	}
}
