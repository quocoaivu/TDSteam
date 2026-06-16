using System;
using Common;
using UnityEngine;

namespace Notify
{
	public abstract class AlertTrooper : MonoBehaviour
	{
		protected abstract bool ShouldShowNotify();

		public void CheckCondition()
		{
			if (ShouldShowNotify())
			{
				onShowNotify.Dispatch();
			}
			else
			{
				onNotShowNotify.Dispatch();
			}
		}

		[Space]
		[SerializeField]
		private OrderedUnityEvent onShowNotify;

		[SerializeField]
		private OrderedUnityEvent onNotShowNotify;
	}
}
