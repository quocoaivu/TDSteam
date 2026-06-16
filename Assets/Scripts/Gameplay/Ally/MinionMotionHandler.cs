using System;
using UnityEngine;

namespace Gameplay
{
	public class MinionMotionHandler : MinionHandler, IMotionHandler
	{

        public static string animAppear = "Appear";

        public static string animDisappear = "Disappear";

        public static string animRun = "Run";

        public static string animMeleeAttack = "MeleeAttack";

        public static string animRangeAttack = "RangeAttack";

        public static string animDie = "Die";

        public static string animIdle = "Idle";

        public static string appearTrigger = "AppearTrigger";

        public static string animActiveSkill = "ActiveSkill";

        private Animator animator;

        public override void Initialize()
		{
			base.Initialize();
			GetAllComponents();
		}

		public override void OnAppear()
		{
			base.OnAppear();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void GetAllComponents()
		{
			animator = base.GetComponent<Animator>();
		}

		public void ToRunState()
		{
			animator.Play(MinionMotionHandler.animRun);
		}

		public void ToIdleState()
		{
			animator.Play(MinionMotionHandler.animIdle);
		}

		public void ToPlayState()
		{
			ToIdleState();
		}

		public void ToMeleeAttackState()
		{
			animator.Play(MinionMotionHandler.animMeleeAttack, -1, 0f);
		}

		public void ToRangeAttackState()
		{
			animator.Play(MinionMotionHandler.animRangeAttack, -1, 0f);
		}

		public void ToDieState()
		{
			animator.Play(MinionMotionHandler.animDie);
		}

		public void ToAppearState()
		{
			animator.SetTrigger(MinionMotionHandler.appearTrigger);
		}

		public void ToDisappearState()
		{
			animator.Play(MinionMotionHandler.animDisappear);
		}

		public bool ContainAppearAnim()
		{
			AnimatorControllerParameter[] parameters = animator.parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Trigger && animatorControllerParameter.name == MinionMotionHandler.appearTrigger)
				{
					return true;
				}
			}
			return false;
		}

		public void ToSpecialState(string animationName, float duration)
		{
			animator.Play(animationName);
		}
	}
}
