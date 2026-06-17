using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeneralVariable;
using MetaGame;
using GameCore;
using Parameter;
using Common;
using UnityEngine;

namespace Gameplay
{

	public class EnemyData : BaseMonoBehaviour
	{
        public OrderedUnityEvent OnAppearEvent;

        public EntityPhaseEnum curState;

        public Vector3 preMovePos;

        private EnemyParameter enemy;

        [Header("Basic Parameter")]
        [SerializeField]
        private bool canAttack;

        [HideInInspector]
        public int moveLine;

        [HideInInspector]
        public CreepPathRecord monsterPathData;

        public float startPosRatio;

        [Header("Required components")]
        [SerializeField]
        private EnemyHealth healthController;

        [SerializeField]
        private EnemyMovement movementController;

        [SerializeField]
        private EnemyVfxHandler effectController;

        [SerializeField]
        private EnemyTargetAcquisition enemyFindTargetController;

        [SerializeField]
        private EnemyAnimation enemyAnimationController;

        [SerializeField]
        private EnemyAudio enemySoundController;

        [SerializeField]
        private BuffHolder buffsHolder;

        public EnemyCombat enemyAttackController;

        private EnemyStateMachine _enemyFsmController;

        private Vector3 cachedPosition;

        private int level;

        private int id;

        private int gate;

        [SerializeField]
        [HideInInspector]
        private List<EnemyBrain> controllers;

        private new Collider2D collider2D;

        private string specialStateAnimationName;

        private float specialStateDuration;

        private int subscriberId;

        private GameObject selectIndicator;

        private float timeTrackingUpdateCachePos = 0.3f;

        private bool updatePhase = true;

        private bool isPositiveForEnemy;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event Action OnHitAllyEvent;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnEnemyDied;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> OnStartRun;

		public bool IsAir { get; set; }

		public bool IsAlive { get; set; }

		public bool CanAttack
		{
			get
			{
				return canAttack;
			}
			set
			{
				canAttack = value;
			}
		}

		public bool IsUnderground { get; set; }

		public bool IsInTunnel { get; set; }

		public bool IsAttacking { get; set; }

		public bool IsSpecialAttacking { get; set; }

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

		public int Gate
		{
			get
			{
				return gate;
			}
			private set
			{
				gate = value;
			}
		}

		public EnemyParameter OriginalParameter
		{
			get
			{
				return enemy;
			}
			private set
			{
				enemy = value;
			}
		}

		public float GetDieDuration()
		{
			return 1f;
		}

		public EnemyMovement EnemyMovementController
		{
			get
			{
				return movementController;
			}
			private set
			{
				movementController = value;
			}
		}

		public EnemyHealth EnemyHealthController
		{
			get
			{
				return healthController;
			}
			private set
			{
				healthController = value;
			}
		}

		public EnemyVfxHandler EnemyEffectController
		{
			get
			{
				return effectController;
			}
			private set
			{
				effectController = value;
			}
		}

		public EnemyTargetAcquisition EnemyFindTargetController
		{
			get
			{
				return enemyFindTargetController;
			}
			private set
			{
				enemyFindTargetController = value;
			}
		}

		public EnemyAnimation EnemyAnimationController
		{
			get
			{
				return enemyAnimationController;
			}
			set
			{
				enemyAnimationController = value;
			}
		}

		public EnemyAudio EnemySoundController
		{
			get
			{
				return enemySoundController;
			}
			set
			{
				enemySoundController = value;
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

		public EnemyStateMachine enemyFsmController
		{
			get
			{
				if (IsAlive && _enemyFsmController == null)
				{
					_enemyFsmController = new EnemyStateMachine(this);
				}
				return _enemyFsmController;
			}
		}

		public Vector3 CachedPosition
		{
			get
			{
				return cachedPosition;
			}
		}

		public void Awake()
		{
			GetAllComponents();
			GetControllers();
			InitializeControllers();
			base.gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f);
		}

		private void Update()
		{
			// Perf: tick the enemy FSM only every other frame (updatePhase toggles each frame).
			// dt is doubled (Time.deltaTime * 2f) so movement and timers stay real-time on
			// average across the two frames. This is intentional and tuned â€” do not "fix" the
			// doubling or remove the every-other-frame gating without re-tuning enemy balance.
			if (updatePhase)
			{
				Vector3 a = base.transform.position - CachedPosition;
				preMovePos = base.transform.position + 5f * ((float)OriginalParameter.speed * GameRecord.REVERSE_PIXEL_PER_UNIT) * a;
				if (timeTrackingUpdateCachePos == 0f)
				{
					UpdateCachedPosition();
				}
				timeTrackingUpdateCachePos = Mathf.MoveTowards(timeTrackingUpdateCachePos, 0f, Time.deltaTime * 2f);
				if (IsAlive && !MonoSingleton<GameRecord>.Instance.IsGameOver && enemyFsmController != null)
				{
					enemyFsmController.OnUpdate(Time.deltaTime * 2f);
					if (IsAlive)
					{
						curState = enemyFsmController.GetCurrentState().entityStateEnum;
					}
				}
			}
			updatePhase = !updatePhase;
		}

		private void GetAllComponents()
		{
			collider2D = base.GetComponent<Collider2D>();
		}

		public EntityFsmController GetFsmController()
		{
			return enemyFsmController;
		}

		private void UpdateCachedPosition()
		{
			cachedPosition = base.transform.position;
			timeTrackingUpdateCachePos = 0.3f;
		}

		private void TurnOnCollider()
		{
			collider2D.enabled = true;
		}

		private void TurnOffCollider()
		{
			collider2D.enabled = false;
		}

		private void GetControllers()
		{
			if (controllers == null || controllers.Count == 0)
			{
				controllers = new List<EnemyBrain>(base.GetComponentsInChildren<EnemyBrain>(true));
			}
		}

		private void InitializeControllers()
		{
			for (int i = 0; i < controllers.Count; i++)
			{
				EnemyBrain enemyController = controllers[i];
				enemyController.EnemyModel = this;
				enemyController.Initialize();
			}
		}

		public bool IsValidTarget(CharacterEntity hero)
		{
			return !(hero == null) && hero.IsAlive;
		}

		private void SetParameter(int id, int level)
		{
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						int loopAmount = GameplayDirector.Instance.endlessModeManager.LoopAmount;
						OriginalParameter = EnemyDatabase.Instance.GetEnemyParameterForEndlessMode(id, level, loopAmount);
					}
				}
				else
				{
					OriginalParameter = EnemyDatabase.Instance.GetEnemyParameter(id, level);
				}
			}
			else
			{
				OriginalParameter = EnemyDatabase.Instance.GetEnemyParameter(id, level);
			}
			IsAlive = true;
			IsAir = OriginalParameter.isAir;
			IsUnderground = OriginalParameter.isUnderGround;
			IsInTunnel = false;
		}

		public void SetDataStartRun(int id, int level, int gate, int line, float startPosRatio = 0f, int customLifeCount = -1)
		{
			Level = level;
			Id = id;
			Gate = gate;
			moveLine = line;
			this.startPosRatio = startPosRatio;
			SetParameter(id, level);
			if (customLifeCount >= 0)
			{
				MonoSingleton<GameRecord>.Instance.TotalEnemy += customLifeCount;
			}
			else
			{
				MonoSingleton<GameRecord>.Instance.TotalEnemy += OriginalParameter.lifeCount;
			}
			TurnOnCollider();
			buffsHolder.ResetBuffs();
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnAppear();
			}
			OnAppearEvent.Dispatch();
			if (OnStartRun != null)
			{
				OnStartRun(line);
			}
			UpdateCachedPosition();
			if (OriginalParameter.isBoss)
			{
				UnityEngine.Debug.Log("Animation when boss appear!");
				MonoSingleton<LensHandler>.Instance.PinchZoomFov.MoveAndZoomToPosition(base.transform.position, 3f, 3f);

				// TODO: lÃ m láº¡i hiá»‡u á»©ng cinematic khi boss xuáº¥t hiá»‡n (DOTween)

				GameObject gameObject = GameObject.FindGameObjectWithTag(GeneralVariable.GeneralDefine.ANIM_BOSS_IN_MAP);
				if (gameObject != null)
				{
					if (OriginalParameter.id == 7)
					{
						UnityEngine.Object.Destroy(gameObject);
					}
					if (OriginalParameter.id == 16)
					{
						gameObject.GetComponentInChildren<Animator>().SetTrigger("Effect");
						UnityEngine.Object.Destroy(gameObject, 2f);
					}
				}
			}
		}

		public void ProcessEffect(string buffKey, int effectValue, float effectDuration, DamageVfxType damageFXtype)
		{
			if (effectValue == 0)
			{
				return;
			}
			BuffsHolder.AddBuff(buffKey, new BuffStatus(isPositiveForEnemy, (float)effectValue, effectDuration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
			EnemyEffectController.PlayDamageFX(damageFXtype, effectDuration);
		}

		public void OnHitAlly()
		{
			if (OnHitAllyEvent != null)
			{
				OnHitAllyEvent();
			}
		}

		public void OnSelected()
		{
			subscriberId = GameKit.GetUniqueId();
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnSelectEnemy, new SelectPersonaListenerRecord(subscriberId, new GameSignalCenter.SelectCharacterMethod(OnSelectEnemyTrigged)));
			selectIndicator = Common.GameObjectPool.Spawn(GeneralVariable.GeneralDefine.SELECT_ENEMY_INDICATOR_NAME, default(Vector3), default(Quaternion));
			selectIndicator.GetComponent<TargetFollower>().Init(base.gameObject, Vector3.zero);
		}

		public void OnSelectEnemyTrigged(int enemyId)
		{
			if (enemyId != OriginalParameter.id)
			{
				UnsubscribeSelectEnemyEvent();
				return;
			}
		}

		private void UnsubscribeSelectEnemyEvent()
		{
			if (selectIndicator != null)
			{
				Common.GameObjectPool.Despawn(selectIndicator);
				selectIndicator = null;
			}
			if (subscriberId >= 0)
			{
				GameSignalCenter.Instance.Unsubscribe(subscriberId, GameSignalKind.OnSelectEnemy);
				subscriberId = -1;
			}
		}

		public void ProcessDamage(DamageKind damageType, SharedStrikeDamage commonAttackDamage, OnHitStatusApplier effectAttack)
		{
			ProcessDamage(damageType, commonAttackDamage);
			if (UnityEngine.Random.Range(0, 100) < effectAttack.debuffChance)
			{
				ProcessEffect(effectAttack.buffKey, effectAttack.debuffEffectValue, effectAttack.debuffEffectDuration, effectAttack.damageFXType);
			}
		}

		public void ProcessDamage(DamageKind damageType, SharedStrikeDamage commonAttackDamage)
		{
			if (commonAttackDamage.sourceId == GameKit.blessedHeroId && FormatDirector.Instance.gameMode == GameFormat.TournamentMode && commonAttackDamage.damageSource == CharacterKind.Hero)
			{
				commonAttackDamage.magicDamage *= 2;
				commonAttackDamage.physicsDamage *= 2;
			}
			else if (GameKit.cachedHavingBooster)
			{
				commonAttackDamage.magicDamage = Mathf.RoundToInt((float)commonAttackDamage.magicDamage * GameKit.cachedBoosterMultiplier * 0.82f);
				commonAttackDamage.physicsDamage = Mathf.RoundToInt((float)commonAttackDamage.physicsDamage * GameKit.cachedBoosterMultiplier * 0.82f);
			}
			if (damageType != DamageKind.Melee)
			{
				if (damageType != DamageKind.Range)
				{
					if (damageType == DamageKind.Magic)
					{
						EnemyHealthController.ChangeHealth(commonAttackDamage);
					}
				}
				else
				{
					EnemyHealthController.ChangeHealth(commonAttackDamage);
				}
			}
			else
			{
				if (UnityEngine.Random.Range(0, 100) < OriginalParameter.change_avoid_melee)
				{
					EnemyEffectController.PlayFXMiss(0.5f);
					return;
				}
				EnemyHealthController.ChangeHealth(commonAttackDamage);
			}
			if (commonAttackDamage.isCrit)
			{
				EnemyEffectController.PlayFXCritical(2f);
			}
		}

		public void Dead()
		{
			EnemySoundController.PlayDead();
			UnsubscribeSelectEnemyEvent();
			GameFormat gameMode = FormatDirector.Instance.gameMode;
			if (gameMode != GameFormat.CampaignMode)
			{
				if (gameMode != GameFormat.DailyTrialMode)
				{
					if (gameMode == GameFormat.TournamentMode)
					{
						MonoSingleton<GameRecord>.Instance.IncreaseMoney(OriginalParameter.value);
					}
				}
			}
			else
			{
				MonoSingleton<GameRecord>.Instance.IncreaseMoney(OriginalParameter.value);
			}
			MonoSingleton<GameRecord>.Instance.TotalExp += OriginalParameter.lifeTaken;
			Items.ItemDropRoller.TryDropOnKill(base.transform.position);
			GameplayDirector.Instance.OnEnemyDie();
			IsAlive = false;
			IsAttacking = false;
			IsSpecialAttacking = false;
			TurnOffCollider();
			if (OnEnemyDied != null)
			{
				OnEnemyDied();
			}
			// Match the lifeCount added on spawn (multi-life enemies count as several).
			if (EnemyDatabase.Instance.IsEnemyHasMoreThanOneLife(Id))
			{
				MonoSingleton<GameRecord>.Instance.TotalEnemy -= OriginalParameter.lifeCount;
			}
			else
			{
				MonoSingleton<GameRecord>.Instance.TotalEnemy--;
			}
		}

		public void ReturnPool(float delayTime)
		{
			IsAlive = false;
			_enemyFsmController = null;
			monsterPathData = null;
			IsAttacking = false;
			IsSpecialAttacking = false;
			UnsubscribeSelectEnemyEvent();
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnReturnPool();
			}
			MonoSingleton<GameRecord>.Instance.RemoveEnemyFromListActiveEnemy(this);
			MonoSingleton<EnemyPool>.Instance.Despawn(this, delayTime);
		}

		public float GetSpecialStateDuration()
		{
			return specialStateDuration;
		}

		public void SetSpecialStateDuration(float duration)
		{
			specialStateDuration = duration;
		}

		public void SetSpecialStateAnimationName(string animationName)
		{
			specialStateAnimationName = animationName;
		}

		public string GetSpecialStateAnimationName()
		{
			return specialStateAnimationName;
		}
	}
}
