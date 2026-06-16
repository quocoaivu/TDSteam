using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class Turret2Mastery1Projectile : BaseMonoBehaviour
	{
		public void Awake()
		{
			bulletModel = base.GetComponent<ProjectileEntity>();
			bulletModel.onDamageAoe += BulletModel_onDamageAoe;
			bulletModel.OnInitialized += BulletModel_OnInitialized;
		}

		private void BulletModel_OnInitialized()
		{
			tower2UltimateSkill1 = bulletModel.towerModel.GetComponentInChildren<Turret2Mastery0Ability1>();
			if (tower2UltimateSkill1)
			{
				if (tower2UltimateSkill1.unlock)
				{
					base.gameObject.transform.localScale = scaleVector;
				}
				else
				{
					base.gameObject.transform.localScale = Vector3.one;
				}
			}
		}

		private void BulletModel_onDamageAoe(List<EnemyData> aoeTargets)
		{
			if (tower2UltimateSkill1 && tower2UltimateSkill1.unlock)
			{
				if (tower2UltimateSkill1.firstTimeUpgrade)
				{
					AttackInRangeEnemies(aoeTargets);
					tower2UltimateSkill1.firstTimeUpgrade = false;
					return;
				}
				if (tower2UltimateSkill1.ChanceToCastSkill > 0 && UnityEngine.Random.Range(0, 100) < tower2UltimateSkill1.ChanceToCastSkill)
				{
					AttackInRangeEnemies(aoeTargets);
				}
			}
		}

		private void AttackInRangeEnemies(List<EnemyData> aoeTargets)
		{
			foreach (EnemyData enemyModel in aoeTargets)
			{
				enemyModel.BuffsHolder.AddBuff(burningBuffKey, new BuffStatus(false, (float)tower2UltimateSkill1.DamageBurn, tower2UltimateSkill1.Duration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				BurningEffect(enemyModel, tower2UltimateSkill1.Duration);
			}
			BurningGround(tower2UltimateSkill1.Duration);
		}

		private void BurningEffect(EnemyData enemy, float burningTime)
		{
			enemy.EnemyEffectController.PlayFXBurning(burningTime);
		}

		private void BurningGround(float burningTime)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.METEOR_EXPLOSION);
			effect.transform.position = base.gameObject.transform.position;
			effect.Init(burningTime);
		}

		private void OnDestroy()
		{
			bulletModel.OnInitialized -= BulletModel_OnInitialized;
		}

		private ProjectileEntity bulletModel;

		private Turret2Mastery0Ability1 tower2UltimateSkill1;

		private string burningBuffKey = "Burning";

		private Vector3 scaleVector = new Vector3(1.3f, 1.3f, 1.3f);
	}
}
