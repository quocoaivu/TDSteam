using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Pet1006Ability : HeroAbilityShared
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
			slowPercent = heroModel.PetConfigData.Skillvalues[0];
			duration = (float)heroModel.PetConfigData.Skillvalues[1];
			damage = heroModel.PetConfigData.Skillvalues[2];
			cooldownTime = (float)heroModel.PetConfigData.Skillvalues[3];
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
			target.ProcessEffect(buffKey, slowPercent, duration, DamageVfxType.Electric);
			ClearTarget();
		}

		private void ClearTarget()
		{
			target = null;
		}

		private void InitFXs()
		{
			MonoSingleton<FXPool>.Instance.InitFX(FXPool.EFFECT_ELECTRIC);
		}
		private bool unLock;

		private int damage;

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
