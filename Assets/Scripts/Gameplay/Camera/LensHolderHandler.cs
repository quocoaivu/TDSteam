using System;
using DG.Tweening;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class LensHolderHandler : MonoBehaviour
	{
        [Space]
        [Header("Camera Shake")]
        [SerializeField]
        private float duration;

        [SerializeField]
        private Vector3 streng;

        [SerializeField]
        private int vibro;

        [SerializeField]
        private float randomness;

        [SerializeField]
        private bool snapping;


        private bool isShaking;
        [ContextMenu("Shake")]
		public void ShakeNormal()
		{
			isShaking = true;
			base.transform.DOShakePosition(duration, streng, vibro, randomness, snapping, true).OnComplete(new TweenCallback(EndShake));
			if (Setup.Instance.Vibration)
			{
				Handheld.Vibrate();
			}
		}

		private void EndShake()
		{
			isShaking = false;
		}
	}
}
