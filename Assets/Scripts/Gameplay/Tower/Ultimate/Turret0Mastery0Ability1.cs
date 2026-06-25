using System;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret0Mastery0Ability1 : TurretMasteryShared
	{
		public override void InitTowerModel(TurretEntity towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			ReadParameter(ultiLevel);
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(arrowPrefab.gameObject);
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_ITEM_FREEZE);
			CastSkill();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToCastSkill = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			damage = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			slowPercent = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			slowTime = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3);
			commonAttackDamage.physicsDamage = damage;
			effectAttack.buffKey = buffKey;
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = slowPercent;
			effectAttack.debuffEffectDuration = slowTime;
			effectAttack.damageFXType = DamageVfxType.Freezing;
		}

		public void TryToCastFreezingArrow()
		{
			if (!unlock)
			{
				return;
			}
			if (chanceToCastSkill > 0 && UnityEngine.Random.Range(0, 100) < chanceToCastSkill)
			{
				CastSkill();
			}
		}

		private void CastSkill()
		{
			if (towerModel.towerFindEnemyController.Targets.Count > 0)
			{
				UnityEngine.Debug.Log("Cast skill freezing arrow!");
				TurretSpec originalParameter = towerModel.OriginalParameter;
				ProjectileEntity bulletByName = MonoSingleton<BulletPool>.Instance.GetBulletByName(bulletName);
				Vector3 position = towerModel.gunBarrel.position;
				float num = originalParameter.range;
				bulletByName.transform.position = position;
				bulletByName.gameObject.SetActive(true);
				EnemyData target = towerModel.towerFindEnemyController.Targets[0];
				bulletByName.InitFromTower(towerModel, new SharedStrikeDamage(damage, 0, 0f), effectAttack, target);
			}
		}

		private int towerID;

		private int ultimateBranch;

		private int skillID = 1;

		private int chanceToCastSkill;

		private int damage;

		private int slowPercent;

		private float slowTime;

		private string buffKey = "Slow";

		[Space]
		[SerializeField]
		private GameObject arrowPrefab;

		[SerializeField]
		private string bulletName;

		private TurretEntity towerModel;

		private SharedStrikeDamage commonAttackDamage = new SharedStrikeDamage();

		private OnHitStatusApplier effectAttack;
	}
}
