using System;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Ability3SunStrike : BaseMonoBehaviour
	{
        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        private SharedStrikeDamage commonAttackDamageSender;

        public void Init(int _physicsDamage, int _magicDamage, float _aoeRange, float _lifeTime, float _distance)
		{
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = _physicsDamage;
			commonAttackDamageSender.magicDamage = _magicDamage;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			commonAttackDamageSender.aoeRange = _aoeRange;
			DamageWithAOE();
			base.CustomInvoke(new Action(OnMoveComplete), _lifeTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamageSender.aoeRange);
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageKind.Range, commonAttackDamageSender);
		}

		private void OnMoveComplete()
		{
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}
	}
}
