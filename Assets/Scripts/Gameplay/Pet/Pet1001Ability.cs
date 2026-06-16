using System;
using System.Collections;
using System.Collections.Generic;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Pet1001Ability : HeroAbilityShared
	{
		public override void Update()
		{
			base.Update();
			if (!unLock)
			{
				return;
			}
			if (isMoveToTarget)
			{
				ChangeAnimationRun(heroModel.transform.position, targetCachePosition);
				return;
			}
			if (isCastingSkill)
			{
				ChangeAnimationRun(heroModel.transform.position, outsidePosition);
				return;
			}
			if (IsCooldownDone())
			{
				TryToCastSkill();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			dragBackValue = (float)heroModel.PetConfigData.Skillvalues[0];
			cooldownTime = (float)heroModel.PetConfigData.Skillvalues[1] / 1000f;
			if (readMaxTargetHpFromConfig)
			{
				maxTargetHp = heroModel.PetConfigData.Skillvalues[2];
			}
			speed = (float)heroModel.PetConfigData.Speed;
			timeTracking = 2f;
			unLock = true;
			isCastingSkill = false;
			isMoveToTarget = false;
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void GetTarget()
		{
			List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
			for (int i = listActiveEnemy.Count - 1; i >= 0; i--)
			{
				if (heroModel.IsInMeleeRange(listActiveEnemy[i]) && !listActiveEnemy[i].IsUnderground && !listActiveEnemy[i].IsInTunnel && !EnemyDatabase.Instance.IsBoss(listActiveEnemy[i].Id) && listActiveEnemy[i].EnemyHealthController.OriginHealth <= maxTargetHp)
				{
					target = listActiveEnemy[i];
					break;
				}
			}
		}

		private void TryToCastSkill()
		{
			GetTarget();
			if (target != null)
			{
				if (target.IsAlive)
				{
					float specialStateDuration = GameKit.MoveToAttackPosition(heroModel, target, speed, new Action(OnMoveToTargetComplete));
					heroModel.GetAnimationController().ToRunState();
					isMoveToTarget = true;
					targetCachePosition = target.transform.position;
					ChangeAnimationRun(heroModel.transform.position, target.transform.position);
					target.SetSpecialStateDuration(specialStateDuration);
					target.SetSpecialStateAnimationName(EnemyAnimation.animIdle);
					target.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
					{
						EnemyAnimation.animIdle
					});
					target.EnemyAnimationController.ToIdleState();
				}
				timeTracking = cooldownTime;
			}
			else
			{
				timeTracking = 1f;
			}
		}

		private void OnMoveToTargetComplete()
		{
			if (target != null && target.IsAlive)
			{
				base.StartCoroutine(CastSkill());
			}
			isMoveToTarget = false;
		}

		private IEnumerator CastSkill()
		{
			isCastingSkill = true;
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(animationTime);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			yield return new WaitForSeconds(animationTime);
			outsidePosition = GetOutsideVector();
			float timeToMoveToTarget = GameKit.TimeMoveToPosition(heroModel, 2f * speed, outsidePosition, new Action(OnMoveOutsideComplete));
			heroModel.GetAnimationController().ToRunState();
			if (GameKit.IsValidEnemy(target))
			{
				target.SetSpecialStateDuration(timeToMoveToTarget);
				target.SetSpecialStateAnimationName(EnemyAnimation.animIdle);
				target.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
				{
					EnemyAnimation.animIdle
				});
				target.EnemyAnimationController.ToIdleState();
				target.IsInTunnel = true;
				target.transform.SetParent(attachPoint);
				target.transform.localPosition = Vector3.zero;
				target.transform.localScale = Vector3.one;
				target.EnemyHealthController.HideHealthBar();
			}
			yield break;
		}

		private void OnMoveOutsideComplete()
		{
			target.transform.SetParent(null);
			if (target.IsAlive)
			{
				// TotalEnemy is decremented inside Dead() (handles multi-life via lifeCount).
				target.Dead();
				target.gameObject.SetActive(false);
				target.ReturnPool(5f);
			}
			target = null;
			isCastingSkill = false;
		}

		private void ChangeAnimationRun(Vector3 currentPosition, Vector3 assignedPosition)
		{
			if (assignedPosition.x - currentPosition.x > 0f)
			{
				heroModel.transform.localScale = Vector3.one;
			}
			else
			{
				heroModel.transform.localScale = invertXVector;
			}
		}

		private Vector3 GetOutsideVector()
		{
			Vector3 result = new Vector3(0f, 0f, 0f);
			if (UnityEngine.Random.Range(0, 100) < 50)
			{
				float newX = 7f;
				float newY = UnityEngine.Random.Range(-5f, 5f);
				result.Set(newX, newY, 0f);
			}
			else
			{
				float newX2 = -7f;
				float newY2 = UnityEngine.Random.Range(-5f, 5f);
				result.Set(newX2, newY2, 0f);
			}
			return result;
		}

		public bool readMaxTargetHpFromConfig;
		private bool unLock;

		private float dragBackValue;

		private float cooldownTime;

		private float speed;

		private float timeTracking;

		private bool isCastingSkill;

		private bool isMoveToTarget;

		private EnemyData target;

		private Vector3 targetCachePosition = Vector3.zero;

		[SerializeField]
		private float animationTime;

		[SerializeField]
		private Transform attachPoint;

		private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

		private Vector3 outsidePosition = Vector3.zero;

		private int maxTargetHp = 300;
	}
}
