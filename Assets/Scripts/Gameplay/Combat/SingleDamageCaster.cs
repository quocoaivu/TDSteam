using System;
using UnityEngine;

namespace Gameplay
{
	public class SingleDamageCaster : MonoBehaviour
	{
		public void CastDamage(DamageKind damageType, EnemyData enemy, SharedStrikeDamage attackDamageParameter)
		{
			enemy.ProcessDamage(damageType, attackDamageParameter);
		}

		public void CastDamage(DamageKind damageType, EnemyData enemy, SharedStrikeDamage attackDamageParameter, OnHitStatusApplier effectAttack)
		{
			enemy.ProcessDamage(damageType, attackDamageParameter, effectAttack);
		}
	}
}
