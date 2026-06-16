using System;
using System.Collections;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class MiniWyrmHandler : BaseMonoBehaviour
	{
		private void Awake()
		{
			animator = base.GetComponentInChildren<Animator>();
			localScale = base.transform.localScale;
		}

		public void Init(TurretEntity towerModel, int damage)
		{
			this.towerModel = towerModel;
			magicDamage = damage;
		}

		public void StartAttack(EnemyData target)
		{
			this.target = target;
			LookAtTarget();
			base.StartCoroutine(Attack(target));
		}

		private IEnumerator Attack(EnemyData target)
		{
			yield return new WaitForSeconds(delayTime);
			PlayAttack();
			ProjectileEntity bullet = MonoSingleton<BulletPool>.Instance.GetBulletByName(bulletName);
			bullet.transform.position = gunPos.position;
			bullet.gameObject.SetActive(true);
			bullet.InitFromTower(towerModel, new SharedStrikeDamage(0, magicDamage, 0f), target);
			yield break;
		}

		private void PlayAttack()
		{
			animator.Play("Attack");
		}

		public void LookAtTarget()
		{
			localScale.x = 0.5f * (float)(-(float)GetDirection(target.gameObject));
			base.gameObject.transform.localScale = localScale;
		}

		private int GetDirection(GameObject target)
		{
			float num = target.transform.position.x - base.gameObject.transform.position.x;
			if (num > 0f)
			{
				num = 1f;
			}
			else
			{
				num = -1f;
			}
			return (int)num;
		}

		public void ReturnPool()
		{
			MonoSingleton<TowerPool>.Instance.Despawn(base.gameObject);
		}

		[SerializeField]
		private float delayTime;

		[SerializeField]
		private Transform gunPos;

		[SerializeField]
		private string bulletName;

		private Vector3 localScale = Vector3.one;

		private TurretEntity towerModel;

		private Animator animator;

		private EnemyData target;

		private int magicDamage;
	}
}
