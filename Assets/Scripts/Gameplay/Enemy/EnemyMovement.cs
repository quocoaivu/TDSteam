using System;
using System.Collections.Generic;
using DG.Tweening;
using Parameter;
using Common;
using UnityEngine;

namespace Gameplay
{
	public class EnemyMovement : EnemyBrain
	{
        [Space]
        [SerializeField]
        private OrderedUnityEvent onStartRun;

        [SerializeField]
        private OrderedUnityEvent onFinishPath;

        private bool isRunning;

        [SerializeField]
        private bool haveRestOnTheWay;

        [SerializeField]
        private float timeStepToRest;

        [SerializeField]
        private float restTimeMin;

        [SerializeField]
        private float restTimeMax;

        [NonSerialized]
        public int currentLine;

        private Tween tween;

        private float speed;

        private float originSpeed;

        private float timeTrackingRest;

        private int currentWaypoint;

        private float boostedSpeed;

        public float currentTweenPosition;

        private float currentSlowPercentage;


        private float currentStunPercentage;
        public float MinRestDuration
		{
			get
			{
				return restTimeMin / 1000f;
			}
		}

		public float MaxRestDuration
		{
			get
			{
				return restTimeMax / 1000f;
			}
		}

		public bool HaveRestOnTheWay
		{
			get
			{
				return haveRestOnTheWay;
			}
		}

		public float DelayToRest
		{
			get
			{
				return timeStepToRest / 1000f;
			}
		}

		public float Speed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = value;
			}
		}

		public float SpeedMultiplier
		{
			get
			{
				return (Speed + boostedSpeed) / OriginSpeed;
			}
		}

		public float OriginSpeed
		{
			get
			{
				return originSpeed;
			}
			private set
			{
				originSpeed = value;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			base.EnemyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
			GameSignalCenter.Instance.Subscribe(GameSignalKind.OnEnemyMoveToEndPoint, new EnemyEventData(GameKit.GetUniqueId(), new GameSignalCenter.EnemySubscribeMethod(OnMoveToEndPoint)));
		}

		public override void OnAppear()
		{
			base.OnAppear();
			boostedSpeed = 0f;
			Speed = (float)base.EnemyModel.OriginalParameter.speed / GameRecord.PIXEL_PER_UNIT;
			OriginSpeed = Speed;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			base.EnemyModel.transform.DOKill(false);
		}

		private void EnemyFindTargetController_OnTargetRemoved(CharacterEntity obj)
		{
		}

		public float MoveToPosition(float fullPos, float randomPosition)
		{
			return fullPos + UnityEngine.Random.Range(0f, randomPosition);
		}

		public void MoveToPosition(float fullPos, float parentSpeed, bool obsolete)
		{
			base.EnemyModel.startPosRatio = fullPos * (parentSpeed / originSpeed);
		}

		private void OnMoveToEndPoint(EnemyData enemyModel)
		{
			if (base.EnemyModel.GetEntityId() == enemyModel.GetEntityId())
			{
				MonoSingleton<LensHandler>.Instance.ShakeNormal();
				onFinishPath.Dispatch();
				GameplayDirector.Instance.gameLogicController.DecreaseHealth(base.EnemyModel.OriginalParameter.lifeTaken);
				if (EnemyDatabase.Instance.IsEnemyHasMoreThanOneLife(enemyModel.Id))
				{
					MonoSingleton<GameRecord>.Instance.TotalEnemy -= base.EnemyModel.OriginalParameter.lifeCount;
				}
				else
				{
					MonoSingleton<GameRecord>.Instance.TotalEnemy--;
				}
				base.EnemyModel.ReturnPool(0f);
			}
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (slowBuffKeys.Contains(buffKey))
			{
				ApplySlowBuffs();
			}
			if (stunBuffKeys.Contains(buffKey))
			{
				ApplyStunBuffs();
			}
		}

		private void ApplySlowBuffs()
		{
			currentSlowPercentage = base.EnemyModel.BuffsHolder.GetBuffsValue(slowBuffKeys);
			if (currentStunPercentage > 0f)
			{
				return;
			}
			Speed = OriginSpeed * (1f - currentSlowPercentage / 100f);
			Speed = Mathf.Clamp(Speed, 0f, 1f);
		}

		private void ApplyStunBuffs()
		{
			currentStunPercentage = base.EnemyModel.BuffsHolder.GetBuffsValue(stunBuffKeys);
			Speed = OriginSpeed * (1f - currentStunPercentage / 100f);
			Speed = Mathf.Clamp(Speed, 0f, 1f);
		}

		private List<string> slowBuffKeys = new List<string>
		{
			"Slow"
		};

		private List<string> stunBuffKeys = new List<string>
		{
			"Stun"
		};
	}
}
