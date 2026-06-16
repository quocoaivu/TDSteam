using System;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class ArmamentBayAnimator : BaseMonoBehaviour
	{
		public void PlayAnimAttack()
		{
			animator.Play("Attack");
		}

		public void PlayAnimAppear()
		{
			animator.Play("Appear");
		}

		public void PlayAnimDisappear()
		{
			animator.Play("Disappear");
		}

		public void Reset()
		{
			animator.Play("Appear");
		}

		[SerializeField]
		private Animator animator;
	}
}
