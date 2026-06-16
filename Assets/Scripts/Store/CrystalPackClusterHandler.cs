using System;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
	public class CrystalPackClusterHandler : MonoBehaviour
	{
		[SerializeField]
		private ScrollRect scrollRect;

		public void EnableScroll()
		{
			scrollRect.enabled = true;
		}

		public void DisableScroll()
		{
			scrollRect.enabled = false;
		}
	}
}
