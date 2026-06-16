using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Gameplay
{
	public class RuleEnemyAbilitySealTowers : EnemyBrain
	{
		public override void OnAppear()
		{
			base.OnAppear();
			timeTracking = ((!activateAtStart) ? (coolDownTime / 1000f) : 0f);
		}

		private void Start()
		{
			waitForAnimation = new WaitForSeconds(attackAnimationDuration);
			MonoSingleton<FXPool>.Instance.InitExtendObject(sealEffectPrefab, 0);
		}

		public override void Update()
		{
			base.Update();
			if (attacking)
			{
				return;
			}
			if (!base.IsEnemyAlive())
			{
				return;
			}
			if (MonoSingleton<GameRecord>.Instance.IsGameOver)
			{
				return;
			}
			if (IsCooldownDone() && base.IsCurrentSpeedGreaterThanMinSpeed())
			{
				GetTowers();
				if (nearestTowers.Count > 0)
				{
					base.StartCoroutine(Attack(nearestTowers));
				}
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, rangeInPixel / GameRecord.PIXEL_PER_UNIT);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator Attack(List<TurretEntity> towers)
		{
			attacking = true;
			onStartAttack.Dispatch();
			if (!base.IsCurrentSpeedGreaterThanMinSpeed())
			{
				yield return null;
			}
			if (!base.IsEnemyAlive())
			{
				yield return null;
			}
			base.EnemyModel.SetSpecialStateDuration(attackAnimationDuration);
			base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimation.animSpecialAttack);
			base.EnemyModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				EnemyAnimation.animSpecialAttack
			});
			base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
			yield return new WaitForSeconds(attackAnimationDuration / 2f);
			SSRTrace.Log(string.Format("Logic Boss1, attack {0} towers", towers.Count));
			bool isPositiveForTower = false;
			foreach (TurretEntity towerModel in towers)
			{
				towerModel.BuffsHolder.AddBuff(silentBuffKey, new BuffStatus(isPositiveForTower, 1f, effectTime / 1000f), BuffStackRule.ChooseMax, BuffStackRule.ChooseMax);
				AddEffectTowerSeal(towerModel);
			}
			timeTracking = coolDownTime / 1000f;
			attacking = false;
			onFinishAttack.Dispatch();
			yield return null;
			yield break;
		}

		private void AddEffectTowerSeal(TurretEntity tower)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(sealEffectName);
			effect.transform.position = tower.transform.position - new Vector3(0f, 0.05f, 0f);
			effect.SetLayerOverTarget(tower.transform);
			effect.Init(effectTime / 1000f);
		}

		private void GetTowers()
		{
			MonoSingleton<GameRecord>.Instance.GetInRangeTowers(base.EnemyModel.transform.position, rangeInPixel / GameRecord.PIXEL_PER_UNIT, inRangeTowers);
			MonoSingleton<GameRecord>.Instance.GetNearestTowers(base.EnemyModel.transform.position, maxTowers, inRangeTowers, nearestTowers, towersSquaredDistances);
		}

		[SerializeField]
		private float coolDownTime;

		[SerializeField]
		private int maxTowers;

		[SerializeField]
		private float effectTime;

		[SerializeField]
		private float rangeInPixel;

		private string silentBuffKey = "Silent";

		[Header("Common setting")]
		[SerializeField]
		private bool activateAtStart;

		[SerializeField]
		private float attackAnimationDuration = 1f;

		[SerializeField]
		private float offsetTimeAfterAnim;

		[SerializeField]
		private float minSpeed = 0.05f;

		[SerializeField]
		private GameObject sealEffectPrefab;

		[SerializeField]
		private string sealEffectName;

		[Space]
		[SerializeField]
		private OrderedUnityEvent onStartAttack = new OrderedUnityEvent();

		[SerializeField]
		private OrderedUnityEvent onFinishAttack = new OrderedUnityEvent();

		[SerializeField]
		private OrderedUnityEvent onCancelAttack = new OrderedUnityEvent();

		private float timeTracking;

		private bool attacking;

		private List<TurretEntity> inRangeTowers = new List<TurretEntity>();

		private List<TurretEntity> nearestTowers = new List<TurretEntity>();

		private List<float> towersSquaredDistances = new List<float>();

		private WaitForSeconds waitForAnimation;
	}
}
