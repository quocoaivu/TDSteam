using System;
using System.Collections.Generic;
using DG.Tweening;
using Parameter;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class TurretSummonMinionHandler : TurretHandler
	{
		public MinionEntity[] ListAllyControl
		{
			get
			{
				return listAllyControl;
			}
			set
			{
				listAllyControl = value;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			MonoSingleton<AllyPool>.Instance.PushAlliesToPool(base.TowerModel.Id, base.TowerModel.Level, 2);
			timeTracking = (float)base.TowerModel.OriginalParameter.reload / 1000f;
			ignoreReloadChance = base.TowerModel.OriginalParameter.ignoreReloadChance;
			base.TowerModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			base.TowerModel.IsSilent = (base.TowerModel.BuffsHolder.GetBuffsValue(silentBuffKeys) > 0f);
			if (base.TowerModel.IsSilent)
			{
				ApplyBuffSilent();
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			Array.Clear(ListAllyControl, 0, ListAllyControl.Length);
			startChecking = false;
			base.CustomInvoke(new Action(setStartChecking), 1f);
			readyPosition = MonoSingleton<ConstructSectorDirector>.Instance.listRegions[base.TowerModel.RegionID].spawnAllyPosition;
			if (base.TowerModel.Level == 0)
			{
				CalculateReadyPosition(readyPosition.position);
			}
			else if (currentTargetPosition.Equals(Vector2.zero))
			{
				UnityEngine.Debug.Log(" vá»‹ trÃ­ chÆ°a Ä‘c chá»‰ Ä‘á»‹nh");
				CalculateReadyPosition(readyPosition.position);
			}
			else
			{
				UnityEngine.Debug.Log(" vá»‹ trÃ­ Ä‘Ã£ Ä‘c chá»‰ Ä‘á»‹nh");
				CalculateReadyPosition(currentTargetPosition);
			}
			base.CustomInvoke(new Action(SpawnAllAllies), 0.2f);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			Array.Clear(ListAllyControl, 0, ListAllyControl.Length);
			startChecking = false;
			currentTargetPosition = Vector2.zero;
		}

		private void TowerModel_OnUpgrade(TurretEntity oldTower, TurretEntity newTower)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"tower upgrade from ",
				oldTower.Level,
				" to ",
				newTower.Level
			}));
			newTower.towerSpawnAllyController.currentTargetPosition = currentTargetPosition;
			base.CancelInvoke("SpawnAllAllies");
			base.CancelInvoke("SpawnAllies");
			if (ListAllyControl.Length > 0)
			{
				foreach (MinionEntity allyModel in ListAllyControl)
				{
					if (allyModel != null)
					{
						VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_UPGRADE_TOWER_ON_ALLY);
						effect.transform.position = allyModel.transform.position;
						effect.Init(1f);
						allyModel.ReturnPool(0f);
					}
				}
			}
		}

		private void TowerModel_OnSell(TurretEntity towerModel)
		{
			base.CancelInvoke("SpawnAllAllies");
			base.CancelInvoke("SpawnAllies");
			if (ListAllyControl.Length > 0)
			{
				foreach (MinionEntity allyModel in ListAllyControl)
				{
					if (allyModel != null)
					{
						VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.EFFECT_SELL_TOWER_ON_ALLY);
						effect.transform.position = allyModel.transform.position;
						effect.Init(0.5f);
						allyModel.ReturnPool(0f);
					}
				}
			}
		}

		private void Awake()
		{
			effectCaster = base.GetComponent<VisualEffectSpawner>();
			originalParameter = base.TowerModel.OriginalParameter;
			base.TowerModel.OnSell += TowerModel_OnSell;
			base.TowerModel.OnUpgrade += TowerModel_OnUpgrade;
		}

		private void Start()
		{
			MonoSingleton<MinionsDirector>.Instance.onAlliesMoveToAssignedPosition += Instance_onAlliesMoveToAssignedPosition;
		}

		private void OnDestroy()
		{
			base.TowerModel.OnSell -= TowerModel_OnSell;
			base.TowerModel.OnUpgrade -= TowerModel_OnUpgrade;
			MinionsDirector am = MonoSingleton<MinionsDirector>.InstanceIfExists;
			if (am != null)
			{
				am.onAlliesMoveToAssignedPosition -= Instance_onAlliesMoveToAssignedPosition;
			}
		}

		public override void Update()
		{
			base.Update();
			if (!startChecking)
			{
				return;
			}
			if (base.TowerModel.IsSilent)
			{
				return;
			}
			if (timeTracking == 0f)
			{
				CheckNumberOfAllies();
			}
			if (GetNumberOfLivingAllies() < desiredAlliesNumber)
			{
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		private void Instance_onAlliesMoveToAssignedPosition(TurretEntity inputTowerModel, Vector2 targetPosition)
		{
			if (inputTowerModel.GetEntityId() == base.TowerModel.GetEntityId())
			{
				float num = Vector2.Distance(targetPosition, base.transform.position);
				if (num < (float)base.TowerModel.OriginalParameter.attackRangeMax / GameRecord.PIXEL_PER_UNIT)
				{
					SetAlliesToAssignedPosition(targetPosition);
					if (effectCaster)
					{
						effectCaster.CastEffect(FXPool.ICON_MOVEABLE_AllY, 2f, targetPosition);
					}
				}
				else if (effectCaster)
				{
					effectCaster.CastEffect(FXPool.ICON_UNMOVEABLE, 1f, targetPosition);
				}
			}
		}

		private void setStartChecking()
		{
			startChecking = true;
		}

		private void CheckNumberOfAllies()
		{
			if (GetNumberOfLivingAllies() < desiredAlliesNumber)
			{
				SpawnAllies(desiredAlliesNumber - GetNumberOfLivingAllies());
			}
			timeTracking = (float)base.TowerModel.OriginalParameter.reload / 1000f;
		}

		private int GetNumberOfLivingAllies()
		{
			int num = 0;
			for (int i = 0; i < ListAllyControl.Length; i++)
			{
				if (ListAllyControl[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		private void SpawnAllies(int amount)
		{
			DoSpawn(amount);
			MoveAllyToReadyPosition(amount);
		}

		private void SpawnAllAllies()
		{
			DoSpawn(desiredAlliesNumber);
			MoveAllyToReadyPositionImmediately(desiredAlliesNumber);
		}

		private void DoSpawn(int amount)
		{
			if (door != null)
			{
				door.transform.localPosition = Vector3.zero;
				Sequence sequence = DOTween.Sequence();
				sequence.Append(door.transform.DOLocalMoveY(0.11f, 0.2f, false));
				sequence.AppendInterval(1f);
				sequence.Append(door.transform.DOLocalMoveY(0f, 0.6f, false));
				sequence.SetLink(door);
				sequence.Play<Sequence>();
			}
			for (int i = 0; i < amount; i++)
			{
				MinionEntity allyModel = MonoSingleton<AllyPool>.Instance.GetAlly(base.TowerModel.Id, base.TowerModel.Level);
				allyModel.InitFromTower(originalParameter, this);
				allyModel.transform.position = spawnPosition.position;
				allyModel.gameObject.SetActive(true);
				AddAllyToListControl(allyModel);
			}
			OnSpawnAllies.Invoke();
		}

		private void AddAllyToListControl(MinionEntity ally)
		{
			int freePosition = GetFreePositionInListControl();
			// List already full: GetFreePositionInListControl returns -1. Bail out
			// so we don't index [-1] and overwrite/throw.
			if (freePosition < 0)
			{
				return;
			}
			ListAllyControl[freePosition] = ally;
		}

		public void RemoveAllyFromListControl(MinionEntity ally)
		{
			int allyIndexOfList = GetAllyIndexOfList(ally);
			// Ally already removed (e.g. a second Dead() before ReturnPool nulls the
			// controller): IndexOf returns -1. Bail out so we don't index [-1].
			// Mirrors the guard in GameRecord.RemoveEnemyFromListActiveEnemy.
			if (allyIndexOfList < 0)
			{
				return;
			}
			startPositionParameters[allyIndexOfList].isUsing = false;
			ListAllyControl[allyIndexOfList] = null;
			CalculateIgnoreReloadChance();
		}

		public int GetAllyIndexOfList(MinionEntity ally)
		{
			return Array.IndexOf<MinionEntity>(ListAllyControl, ally);
		}

		private int GetFreePositionInListPosition()
		{
			int result = -1;
			for (int i = 0; i < startPositionParameters.Length; i++)
			{
				if (!startPositionParameters[i].isUsing)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private int GetFreePositionInListControl()
		{
			int result = -1;
			for (int i = 0; i < ListAllyControl.Length; i++)
			{
				if (ListAllyControl[i] == null)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private void MoveAllyToReadyPosition(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				int freePositionInListPosition = GetFreePositionInListPosition();
				// No free start position left: GetFreePositionInListPosition returns -1.
				// Stop assigning so we don't index [-1].
				if (freePositionInListPosition < 0)
				{
					break;
				}
				ListAllyControl[freePositionInListPosition].MinionTravelHandler.MoveToReadyPos(startPositionParameters[freePositionInListPosition].position);
				startPositionParameters[freePositionInListPosition].isUsing = true;
			}
		}

		private void MoveAllyToReadyPositionImmediately(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				ListAllyControl[i].MinionTravelHandler.MoveToReadyPosImmediately(startPositionParameters[i].position, 0.75f);
				startPositionParameters[i].isUsing = true;
			}
		}

		private void CalculateReadyPosition(Vector2 readyPos)
		{
			Vector2 vector = readyPos;
			Vector3 position = new Vector3(vector.x, vector.y + offsetSpawnPosition.y, 0f);
			Vector3 position2 = new Vector3(vector.x + offsetSpawnPosition.x, vector.y - offsetSpawnPosition.y, 0f);
			Vector3 position3 = new Vector3(vector.x - offsetSpawnPosition.x, vector.y - offsetSpawnPosition.y, 0f);
			startPositionParameters[0].position = position;
			startPositionParameters[1].position = position2;
			startPositionParameters[2].position = position3;
		}

		public void SetAlliesToAssignedPosition(Vector2 targetPosition)
		{
			currentTargetPosition = targetPosition;
			CalculateReadyPosition(targetPosition);
			for (int i = 0; i < ListAllyControl.Length; i++)
			{
				if (ListAllyControl[i] != null)
				{
					ListAllyControl[i].MinionTravelHandler.MoveToReadyPos(startPositionParameters[i].position);
					startPositionParameters[i].isUsing = true;
				}
			}
		}

		private void ApplyBuffSilent()
		{
		}

		public void AddPassiveArmor(float bonusPhysicsArmor, float bonusMagicArmor)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"add ",
				bonusPhysicsArmor,
				" ",
				bonusMagicArmor,
				" to all allies!"
			}));
			foreach (MinionEntity allyModel in ListAllyControl)
			{
				if (allyModel)
				{
					allyModel.MinionVitalityHandler.CurrentPhysicsArmor = allyModel.MinionVitalityHandler.OriginPhysicsArmor + bonusPhysicsArmor;
					allyModel.MinionVitalityHandler.CurrentMagicArmor = allyModel.MinionVitalityHandler.OriginMagicArmor + bonusMagicArmor;
				}
			}
		}

		private void CalculateIgnoreReloadChance()
		{
			if (ignoreReloadChance > 0 && UnityEngine.Random.Range(0, 100) < ignoreReloadChance)
			{
				UnityEngine.Debug.Log("bo qua reload!");
				timeTracking = 0f;
			}
		}

		public void UnlockRangeAttackAbility(float attackRangeBonus)
		{
			UnityEngine.Debug.Log("add " + attackRangeBonus + " attack range to all allies!");
			foreach (MinionEntity allyModel in ListAllyControl)
			{
				if (allyModel)
				{
					allyModel.MinionStrikeHandler.rangeAttack = true;
					allyModel.CurrentAttackRangeMax = allyModel.AttackRangeMax + attackRangeBonus;
				}
			}
		}

		public void UnlockDodgeAbility(int dodgeRateBonus)
		{
			foreach (MinionEntity allyModel in ListAllyControl)
			{
				if (allyModel)
				{
					allyModel.CurrentDodgeChance = allyModel.DodgeChance + dodgeRateBonus;
				}
			}
		}

		public void UnlockIgnoreArmorAbility(int ignoreArmorRateBonus)
		{
			foreach (MinionEntity allyModel in ListAllyControl)
			{
				if (allyModel)
				{
					allyModel.CurrentIgnoreArmorChance = allyModel.IgnoreArmorChance + ignoreArmorRateBonus;
				}
			}
		}

		public void UnlockSkillSlash(float duration, float cooldown, string description, bool isActiveAtStart)
		{
			foreach (MinionEntity allyModel in ListAllyControl)
			{
				if (allyModel)
				{
					MinionAbilitySlash componentInChildren = allyModel.GetComponentInChildren<MinionAbilitySlash>();
					if (componentInChildren)
					{
						componentInChildren.Init(duration, cooldown, description, isActiveAtStart);
					}
				}
			}
		}

		public UnityEvent OnSpawnAllies;

		private List<string> silentBuffKeys = new List<string>
		{
			"Silent"
		};

		[SerializeField]
		private int desiredAlliesNumber;

		[SerializeField]
		private Vector3 offsetSpawnPosition;

		[SerializeField]
		private Transform spawnPosition;

		public GameObject door;

		[Header("Movement parameter")]
		[SerializeField]
		private LayerMask avaiableMovingLayerMask;

		private Transform readyPosition;

		private Transform targetPosition;

		private float timeTracking;

		private bool startChecking;

		private int ignoreReloadChance;

		private MinionEntity[] listAllyControl = new MinionEntity[3];

		[NonSerialized]
		public Vector2 currentTargetPosition = new Vector2(0f, 0f);

		private BeginPositionSpec[] startPositionParameters = new BeginPositionSpec[3];

		private TurretSpec originalParameter;

		private VisualEffectSpawner effectCaster;
	}
}
