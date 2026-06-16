using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class EnemyAnimation : EnemyBrain
	{
        public static string animRunRight = "Run-Right";

        public static string animRunUp = "Run-Up";

        public static string animRunDown = "Run-Down";

        public static string animRunUnderground = "RunUnderground";

        public static string animAttackMelee = "MeleeAttack";

        public static string animAttackRange = "RangeAttack";

        public static string animSpecialAttack = "SpecialAttack";

        public static string animDie = "Die";

        public static string animIdle = "Idle";

        public static string animAppear = "Appear";

        public static string animHidingAlert = "HidingAlert";

        private Animator animator;

        private EnemyAnimationSpeedModifier enemyAnimationSpeedController;

        private static readonly HashSet<int> _loggedMissingStates = new HashSet<int>();

        public override void Initialize()
		{
			base.Initialize();
			GetAllComponents();
		}

		public override void OnAppear()
		{
			base.OnAppear();
		}

		private void GetAllComponents()
		{
			animator = base.GetComponent<Animator>();
			enemyAnimationSpeedController = base.EnemyModel.GetComponent<EnemyAnimationSpeedModifier>();
		}

		public void ToSpawnFromGroundState()
		{
			SafePlay(EnemyAnimation.animAppear);
		}

		public void ToRunState(string animationName)
		{
			SafePlay(animationName);
		}

		public void ToIdleState()
		{
			SafePlay(EnemyAnimation.animIdle);
		}

		public void ToMeleeAttackState()
		{
			SafePlay(EnemyAnimation.animAttackMelee);
		}

		public void ToRangeAttackState()
		{
			SafePlay(EnemyAnimation.animAttackRange);
		}

		public void ToDieState()
		{
			enemyAnimationSpeedController.SetNormalSpeed();
			SafePlay(EnemyAnimation.animDie);
		}

		public void ToSpecialAttackState()
		{
			SafePlay(EnemyAnimation.animSpecialAttack);
		}

		private void SafePlay(string stateName)
		{
			RuntimeAnimatorController controller = animator.runtimeAnimatorController;
			if (controller == null)
			{
				return;
			}
			int hash = Animator.StringToHash(stateName);
			if (!animator.HasState(0, hash))
			{
				int key = controller.GetEntityId() ^ hash;
				if (EnemyAnimation._loggedMissingStates.Add(key))
				{
					Debug.LogWarning(string.Format("[EnemyAnimation] State '{0}' missing on controller '{1}' (enemy: {2}).", stateName, controller.name, base.gameObject.name), this);
				}
				return;
			}
			animator.Play(hash);
		}

		public void TurnBack()
		{
			Vector3 localScale = base.EnemyModel.transform.localScale;
			Vector3 localScale2 = new Vector3(-localScale.x, localScale.y, 1f);
			base.EnemyModel.transform.localScale = localScale2;
		}

		public void TurnOnAnimator()
		{
			animator.enabled = true;
		}

		public void TurnOffAnimator()
		{
			animator.enabled = false;
		}

	}
}
