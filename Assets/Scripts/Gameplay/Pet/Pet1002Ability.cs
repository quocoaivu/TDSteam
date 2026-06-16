using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Pet1002Ability : HeroAbilityShared
	{
		public override void Update()
		{
			base.Update();
			if (!unLock)
			{
				return;
			}
			if (IsCooldownDone())
			{
				TryToCastSkill();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		public override void Init(HeroEntity heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			duration = (float)heroModel.PetConfigData.Skillvalues[0];
			slowPercent = heroModel.PetConfigData.Skillvalues[1];
			cooldownTime = (float)heroModel.PetConfigData.Skillvalues[2] / 1000f;
			speed = (float)heroModel.PetConfigData.Speed;
			skillRange = (float)heroModel.PetConfigData.Atk_range_max / GameRecord.PIXEL_PER_UNIT;
			timeTracking = cooldownTime;
			unLock = true;
			InitFXs();
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void TryToCastSkill()
		{
			List<EnemyData> listActiveEnemy = MonoSingleton<GameRecord>.Instance.ListActiveEnemy;
			for (int i = listActiveEnemy.Count - 1; i >= 0; i--)
			{
				if (heroModel.IsInMeleeRange(listActiveEnemy[i]))
				{
					target = listActiveEnemy[i];
					break;
				}
			}
			if (GameKit.IsValidEnemy(target))
			{
				float specialStateDuration = GameKit.MoveToAttackPosition(heroModel, target, speed, new Action(OnMoveToTargetComplete));
				heroModel.SetSpecialStateDuration(specialStateDuration);
				heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animRun);
				heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[0]);
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

		private void OnMoveToTargetComplete()
		{
			if (target != null && target.IsAlive)
			{
				CastSkill();
			}
		}

		private void CastSkill()
		{
			heroModel.SetSpecialStateDuration(animationTime);
			heroModel.SetSpecialStateAnimationName(HeroMotionHandler.animActiveSkill);
			heroModel.GetFsmController().GetCurrentState().OnInput(PhaseInputKind.SpecialState, new object[]
			{
				HeroMotionHandler.animActiveSkill
			});
			heroModel.AddTarget(target);
			heroModel.LookAtEnemy();
			target.ProcessEffect(buffKey, 100, duration, DamageVfxType.Stun);
			ClearTarget();
		}

		private void ClearTarget()
		{
			target = null;
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_STUN);
		}
		private bool unLock;

		private int slowPercent;

		private float duration;

		private float cooldownTime;

		private float speed;

		private float skillRange;

		private float timeTracking;

		private string buffKey = "Slow";

		private EnemyData target;

		[SerializeField]
		private float animationTime;
	}
}
