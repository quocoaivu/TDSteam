using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Gameplay
{
	public class EnemyHealth : EnemyBrain
	{
        [SerializeField]
        private OrderedUnityEvent outOfHealthEvent;

        [Space]
        [SerializeField]
        private OrderedUnityEvent onIncreaseHealthEvent;

        [Space]
        [SerializeField]
        private OrderedUnityEvent onDecreaseHealthEvent;

        private List<string> overtimeDamageBuffKeys = new List<string>
        {
            "Burning"
        };

        private List<string> bleededBuffKeys = new List<string>
        {
            "Bleed"
        };

        private List<string> reduceMagicArmorBussKeys = new List<string>
        {
            "ReduceMagicArmor"
        };

        private List<string> defDownBuffKeys = new List<string>
        {
            "DefDown"
        };

        private float timeTrackingOverTimeDamage;

        private int originHealth;

        private int currentHealth;

        private bool isFirstHealthChange;

        private float currentPhysicsArmor;

        private float currentMagicArmor;

        private float originPhysicsArmor;

        private float originMagicArmor;

        private float ignoreDamgeChance;

        private float inputDamageIncrementPercentage;

        private BuffHolder buffsHolder;

        [Space]
        [Header("Health Bar View")]
        [SerializeField]
        private bool haveHealthBar = true;

        [SerializeField]
        private Transform healthBarPosition;

        private TrooperVitalityView enemyHealthView;

        public int OriginHealth
		{
			get
			{
				return originHealth;
			}
			private set
			{
				originHealth = value;
			}
		}

		public int CurrentHealth
		{
			get
			{
				return currentHealth;
			}
			private set
			{
				if (currentHealth > 0 && value <= 0)
				{
					GameSignalCenter.Instance.Trigger(GameSignalKind.EventKillMonster, new SignalTriggerRecord(SignalTriggerKind.KillMonster, base.EnemyModel.Id, 1, false));
				}
				currentHealth = value;
				if (currentHealth <= 0)
				{
					if (!base.EnemyModel.IsAlive)
					{
						return;
					}
					outOfHealthEvent.Dispatch();
				}
			}
		}

		public float OriginPhysicsArmor
		{
			get
			{
				return originPhysicsArmor;
			}
			private set
			{
				originPhysicsArmor = value;
			}
		}

		public float CurrentPhysicsArmor
		{
			get
			{
				return currentPhysicsArmor;
			}
			private set
			{
				currentPhysicsArmor = value;
				currentPhysicsArmor = Mathf.Clamp(currentPhysicsArmor, 0f, 0.95f);
			}
		}

		public float OriginMagicArmor
		{
			get
			{
				return originMagicArmor;
			}
			private set
			{
				originMagicArmor = value;
			}
		}

		public float CurrentMagicArmor
		{
			get
			{
				return currentMagicArmor;
			}
			private set
			{
				currentMagicArmor = value;
				currentMagicArmor = Mathf.Clamp(currentMagicArmor, 0f, 0.95f);
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			OriginHealth = base.EnemyModel.OriginalParameter.health;
			CurrentHealth = OriginHealth;
			ignoreDamgeChance = 0f;
			OriginPhysicsArmor = (float)base.EnemyModel.OriginalParameter.armor_physics / 100f;
			CurrentPhysicsArmor = OriginPhysicsArmor;
			OriginMagicArmor = (float)base.EnemyModel.OriginalParameter.armor_magic / 100f;
			CurrentMagicArmor = OriginMagicArmor;
		}

		public override void Initialize()
		{
			base.Initialize();
			base.EnemyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			if (enemyHealthView)
			{
				enemyHealthView.OnReturnPool();
			}
		}

		public void Awake()
		{
			buffsHolder = base.EnemyModel.BuffsHolder;
			base.EnemyModel.OnStartRun += EnemyModel_OnStartRun;
		}

		private void EnemyModel_OnStartRun(int obj)
		{
			if (haveHealthBar)
			{
				SetupHealthBar();
			}
		}

		public void SetupHealthBar()
		{
			enemyHealthView = MonoSingleton<UnitHealthBarPool>.Instance.Get();
			if (base.EnemyModel.OriginalParameter.isBoss)
			{
				enemyHealthView.SetupHealth(CharacterKind.Boss, base.gameObject, healthBarPosition);
			}
			else
			{
				enemyHealthView.SetupHealth(CharacterKind.Enemy, base.gameObject, healthBarPosition);
			}
		}

		public void HideHealthBar()
		{
			if (haveHealthBar)
			{
				enemyHealthView.Hide();
			}
		}

		private void OnDestroy()
		{
			base.EnemyModel.OnStartRun -= EnemyModel_OnStartRun;
		}

		public override void Update()
		{
			base.Update();
			if (!base.EnemyModel.IsAlive)
			{
				return;
			}
			if (timeTrackingOverTimeDamage == 0f)
			{
				UpdateOvertimeDamage();
			}
			timeTrackingOverTimeDamage = Mathf.MoveTowards(timeTrackingOverTimeDamage, 0f, Time.deltaTime);
		}

		public void AddHealth(int amount)
		{
			CurrentHealth += amount;
			if (CurrentHealth >= OriginHealth)
			{
				CurrentHealth = OriginHealth;
			}
			onIncreaseHealthEvent.Dispatch();
			UpdateHealthView();
		}

		public void ChangeHealth(SharedStrikeDamage damageInfo)
		{
			if (CurrentHealth <= 0)
			{
				return;
			}
			int damageDealt = 0;
			if (inputDamageIncrementPercentage > 0f)
			{
				damageInfo.physicsDamage = (int)((float)damageInfo.physicsDamage * (1f + inputDamageIncrementPercentage / 100f));
				damageInfo.magicDamage = (int)((float)damageInfo.magicDamage * (1f + inputDamageIncrementPercentage / 100f));
			}
			damageInfo.targetInstanceId = base.EnemyModel.gameObject.GetEntityId();
			damageInfo.targetEnemyModel = base.EnemyModel;
			if (damageInfo.physicsDamage > 0)
			{
				GameSignalCenter.Instance.Trigger(GameSignalKind.OnBeforeCalculatePhysicsDamage, damageInfo);
				if (CurrentPhysicsArmor > 0f)
				{
					if (!damageInfo.isIgnoreArmor)
					{
						damageInfo.physicsDamage -= (int)((float)damageInfo.physicsDamage * CurrentPhysicsArmor);
					}
					else if (!damageInfo.isNotPlayIgnoreArmorEffect)
					{
						ObjectCache.Spawn(ConfigRegistry.Instance.IgnoreDefPrefab, base.EnemyModel.transform.position + new Vector3(0f, 0.2f, 0f));
					}
					damageInfo.physicsDamage = Mathf.Clamp(damageInfo.physicsDamage, 1, 999999);
				}
				if (damageInfo.isInstantKill && !base.EnemyModel.OriginalParameter.isBoss)
				{
					damageInfo.physicsDamage = CurrentHealth;
				}
				if (damageInfo.physicsDamage > 0)
				{
					GameSignalCenter.Instance.Trigger(GameSignalKind.OnAfterCalculatePhysicsDamage, damageInfo);
					CurrentHealth -= damageInfo.physicsDamage;
					damageDealt += damageInfo.physicsDamage;
					onDecreaseHealthEvent.Dispatch();
					UpdateHealthView();
				}
			}
			if (damageInfo.magicDamage > 0)
			{
				if (CurrentMagicArmor > 0f)
				{
					damageInfo.magicDamage -= (int)((float)damageInfo.magicDamage * CurrentMagicArmor);
					damageInfo.magicDamage = Mathf.Clamp(damageInfo.magicDamage, 1, 999999);
				}
				if (damageInfo.magicDamage > 0)
				{
					GameSignalCenter.Instance.Trigger(GameSignalKind.OnAfterCalculateMagicDamage, damageInfo);
					CurrentHealth -= damageInfo.magicDamage;
					damageDealt += damageInfo.magicDamage;
					onDecreaseHealthEvent.Dispatch();
					UpdateHealthView();
				}
			}
			ShowFloatingDamage(damageDealt);
		}

		// Pooled floating damage number at the hit enemy's position (see FloatingDamageTextPool).
		// Pass a position snapshot (NOT the transform): the number is static and must survive the
		// enemy dying / returning to pool. EnemyModel is on enemy_0, the on-screen unit.
		private void ShowFloatingDamage(int amount)
		{
			if (amount <= 0)
			{
				return;
			}
			FloatingDamageTextPool.Instance.Show(amount, base.EnemyModel.transform.position);
		}

		private void UpdateHealthView()
		{
			if (haveHealthBar)
			{
				enemyHealthView.UpdateHealth(CurrentHealth, OriginHealth);
			}
		}

		private bool ShouldIgnoreDamage()
		{
			return (float)UnityEngine.Random.Range(0, 100) < ignoreDamgeChance;
		}

		private void UpdateOvertimeDamage()
		{
			if (!base.EnemyModel.IsAlive)
			{
				timeTrackingOverTimeDamage = 1f;
				return;
			}
			int num = 0;
			for (int i = 0; i < overtimeDamageBuffKeys.Count; i++)
			{
				float num2;
				if (buffsHolder.TryGetBuffValue(overtimeDamageBuffKeys[i], out num2))
				{
					num = (int)num2;
				}
			}
			if (num > 0)
			{
				ChangeHealth(new SharedStrikeDamage(num, 0, 0f));
			}
			timeTrackingOverTimeDamage = 1f;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (bleededBuffKeys.Contains(buffKey))
			{
				ApplyBuffBleeded();
			}
			if (reduceMagicArmorBussKeys.Contains(buffKey))
			{
				ApplyBuffReduceMagicArmor();
			}
			if (defDownBuffKeys.Contains(buffKey))
			{
				ApplyBuffDefdown();
			}
		}

		private void ApplyBuffBleeded()
		{
			inputDamageIncrementPercentage = base.EnemyModel.BuffsHolder.GetBuffsValue(bleededBuffKeys);
		}

		private void ApplyBuffReduceMagicArmor()
		{
			float num = base.EnemyModel.BuffsHolder.GetBuffsValue(reduceMagicArmorBussKeys) / 100f;
			CurrentMagicArmor = OriginMagicArmor - num;
			CurrentMagicArmor = Mathf.Clamp(CurrentMagicArmor, 0f, 1f);
		}

		private void ApplyBuffDefdown()
		{
			float num = base.EnemyModel.BuffsHolder.GetBuffsValue(defDownBuffKeys) / 100f;
			CurrentMagicArmor = OriginMagicArmor - num;
			CurrentMagicArmor = Mathf.Clamp(CurrentMagicArmor, 0f, 1f);
			CurrentPhysicsArmor = OriginPhysicsArmor - num;
			CurrentPhysicsArmor = Mathf.Clamp(CurrentPhysicsArmor, 0f, 1f);
		}
	}
}
