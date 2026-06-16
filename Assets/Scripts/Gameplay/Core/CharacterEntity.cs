using System;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class CharacterEntity : BaseMonoBehaviour
	{
        public bool IsAlive;

        public bool IsSpecialState;

        public bool IsInvisible;

        public BoxCollider2D boxCollider;

        [SerializeField]
        private BuffHolder buffsHolder;


        public EntityPhaseEnum curState;
        public BuffHolder BuffsHolder
		{
			get
			{
				return buffsHolder;
			}
			private set
			{
				buffsHolder = value;
			}
		}

		public virtual void ProcessDamage(DamageKind damageType, SharedStrikeDamage commonAttackDamage)
		{
		}

		public virtual void ChangeHealth(int damagePhysics, int damageMagic, int criticalStrikeChance = 0)
		{
		}

		public virtual void RestoreHealth()
		{
		}

		public virtual void IncreaseHealth(int hpAmount)
		{
		}

		public virtual int GetCurHp()
		{
			return 0;
		}

		public virtual int GetMaxHp()
		{
			return 1;
		}

		public virtual void Dead()
		{
		}

		public virtual void ReturnPool(float delayTime)
		{
		}

		public virtual void Update()
		{
			if (IsAlive && !MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				EntityFsmController fsm = GetFsmController();
				if (fsm == null)
				{
					return;
				}
				fsm.OnUpdate(Time.deltaTime);
				IEntityState currentState = fsm.GetCurrentState();
				if (currentState != null)
				{
					curState = currentState.entityStateEnum;
				}
			}
		}

		public void LookAtEnemy()
		{
			Vector3 position = base.transform.position;
			if (GetCurrentTarget().transform.position.x - position.x > 0f)
			{
				base.transform.localScale = Vector3.one;
			}
			else
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}

		public bool IsInMeleeRange(EnemyData enemy)
		{
			if (enemy == null)
			{
				return false;
			}
			if (enemy.IsAir)
			{
				return false;
			}
			float num = GetMeleeRange() * GetMeleeRange();
			return MonoSingleton<GameRecord>.Instance.SqrDistance(GetAssignedPosition(), enemy.transform.position) <= num;
		}

		public bool IsInRangerRange(EnemyData enemy)
		{
			if (enemy == null)
			{
				return false;
			}
			if (!IsRanger())
			{
				return false;
			}
			if (CanAttackAirEnemy() && enemy.IsUnderground)
			{
				return false;
			}
			float num = GetRangerRange() * GetRangerRange();
			return MonoSingleton<GameRecord>.Instance.SqrDistance(GetAssignedPosition(), enemy.transform.position) <= num;
		}

		public bool IsInMeleeActionRange(EnemyData enemy)
		{
			if (enemy == null)
			{
				return false;
			}
			float num = GetAttackRangeMin() + (float)enemy.OriginalParameter.body_size / GameRecord.PIXEL_PER_UNIT;
			float num2 = num * num * 1.1f;
			return MonoSingleton<GameRecord>.Instance.SqrDistance(base.gameObject, enemy.gameObject) <= num2;
		}

		public bool IsMeleeAttacking()
		{
			return GetFsmController().GetCurrentState() is CharacterMeleeAtkState;
		}

		public virtual void AddTarget(EnemyData enemy)
		{
		}

		public virtual EnemyData GetCurrentTarget()
		{
			return null;
		}

		public virtual bool CanAttackAirEnemy()
		{
			return false;
		}

		public virtual float GetRangerRange()
		{
			return 1f;
		}

		public virtual float GetMeleeRange()
		{
			return 1f;
		}

		public virtual float GetAttackRangeMin()
		{
			return 0f;
		}

		public virtual int GetCriticalStrikeChance()
		{
			return 0;
		}

		public virtual int GetDodgeChance()
		{
			return 0;
		}

		public virtual int GetIgnoreArmorChance()
		{
			return 0;
		}

		public virtual float GetSpeed()
		{
			return 0f;
		}

		public virtual IMotionHandler GetAnimationController()
		{
			return null;
		}

		public virtual void DoRangeAttack()
		{
		}

		public virtual void DoMeleeAttack()
		{
		}

		public virtual float GetAtkCooldownDuration()
		{
			return 1f;
		}

		public virtual float GetShortIdleDuration()
		{
			return 2f;
		}

		public virtual Vector3 GetAssignedPosition()
		{
			return Vector3.zero;
		}

		public virtual void SetAssignedPosition(Vector3 assignedPos)
		{
		}

		public virtual float GetDieDuration()
		{
			return 1f;
		}

		public virtual void SetSpecialStateDuration(float duration)
		{
		}

		public virtual float GetSpecialStateDuration()
		{
			return 1f;
		}

		public virtual void SetSpecialStateAnimationName(string animationName)
		{
		}

		public virtual string GetSpecialStateAnimationName()
		{
			return string.Empty;
		}

		public virtual EntityFsmController GetFsmController()
		{
			return null;
		}

		public virtual bool IsRanger()
		{
			return false;
		}

		public virtual float GetCharacterHeight()
		{
			if (boxCollider == null)
			{
				boxCollider = base.GetComponent<BoxCollider2D>();
			}
			if (boxCollider != null)
			{
				return boxCollider.size.y;
			}
			return 1f;
		}
	}
}
