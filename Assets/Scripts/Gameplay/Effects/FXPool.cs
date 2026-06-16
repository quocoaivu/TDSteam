using System;
using UnityEngine;

namespace Gameplay
{
	public class FXPool : MonoSingleton<FXPool>
	{
		private void Awake()
		{
			InitGemnGoldDropped();
		}

		public void InitExtendObject(GameObject sourceObject, int alloCateNumber)
		{
			GameObject gameObject = Instantiate(sourceObject);
			gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(gameObject, alloCateNumber);
			Common.GameObjectPool.Despawn(gameObject);
		}

		private void InitGemnGoldDropped()
		{
			CrystalHandler gemController = Instantiate(gemControllerPrefab);
			gemController.gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(gemController.gameObject, 0);
			Common.GameObjectPool.Despawn(gemController.gameObject);
			DroppedBullionHandler droppedGoldController = Instantiate(goldControllerPrefab);
			droppedGoldController.gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(droppedGoldController.gameObject, 0);
			Common.GameObjectPool.Despawn(droppedGoldController.gameObject);
		}

		public void InitFX(string effectName)
		{
			PoolEffect(effectName, 0);
		}

		public void InitFX(string effectName, int numberOfAllocate)
		{
			PoolEffect(effectName, numberOfAllocate);
		}

		// Load, register and pre-despawn one FX prefab (under Resources "FXs/") into the pool.
		private void PoolEffect(string effectName, int allocate)
		{
			VisualEffectInstance prefab = Common.AssetLoader.Load<VisualEffectInstance>(string.Format("FXs/{0}", effectName));
			if (prefab == null)
			{
				UnityEngine.Debug.LogError("PoolEffect: FX prefab not found: " + effectName);
				return;
			}
			VisualEffectInstance effectController = Instantiate(prefab);
			effectController.gameObject.SetActive(false);
			Common.GameObjectPool.ManagePool(effectController.gameObject, allocate);
			Common.GameObjectPool.Despawn(effectController.gameObject);
		}

		public void Despawn(VisualEffectInstance _effectcontroller)
		{
			_effectcontroller.transform.position = Common.GameObjectPool.PoolPosition;
			_effectcontroller.gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(_effectcontroller.gameObject);
		}

		public void Despawn(GameObject _gameObject)
		{
			_gameObject.transform.position = Common.GameObjectPool.PoolPosition;
			_gameObject.SetActive(false);
			Common.GameObjectPool.Despawn(_gameObject);
		}

		// Spawn a pooled object by its "(Clone)" name, parent it to this pool (inactive), and return its component.
		private T GetPooled<T>(string cloneName) where T : Component
		{
			GameObject gameObject = Common.GameObjectPool.Spawn(cloneName, default(Vector3), default(Quaternion));
			T component = gameObject.GetComponent<T>();
			component.gameObject.SetActive(false);
			component.transform.parent = transform;
			return component;
		}

		public VisualEffectInstance GetEffect(string effectName)
		{
			VisualEffectInstance component = GetPooled<VisualEffectInstance>(effectName + "(Clone)");
			component.transform.localPosition = Vector3.zero;
			return component;
		}

		public VisualEffectInstance GetExplosion(string explosionName)
		{
			return GetEffect(explosionName);
		}

		public ChillFx GetFrostEffectOnCamera()
		{
			return Camera.main.GetComponent<ChillFx>();
		}

		public GameObject GetObjectByName(string name)
		{
			GameObject gameObject = Common.GameObjectPool.Spawn(name + "(Clone)", default(Vector3), default(Quaternion));
			gameObject.SetActive(false);
			return gameObject;
		}

		public CrystalHandler GetDroppedGem()
		{
			return GetPooled<CrystalHandler>(DROPPED_GEM_NAME + "(Clone)");
		}

		public DroppedBullionHandler GetDroppedGold()
		{
			return GetPooled<DroppedBullionHandler>(DROPPED_GOLD_NAME + "(Clone)");
		}

		[Space]
		[Header("Start Wave Button n Bonus Money")]
		[SerializeField]
		private BonusBountyMotionHandler bonusMoneyAnimControllerPrefab;

		[Space]
		[Header("Gem n Gold")]
		[SerializeField]
		private CrystalHandler gemControllerPrefab;

		[SerializeField]
		private DroppedBullionHandler goldControllerPrefab;

		public const string EFFECT_MISS = "Miss";
		public const string EFFECT_CRITICAL = "Critical";
		public const string EFFECT_SLOW = "Slow";
		public const string EFFECT_STUN = "Stun";
		public const string EFFECT_POISON0 = "Poison0";
		public const string EFFECT_POISON1 = "Poison1";
		public const string EFFECT_BURNING = "Burning";
		public const string EFFECT_ROOT = "Root";
		public const string EFFECT_ELECTRIC = "Electric";
		public const string EFFECT_THUNDER = "Thunder";
		public const string EFFECT_BLEED = "Bleed";
		public const string EFFECT_BLOOD_SPREAD = "BloodSpread";
		public const string EFFECT_DEF_DOWN = "DefDown";
		public const string ARROW_ON_THE_GROUND = "ArrowOnTheGround";
		public const string GROUND_BREAK = "GroundBreak";
		public const string EFFECT_BUILD_TOWER = "BuildTower";
		public const string EFFECT_SELL_TOWER_ON_ALLY = "TowerSellOnAlly";
		public const string EFFECT_UPGRADE_TOWER_ON_ALLY = "TowerUpgradeOnAlly";
		public const string ICON_MOVEABLE_AllY = "Moveable_Ally";
		public const string ICON_MOVEABLE_HERO = "Moveable_Hero";
		public const string ICON_UNMOVEABLE = "UnMoveable";
		private const string DROPPED_GEM_NAME = "GemDropped";
		private const string DROPPED_GOLD_NAME = "GoldDropped";
		public const string EFFECT_ITEM_FREEZE = "Item-Freeze";
		public const string LIGHTNING_PROJECTILE_RANGE = "LightningProjectileRange";
		public const string LIGHTNING_PROJECTILE = "LightningProjectile";
		public const string LIGHTNING_EXPLOSION = "LightningExplosion";
		public const string LIGHTNING_PROJECTILE_SHADOW = "LightningProjectileShadow";
		public const string GROUND_STOMP = "GroundStomp";
		public const string EFFECT_FADE_SCREEN = "FadeScreen";
		public const string METEOR_EXPLOSION2 = "MeteorExplosion2";
		public const string METEOR_SELF_EXPLOSION = "MeteorSelfExplosion";
		public const string INFERNO_GOLEM_AURA = "InfernoGolemAura";
		public const string ATTACK_UP_FX = "AttackUpFx";
		public const string LIGHTNING_EXPLOSION_2 = "LightningExplosion2";
		public const string THOR_MASSIVE_THUNDER = "MassiveThunder";
		public const string THOR_LANDING_THUNDER = "LandingThunder";
		public const string THOR_AIR_THUNDER = "AirThunder";
		public const string TIMER_BOMB_EXPLOSION = "TimerBombExplosion";
		public const string GROUND_AIMING_1 = "GroundAiming1";
		public const string GOLD_CHEST = "GoldChest";
		public const string FLYING_COIN = "FlyingCoin";
		public const string METEOR_EXPLOSION = "MeteorExplosion";
		public const string HEALING_WAND = "HealingWand";
		public const string BUFF_SPEED_AURA = "BuffSpeedAura";
		public const string BUFF_SPEED_ON_TOWER = "BuffSpeedOnTower";
		public const string POISON_AREA = "PoisonArea";
		public const string STORM = "Storm";
		public const string EFFECT_HEAL_0 = "HealFX0";
		public const string EFFECT_HEAL_1 = "HealFX1";
		public const string EFFECT_HEAL_2 = "HealFX2";
	}
}
