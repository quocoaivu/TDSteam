using System;
using System.Collections.Generic;
using System.Diagnostics;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class MinionEntity : CharacterEntity
	{

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnHitEnemyEvent;


        [Header("Required components")]
        [SerializeField]
        private MinionTravelHandler allyMovementController;

        [SerializeField]
        private MinionStrikeHandler allyAttackController;

        [SerializeField]
        private MinionVitalityHandler allyHealthController;

        [SerializeField]
        private MinionMotionHandler allyAnimationController;

        [SerializeField]
        private TrooperSfxHandler unitSoundController;

        public bool freeAlly;

        public bool controlledAlly;

        public EntityFsmController allyFsmController;

        private string specialStateAnimationName;

        private float specialStateDuration;

        [SerializeField]
        [HideInInspector]
        private List<MinionHandler> controllers;

        private Vector3 PoolPos = new Vector3(1000f, 1000f, 0f);

        private new Collider2D collider2D;

        private SpriteRenderer spriteRenderer;

        [Space]
        [Header("Target")]
        public EnemyData currentTarget;

        private TurretSpec towerParameter;

        private HeroSpec heroParameter;

        private int id;

        private int level;

        private int health;

        private int physicsArmor;

        private int magicArmor;

        private int physicsDamage_min;

        private int physicsDamage_max;

        private int criticalStrikeChange;

        private float attackCooldown;

        private float attackRangeAverage;

        private float attackRangeMin;

        private float attackRangeMax;

        private float currentAttackRangeMax;

        private float moveSpeed;

        private int dodgeChance;

        private int currentDodgeChance;

        private int ignoreArmorChance;

        private int currentIgnoreArmorChance;

        private int hpRegen;

        private int shield;

        private TurretSummonMinionHandler towerSpawnAllyController;


        public MinionTravelHandler MinionTravelHandler
		{
			get
			{
				return allyMovementController;
			}
			private set
			{
				allyMovementController = value;
			}
		}

		public MinionStrikeHandler MinionStrikeHandler
		{
			get
			{
				return allyAttackController;
			}
			private set
			{
				allyAttackController = value;
			}
		}

		public MinionVitalityHandler MinionVitalityHandler
		{
			get
			{
				return allyHealthController;
			}
			private set
			{
				allyHealthController = value;
			}
		}

		public MinionMotionHandler MinionMotionHandler
		{
			get
			{
				return allyAnimationController;
			}
			set
			{
				allyAnimationController = value;
			}
		}

		public TrooperSfxHandler UnitSoundController
		{
			get
			{
				return unitSoundController;
			}
			set
			{
				unitSoundController = value;
			}
		}

		public int Id
		{
			get
			{
				return id;
			}
			private set
			{
				id = value;
			}
		}

		public int Level
		{
			get
			{
				return level;
			}
			private set
			{
				level = value;
			}
		}

		public int Health
		{
			get
			{
				return health;
			}
			private set
			{
				health = value;
			}
		}

		public int PhysicsArmor
		{
			get
			{
				return physicsArmor;
			}
			private set
			{
				physicsArmor = value;
			}
		}

		public int MagicArmor
		{
			get
			{
				return magicArmor;
			}
			set
			{
				magicArmor = value;
			}
		}

		public int PhysicsDamage_min
		{
			get
			{
				return physicsDamage_min;
			}
			private set
			{
				physicsDamage_min = value;
			}
		}

		public int PhysicsDamage_max
		{
			get
			{
				return physicsDamage_max;
			}
			private set
			{
				physicsDamage_max = value;
			}
		}

		public int CriticalStrikeChange
		{
			get
			{
				return criticalStrikeChange;
			}
			private set
			{
				criticalStrikeChange = value;
			}
		}

		public float AttackCooldown
		{
			get
			{
				return attackCooldown;
			}
			private set
			{
				attackCooldown = value;
			}
		}

		public float AttackRangeAverage
		{
			get
			{
				return attackRangeAverage;
			}
			private set
			{
				attackRangeAverage = value;
			}
		}

		public float AttackRangeMin
		{
			get
			{
				return attackRangeMin;
			}
			private set
			{
				attackRangeMin = value;
			}
		}

		public float AttackRangeMax
		{
			get
			{
				return attackRangeMax;
			}
			set
			{
				attackRangeMax = value;
			}
		}

		public float CurrentAttackRangeMax
		{
			get
			{
				return currentAttackRangeMax;
			}
			set
			{
				currentAttackRangeMax = value;
			}
		}

		public float MoveSpeed
		{
			get
			{
				return moveSpeed;
			}
			private set
			{
				moveSpeed = value;
			}
		}

		public int DodgeChance
		{
			get
			{
				return dodgeChance;
			}
			set
			{
				dodgeChance = value;
			}
		}

		public int CurrentDodgeChance
		{
			get
			{
				return currentDodgeChance;
			}
			set
			{
				currentDodgeChance = value;
			}
		}

		public int IgnoreArmorChance
		{
			get
			{
				return ignoreArmorChance;
			}
			set
			{
				ignoreArmorChance = value;
			}
		}

		public int CurrentIgnoreArmorChance
		{
			get
			{
				return currentIgnoreArmorChance;
			}
			set
			{
				currentIgnoreArmorChance = value;
			}
		}

		public int HpRegen
		{
			get
			{
				return hpRegen;
			}
			private set
			{
				hpRegen = value;
			}
		}

		public int Shield
		{
			get
			{
				return shield;
			}
			private set
			{
				shield = value;
			}
		}

		public TurretSummonMinionHandler TowerSpawnAllyController
		{
			get
			{
				return towerSpawnAllyController;
			}
			set
			{
				towerSpawnAllyController = value;
			}
		}

		private void Awake()
		{
			GetAllComponents();
			GetControllers();
			InitializeControllers();
		}

		private void GetAllComponents()
		{
			collider2D = base.GetComponent<Collider2D>();
			spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		private void TurnOnCollider()
		{
			collider2D.enabled = true;
		}

		private void TurnOffCollider()
		{
			collider2D.enabled = false;
		}

		private void GetControllers()
		{
			if (controllers == null || controllers.Count == 0)
			{
				controllers = new List<MinionHandler>(base.GetComponentsInChildren<MinionHandler>(true));
			}
		}

		private void InitializeControllers()
		{
			for (int i = 0; i < controllers.Count; i++)
			{
				MinionHandler allyController = controllers[i];
				allyController.MinionEntity = this;
				allyController.Initialize();
			}
		}

		public void InitFromTower(TurretSpec _towerParameter, TurretSummonMinionHandler _towerSpawnAllyController)
		{
			towerParameter = _towerParameter;
			TowerSpawnAllyController = _towerSpawnAllyController;
			Id = 4000 + GameKit.GetTowerSourceId(towerParameter.level, towerParameter.id);
			level = towerParameter.level;
			Health = towerParameter.unit_health;
			PhysicsArmor = towerParameter.unit_armor;
			MagicArmor = 0;
			PhysicsDamage_min = towerParameter.damage;
			PhysicsDamage_max = towerParameter.damage;
			CriticalStrikeChange = (int)towerParameter.critChance;
			AttackCooldown = (float)towerParameter.unit_attackCooldown / 1000f;
			AttackRangeMin = towerParameter.unit_attackRange / GameRecord.PIXEL_PER_UNIT;
			AttackRangeAverage = towerParameter.unit_attackRange / GameRecord.PIXEL_PER_UNIT;
			AttackRangeMax = towerParameter.unit_attackRange / GameRecord.PIXEL_PER_UNIT;
			CurrentAttackRangeMax = AttackRangeMax;
			moveSpeed = towerParameter.unit_moveSpeed;
			DodgeChance = 0;
			IgnoreArmorChance = towerParameter.ignoreArmorChance;
			HpRegen = towerParameter.unit_hpRegen;
			Shield = towerParameter.unit_shield;
			MonoSingleton<GameRecord>.Instance.ListActiveAlly.Add(this);
			TurnOnCollider();
			IsAlive = true;
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnAppear();
			}
		}

		public void InitFromHero(HeroSpec _heroParameter, float parameterScale, float lifeTime)
		{
			heroParameter = _heroParameter;
			Id = 2000 + heroParameter.id;
			level = heroParameter.level;
			Health = (int)((float)heroParameter.health * parameterScale);
			PhysicsArmor = (int)((float)heroParameter.armor_physics * parameterScale);
			MagicArmor = (int)((float)heroParameter.armor_magic * parameterScale);
			PhysicsDamage_min = (int)((float)heroParameter.attack_physics_min * parameterScale);
			PhysicsDamage_max = (int)((float)heroParameter.attack_physics_max * parameterScale);
			CriticalStrikeChange = (int)((float)heroParameter.critical_strike_change * parameterScale);
			AttackCooldown = (float)heroParameter.attack_cooldown / 1000f;
			AttackRangeAverage = (float)heroParameter.attack_range_average / GameRecord.PIXEL_PER_UNIT;
			AttackRangeMin = (float)heroParameter.attack_range_min / GameRecord.PIXEL_PER_UNIT;
			AttackRangeMax = (float)heroParameter.attack_range_max / GameRecord.PIXEL_PER_UNIT;
			CurrentAttackRangeMax = AttackRangeMax;
			moveSpeed = (float)heroParameter.speed;
			HpRegen = 0;
			Shield = 0;
			MonoSingleton<GameRecord>.Instance.ListActiveAlly.Add(this);
			TurnOnCollider();
			IsAlive = true;
			SetAssignedPosition(base.transform.position);
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnAppear();
			}
			Color color = spriteRenderer.color;
			spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
			base.CustomInvoke(new Action(EndOfLifeTime), lifeTime);
		}

		public void OnHitEnemy()
		{
			if (OnHitEnemyEvent != null)
			{
				OnHitEnemyEvent();
			}
		}

		public override void ProcessDamage(DamageKind damageType, SharedStrikeDamage commonAttackDamage)
		{
			MinionVitalityHandler.ChangeHealth(damageType, commonAttackDamage);
		}

		public override void RestoreHealth()
		{
			base.RestoreHealth();
			MinionVitalityHandler.RestoreHealth();
		}

		public override void IncreaseHealth(int hpAmount)
		{
			base.IncreaseHealth(hpAmount);
			MinionVitalityHandler.AddHealth(hpAmount);
		}

		public override void Dead()
		{
			unitSoundController.PlayDie();
			TurnOffCollider();
			if (TowerSpawnAllyController != null)
			{
				TowerSpawnAllyController.RemoveAllyFromListControl(this);
			}
			IsAlive = false;
			IsInvisible = false;
		}

		private void EndOfLifeTime()
		{
			Color color = spriteRenderer.color;
			if (IsAlive)
			{
				IsAlive = false;
				allyAnimationController.ToDisappearState();
				ReturnPool(1f);
			}
		}

		private void OnFadeOutComplete()
		{
			TurnOffCollider();
			ReturnPool(0f);
		}

		public override void ReturnPool(float delayTime)
		{
			IsAlive = false;
			IsInvisible = false;
			allyFsmController = null;
			MonoSingleton<GameRecord>.Instance.ListActiveAlly.Remove(this);
			TowerSpawnAllyController = null;
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnReturnPool();
			}
			MonoSingleton<AllyPool>.Instance.Despawn(this, delayTime);
		}

		public override void AddTarget(EnemyData enemy)
		{
			currentTarget = enemy;
		}

		public override EnemyData GetCurrentTarget()
		{
			return currentTarget;
		}

		public override bool CanAttackAirEnemy()
		{
			return false;
		}

		public override float GetRangerRange()
		{
			return CurrentAttackRangeMax;
		}

		public override float GetMeleeRange()
		{
			return AttackRangeAverage;
		}

		public override float GetAttackRangeMin()
		{
			return AttackRangeMin;
		}

		public override int GetCriticalStrikeChance()
		{
			return CriticalStrikeChange;
		}

		public override int GetDodgeChance()
		{
			return CurrentDodgeChance;
		}

		public override int GetIgnoreArmorChance()
		{
			return CurrentIgnoreArmorChance;
		}

		public override float GetSpeed()
		{
			return MinionTravelHandler.MoveSpeed;
		}

		public override IMotionHandler GetAnimationController()
		{
			return allyAnimationController;
		}

		public override void DoMeleeAttack()
		{
			allyAttackController.PrepareToMeleeAttack();
		}

		public override void DoRangeAttack()
		{
			allyAttackController.PrepareToRangeAttack();
		}

		public override float GetAtkCooldownDuration()
		{
			return allyAttackController.Cooldowntime;
		}

		public override Vector3 GetAssignedPosition()
		{
			return allyMovementController.assignedPosition;
		}

		public override void SetAssignedPosition(Vector3 assignedPos)
		{
			allyMovementController.assignedPosition = assignedPos;
		}

		public override float GetDieDuration()
		{
			return 2f;
		}

		public override float GetSpecialStateDuration()
		{
			return specialStateDuration;
		}

		public override void SetSpecialStateDuration(float duration)
		{
			specialStateDuration = duration;
		}

		public override string GetSpecialStateAnimationName()
		{
			return specialStateAnimationName;
		}

		public override void SetSpecialStateAnimationName(string animationName)
		{
			specialStateAnimationName = animationName;
		}

		public override EntityFsmController GetFsmController()
		{
			if (allyFsmController == null)
			{
				allyFsmController = new AllyFsmController(this);
			}
			return allyFsmController;
		}

		public override bool IsRanger()
		{
			return allyAttackController.rangeAttack;
		}

		public override int GetCurHp()
		{
			return allyHealthController.CurrentHealth;
		}

		public override int GetMaxHp()
		{
			return allyHealthController.OriginHealth;
		}

	}
}
