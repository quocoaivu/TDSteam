using System;
using System.Collections.Generic;
using DG.Tweening;
using GeneralVariable;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Ability0Comet : MonoBehaviour
	{
        private int physicsDamage;

        private int magicDamage;

        private float aoeRange;

        private float lifeTime;

        private float moveSpeed;

        private Vector2 heroPosition;

        private Vector2 skillCastPosition;

        private SharedStrikeDamage commonAttackDamageSender;

        [SerializeField]
        private SpinOverPivot meteorObject;

        [SerializeField]
        private GameObject shadow;
        
		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, aoeRange);
		}

		public void Init(int physicsDamage, int magicDamage, float aoeRange, float lifeTime, float moveSpeed, Vector2 heroPosition, Vector2 skillCastPosition, float _distance)
		{
			this.physicsDamage = physicsDamage;
			this.magicDamage = magicDamage;
			this.aoeRange = aoeRange;
			this.lifeTime = lifeTime;
			this.moveSpeed = moveSpeed;
			this.heroPosition = heroPosition;
			this.skillCastPosition = skillCastPosition;
			shadow.SetActive(false);
			if (skillCastPosition.x - heroPosition.x > 0f)
			{
				meteorObject.RotateDirection = 1;
			}
			else
			{
				meteorObject.RotateDirection = -1;
			}
			base.transform.DOMoveY(base.transform.position.y - _distance, 0.5f, false).SetEase(Ease.Linear).OnComplete(new TweenCallback(OnFallingComplete));
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag == GeneralVariable.GeneralDefine.ENEMY_TAG)
			{
				EnemyData component = other.gameObject.GetComponent<EnemyData>();
				if (!component.IsAir && component.IsAlive && !component.IsUnderground)
				{
					SingleDamageCaster(component);
				}
			}
		}

		private void SingleDamageCaster(EnemyData enemy)
		{
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = physicsDamage;
			commonAttackDamageSender.magicDamage = magicDamage;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			enemy.ProcessDamage(DamageKind.Range, commonAttackDamageSender);
			UnityEngine.Debug.Log("Deal damage!");
		}

		private void OnFallingComplete()
		{
			shadow.SetActive(true);
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.METEOR_EXPLOSION2);
			effect.transform.position = skillCastPosition;
			effect.Init(2f);
			MonoSingleton<LensHandler>.Instance.ShakeNormal();
			Vector3 a = Vector3.Normalize(skillCastPosition - heroPosition);
			Vector3 vector = a * moveSpeed * lifeTime;
			vector += base.transform.position;
			base.transform.DOMove(vector, lifeTime, false).OnComplete(new TweenCallback(OnMoveComplete));
		}

		private void OnMoveComplete()
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.METEOR_SELF_EXPLOSION);
			effect.transform.position = base.transform.position;
			effect.Init(2f);
			shadow.SetActive(false);
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}

		private List<EnemyData> enemiesInAoeRange = new List<EnemyData>();
	}
}
