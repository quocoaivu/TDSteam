using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class HasteUpAuraHandler : BaseMonoBehaviour
	{
		private void Update()
		{
			if (!isReady)
			{
				return;
			}
			if (timeTracking == 0f)
			{
				TryToCastBuff();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			if (isReady)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(base.transform.position, aoeRange);
			}
		}

		public void Init(float _aoeRange, float _activationTime, int _attackSpeedIncreasePercentage)
		{
			aoeRange = _aoeRange;
			activationTime = _activationTime;
			attackSpeedIncreasePercentage = _attackSpeedIncreasePercentage;
			base.CustomInvoke(new Action(GetReady), appearAnimDuration);
			base.CustomInvoke(new Action(EndOfLifeTime), activationTime + appearAnimDuration);
		}

		private void TryToCastBuff()
		{
			UnityEngine.Debug.Log("Try To cast buff speed!");
			GetInRangeTower();
			if (listInRangeTowers.Count > 0)
			{
				foreach (TurretEntity towerModel in listInRangeTowers)
				{
					towerModel.BuffsHolder.AddBuff(buffKey, new BuffStatus(false, (float)attackSpeedIncreasePercentage, trackingDuration), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
					VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.BUFF_SPEED_ON_TOWER);
					effect.transform.position = towerModel.transform.position;
					effect.Init(trackingDuration + Time.deltaTime);
				}
			}
			timeTracking = trackingDuration;
		}

		private void GetInRangeTower()
		{
			MonoSingleton<GameRecord>.Instance.GetInRangeTowers(base.gameObject.transform.position, aoeRange, listInRangeTowers);
		}

		private void GetReady()
		{
			timeTracking = trackingDuration;
			isReady = true;
		}

		private void EndOfLifeTime()
		{
			isReady = false;
			listInRangeTowers.Clear();
			animator.Play("Disappear");
			base.CustomInvoke(new Action(ReturnPool), disAppearAnimDuration);
		}

		private void ReturnPool()
		{
			MonoSingleton<TowerPool>.Instance.Despawn(base.gameObject);
		}

		private float aoeRange;

		private float activationTime;

		private int attackSpeedIncreasePercentage;

		private float timeTracking;

		private float trackingDuration = 1f;

		[SerializeField]
		private float appearAnimDuration;

		[SerializeField]
		private float disAppearAnimDuration;

		private bool isReady;

		[SerializeField]
		private Animator animator;

		private string buffKey = "IncreaseAttackSpeedByPercentage";

		private List<TurretEntity> listInRangeTowers = new List<TurretEntity>();
	}
}
