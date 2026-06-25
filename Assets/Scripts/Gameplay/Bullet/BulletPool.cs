using System;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class BulletPool : MonoSingleton<BulletPool>
	{
		public void InitBulletsFromTower(int towerID, int towerLevel)
		{
			PoolBullet(PoolNames.Bullet(towerID, towerLevel));
		}

		public void InitBulletsFromHeroes(int heroID, int bulletIndex)
		{
			PoolBullet(PoolNames.HeroBullet(heroID, bulletIndex));
		}

		// Load, register and pre-despawn one bullet prefab (under Resources "Bullets/") into the pool.
		private void PoolBullet(string resourceName)
		{
			ProjectileEntity prefab = Common.AssetLoader.Load<ProjectileEntity>(string.Format("Bullets/{0}", resourceName));
			if (prefab == null)
			{
				UnityEngine.Debug.LogError("PoolBullet: bullet prefab not found: " + resourceName);
				return;
			}
			ProjectileEntity bulletModel = Instantiate(prefab);
			bulletModel.gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(bulletModel.gameObject, 0);
			Common.GameObjectPool.Despawn(bulletModel.gameObject);
		}

		public void InitExtendBullet(GameObject bullet)
		{
			GameObject gameObject = Instantiate(bullet);
			gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(gameObject, 0);
			Common.GameObjectPool.Despawn(gameObject);
		}

		// Spawn a pooled object by its "(Clone)" name, parent it to this pool, and return its component.
		private T GetPooled<T>(string cloneName) where T : Component
		{
			GameObject gameObject = Common.GameObjectPool.Spawn(cloneName, default(Vector3), default(Quaternion));
			T component = gameObject.GetComponent<T>();
			component.transform.parent = transform;
			return component;
		}

		public ProjectileEntity GetForTower(int towerID, int towerLevel)
		{
			if (towerID < 0 || towerLevel < 0 || towerID >= TowerParameterManager.Instance.GetNumberOfTower())
			{
				return null;
			}
			return GetPooled<ProjectileEntity>(PoolNames.Clone(PoolNames.Bullet(towerID, towerLevel)));
		}

		public ProjectileEntity GetForHero(int heroID, int bulletIndex)
		{
			if (heroID < 0)
			{
				return null;
			}
			return GetPooled<ProjectileEntity>(PoolNames.Clone(PoolNames.HeroBullet(heroID, bulletIndex)));
		}

		public ProjectileEntity GetBulletByName(string name)
		{
			return GetPooled<ProjectileEntity>(PoolNames.Clone(name));
		}

		public EnemyProjectile GetEnemyBulletByName(string name)
		{
			return GetPooled<EnemyProjectile>(PoolNames.Clone(name));
		}

		public HeroAbilityAOEShared GetHeroSkillAOECommon(string name)
		{
			return GetPooled<HeroAbilityAOEShared>(PoolNames.Clone(name));
		}

		public Hero1Ability0Projectile GetLightningBullet()
		{
			return GetPooled<Hero1Ability0Projectile>("LightningProjectile(Clone)");
		}

		public Hero3Ability0Comet GetHero3Skill0Meteor()
		{
			return GetPooled<Hero3Ability0Comet>("Hero3Skill0Meteor(Clone)");
		}

		public Hero3Ability2IceTrap GetHero3Skill2IceTrap()
		{
			return GetPooled<Hero3Ability2IceTrap>("Hero3Skill2IceTrap(Clone)");
		}

		public Hero3Ability3SunStrike GetHero3Skill3SunStrike()
		{
			return GetPooled<Hero3Ability3SunStrike>("Hero3Skill3SunStrike(Clone)");
		}

		public Hero4Ability0Breakdown GetHero4Skill0Breakdown()
		{
			return GetPooled<Hero4Ability0Breakdown>("Hero4Skill0Breakdown(Clone)");
		}

		public Hero5Ability0MendBomb GetHero5Skill0HealingBomb()
		{
			return GetPooled<Hero5Ability0MendBomb>("Hero5Skill0HealingBomb(Clone)");
		}

		public Hero5Ability3LightningSpear GetHero5Skill3LightningSpear()
		{
			return GetPooled<Hero5Ability3LightningSpear>("Hero5Skill3LightningSpear(Clone)");
		}

		public CometHandler GetMeteorController()
		{
			return GetPooled<CometHandler>("Meteor(Clone)");
		}

		public Turret0Mastery0Projectile GetTower0Ultimate0Bullet()
		{
			return GetPooled<Turret0Mastery0Projectile>("Tower0Ultimate0Bullet(Clone)");
		}

		public void Despawn(ProjectileEntity bullet)
		{
			bullet.transform.position = Common.GameObjectPool.PoolPosition;
			bullet.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(bullet.gameObject);
		}

		public void Despawn(GameObject bullet)
		{
			bullet.transform.position = Common.GameObjectPool.PoolPosition;
			bullet.SetActive(false);
			Common.GameObjectPool.Despawn(bullet);
		}
	}
}
