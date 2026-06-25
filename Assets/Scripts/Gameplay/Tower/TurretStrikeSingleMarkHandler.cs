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

			public int BuffedDamage
		{
			get { return buffedDamage; }
		}

		// Legacy aliases for code not yet updated.
		public int BuffedDamagePhysics => base.TowerModel.OriginalParameter.damageType == Parameter.DamageType.Physical ? buffedDamage : 0;
		public int BuffedDamageMagic => base.TowerModel.OriginalParameter.damageType == Parameter.DamageType.Magic ? buffedDamage : 0;

		public float BuffedCritChance
		{
			get { return buffedCritChance; }
		}

		public float BuffedCriticalStrikeChance => buffedCritChance;

		public int BuffedInstantKillChance => 0;

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
			// Crit % must be refreshed before UpdateBuffDamage so the new value is folded in this pass.
			if (critIncrementBuffKeys.Contains(buffKey))
			{
				UpdateCritIncrement();
			}
			UpdateBuffDamage();
			if (damageIncrementPercentageBuffKeys.Contains(buffKey))
			{
				UpdateDamageIncrement();
			}
			if (damageDecrementPercentageBuffKeys.Contains(buffKey))
			{
				UpdateDamageDecrement();
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

		private void UpdateCritIncrement()
		{
			critIncrementPercentage = base.TowerModel.BuffsHolder.GetBuffsValue(critIncrementBuffKeys);
		}

		private void UpdateBuffDamage()
		{
			float multiplier = 1f + (damageIncrementPercentage - damageDecrementPercentage) / 100f;
			if (multiplier < 0f) multiplier = 0f;
			buffedDamage = (int)(base.TowerModel.finalDamage * multiplier);
			// Item crit buff adds flat % on top of base crit.
			buffedCritChance = base.TowerModel.finalCritChance + critIncrementPercentage;
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

		private List<string> critIncrementBuffKeys = new List<string>
		{
			"CritIncrementCommon"
		};

		private float critIncrementPercentage;
		private float damageIncrementPercentage;
		private float damageDecrementPercentage;
		private int buffedDamage;
		private float buffedCritChance;
	}
}
