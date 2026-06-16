using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class MinionStrikeHandler : MinionHandler
	{
        private SharedStrikeDamage commonAttackDamageSender;

        private float cooldownTimeTracking;

        private float cooldownTime;

        [Space]
        [Header("Hero attack type")]
        public bool meleeAttack;

        public bool rangeAttack;

        public bool rangeAttackForMeleeAlly;

        [Space]
        [Header("Specific for range attack")]
        [SerializeField]
        private Transform gunPos;

        [SerializeField]
        private string bulletName;

        [Space]
        [SerializeField]
        private int attackFrame;

        private int atkPhysicsMin;

        private int atkPhysicsMax;
        
		public float Cooldowntime
		{
			get
			{
				return cooldownTime;
			}
			set
			{
				cooldownTime = value;
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
			base.MinionEntity.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			rangeAttack = false;
			rangeAttackForMeleeAlly = false;
		}

		private void SetParameter()
		{
			cooldownTime = base.MinionEntity.AttackCooldown + UnityEngine.Random.Range(0f, 0.5f);
			cooldownTimeTracking = 0f;
			rangeAttack = false;
			rangeAttackForMeleeAlly = false;
			atkPhysicsMin = base.MinionEntity.PhysicsDamage_min;
			atkPhysicsMax = base.MinionEntity.PhysicsDamage_max;
		}

		private void GetAllComponents()
		{
		}

		private void Awake()
		{
			GetAllComponents();
		}

		private void Start()
		{
			base.MinionEntity.OnHitEnemyEvent += AllyModel_OnHitEnemyEvent;
		}

		private void AllyModel_OnHitEnemyEvent()
		{
			if (base.MinionEntity.currentTarget)
			{
				DamageToEnemy(base.MinionEntity.currentTarget);
				OnAttack.Invoke();
			}
		}

		private void OnDestroy()
		{
			base.MinionEntity.OnHitEnemyEvent -= AllyModel_OnHitEnemyEvent;
		}

		public void PrepareToMeleeAttack()
		{
		}

		public void PrepareToRangeAttack()
		{
			base.StartCoroutine(CreateBullet());
		}

		private IEnumerator CreateBullet()
		{
			yield return new WaitForSeconds(0.2f);
			if (base.MinionEntity.currentTarget)
			{
				ProjectileEntity bulletByName = MonoSingleton<BulletPool>.Instance.GetBulletByName(bulletName);
				bulletByName.transform.eulerAngles = gunPos.eulerAngles;
				commonAttackDamageSender = new SharedStrikeDamage();
				commonAttackDamageSender.physicsDamage = UnityEngine.Random.Range(base.MinionEntity.PhysicsDamage_min, base.MinionEntity.PhysicsDamage_max);
				commonAttackDamageSender.magicDamage = 0;
				commonAttackDamageSender.criticalStrikeChance = 0;
				commonAttackDamageSender.ignoreArmorChance = 0;
				commonAttackDamageSender.isIgnoreArmor = (UnityEngine.Random.Range(0, 100) < commonAttackDamageSender.ignoreArmorChance);
				bulletByName.transform.position = gunPos.position;
				bulletByName.gameObject.SetActive(true);
				if (base.MinionEntity.TowerSpawnAllyController)
				{
					bulletByName.InitFromTower(base.MinionEntity.TowerSpawnAllyController.TowerModel, commonAttackDamageSender, base.MinionEntity.currentTarget);
				}
				else
				{
					bulletByName.InitCommon(commonAttackDamageSender, base.MinionEntity.currentTarget);
				}
			}
			yield break;
		}

		private void DamageToEnemy(EnemyData enemy)
		{
			commonAttackDamageSender = new SharedStrikeDamage();
			// Guard against a prefab left with attackFrame=0 (integer divide-by-zero).
			// Reachable even for ranged units pulled into melee (RangeAtk->MoveToTarget->MeleeAtk).
			int frames = Mathf.Max(1, attackFrame);
			commonAttackDamageSender.physicsDamage = UnityEngine.Random.Range(atkPhysicsMin / frames, atkPhysicsMax / frames);
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = base.MinionEntity.GetCriticalStrikeChance();
			commonAttackDamageSender.ignoreArmorChance = base.MinionEntity.GetIgnoreArmorChance();
			commonAttackDamageSender.isIgnoreArmor = (UnityEngine.Random.Range(0, 100) < commonAttackDamageSender.ignoreArmorChance);
			commonAttackDamageSender.sourceId = base.MinionEntity.Id;
			commonAttackDamageSender.damageSource = CharacterKind.Ally;
			enemy.ProcessDamage(DamageKind.Melee, commonAttackDamageSender);
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseAttackSpeedBuffKeys.Contains(buffKey))
			{
				ApplyIncreaseAttackSpeed();
			}
			if (increasePhysicsDamageBuffKeys.Contains(buffKey))
			{
				ApplyIncreasePhysicsDamage();
			}
		}

		private void ApplyIncreaseAttackSpeed()
		{
			float buffsValue = base.MinionEntity.BuffsHolder.GetBuffsValue(increaseAttackSpeedBuffKeys);
			cooldownTime = base.MinionEntity.AttackCooldown - base.MinionEntity.AttackCooldown * buffsValue / 100f / 1000f;
			cooldownTime = Mathf.Clamp(cooldownTime, 0.1f, 999f);
		}

		private void ApplyIncreasePhysicsDamage()
		{
			float buffsValue = base.MinionEntity.BuffsHolder.GetBuffsValue(increasePhysicsDamageBuffKeys);
			atkPhysicsMin = base.MinionEntity.PhysicsDamage_min + (int)buffsValue;
			atkPhysicsMax = base.MinionEntity.PhysicsDamage_max + (int)buffsValue;
		}

		public UnityEvent OnAttack;

		private List<string> increaseAttackSpeedBuffKeys = new List<string>
		{
			"IncreaseAttackSpeed"
		};

		private List<string> increasePhysicsDamageBuffKeys = new List<string>
		{
			"IncreaseDamagePhysics"
		};
	}
}
