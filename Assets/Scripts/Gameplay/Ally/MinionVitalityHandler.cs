using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Gameplay
{
	public class MinionVitalityHandler : MinionHandler
	{

        private int originHealth;

        private int currentHealth;

        private bool isFirstHealthChange;

        private float currentPhysicsArmor;

        private float currentMagicArmor;

        private float originPhysicsArmor;

        private float originMagicArmor;

        private int dodgeChance;

        private BuffHolder buffsHolder;

        [SerializeField]
        private OrderedUnityEvent outOfHealthEvent;

        [Space]
        [SerializeField]
        private OrderedUnityEvent onHealthChangeEvent;

        private List<string> increaseDodgeRateBuffKeys = new List<string>
        {
            "IncreaseDodgeRate"
        };

        [Space]
        [Header("Health Bar View")]
        private TrooperVitalityView allyHealthView;

        [SerializeField]
        private Transform healthBarPosition;

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
				currentHealth = value;
				if (currentHealth <= 0)
				{
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
			set
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
			set
			{
				currentMagicArmor = value;
				currentMagicArmor = Mathf.Clamp(currentMagicArmor, 0f, 0.95f);
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			OriginHealth = base.MinionEntity.Health;
			CurrentHealth = OriginHealth;
			OriginPhysicsArmor = (float)base.MinionEntity.PhysicsArmor / 100f;
			CurrentPhysicsArmor = OriginPhysicsArmor;
			OriginMagicArmor = (float)base.MinionEntity.MagicArmor / 100f;
			CurrentMagicArmor = OriginMagicArmor;
			dodgeChance = base.MinionEntity.GetDodgeChance();
			SetupHealthBar();
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			allyHealthView.OnReturnPool();
		}

		public void SetupHealthBar()
		{
			allyHealthView = MonoSingleton<UnitHealthBarPool>.Instance.Get();
			allyHealthView.SetupHealth(CharacterKind.Ally, base.gameObject, healthBarPosition);
		}

		public override void Update()
		{
			base.Update();
		}

		public void RestoreHealth()
		{
			currentHealth = OriginHealth;
			UpdateHealthView();
		}

		public void AddHealth(int amount)
		{
			CurrentHealth += amount;
			if (CurrentHealth >= OriginHealth)
			{
				CurrentHealth = OriginHealth;
			}
			UpdateHealthView();
		}

		public void ChangeHealth(DamageKind damageType, SharedStrikeDamage damageInfor)
		{
			if (CurrentHealth <= 0)
			{
				return;
			}
			if (damageType == DamageKind.Melee && ShouldIgnoreMeleeDamage())
			{
				return;
			}
			if (damageInfor.physicsDamage > 0)
			{
				int num = damageInfor.physicsDamage;
				if (CurrentPhysicsArmor > 0f)
				{
					num -= (int)((float)damageInfor.physicsDamage * CurrentPhysicsArmor);
					num = Mathf.Clamp(num, 1, 999999);
				}
				if (num > 0)
				{
					CurrentHealth -= num;
					onHealthChangeEvent.Dispatch();
					UpdateHealthView();
				}
			}
			if (damageInfor.magicDamage > 0)
			{
				int num2 = damageInfor.magicDamage;
				if (CurrentMagicArmor > 0f)
				{
					num2 -= (int)((float)damageInfor.magicDamage * CurrentMagicArmor);
					num2 = Mathf.Clamp(num2, 1, 999999);
				}
				if (num2 > 0)
				{
					CurrentHealth -= num2;
					onHealthChangeEvent.Dispatch();
					UpdateHealthView();
				}
			}
		}

		private void UpdateHealthView()
		{
			allyHealthView.UpdateHealth(CurrentHealth, OriginHealth);
		}

		private bool ShouldIgnoreMeleeDamage()
		{
			return dodgeChance > 0 && UnityEngine.Random.Range(0, 100) < dodgeChance;
		}

		private void UpdateOvertimeDamage()
		{
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseDodgeRateBuffKeys.Contains(buffKey))
			{
				ApplyBuffDodge();
			}
		}

		private void ApplyBuffDodge()
		{
			dodgeChance = base.MinionEntity.GetDodgeChance() + (int)base.MinionEntity.BuffsHolder.GetBuffsValue(increaseDodgeRateBuffKeys);
		}
	}
}
