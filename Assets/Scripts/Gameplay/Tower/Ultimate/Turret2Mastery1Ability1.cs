using System;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret2Mastery1Ability1 : TurretMasteryShared
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
			// attackAnimationController is optional and unassigned on existing tower prefabs
			if (attackAnimationController != null)
			{
				attackAnimationController.OnReturnPool();
			}
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
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void ReadParameter(int currentSkillLevel)
		{
			physicsDamage = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			enemyAffected = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			cooldownTime = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			skillRange = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameRecord.PIXEL_PER_UNIT;
			unlock = true;
			timeTracking = cooldownTime;
			commonAttackDamage = new SharedStrikeDamage();
			commonAttackDamage.physicsDamage = physicsDamage;
			commonAttackDamage.magicDamage = magicDamage;
			commonAttackDamage.aoeRange = skillRange;
			commonAttackDamage.targetType.isAir = true;
			attackWithSpecialEffect.attackFXType = StrikeFXKind.Electric;
			attackWithSpecialEffect.duration = 0.75f;
		}

		private void TryToCastSkill()
		{
			if (!unlock)
			{
				return;
			}
			List<EnemyData> listEnemiesInRange = GameKit.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			if (listEnemiesInRange.Count > 0)
			{
				damageToEnemiesInRange.CastDamage(enemyAffected, DamageKind.Range, commonAttackDamage, attackWithSpecialEffect);
				towerModel.towerSoundController.PlayCastSkillSound(skillID);
			}
			timeTracking = cooldownTime;
		}

		private bool isCooldownDone()
		{
			return timeTracking == 0f;
		}

		private int towerID = 2;

		private int ultimateBranch = 1;

		private int skillID = 1;

		private int enemyAffected;

		private int physicsDamage;

		private int magicDamage;

		private float skillRange;

		private float cooldownTime;

		private float timeTracking;

		private SharedStrikeDamage commonAttackDamage;

		private StrikeWithSpecialFx attackWithSpecialEffect;

		private TurretEntity towerModel;

		[SerializeField]
		private RangeDamageCaster damageToEnemiesInRange;

		[SerializeField]
		private StrikeMotionHandler attackAnimationController;
	}
}
