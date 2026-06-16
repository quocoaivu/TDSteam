using System;
using UnityEngine;

namespace GameCore
{
	public class BaseMonoBehaviour : MonoBehaviour
	{
		public void CustomInvoke(Action a, float delayTime)
		{
			base.Invoke(a.Method.Name, delayTime);
		}

		public void CustomCancelInvoke(Action a)
		{
			base.CancelInvoke(a.Method.Name);
		}

		public void CustomCancelInvoke()
		{
			base.CancelInvoke();
		}

		public void CustomInvokeRepeating(Action a, float delayTime, float repeatRate)
		{
			base.InvokeRepeating(a.Method.Name, delayTime, repeatRate);
		}
	}
}
