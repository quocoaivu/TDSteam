using System;
using UnityEngine;

namespace Gameplay
{
	public class EnemyProjectile : MonoBehaviour
	{
        private CharacterEntity targetModel;

        private SharedStrikeDamage commonAttackDamageSender;

        [Space]
        [Header("New trajectory parameter")]
        public float originSpeed = 3f;

        private float currentSpeed;

        [SerializeField]
        private float speedUpOverTime = 0.5f;

        [SerializeField]
        private float hitDistance = 0.2f;

        [SerializeField]
        private float ballisticOffset = 0.5f;

        [SerializeField]
        private int bulletDirection = 1;

        [SerializeField]
        private bool bulletRandomDirection;

        private Vector2 originPoint;

        private Vector2 aimPoint;

        private Vector2 myVirtualPosition;

        private Vector2 myPreviousPosition;

        private float counter;

        private bool activeTrajectoryShot;

        [Space]
        [Header("Effect")]
        [SerializeField]
        private GameObject groundEffectPrefab;

        private CastFxOnDiedMark castEffectOnDieTarget;

        private BlastInMark explosionInTarget;

        private void Awake()
		{
			castEffectOnDieTarget = base.GetComponent<CastFxOnDiedMark>();
			explosionInTarget = base.GetComponent<BlastInMark>();
			MonoSingleton<FXPool>.Instance.InitExtendObject(groundEffectPrefab, 0);
		}

		private void FixedUpdate()
		{
			if (activeTrajectoryShot)
			{
				counter += Time.fixedDeltaTime;
				currentSpeed += Time.fixedDeltaTime * speedUpOverTime;
				if (targetModel != null)
				{
					aimPoint = targetModel.transform.position;
				}
				if ((double)aimPoint.x < 6.5 && (double)aimPoint.x > -6.5)
				{
					Vector2 vector = aimPoint - originPoint;
					Vector2 vector2 = aimPoint - myVirtualPosition;
					myVirtualPosition = Vector2.Lerp(originPoint, aimPoint, counter * currentSpeed / vector.magnitude);
					base.transform.position = AddBallisticOffset(vector.magnitude, (float)bulletDirection * vector2.magnitude);
					myPreviousPosition = base.transform.position;
					if (vector2.magnitude <= hitDistance)
					{
						if (targetModel != null && targetModel.IsAlive)
						{
							DamageToAlly();
						}
						else if (castEffectOnDieTarget)
						{
							castEffectOnDieTarget.CastEffect(base.gameObject.transform);
						}
						activeTrajectoryShot = false;
						ReturnPool();
					}
				}
				else
				{
					if (castEffectOnDieTarget)
					{
						castEffectOnDieTarget.CastEffect(base.gameObject.transform);
					}
					ReturnPool();
				}
			}
		}

		public void Init(CharacterEntity _characterModel, int physicsDamage)
		{
			targetModel = _characterModel;
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = physicsDamage;
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = 0;
			CalculateParameterForTrajectoryShot();
		}

		public void DamageToAlly()
		{
			if (explosionInTarget)
			{
				explosionInTarget.CastExplosion(targetModel.transform);
			}
			if (commonAttackDamageSender.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamageSender.criticalStrikeChance)
			{
				commonAttackDamageSender.physicsDamage *= 2;
				commonAttackDamageSender.magicDamage *= 2;
			}
			targetModel.ProcessDamage(DamageKind.Range, commonAttackDamageSender);
		}

		private void CalculateParameterForTrajectoryShot()
		{
			originPoint = (myVirtualPosition = (myPreviousPosition = base.transform.position));
			aimPoint = targetModel.transform.position;
			counter = 0f;
			currentSpeed = originSpeed;
			if (bulletRandomDirection)
			{
				bulletDirection = ((UnityEngine.Random.Range(0, 100) <= 50) ? -1 : 1);
			}
			activeTrajectoryShot = true;
		}

		private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
		{
			if (ballisticOffset > 0f)
			{
				float num = Mathf.Sin(3.14159274f * ((originDistance - distanceToAim) / originDistance));
				num *= originDistance;
				return myVirtualPosition + ballisticOffset * num * Vector2.up;
			}
			return myVirtualPosition;
		}

		public void ReturnPool()
		{
			MonoSingleton<BulletPool>.Instance.Despawn(base.gameObject);
		}

	}
}
