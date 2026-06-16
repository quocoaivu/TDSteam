using System;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret2Mastery1Ability0 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
			TryToCastSkill();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void Update()
		{
			if (!unlock)
			{
				return;
			}
			if (isCooldownDone())
			{
				TryToCastSkill();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToCast = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			enemyAffected = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			stunDuration = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			skillRange = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 4);
			unlock = true;
			timeTracking = cooldownTime;
			commonAttackDamage.aoeRange = skillRange;
			effectAttack.buffKey = "Stun";
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = 100;
			effectAttack.debuffEffectDuration = stunDuration;
			effectAttack.damageFXType = DamageVfxType.Thunder;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_THUNDER);
		}

		private void TryToCastSkill()
		{
			if (!unlock)
			{
				return;
			}
			if (chanceToCast > 0 && UnityEngine.Random.Range(0, 100) < chanceToCast)
			{
				List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
				if (listEnemiesInRange.Count > 0)
				{
					damageToEnemiesInRange.CastDamage(enemyAffected, DamageKind.Range, commonAttackDamage, effectAttack);
					towerModel.towerSoundController.PlayCastSkillSound(skillID);
				}
			}
			timeTracking = cooldownTime;
		}

		private bool isCooldownDone()
		{
			return timeTracking == 0f;
		}

		private int towerID = 2;

		private int ultimateBranch = 1;

		private int skillID;

		private int enemyAffected;

		private int chanceToCast;

		private float stunDuration;

		private float skillRange;

		private float cooldownTime;

		private float timeTracking;

		private SharedStrikeDamage commonAttackDamage = new SharedStrikeDamage();

		private OnHitStatusApplier effectAttack;

		private TurretEntity towerModel;

		[SerializeField]
		private RangeDamageCaster damageToEnemiesInRange;
	}
}
