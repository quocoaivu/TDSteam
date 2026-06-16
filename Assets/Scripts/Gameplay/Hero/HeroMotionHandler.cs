using System;
using UnityEngine;

namespace Gameplay
{
	public class HeroMotionHandler : HeroHandler, IMotionHandler
	{
		public static string animRun = "Run";
        public static string animAttackMeLee = "MeleeAttack";
        public static string animAttackRange = "RangeAttack";
        public static string animDie = "Die";
        public static string animIdle = "Idle";
        public static string animPlay = "Play";
        public static string animActiveSkill = "ActiveSkill";
        public static string animPassiveSkill_0 = "Passive0";
        public static string animPassiveSkill_1 = "Passive1";
        public static string animPassiveSkill_2 = "Passive2";

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

		public void ToWakeupState()
		{
		}

		public void ToRunState()
		{
			animator.Play(HeroMotionHandler.animRun);
		}

		public void ToIdleState()
		{
			animator.Play(HeroMotionHandler.animIdle);
		}

		public void ToMeleeAttackState()
		{
			animator.Play(HeroMotionHandler.animAttackMeLee);
		}

		public void ToRangeAttackState()
		{
			animator.Play(HeroMotionHandler.animAttackRange);
		}

		public void ToDieState()
		{
			animator.Play(HeroMotionHandler.animDie);
		}

		public void ToAppearState()
		{
			ToIdleState();
		}

		public void ToPlayState()
		{
			animator.Play(HeroMotionHandler.animPlay);
		}

		public bool ContainAppearAnim()
		{
			return false;
		}

		public void ToSpecialState(string animationName, float duration)
		{
			animator.Play(animationName);
		}
	}
}
