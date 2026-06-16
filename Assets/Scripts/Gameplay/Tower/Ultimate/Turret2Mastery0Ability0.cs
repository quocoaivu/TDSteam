using System;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret2Mastery0Ability0 : TurretMasteryShared
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
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(cannonBallPrefab);
			CastSkill();
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

		private void ReadParameter(int currentSkillLevel)
		{
			damage = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			skillRange = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1) / GameRecord.PIXEL_PER_UNIT;
			cooldownTime = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			timeTracking = cooldownTime;
		}

		public void TryToCastSkill()
		{
			if (!unlock)
			{
				return;
			}
			CastSkill();
		}

		private void CastSkill()
		{
			List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
			if (listActiveEnemy.Count > 0)
			{
				EnemyData enemyWithHighestHealth = MonoSingleton<GameRecord>.Instance.getEnemyWithHighestHealth(false, false);
				if (enemyWithHighestHealth == null)
				{
					return;
				}
				UnityEngine.Debug.Log("Cast skill Cannonball of Death!");
				ProjectileEntity bulletByName = MonoSingleton<BulletPool>.Instance.GetBulletByName(bulletName);
				bulletByName.transform.position = gunPos.position;
				bulletByName.gameObject.SetActive(true);
				bulletByName.InitFromTower(towerModel, new SharedStrikeDamage(damage, 0, skillRange), enemyWithHighestHealth);
				timeTracking = cooldownTime;
			}
		}

		private bool isCooldownDone()
		{
			return timeTracking == 0f;
		}

		private int towerID = 2;

		private int ultimateBranch;

		private int skillID;

		private int damage;

		private float skillRange;

		private float cooldownTime;

		private string description;

		private float timeTracking;

		private TurretEntity towerModel;

		[SerializeField]
		private Transform gunPos;

		[SerializeField]
		private GameObject cannonBallPrefab;

		[SerializeField]
		private string bulletName;
	}
}
