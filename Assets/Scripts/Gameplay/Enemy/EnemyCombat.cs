using System;
using System.Collections;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class EnemyCombat : EnemyBrain
	{
        private EnemyParameter originalParameter;

        private float cooldownTime;

        private CharacterEntity target;

        private SharedStrikeDamage commonAttackDamageSender;

        private float damageRatio;

        private float attackRangeAverage;

        private float attackRangeMax;

        [Space]
        [Header("Attack type")]
        public bool meleeAttack = true;

        public bool rangeAttack;

        [SerializeField]
        private int attackFrame = 1;

        [Space]
        [Header("Range Attack Attribute")]
        [SerializeField]
        private float delayTimeToCreateBullet;

        [SerializeField]
        private Transform gunBarrel;

        [SerializeField]
        private GameObject bulletPrefab;

        [SerializeField]
        private string bulletName;

        [Space]
        [Header("Effect")]
        [SerializeField]
        private bool haveEffectOnAttack;

        [SerializeField]
        private GameObject attackEffectPrefab;

        private CastFxOnDiedMark castEffectOnDieTarget;

        public float GetCooldownTime()
		{
			return cooldownTime;
		}

		public float GetRangerAtkRange()
		{
			return attackRangeMax;
		}

		public override void OnAppear()
		{
			base.OnAppear();
		}

		public override void Initialize()
		{
			base.Initialize();
			base.EnemyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void EnemyModel_OnStartRun(int obj)
		{
			SetParameter();
		}

		private void SetParameter()
		{
			originalParameter = base.EnemyModel.OriginalParameter;
			cooldownTime = (float)originalParameter.attack_cooldown / 1000f + UnityEngine.Random.Range(0f, 0.5f);
			damageRatio = 1f;
			attackRangeAverage = (float)originalParameter.attack_range_average / GameRecord.PIXEL_PER_UNIT;
			attackRangeMax = (float)originalParameter.attack_range_max / GameRecord.PIXEL_PER_UNIT;
		}

		private void GetAllComponents()
		{
			castEffectOnDieTarget = base.GetComponent<CastFxOnDiedMark>();
		}

		private void Awake()
		{
			GetAllComponents();
			base.EnemyModel.enemyAttackController = this;
			base.EnemyModel.OnHitAllyEvent += EnemyModel_OnHitAllyEvent;
			base.EnemyModel.OnStartRun += EnemyModel_OnStartRun;
			if (rangeAttack)
			{
				MonoSingleton<BulletPool>.Instance.InitExtendBullet(bulletPrefab);
			}
			if (haveEffectOnAttack)
			{
				MonoSingleton<FXPool>.Instance.InitExtendObject(attackEffectPrefab, 0);
			}
		}

		private void OnDestroy()
		{
			base.EnemyModel.OnHitAllyEvent -= EnemyModel_OnHitAllyEvent;
			base.EnemyModel.OnStartRun -= EnemyModel_OnStartRun;
		}

		private void EnemyModel_OnHitAllyEvent()
		{
			if (base.EnemyModel.EnemyFindTargetController.Target)
			{
				DamageToAlly(base.EnemyModel.EnemyFindTargetController.Target);
			}
		}

		public void PrepareToMeleeAttack()
		{
			base.EnemyModel.EnemyAnimationController.ToMeleeAttackState();
		}

		private void DamageToAlly(CharacterEntity characterModel)
		{
			if (castEffectOnDieTarget)
			{
				castEffectOnDieTarget.CastEffect(characterModel.transform);
			}
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = UnityEngine.Random.Range((int)(damageRatio * (float)originalParameter.attack_physics_min / (float)attackFrame), (int)(damageRatio * (float)originalParameter.attack_physics_max / (float)attackFrame));
			commonAttackDamageSender.magicDamage = UnityEngine.Random.Range((int)(damageRatio * (float)originalParameter.attack_magic_min / (float)attackFrame), (int)(damageRatio * (float)originalParameter.attack_magic_max / (float)attackFrame));
			commonAttackDamageSender.criticalStrikeChance = 0;
			if (commonAttackDamageSender.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamageSender.criticalStrikeChance)
			{
				commonAttackDamageSender.physicsDamage *= 2;
				commonAttackDamageSender.magicDamage *= 2;
			}
			characterModel.ProcessDamage(DamageKind.Melee, commonAttackDamageSender);
		}

		public void PrepareToRangeAttack()
		{
			base.EnemyModel.EnemyAnimationController.ToRangeAttackState();
			base.StartCoroutine(CreateBullet());
		}

		private IEnumerator CreateBullet()
		{
			yield return new WaitForSeconds(delayTimeToCreateBullet);
			if (base.EnemyModel.EnemyFindTargetController.Target)
			{
				EnemyProjectile enemyBulletByName = MonoSingleton<BulletPool>.Instance.GetEnemyBulletByName(bulletName);
				enemyBulletByName.transform.eulerAngles = gunBarrel.eulerAngles;
				int physicsDamage = UnityEngine.Random.Range((int)(damageRatio * (float)base.EnemyModel.OriginalParameter.attack_physics_min), (int)(damageRatio * (float)base.EnemyModel.OriginalParameter.attack_physics_max));
				enemyBulletByName.transform.position = gunBarrel.position;
				enemyBulletByName.gameObject.SetActive(true);
				enemyBulletByName.Init(base.EnemyModel.EnemyFindTargetController.Target, physicsDamage);
			}
			yield break;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (reduceAttackDamageByPercentageBuffKeys.Contains(buffKey))
			{
				ApplyBuffDecreaseAttackDamage();
			}
		}

		private void ApplyBuffDecreaseAttackDamage()
		{
			float num = base.EnemyModel.BuffsHolder.GetBuffsValue(reduceAttackDamageByPercentageBuffKeys) / 100f;
			damageRatio = 1f - num;
		}

		private List<string> reduceAttackDamageByPercentageBuffKeys = new List<string>
		{
			"DecreaseAttackByPercentage"
		};

	}
}
