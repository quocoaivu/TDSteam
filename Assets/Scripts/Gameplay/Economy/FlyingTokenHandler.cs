using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class FlyingTokenHandler : MonoBehaviour
	{
        [SerializeField]
        private float timeToMove;

        public void Init(Vector3 target)
		{
			base.transform.DOMove(target, timeToMove, false).SetEase(Ease.Linear);
		}
	}
}
