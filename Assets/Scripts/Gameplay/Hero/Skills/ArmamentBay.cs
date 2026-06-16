using System;
using System.Collections;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class ArmamentBay : BaseMonoBehaviour
	{
		public void Update()
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
				base.StartCoroutine(CastFire());
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		public void InitFromHero(HeroEntity sourceHero, int physicsDamage, float cooldownTime, float attackRange, float lifeTime)
		{
			this.sourceHero = sourceHero;
			this.cooldownTime = cooldownTime;
			this.attackRange = attackRange;
			this.physicsDamage = physicsDamage;
			timeTracking = cooldownTime;
			weaponStationAnimator.Reset();
			InitFXs();
			base.CustomInvoke(new Action(GetReady), appearAnimDuration);
			base.CustomInvoke(new Action(EndOfLifeTime), lifeTime + appearAnimDuration);
		}

		private void GetReady()
		{
			unlock = true;
		}

		public void OnDrawGizmosSelected()
		{
			if (unlock)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(base.transform.position, attackRange);
			}
		}

		private void InitFXs()
		{
			MonoSingleton<BulletPool>.Instance.InitExtendBullet(bulletPrefab);
		}

		private IEnumerator CastFire()
		{
			timeTracking = cooldownTime;
			EnemyData target = GameKit.GetRandomEnemyInRange(base.gameObject, new SharedStrikeDamage(0, 0, attackRange));
			if (GameKit.IsValidEnemy(target))
			{
				weaponStationAnimator.PlayAnimAttack();
				yield return new WaitForSeconds(delayTimeCastSkill);
				ProjectileEntity bullet = MonoSingleton<BulletPool>.Instance.GetBulletByName(bulletName);
				bullet.transform.position = gunPos.position;
				if (sourceHero != null)
				{
					bullet.InitFromHero(sourceHero, new SharedStrikeDamage(physicsDamage, 0, 0f), target);
				}
				bullet.gameObject.SetActive(true);
			}
			yield break;
		}

		private void EndOfLifeTime()
		{
			unlock = false;
			weaponStationAnimator.PlayAnimDisappear();
			base.CustomInvoke(new Action(ReturnPool), disAppearAnimDuration);
		}

		private void ReturnPool()
		{
			MonoSingleton<TowerPool>.Instance.Despawn(base.gameObject);
		}

		[SerializeField]
		private Transform gunPos;

		[SerializeField]
		private GameObject bulletPrefab;

		[SerializeField]
		private string bulletName;

		private TurretEntity sourceTower;

		private HeroEntity sourceHero;

		private bool unlock;

		[Space]
		[SerializeField]
		private ArmamentBayAnimator weaponStationAnimator;

		private int physicsDamage;

		private float attackRange;

		private float timeTracking;

		private float cooldownTime;

		private float lifeTime;

		[Space]
		[SerializeField]
		private float appearAnimDuration;

		[SerializeField]
		private float disAppearAnimDuration;

		[SerializeField]
		private float delayTimeCastSkill;
	}
}
