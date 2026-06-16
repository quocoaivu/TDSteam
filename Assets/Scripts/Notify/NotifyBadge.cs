using System;
using GameCore;
using UnityEngine;

namespace Notify
{
	public class NotifyBadge : BaseMonoBehaviour
	{
		public void TryShowNotify(bool isShow)
		{
			notifyUnit.SetActive(isShow);
		}

		[SerializeField]
		private GameObject notifyUnit;
	}
}
