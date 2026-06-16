using System;
using UnityEngine;

namespace Common
{
	public class RandomAnimatorHasteSetter : MonoBehaviour
	{
        [SerializeField]
        private float minSpeed;

        [SerializeField]

        private float maxSpeed;
        private void Start()
		{
			Set();
		}

		private void Set()
		{
			float speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
			base.GetComponent<Animator>().speed = speed;
		}
	}
}
