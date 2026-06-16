using System;
using UnityEngine;

namespace Gameplay
{
	public class BlastInMark : MonoBehaviour
	{
        [SerializeField]
        private float explosionDuration;

        [SerializeField]
        private string explosionFXName;

        public void CastExplosion(Transform targetTransform)
		{
			if (targetTransform != null && targetTransform.gameObject.activeSelf)
			{
				VisualEffectInstance explosion = MonoSingleton<FXPool>.Instance.GetExplosion(explosionFXName);
				explosion.Init(explosionDuration, targetTransform);
			}
		}
	}
}
