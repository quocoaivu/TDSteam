using System;
using System.Collections.Generic;
using DG.Tweening;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class MinionTravelHandler : MinionHandler
	{
        public Vector3 assignedPosition;

        private Vector3 PoolPos = new Vector3(1000f, 1000f, 0f);

        private float moveSpeed;

        private TurretSummonMinionHandler towerSpawnAllyController;

        private TurretSpec originalParameter;

        private List<string> increaseMovementSpeedBuffKeys = new List<string>
        {
            "IncreaseMovementSpeed"
        };


        public float MoveSpeed
		{
			get
			{
				return moveSpeed;
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
			base.MinionEntity.transform.DOKill(false);
			if (base.MinionEntity.controlledAlly)
			{
				towerSpawnAllyController = base.MinionEntity.TowerSpawnAllyController;
				originalParameter = base.MinionEntity.TowerSpawnAllyController.TowerModel.OriginalParameter;
			}
			base.MinionEntity.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void SetParameter()
		{
			moveSpeed = base.MinionEntity.MoveSpeed;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			base.MinionEntity.transform.DOKill(false);
		}

		public void MoveToReadyPos(Vector3 end)
		{
			assignedPosition = end;
			base.MinionEntity.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.UserAssignPosition, new object[]
			{
				end
			});
		}

		public void MoveToReadyPosImmediately(Vector3 end, float time)
		{
			assignedPosition = end;
			base.MinionEntity.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.UserAssignPosition, new object[]
			{
				end
			});
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseMovementSpeedBuffKeys.Contains(buffKey))
			{
				ApplyIncreaseMovementSpeed();
			}
		}

		private void ApplyIncreaseMovementSpeed()
		{
			float buffsValue = base.MinionEntity.BuffsHolder.GetBuffsValue(increaseMovementSpeedBuffKeys);
			moveSpeed = base.MinionEntity.MoveSpeed + (float)((int)(base.MinionEntity.MoveSpeed * buffsValue / 100f));
		}
	}
}
