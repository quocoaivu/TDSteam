using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class RangeDamageCaster : MonoBehaviour
	{
        [SerializeField]
        private SingleDamageCaster damageToSingleEnemy;

        [SerializeField]
        private StrikeMotionHandler attackAnimationController;


        private List<EnemyData> enemiesInAoeRange = new List<EnemyData>();
        public void CastDamage(int numberOfEnemy, DamageKind damageType, SharedStrikeDamage commonAttackDamage, StrikeWithSpecialFx attackWithSpecialEffect)
		{
			enemiesInAoeRange = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			if (enemiesInAoeRange.Count > 0)
			{
				for (int i = 0; i < numberOfEnemy; i++)
				{
					int index = UnityEngine.Random.Range(0, enemiesInAoeRange.Count);
					damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[index], commonAttackDamage);
					StrikeFXKind attackFXType = attackWithSpecialEffect.attackFXType;
					if (attackFXType == StrikeFXKind.Electric)
					{
						RunAnimation(enemiesInAoeRange[index].gameObject, attackWithSpecialEffect.duration);
					}
				}
			}
		}

		public void CastDamage(int numberOfEnemy, DamageKind damageType, SharedStrikeDamage commonAttackDamage, OnHitStatusApplier effectAttack)
		{
			enemiesInAoeRange = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			if (enemiesInAoeRange.Count > 0)
			{
				for (int i = 0; i < numberOfEnemy; i++)
				{
					damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[UnityEngine.Random.Range(0, enemiesInAoeRange.Count)], commonAttackDamage, effectAttack);
				}
			}
		}

		private void RunAnimation(GameObject target, float lifeTime)
		{
			if (attackAnimationController == null)
			{
				return;
			}
			attackAnimationController.Init(target, lifeTime);
		}
	}
}
