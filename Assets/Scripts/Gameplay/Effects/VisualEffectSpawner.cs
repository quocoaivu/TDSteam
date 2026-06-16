using System;
using UnityEngine;

namespace Gameplay
{
	public class VisualEffectSpawner : MonoBehaviour
	{
		public void CastEffect(string effectName, float duration, Vector2 targetPosition)
		{
			VisualEffectInstance effect = MonoSingleton<FXPool>.Instance.GetEffect(effectName);
			effect.transform.position = targetPosition;
			effect.Init(duration);
		}
	}
}
