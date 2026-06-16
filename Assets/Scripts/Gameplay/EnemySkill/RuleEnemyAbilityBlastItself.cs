using System;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class RuleEnemyAbilityBlastItself : EnemyBrain
	{
		public override void OnAppear()
		{
			base.OnAppear();
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		private void EnemyModel_OnStartRun(int obj)
		{
			SetParameter();
		}

		private void SetParameter()
		{
			originalParameter = base.EnemyModel.OriginalParameter;
			attackRangeAverage = (float)originalParameter.attack_range_average / GameRecord.PIXEL_PER_UNIT;
		}

		private void GetAllComponents()
		{
			enemyFindTargetController = base.EnemyModel.EnemyFindTargetController;
			enemyMovementController = base.EnemyModel.EnemyMovementController;
		}

		private void Awake()
		{
			GetAllComponents();
			base.EnemyModel.OnStartRun += EnemyModel_OnStartRun;
		}

		private void OnDestroy()
		{
			base.EnemyModel.OnStartRun -= EnemyModel_OnStartRun;
		}

		public override void Update()
		{
			base.Update();
			if (!base.IsEnemyAlive())
			{
				return;
			}
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			if (IsAliveAndHaveTarget())
			{
				PrepareToExplode();
			}
		}

		private void PrepareToExplode()
		{
			if (IsCloseToTarget())
			{
				UnityEngine.Debug.Log("Close to explosion!");
				DamageToAlly(enemyFindTargetController.Target);
			}
		}

		private bool IsAliveAndHaveTarget()
		{
			return base.EnemyModel.IsAlive && !(enemyFindTargetController.Target == null);
		}

		private bool IsCloseToTarget()
		{
			float num = Mathf.Abs(enemyFindTargetController.Target.transform.position.x - base.gameObject.transform.position.x);
			float num2 = Mathf.Abs(enemyFindTargetController.Target.transform.position.y - base.gameObject.transform.position.y);
			return num <= attackRangeAverage && num2 < 0.1f;
		}

		private void DamageToAlly(CharacterEntity characterModel)
		{
			commonAttackDamageSender = new SharedStrikeDamage();
			commonAttackDamageSender.physicsDamage = UnityEngine.Random.Range(originalParameter.attack_physics_min, originalParameter.attack_physics_max);
			commonAttackDamageSender.magicDamage = UnityEngine.Random.Range(originalParameter.attack_magic_min, originalParameter.attack_magic_max);
			commonAttackDamageSender.criticalStrikeChance = 0;
			characterModel.ProcessDamage(DamageKind.Magic, commonAttackDamageSender);
			base.EnemyModel.EnemyAnimationController.ToDieState();
			base.EnemyModel.Dead();
			base.EnemyModel.ReturnPool(1f);
		}

		private EnemyParameter originalParameter;

		private EnemyTargetAcquisition enemyFindTargetController;

		private EnemyMovement enemyMovementController;

		private CharacterEntity target;

		private float attackRangeAverage;

		private SharedStrikeDamage commonAttackDamageSender;
	}
}
