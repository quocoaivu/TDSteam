using System;
using DG.Tweening;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class Hero1Ability0Projectile : BaseMonoBehaviour
	{

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        [SerializeField]
        private VisualEffectSpawner effectCaster;

        private SharedStrikeDamage commonAttackDamageSender;

        public void Init(int _damage, float _aoeRange, float _lifeTime, float _distance)
		{
			base.transform.DOMoveY(base.transform.position.y - _distance, _lifeTime, false).SetEase(Ease.Linear).OnComplete(new TweenCallback(OnMoveComplete));
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.LIGHTNING_PROJECTILE_SHADOW);
			effect.Init(_lifeTime, new Vector2(base.transform.position.x, base.transform.position.y - _distance));
			effect.DoFadeIn(_lifeTime, 1f);
			effect.SetBiggerOverTime(_lifeTime);
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = _damage;
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.aoeRange = _aoeRange;
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamageSender.aoeRange);
		}

		private void DamageWithAOE()
		{
			ExplosionNoTarget();
			damageToAOERange.CastDamage(DamageKind.Range, commonAttackDamageSender);
		}

		private void ExplosionNoTarget()
		{
			effectCaster.CastEffect(FXPool.LIGHTNING_EXPLOSION, 2f, base.gameObject.transform.position);
		}

		private void OnMoveComplete()
		{
			MonoSingleton<LensHandler>.Instance.ShakeNormal();
			DamageWithAOE();
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}

	}
}
