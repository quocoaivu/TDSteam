using System;
using DG.Tweening;
using GeneralVariable;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class Turret0Mastery0Projectile : BaseMonoBehaviour
	{
		private void Awake()
		{
			spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		private void Update()
		{
			if (isReady)
			{
				spriteRenderer.enabled = true;
				base.transform.right = target - base.transform.position;
			}
		}

		public void Init(int _damage, float _lifeTime, Vector2 targerPosition)
		{
			damage = _damage;
			target = targerPosition;
			spriteRenderer.enabled = false;
			isReady = true;
			base.transform.DOKill(false);
			base.transform.DOMove(targerPosition, _lifeTime, false).SetEase(Ease.Linear).OnComplete(new TweenCallback(OnMoveComplete));
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.tag == GeneralVariable.GeneralDefine.ENEMY_TAG)
			{
				EnemyData component = coll.gameObject.GetComponent<EnemyData>();
				if (!component.IsUnderground)
				{
					SharedStrikeDamage commonAttackDamage = new SharedStrikeDamage(damage, 0, 0f);
					component.ProcessDamage(DamageKind.Range, commonAttackDamage);
					VisualEffectInstance effectController = SpawnExplosion();
					effectController.Init(explosionFXDuration, target);
				}
			}
		}

		private void OnMoveComplete()
		{
			CastEffectOnDiedEnemy();
			ReturnPool();
		}

		public void CastEffectOnDiedEnemy()
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(effectName);
			effect.transform.position = base.gameObject.transform.position;
			effect.Init(effectDuration);
		}

		private VisualEffectInstance SpawnExplosion()
		{
			return MonoSingleton<FXPool>.Instance.GetExplosion(explosionFXName);
		}

		private void ReturnPool()
		{
			isReady = true;
			base.transform.DOKill(false);
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}

		[Header("Effect hit enemy")]
		[SerializeField]
		private string explosionFXName;

		[SerializeField]
		private float explosionFXDuration;

		[Space]
		[Header("Effect on died enemy")]
		[SerializeField]
		private string effectName;

		[SerializeField]
		private float effectDuration;

		private int damage;

		private Vector3 target;

		private SpriteRenderer spriteRenderer;

		private bool isReady;
	}
}
