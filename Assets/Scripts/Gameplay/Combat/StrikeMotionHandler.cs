using System;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class StrikeMotionHandler : BaseMonoBehaviour
	{
        public GameObject source;

        public GameObject target;

        public virtual void Init(GameObject target, float lifeTime)
		{
			this.target = target;
			base.CustomInvoke(new Action(OnReturnPool), lifeTime);
		}

		public virtual void Run()
		{
			base.gameObject.SetActive(true);
		}

		private void StopImmediately()
		{
			base.gameObject.SetActive(false);
		}

		public void OnReturnPool()
		{
			StopImmediately();
		}


	}
}
