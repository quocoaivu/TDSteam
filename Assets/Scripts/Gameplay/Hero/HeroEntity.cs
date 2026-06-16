using System;
using System.Collections.Generic;
using System.Diagnostics;
using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class HeroEntity : CharacterEntity
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnBeHitEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnSpecialStateEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnHitEnemyEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnAttackEvent;

        [Header("General")]
        [SerializeField]
        private int heroID;

        [SerializeField]
        private GameObject selectedImage;

        private GameObject _selectIndicator;

        private float selectedImageScale;

        private Vector3 selectedImageVector = Vector3.zero;

        private Vector3 PoolPos = new Vector3(1000f, 1000f, 0f);

        private new Collider2D collider2D;

        [Space]
        [Header("Attack type")]
        public EnemyData currentTarget;

        private string specialStateAnimationName;

        private float specialStateDuration;

        private EntityFsmController heroFsmController;

        [Header("Required components")]
        [SerializeField]
        private HeroTravelHandler heroMovementController;

        [SerializeField]
        private HeroStrikeHandler heroAttackController;

        [SerializeField]
        private HeroVitalityHandler heroHealthController;

        [SerializeField]
        private HeroMotionHandler heroAnimationController;

        [SerializeField]
        private HeroAbilityHandler heroSkillController;

        [SerializeField]
        private TrooperSfxHandler unitSoundController;

        [SerializeField]
        [HideInInspector]
        private List<HeroHandler> controllers;

        private HeroSpec originalParameter;

        private PetSetupRecord petConfigData;

        private HeroEntity petOwner;

        [HideInInspector]
        public HeroEntity _pet;
        
		private GameObject selectIndicator
		{
			get
			{
				if (_selectIndicator == null)
				{
					_selectIndicator = UnityEngine.Object.Instantiate(Common.AssetLoader.Load<GameObject>("FXs/SelectIndicator"), base.transform);
					_selectIndicator.transform.localPosition = Vector3.zero;
				}
				return _selectIndicator;
			}
		}

		public HeroTravelHandler HeroMovementController
		{
			get
			{
				return heroMovementController;
			}
			private set
			{
				heroMovementController = value;
			}
		}

		public HeroStrikeHandler HeroAttackController
		{
			get
			{
				return heroAttackController;
			}
			private set
			{
				heroAttackController = value;
			}
		}

		public HeroVitalityHandler HeroHealthController
		{
			get
			{
				return heroHealthController;
			}
			private set
			{
				heroHealthController = value;
			}
		}

		public HeroMotionHandler HeroAnimationController
		{
			get
			{
				return heroAnimationController;
			}
			private set
			{
				heroAnimationController = value;
			}
		}

		public HeroAbilityHandler HeroSkillController
		{
			get
			{
				return heroSkillController;
			}
			private set
			{
				heroSkillController = value;
			}
		}

		public TrooperSfxHandler UnitSoundController
		{
			get
			{
				return unitSoundController;
			}
			set
			{
				unitSoundController = value;
			}
		}

		public int HeroID
		{
			get
			{
				return heroID;
			}
		}

		public HeroSpec OriginalParameter
		{
			get
			{
				return originalParameter;
			}
			private set
			{
				originalParameter = value;
			}
		}

		public bool MeleeHero
		{
			get
			{
				return OriginalParameter.attack_range_max < 100;
			}
		}

		public bool RangeHero
		{
			get
			{
				return OriginalParameter.attack_range_max > 100;
			}
		}

		public bool IsPet
		{
			get
			{
				return HeroID >= 1000;
			}
		}

		public PetSetupRecord PetConfigData
		{
			get
			{
				return petConfigData;
			}
			set
			{
				petConfigData = value;
			}
		}

		public HeroEntity PetOwner
		{
			get
			{
				return petOwner;
			}
			set
			{
				petOwner = value;
				value._pet = this;
			}
		}

		public HeroEntity GetPet()
		{
			return _pet;
		}

		private void Awake()
		{
			GetAllComponents();
			GetControllers();
			InitializeControllers();
		}

		private void Start()
		{
			OnAppear();
			if (IsPet)
			{
				heroFsmController = new PetFsmController(this);
			}
			else
			{
				heroFsmController = new HeroFsmHandler(this);
			}
			HerosDirector.Instance.onChooseHero += Instance_onChooseHero;
			HerosDirector.Instance.onUnChooseHero += Instance_onUnChooseHero;
		}

		private void OnDestroy()
		{
			if (HerosDirector.Instance != null)
			{
				HerosDirector.Instance.onChooseHero -= Instance_onChooseHero;
				HerosDirector.Instance.onUnChooseHero -= Instance_onUnChooseHero;
			}
		}

		private void Instance_onChooseHero(int currentHeroID)
		{
			if (HeroID == currentHeroID)
			{
				ChooseHero();
				UnitSoundController.PlaySelect();
			}
			else
			{
				UnChooseHero();
			}
		}

		private void Instance_onUnChooseHero(int currentHeroID)
		{
			if (HeroID == currentHeroID)
			{
				UnChooseHero();
			}
		}

		private void OnAppear()
		{
			if (!IsPet)
			{
				GameplayDirector.Instance.heroesManager.AddToList(HeroID, this);
			}
			ReloadData();
			if (!IsPet || GameKit.IsPetHavingAtkState(this))
			{
				MonoSingleton<GameRecord>.Instance.ListActiveAlly.Add(this);
			}
			base.BuffsHolder.ResetBuffs();
			if (HavePet())
			{
				SummonPet();
			}
		}

		private bool HavePet()
		{
			return !IsPet && GameKit.IsUltimateHero(HeroID);
		}

		private void SummonPet()
		{
			int num = heroID + 1000;
			Common.GameObjectPool.InitPool("Pet/pet_" + num, 0);
			GameObject gameObject = Common.GameObjectPool.Spawn(string.Format("pet_{0}(Clone)", num), default(Vector3), default(Quaternion));
			if (gameObject == null)
			{
				UnityEngine.Debug.LogWarning("[HeroEntity] SummonPet bá» qua: khÃ´ng spawn Ä‘Æ°á»£c pet_" + num + ". Pet prefab cÃ³ thá»ƒ chÆ°a mark Addressable.");
				return;
			}
			HeroEntity component = gameObject.GetComponent<HeroEntity>();
			component.PetOwner = this;
			component.transform.position = base.transform.position + new Vector3(0f, -0.3f, 0f);
			component.gameObject.SetActive(true);
		}

		private void ReloadData()
		{
			SetParameter();
			IsAlive = true;
			TurnOnCollider();
			UnChooseHero();
			foreach (HeroHandler heroController in controllers)
			{
				heroController.OnAppear();
			}
			if (!IsPet)
			{
				MonoSingleton<GameplayUIHeroDirector>.Instance.InitListHeroesSelected(HeroID);
			}
		}

		private void GetAllComponents()
		{
			collider2D = base.GetComponent<Collider2D>();
		}

		private void GetControllers()
		{
			if (controllers == null || controllers.Count == 0)
			{
				controllers = new List<HeroHandler>(base.GetComponentsInChildren<HeroHandler>(true));
			}
		}

		private void InitializeControllers()
		{
			for (int i = 0; i < controllers.Count; i++)
			{
				HeroHandler heroController = controllers[i];
				heroController.HeroModel = this;
				heroController.Initialize();
			}
		}

		private void SetParameter()
		{
			if (IsPet)
			{
				PetConfigData = ConfigRegistry.Instance.petConfig.dataArray[HeroID % 1000];
				OriginalParameter = HeroParameterManager.Instance.GetPetParameter(PetConfigData);
			}
			else
			{
				int currentHeroLevel = HeroStore.Instance.GetCurrentHeroLevel(HeroID);
				OriginalParameter = HeroParameterManager.Instance.GetHeroParameter(HeroID, currentHeroLevel);
			}
			if (!IsPet)
			{
				HeroSkillController.InitHeroSkills();
			}
			else
			{
				HeroSkillController.InitPetSkills();
			}
		}

		private void ChooseHero()
		{
			selectedImageScale = heroAttackController.CurrentAttackRangeMax * GameRecord.PIXEL_PER_UNIT * (GameRecord.PIXEL_PER_UNIT / 100f) * 0.01f;
			selectedImageVector.Set(selectedImageScale, selectedImageScale, selectedImageScale);
			selectedImage.SetActive(true);
			selectedImage.transform.localScale = selectedImageVector;
			selectIndicator.SetActive(true);
		}

		private void UnChooseHero()
		{
			selectedImage.SetActive(false);
			selectIndicator.SetActive(false);
		}

		private void TurnOnCollider()
		{
			if (IsPet)
			{
				return;
			}
			collider2D.enabled = true;
		}

		private void TurnOffCollider()
		{
			if (IsPet)
			{
				return;
			}
			collider2D.enabled = false;
		}

		public override void ProcessDamage(DamageKind damageType, SharedStrikeDamage commonAttackDamage)
		{
			ChangeHealth(commonAttackDamage.physicsDamage, commonAttackDamage.magicDamage, commonAttackDamage.criticalStrikeChance);
		}

		public override void RestoreHealth()
		{
			base.RestoreHealth();
			if (!IsAlive)
			{
				Resurge();
			}
			else
			{
				HeroHealthController.RestoreHealth();
			}
		}

		public override void IncreaseHealth(int hpAmount)
		{
			base.IncreaseHealth(hpAmount);
			HeroHealthController.AddHealth(hpAmount);
		}

		public override void ChangeHealth(int damagePhysics, int damageMagic, int criticalStrikeChance = 0)
		{
			int num = 0;
			int magicAttackDamage = 0;
			if (damagePhysics > 0)
			{
				num = damagePhysics;
			}
			if (damageMagic > 0)
			{
				magicAttackDamage = damageMagic;
			}
			if (criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < criticalStrikeChance)
			{
				num *= 2;
			}
			HeroHealthController.ChangeHealth(num, magicAttackDamage);
		}

		public override void Dead()
		{
			TurnOffCollider();
			IsAlive = false;
			GetFsmController().GetCurrentState().OnInput(PhaseInputKind.Die, new object[0]);
			IsSpecialState = false;
			foreach (HeroHandler heroController in controllers)
			{
				heroController.OnDead();
			}
			MonoSingleton<GameplayUIHeroDirector>.Instance.DisableHeroesUI(HeroID);
			base.CustomInvoke(new Action(Resurge), (float)originalParameter.respawn_time / 1000f);
			UnitSoundController.PlayDie();
		}

		private void Resurge()
		{
			base.CancelInvoke("Resurge");
			GetFsmController().GetCurrentState().OnInput(PhaseInputKind.Resurge, new object[0]);
			ReloadData();
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_HEAL_0);
			effect.transform.position = base.transform.position;
			effect.Init(1f, base.transform);
			UnitSoundController.PlayRespawn();
			HeroHealthController.UpdateHealthView();
		}

		public override void ReturnPool(float delayTime)
		{
			IsAlive = false;
			IsSpecialState = false;
			MonoSingleton<GameRecord>.Instance.ListActiveAlly.Remove(this);
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnReturnPool();
			}
			MonoSingleton<AllyPool>.Instance.Despawn(this);
		}

		public void AddExp(int amountEXP)
		{
			HeroStore.Instance.AddExp(HeroID, amountEXP);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Hero ",
				HeroID,
				" add exp ",
				amountEXP
			}));
		}

		public void OnHitEnemy()
		{
			if (OnHitEnemyEvent != null)
			{
				OnHitEnemyEvent();
			}
		}

		public void OnSpecialState()
		{
			if (OnSpecialStateEvent != null)
			{
				OnSpecialStateEvent();
			}
		}

		public void OnBeHit()
		{
			if (OnBeHitEvent != null)
			{
				OnBeHitEvent();
			}
		}

		public void OnAttack()
		{
			if (OnAttackEvent != null)
			{
				OnAttackEvent();
			}
		}

		public override bool IsRanger()
		{
			return RangeHero;
		}

		public override void AddTarget(EnemyData enemy)
		{
			currentTarget = enemy;
		}

		public override EnemyData GetCurrentTarget()
		{
			return currentTarget;
		}

		public override bool CanAttackAirEnemy()
		{
			if (IsPet)
			{
				return petConfigData.Can_attack_air > 0;
			}
			return HeroParameterManager.Instance.CanAttackAir(HeroID);
		}

		public override float GetRangerRange()
		{
			return heroAttackController.CurrentAttackRangeMax;
		}

		public override float GetMeleeRange()
		{
			return heroAttackController.AttackRangeAverage;
		}

		public override float GetAttackRangeMin()
		{
			return heroAttackController.AttackRangeMin;
		}

		public override float GetSpeed()
		{
			return HeroMovementController.GetSpeed();
		}

		public override IMotionHandler GetAnimationController()
		{
			return HeroAnimationController;
		}

		public override void DoRangeAttack()
		{
			OnAttack();
			HeroAttackController.PrepareToRangeAttack();
		}

		public override void DoMeleeAttack()
		{
			OnAttack();
			HeroAttackController.PrepareToMeleeAttack();
		}

		public override float GetAtkCooldownDuration()
		{
			return HeroAttackController.CooldownTime;
		}

		public override Vector3 GetAssignedPosition()
		{
			return HeroMovementController.assignedPosition;
		}

		public override void SetAssignedPosition(Vector3 assignedPos)
		{
			HeroMovementController.assignedPosition = assignedPos;
		}

		public override float GetDieDuration()
		{
			return (float)originalParameter.respawn_time / 1000f;
		}

		public override float GetSpecialStateDuration()
		{
			return specialStateDuration;
		}

		public override void SetSpecialStateDuration(float duration)
		{
			specialStateDuration = duration;
		}

		public override void SetSpecialStateAnimationName(string animationName)
		{
			specialStateAnimationName = animationName;
		}

		public override string GetSpecialStateAnimationName()
		{
			return specialStateAnimationName;
		}

		public override EntityFsmController GetFsmController()
		{
			return heroFsmController;
		}

		public override int GetCurHp()
		{
			return HeroHealthController.CurrentHealth;
		}

		public override int GetMaxHp()
		{
			return HeroHealthController.OriginHealth;
		}
	}
}
