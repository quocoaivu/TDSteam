using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Ability3LightningSpear : MonoBehaviour
	{
        private float duration;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        [SerializeField]
        private VisualEffectSpawner effectCaster;

        private SharedStrikeDamage commonAttackDamageSender;

        private Vector3 target;

        private bool isReady;

        private void Update()
		{
			if (!isReady)
			{
				return;
			}
			base.transform.right = target - base.transform.position;
		}

		public void Init(float skillRange, int damagePhysics, float duration, Vector2 targetPosition)
		{
			this.duration = duration;
			target = targetPosition;
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = damagePhysics;
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			commonAttackDamageSender.aoeRange = skillRange;
			base.transform.DOMove(targetPosition, duration, false).OnComplete(new TweenCallback(OnMoveComplete));
			isReady = true;
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamageSender.aoeRange);
		}

		private void OnMoveComplete()
		{
			DamageWithAOE();
			effectCaster.CastEffect(FXPool.LIGHTNING_EXPLOSION_2, 0.75f, target);
			ReturnPool();
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageKind.Range, commonAttackDamageSender);
		}

		private void SingleDamageCaster(EnemyData enemy)
		{
			enemy.ProcessDamage(DamageKind.Range, commonAttackDamageSender);
		}

		private void ReturnPool()
		{
			isReady = false;
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}
	}
}
