using System;
using DG.Tweening;
using GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
	public class PlayerVitalityHandler : BaseMonoBehaviour
	{
		public void SetHealthMessage()
		{
			int currentHealth = MonoSingleton<GameRecord>.Instance.CurrentHealth;
			if (currentHealth != lastHealth)
			{
				healthMessage.SetText("{0}", currentHealth);
				AnimationChangeHealth();
				lastHealth = currentHealth;
			}
		}

		private void AnimationChangeHealth()
		{
			tween.Restart(true);
			tween = healthMessage.transform.DOScale(scaleVector, 0.2f).OnComplete(new TweenCallback(LateAnimation));
		}

		private void LateAnimation()
		{
			tween = healthMessage.transform.DOScale(Vector3.one, 0.1f);
		}

		[SerializeField]
		[FormerlySerializedAs("heathMessage")]
		private TextMeshProUGUI healthMessage;

		private int lastHealth = -1;

		private Vector3 scaleVector = new Vector3(1.2f, 1.2f, 1.2f);

		private Tween tween;
	}
}
