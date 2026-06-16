using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class ProjectileEntity : BaseMonoBehaviour
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<List<EnemyData>> onDamageAoe;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnInitialized;

        [SerializeField]
        private AoeDamageCaster damageToAOERange;

        public float damageAOERange;

        [SerializeField]
        private SingleDamageCaster damageToSingleEnemy;

        [Space]
        [Header("View")]
        [SerializeField]
        private bool isChangeAppearanceWhenCrit;

        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private Sprite critBulletImage;

        [SerializeField]
        private Sprite normalBulletImage;

        [SerializeField]
        private GameObject critBulletFX;

        [Space]
        [Header("FX")]
        [SerializeField]
        private bool hasExplosionAOE;

        [SerializeField]
        private bool hasExplosionInTarget;

        [SerializeField]
        private float explosionDuration;

        public DamageVfxType damageFxType;

        [SerializeField]
        private string explosionFXName;

        [SerializeField]
        [HideInInspector]
        public int id = -1;

        [SerializeField]
        [HideInInspector]
        public int level = -1;

        private bool isInitFromTower;

        private bool isInitFromHero;

        [HideInInspector]
        public SharedStrikeDamage commonAttackDamage;

        private OnHitStatusApplier effectAttack;

        [NonSerialized]
        public int ignoreArmorChance;

        [NonSerialized]
        public EnemyData target;

        [NonSerialized]
        public Vector2 targetPosition;

        private List<EnemyData> enemiesInAoeRange = new List<EnemyData>();

        [NonSerialized]
        public TurretEntity towerModel;

        private HeroEntity heroModel;

        private Vector3 cachedPosition;

        
		public Vector3 CachedPosition
		{
			get
			{
				return cachedPosition;
			}
		}

		public void Awake()
		{
			GetAllComponents();
		}

		private void Update()
		{
			UpdateCachedPosition();
		}

		private void GetAllComponents()
		{
			spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamage.aoeRange);
		}

		public void InitFromTower(TurretEntity _towerModel, SharedStrikeDamage commonAttackDamage, OnHitStatusApplier effectAttack, EnemyData target)
		{
			this.effectAttack = effectAttack;
			InitFromTower(_towerModel, commonAttackDamage, target);
		}

		public void InitFromTower(TurretEntity _towerModel, SharedStrikeDamage commonAttackDamage, EnemyData target)
		{
			isInitFromTower = true;
			towerModel = _towerModel;
			this.target = target;
			this.commonAttackDamage = commonAttackDamage;
			commonAttackDamage.damageSource = CharacterKind.Tower;
			commonAttackDamage.sourceId = GameKit.GetTowerSourceId(_towerModel.Level, _towerModel.Id);
			commonAttackDamage.isCrit = false;
			if (commonAttackDamage.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.criticalStrikeChance)
			{
				commonAttackDamage.isCrit = true;
				commonAttackDamage.physicsDamage *= 2;
				commonAttackDamage.magicDamage *= 2;
			}
			commonAttackDamage.isIgnoreArmor = false;
			if (commonAttackDamage.ignoreArmorChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.ignoreArmorChance)
			{
				commonAttackDamage.isIgnoreArmor = true;
			}
			commonAttackDamage.isInstantKill = false;
			if (commonAttackDamage.instantKillChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.instantKillChance)
			{
				commonAttackDamage.isInstantKill = true;
			}
			InitEvent();
		}

		public void InitCommon(SharedStrikeDamage commonAttackDamage, OnHitStatusApplier effectAttack, EnemyData target)
		{
			this.effectAttack = effectAttack;
			InitCommon(commonAttackDamage, target);
		}

		public void InitCommon(SharedStrikeDamage commonAttackDamage, EnemyData target)
		{
			this.target = target;
			this.commonAttackDamage = commonAttackDamage;
			commonAttackDamage.isCrit = false;
			if (commonAttackDamage.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.criticalStrikeChance)
			{
				commonAttackDamage.isCrit = true;
				commonAttackDamage.physicsDamage *= 2;
				commonAttackDamage.magicDamage *= 2;
			}
			commonAttackDamage.isIgnoreArmor = false;
			if (commonAttackDamage.ignoreArmorChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.ignoreArmorChance)
			{
				commonAttackDamage.isIgnoreArmor = true;
			}
			commonAttackDamage.isInstantKill = false;
			if (commonAttackDamage.instantKillChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.instantKillChance)
			{
				commonAttackDamage.isInstantKill = true;
			}
			InitEvent();
		}

		public void InitFromHero(HeroEntity _heroModel, SharedStrikeDamage commonAttackDamage, EnemyData target, OnHitStatusApplier effectAttack)
		{
			this.effectAttack = effectAttack;
			InitFromHero(_heroModel, commonAttackDamage, target);
		}

		public void InitFromHero(HeroEntity _heroModel, SharedStrikeDamage commonAttackDamage, EnemyData target)
		{
			isInitFromHero = true;
			this.commonAttackDamage = commonAttackDamage;
			commonAttackDamage.damageSource = CharacterKind.Hero;
			commonAttackDamage.sourceId = _heroModel.HeroID;
			if (damageAOERange > 0f)
			{
				commonAttackDamage.aoeRange = damageAOERange;
			}
			this.target = target;
			commonAttackDamage.isCrit = false;
			if (commonAttackDamage.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.criticalStrikeChance)
			{
				commonAttackDamage.isCrit = true;
				commonAttackDamage.physicsDamage *= 2;
				commonAttackDamage.magicDamage *= 2;
			}
			commonAttackDamage.isIgnoreArmor = false;
			if (commonAttackDamage.ignoreArmorChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.ignoreArmorChance)
			{
				commonAttackDamage.isIgnoreArmor = true;
			}
			if (isChangeAppearanceWhenCrit)
			{
				setBulletView();
			}
			InitEvent();
		}

		public void InitFromHero(HeroEntity _heroModel, SharedStrikeDamage commonAttackDamage, Vector2 targetPosition)
		{
			isInitFromHero = true;
			this.commonAttackDamage = commonAttackDamage;
			commonAttackDamage.damageSource = CharacterKind.Hero;
			commonAttackDamage.sourceId = _heroModel.HeroID;
			this.targetPosition = targetPosition;
			commonAttackDamage.isCrit = false;
			if (commonAttackDamage.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.criticalStrikeChance)
			{
				commonAttackDamage.isCrit = true;
				commonAttackDamage.physicsDamage *= 2;
				commonAttackDamage.magicDamage *= 2;
			}
			commonAttackDamage.isIgnoreArmor = false;
			if (commonAttackDamage.ignoreArmorChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.ignoreArmorChance)
			{
				commonAttackDamage.isIgnoreArmor = true;
			}
			if (isChangeAppearanceWhenCrit)
			{
				setBulletView();
			}
			InitEvent();
		}

		private void setBulletView()
		{
			if (commonAttackDamage.isCrit)
			{
				if (spriteRenderer)
				{
					spriteRenderer.sprite = critBulletImage;
				}
				critBulletFX.SetActive(true);
			}
			else
			{
				if (spriteRenderer)
				{
					spriteRenderer.sprite = normalBulletImage;
				}
				critBulletFX.SetActive(false);
			}
		}

		private void InitEvent()
		{
			if (OnInitialized != null)
			{
				OnInitialized();
			}
		}

		private void UpdateCachedPosition()
		{
			cachedPosition = base.transform.position;
		}

		public void AttackEnemy(EnemyData enemy)
		{
			if (enemy != null)
			{
				if (commonAttackDamage.aoeRange > 0f)
				{
					DamageWithAOE();
				}
				else
				{
					DamageWithoutAOE(enemy);
				}
			}
		}

		private void DamageWithoutAOE(EnemyData enemy)
		{
			if (enemy.OriginalParameter.armor_physics > 0 || enemy.OriginalParameter.armor_magic > 0)
			{
				if (isInitFromTower)
				{
					towerModel.towerSoundController.PlayHitEnemyWithArmor();
				}
			}
			else if (isInitFromTower)
			{
				towerModel.towerSoundController.PlayHitEnemyWithoutArmor();
			}
			SingleDamageCaster(enemy);
		}

		private void SingleDamageCaster(EnemyData enemy)
		{
			damageToSingleEnemy.CastDamage(DamageKind.Range, enemy, commonAttackDamage, effectAttack);
			if (hasExplosionInTarget)
			{
				BlastInMark();
			}
		}

		private void DamageWithAOE()
		{
			if (isInitFromTower)
			{
				towerModel.towerSoundController.PlayExplosion();
			}
			if (hasExplosionAOE)
			{
				ExplosionNoTarget();
			}
			if (onDamageAoe != null)
			{
				onDamageAoe(enemiesInAoeRange);
			}
			damageToAOERange.CastDamage(DamageKind.Range, commonAttackDamage);
		}

		private VisualEffectInstance SpawnExplosion()
		{
			return MonoSingleton<FXPool>.Instance.GetExplosion(explosionFXName);
		}

		private void ExplosionNoTarget()
		{
			Vector3 position = base.transform.position;
			VisualEffectInstance effectController = SpawnExplosion();
			effectController.Init(explosionDuration);
			effectController.transform.position = position;
		}

		private void BlastInMark()
		{
			Vector3 vector = Vector3.zero;
			if (target != null && target.gameObject.activeSelf)
			{
				vector = target.transform.position;
				VisualEffectInstance effectController = SpawnExplosion();
				effectController.Init(explosionDuration, target.transform);
			}
		}

		public void ReturnPool()
		{
			MonoSingleton<BulletPool>.Instance.Despawn(this);
		}

		public void Show()
		{
			if (spriteRenderer)
			{
				spriteRenderer.enabled = true;
			}
		}

		public void Hide()
		{
			if (spriteRenderer)
			{
				spriteRenderer.enabled = false;
			}
		}

		public void OnMoveToPosition()
		{
			DamageWithAOE();
			ReturnPool();
		}
	}
}
