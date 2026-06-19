using System;
using System.Collections.Generic;
using System.Diagnostics;
using MetaGame;
using GameCore;
using Parameter;
using Common;
using UnityEngine;

namespace Gameplay
{
	public class TurretEntity : BaseMonoBehaviour
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TurretEntity.TowerCommonEventHandler OnReturnToPool;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TurretEntity.TowerUpgradeHandler OnUpgrade;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TurretEntity.TowerCommonEventHandler OnBuildFinished;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TurretEntity.TowerCommonEventHandler OnSell;

		public CanEnhanceStanding CanUpgradeStatus { get; set; }

		internal int finalDamagePhysics_min
		{
			get
			{
				return (int)((float)OriginalParameter.damage_Physics_min * (1f + (float)bonusDmgPercent / 100f));
			}
		}

		internal int finalDamagePhysics_max
		{
			get
			{
				return (int)((float)OriginalParameter.damage_Physics_max * (1f + (float)bonusDmgPercent / 100f));
			}
		}

		internal int finalDamageMagic_min
		{
			get
			{
				return (int)((float)OriginalParameter.damage_Magic_min * (1f + (float)bonusDmgPercent / 100f));
			}
		}

		internal int finalDamageMagic_max
		{
			get
			{
				return (int)((float)OriginalParameter.damage_Magic_max * (1f + (float)bonusDmgPercent / 100f));
			}
		}

		internal int finalCriticalStrikeChange
		{
			get
			{
				return (int)((float)OriginalParameter.criticalStrikeChance * (1f + (float)bonusCriticalStrikeChange / 100f));
			}
		}

		public BuffHolder BuffsHolder
		{
			get
			{
				return buffsHolder;
			}
			private set
			{
				buffsHolder = value;
			}
		}

		public TurretSpec OriginalParameter
		{
			get
			{
				return tower;
			}
			private set
			{
				tower = value;
			}
		}

		public int Id
		{
			get
			{
				return id;
			}
			private set
			{
				id = value;
			}
		}

		public int Level
		{
			get
			{
				return level;
			}
			private set
			{
				level = value;
			}
		}

		public Vector3 CachedPosition
		{
			get
			{
				return cachedPosition;
			}
		}

		public int RegionID
		{
			get
			{
				return regionID;
			}
			private set
			{
				regionID = value;
			}
		}

		private void Awake()
		{
			LoadParameters();
			PrototypeInitialize();
		}

		private void Update()
		{
			UpdateCachedPosition();
		}

		private void Initialize()
		{
			foreach (TurretHandler towerController in controllers)
			{
				towerController.TowerModel = this;
				towerController.Initialize();
			}
		}

		private void LoadParameters()
		{
			// Stats now come from the permanent skill tree (base + unlocked nodes), not the tower's
			// level. The serialized Level only selects which prefab visual is used (canonical tier).
			OriginalParameter = TowerParameterManager.Instance.GetStatWithTree(Id);
		}

		public TurretSpec GetTowerParameter()
		{
			return OriginalParameter;
		}

		private void GetControllers()
		{
			controllers = new List<TurretHandler>(base.GetComponentsInChildren<TurretHandler>(true));
		}

		private void GetAllComponents()
		{
			towerAttackController = base.GetComponentInChildren<TurretStrikeHandler>();
			towerSoundController = base.GetComponent<TurretSfxHandler>();
			towerFindEnemyController = base.GetComponentInChildren<TurretSeekEnemyHandler>();
			towerSpawnAllyController = base.GetComponentInChildren<TurretSummonMinionHandler>();
		}

		private void UpdateCachedPosition()
		{
			cachedPosition = base.transform.position;
		}

		private void PrototypeInitialize()
		{
			GetControllers();
			GetAllComponents();
			SetupEquipment();
			Initialize();
		}

		// Item-equip holder. Added at runtime so no per-prefab wiring is needed; shares this tower's
		// BuffHolder so item stats flow through the same buff pipeline as everything else.
		private void SetupEquipment()
		{
			Equipment = base.GetComponentInChildren<TowerEquipment>();
			if (Equipment == null)
			{
				Equipment = base.gameObject.AddComponent<TowerEquipment>();
			}
			Equipment.Initialize(this, BuffsHolder);
		}

		public void Appear()
		{
			foreach (TurretHandler towerController in controllers)
			{
				towerController.OnAppear();
			}
			OnAppearEvent.Dispatch();
		}

		// Level-up upgrades are retired: tower power now comes from the permanent skill tree, bought
		// between matches. Kept as a no-op so existing UI callers still compile; the in-match upgrade
		// UI is removed in a later phase.
		public void Upgrade(int ultiNo)
		{
		}

		public void Sell()
		{
			if (Equipment != null)
			{
				Equipment.ReturnAllToInventory();
			}
			MonoSingleton<GameRecord>.Instance.IncreaseMoney(OriginalParameter.value);
			MonoSingleton<TurretControlSfxHandler>.Instance.PlaySell();
			MonoSingleton<ConstructSectorDirector>.Instance.listRegions[RegionID].DisplayBuildable();
			if (OnSell != null)
			{
				OnSell(this);
			}
			ReturnPool(true);
		}

		private void OnDisable()
		{
			if (base.GetComponent<TurretItem>() != null)
			{
				base.GetComponent<TurretEntity>().enabled = false;
				base.GetComponent<TurretItem>().enabled = true;
			}
		}

		private void UpdateUpgrade(int level)
		{
			int price;
			if (level <= TowerParameterManager.MAX_BASE_LEVEL)
			{
				price = TowerParameterManager.Instance.GetPrice(id, level + 1);
			}
			else
			{
				price = TowerParameterManager.Instance.GetPrice(id, level);
			}
			if (price < 0)
			{
				this.CanUpgradeStatus = CanEnhanceStanding.MaxUpgrade;
			}
			else if (price <= MonoSingleton<GameRecord>.Instance.Money)
			{
				this.CanUpgradeStatus = CanEnhanceStanding.CanUpgrade;
			}
			else
			{
				this.CanUpgradeStatus = CanEnhanceStanding.CannotUpgrade;
			}
		}

		private void RunEffectBuild()
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_BUILD_TOWER);
			effect.transform.position = MonoSingleton<ConstructSectorDirector>.Instance.listRegions[regionID].transform.position;
			effect.Init(2f);
		}

		public void StartBuild(int id, int level, int regionID)
		{
			RegionID = regionID;
			RunEffectBuild();
			if (towerSoundController && level > 0)
			{
				MonoSingleton<TurretControlSfxHandler>.Instance.PlayUpgradeNormal(id);
			}
			MonoSingleton<GameRecord>.Instance.AddTowerToActiveList(this);
			BuffsHolder.ResetBuffs();
			// Fresh build from the pool: start with no items so a reused tower never inherits old gear.
			if (Equipment != null)
			{
				Equipment.ClearAll();
			}
			OnBuildDone();
		}

		private void OnBuildDone()
		{
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnBuildFinished();
			}
			if (OnBuildFinished != null)
			{
				OnBuildFinished(this);
			}
		}

		public void ChooseTower()
		{
			if (!IsSilent)
			{
				MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.Init(this, base.transform);
				Setup.Instance.currentTowerRegionIDSelected = regionID;
				MonoSingleton<GameRecord>.Instance.RecordingPosition = false;
				MonoSingleton<GameRecord>.Instance.CurrentTowerSelected = this;
			}
		}

		public void UnChooseTower()
		{
			MonoSingleton<UIRootHandler>.Instance.UpgradeTowerPopupController.Close();
			MonoSingleton<GameRecord>.Instance.RecordingPosition = false;
			MonoSingleton<GameRecord>.Instance.CurrentTowerSelected = null;
			GameplayDirector.Instance.CurrentTowerRange.GetComponent<TurretRangeHandler>().HideRange();
		}

		public CanEnhanceStanding GetUpgradeEnable(int level)
		{
			UpdateUpgrade(level);
			return CanUpgradeStatus;
		}

		public void ReturnPool(bool isSold)
		{
			if (isSold)
			{
			}
			MonoSingleton<GameRecord>.Instance.RemoveTowerFromActiveList(this, isSold);
			MonoSingleton<TowerPool>.Instance.Despawn(this);
			foreach (TurretHandler towerController in controllers)
			{
				towerController.OnReturnPool();
			}
			if (OnReturnToPool != null)
			{
				OnReturnToPool(this);
			}
		}

		public OrderedUnityEvent OnAppearEvent;

		[SerializeField]
		private int id;

		[SerializeField]
		private int level;

		[NonSerialized]
		public bool IsSilent;

		[HideInInspector]
		[NonSerialized]
		public int damagePhysics;

		[HideInInspector]
		[NonSerialized]
		public int damageMagic;

		[HideInInspector]
		[NonSerialized]
		public int bonusDmgPercent;

		[HideInInspector]
		[NonSerialized]
		public int bonusCriticalStrikeChange;

		private TurretSpec tower;

		public Transform gun;

		public Transform gunBarrel;

		[SerializeField]
		[HideInInspector]
		private List<TurretHandler> controllers = new List<TurretHandler>();

		[HideInInspector]
		public TurretStrikeHandler towerAttackController;

		[HideInInspector]
		public TurretSfxHandler towerSoundController;

		[HideInInspector]
		public TurretSeekEnemyHandler towerFindEnemyController;

		[HideInInspector]
		public TurretSummonMinionHandler towerSpawnAllyController;

		[Header("Required components")]
		[SerializeField]
		private BuffHolder buffsHolder;

		public TurretMasteryHandler towerUltimateController;

		// Item-equip holder (4 slots). Set up at runtime in PrototypeInitialize.
		public TowerEquipment Equipment { get; private set; }

		private Vector3 cachedPosition;

		private int regionID;

		public delegate void TowerCommonEventHandler(TurretEntity towerModel);

		public delegate void TowerUpgradeHandler(TurretEntity oldTower, TurretEntity newTower);
	}
}
