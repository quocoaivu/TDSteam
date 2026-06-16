using System;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class TurretAbilityScaleDamageByRange : TurretHandler
	{
		public float DamageScaleMin
		{
			get
			{
				return damageScaleMin;
			}
			private set
			{
				damageScaleMin = value;
			}
		}

		public float DamageScaleMax
		{
			get
			{
				return damageScaleMax;
			}
			private set
			{
				damageScaleMax = value;
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
		}

		private void SetParameter()
		{
			param = TurretDefaultAbilitySpec.Instance.GetTowerParameter(towerID, towerLevel);
			DamageScaleMin = (float)param.skillParam0 / 100f;
			DamageScaleMax = (float)param.skillParam1 / 100f;
		}

		public int GetScaledDamage(int originDamage, float towerMaxRange, EnemyData target)
		{
			float num = Vector3.Distance(base.gameObject.transform.position, target.transform.position);
			float num2 = num / towerMaxRange;
			float num3 = DamageScaleMin + (DamageScaleMax - DamageScaleMin) * num2;
			return (int)((float)originDamage * num3);
		}

		[SerializeField]
		private int towerID;

		[SerializeField]
		private int towerLevel;

		private TurretDefaultAbility param;

		private float damageScaleMin;

		private float damageScaleMax;
	}
}
