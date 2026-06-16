using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public abstract class TurretStrikeSingleMarkHandler : TurretHandler
	{
		public int GetNumberBulletInOneTurn()
		{
			return bulletParametersInOneTurn.Count;
		}

		public abstract event Action<ProjectileEntity, EnemyData> OnFireBullet;

		public int BuffedDamagePhysics
		{
			get
			{
				return (int)UnityEngine.Random.Range(buffedDamagePhysics_min, buffedDamagePhysics_max);
			}
		}

		public int BuffedDamageMagic
		{
			get
			{
				return (int)UnityEngine.Random.Range(buffedDamageMagic_min, buffedDamageMagic_max);
			}
		}

		public float BuffedCriticalStrikeChance
		{
			get
			{
				return (float)((int)buffedCriticalStrikeChance);
			}
		}

		public int BuffedInstantKillChance
		{
			get
			{
				return buffedInstantKillChance;
			}
		}

		public EnemyData Target
		{
			get
			{
				return target;
			}
			private set
			{
				target = value;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			base.TowerModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		public override void OnAppear()
		{
			base.OnAppear();
		}

		public override void OnBuildFinished()
		{
			base.OnBuildFinished();
			UpdateBuffDamage();
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			UpdateBuffDamage();
			if (damageIncrementPercentageBuffKeys.Contains(buffKey))
			{
				UpdateDamageIncrement();
			}
			if (damageDecrementPercentageBuffKeys.Contains(buffKey))
			{
				UpdateDamageDecrement();
			}
			if (intstantKillRateIncrementBuffKeys.Contains(buffKey))
			{
				UpdateInstantKillRate();
			}
		}

		private void UpdateDamageIncrement()
		{
			damageIncrementPercentage = base.TowerModel.BuffsHolder.GetBuffsValue(damageIncrementPercentageBuffKeys);
		}

		private void UpdateDamageDecrement()
		{
			damageDecrementPercentage = base.TowerModel.BuffsHolder.GetBuffsValue(damageDecrementPercentageBuffKeys);
		}

		private void UpdateInstantKillRate()
		{
			float buffsValue = base.TowerModel.BuffsHolder.GetBuffsValue(intstantKillRateIncrementBuffKeys);
			buffedInstantKillChance = base.TowerModel.OriginalParameter.instantKillChance + (int)buffsValue;
		}

		private void UpdateBuffDamage()
		{
			float num = 1f + (damageIncrementPercentage - damageDecrementPercentage) / 100f;
			if (num < 0f)
			{
				num = 0f;
			}
			buffedDamagePhysics_min = (float)base.TowerModel.finalDamagePhysics_min * num;
			buffedDamagePhysics_max = (float)base.TowerModel.finalDamagePhysics_max * num;
			buffedDamageMagic_min = (float)base.TowerModel.finalDamageMagic_min * num;
			buffedDamageMagic_max = (float)base.TowerModel.finalDamageMagic_max * num;
			buffedCriticalStrikeChance = (float)base.TowerModel.finalCriticalStrikeChange * num;
			buffedInstantKillChance = base.TowerModel.OriginalParameter.instantKillChance;
		}

		public void StartAttack(EnemyData target)
		{
			this.target = target;
			OnStartAttack();
		}

		protected abstract void OnStartAttack();

		public abstract void StopAttack();

		[SerializeField]
		protected List<ProjectileSpec> bulletParametersInOneTurn = new List<ProjectileSpec>();

		private EnemyData target;

		private List<string> damageIncrementPercentageBuffKeys = new List<string>
		{
			"DamageIncrementCommon"
		};

		private List<string> damageDecrementPercentageBuffKeys = new List<string>
		{
			"DamageDecrementCommon"
		};

		private List<string> intstantKillRateIncrementBuffKeys = new List<string>
		{
			"InstantKillRateIncrementCommon"
		};

		private float damageIncrementPercentage;

		private float damageDecrementPercentage;

		private float buffedDamagePhysics_min;

		private float buffedDamagePhysics_max;

		private float buffedDamageMagic_min;

		private float buffedDamageMagic_max;

		private float buffedCriticalStrikeChance;

		private int buffedInstantKillChance;
	}
}
