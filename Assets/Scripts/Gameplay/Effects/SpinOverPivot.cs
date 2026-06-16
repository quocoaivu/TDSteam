using System;
using UnityEngine;

namespace Gameplay
{
	public class SpinOverPivot : MonoBehaviour
	{
		private void Update()
		{
			Rotate();
		}

		private void Rotate()
		{
			base.transform.Rotate(Vector3.back * (float)RotateDirection * speed);
		}

		[SerializeField]
		private float speed;

		public int RotateDirection;
	}
}
