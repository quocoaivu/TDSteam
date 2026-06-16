using System;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Ability0Breakdown : BaseMonoBehaviour
	{
        private string buffKey;

        private int slowPercent;

        private float duration;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        private SharedStrikeDamage commonAttackDamageSender;

        private OnHitStatusApplier effectAttackSender;
        
		public void Init(int physicsDamage, float aoeRange, string buffKey, float duration)
		{
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = physicsDamage;
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			commonAttackDamageSender.aoeRange = aoeRange;
			effectAttackSender.buffKey = buffKey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = 100;
			effectAttackSender.debuffEffectDuration = duration;
			effectAttackSender.damageFXType = DamageVfxType.Stun;
			DamageWithAOE();
			base.CustomInvoke(new Action(ReturnPool), duration);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamageSender.aoeRange);
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageKind.Range, commonAttackDamageSender, effectAttackSender);
		}

		private void ReturnPool()
		{
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}


	}
}
