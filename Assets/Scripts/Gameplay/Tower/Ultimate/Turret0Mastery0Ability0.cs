using System;
using System.Collections;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Turret0Mastery0Ability0 : TurretMasteryShared
	{
		private void Start()
		{
			offset.Set(offsetX, offsetY);
		}

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
			CastSkill();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToCastSkill = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			numberOfArrow = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			damagePerArrow = TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			skillRange = (float)TurretAbilitySpec.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameRecord.PIXEL_PER_UNIT;
			InitFXs();
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.LIGHTNING_PROJECTILE_RANGE);
		}

		public void TryToCastRainOfArrow()
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
				UnityEngine.Debug.Log("Cast skill rain of arrow!");
				EnemyData enemyModel = towerModel.towerFindEnemyController.Targets[0];
				CastEffectSkillRange(enemyModel.transform.position);
				base.StartCoroutine(CastSkill(enemyModel.transform.position));
			}
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			for (int i = 0; i < numberOfArrow; i++)
			{
				Turret0Mastery0Projectile bullet = MonoSingleton<BulletPool>.Instance.GetTower0Ultimate0Bullet();
				bullet.transform.position = targetPosition + offset + UnityEngine.Random.insideUnitCircle * skillRange;
				Vector3 destination = bullet.transform.position - (Vector3)offset;
				bullet.Init(damagePerArrow, duration, destination);
				yield return new WaitForSeconds(delayTime);
			}
			yield break;
		}

		private void CastEffectSkillRange(Vector2 targetPosition)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.LIGHTNING_PROJECTILE_RANGE);
			effect.transform.position = targetPosition;
			effect.Init(0.75f);
		}

		private int towerID;

		private int ultimateBranch;

		private int skillID;

		private int chanceToCastSkill;

		private int numberOfArrow;

		private int damagePerArrow;

		private float skillRange;

		[SerializeField]
		private float delayTime;

		[SerializeField]
		private float duration;

		[SerializeField]
		private GameObject arrowPrefab;

		[Space]
		[SerializeField]
		private float offsetX;

		[Space]
		[SerializeField]
		private float offsetY;

		private Vector2 offset;

		private TurretEntity towerModel;
	}
}
