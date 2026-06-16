using System;
using UnityEngine;

namespace Gameplay
{
	public class GameplaySwitchHandler : MonoBehaviour
	{
		public virtual void OnClick()
		{
		}

		public virtual void OnMouseDown()
		{
		}

		protected enum ButtonStatus
		{
			Available,
			Confirm
		}
	}
}
