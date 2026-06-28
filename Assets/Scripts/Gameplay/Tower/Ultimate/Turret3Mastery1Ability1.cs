using System;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret3Mastery1Ability1 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void Update()
		{
			if (!MonoSingleton<GameRecord>.Instance.IsGameStart)
			{
				return;
			}
			if (!unlock)
			{
				return;
			}
			if (IsCooldownDone())
			{
				CastSkill();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void ReadParameter(int currentSkillLevel)
		{
			magicArmorDecrease = TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			skillRange = (float)TurretAbilitySpec.Instance.GetParamWithTree(towerID, ultimateBranch, skillID, currentSkillLevel, 1) / GameRecord.PIXEL_PER_UNIT;
			unlock = true;
			effectAttack.buffKey = buffKey;
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = magicArmorDecrease;
			effectAttack.debuffEffectDuration = duration;
			InitFXs();
		}

		private void InitFXs()
		{
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void CastSkill()
		{
			damageToAOERange.CastDamage(DamageKind.Range, new SharedStrikeDamage(0, 0, skillRange), effectAttack);
			timeTracking = cooldownTime;
			towerModel.towerSoundController.PlayCastSkillSound(skillID);
		}

		private int towerID = 3;

		private int ultimateBranch = 1;

		private int skillID = 1;

		private TurretEntity towerModel;

		private int magicArmorDecrease;

		private float skillRange;

		[SerializeField]
		private float duration;

		[SerializeField]
		private float cooldownTime;

		private float timeTracking;

		private string buffKey = "ReduceMagicArmor";

		[SerializeField]
		private AoeDamageCaster damageToAOERange;

		private OnHitStatusApplier effectAttack;
	}
}
