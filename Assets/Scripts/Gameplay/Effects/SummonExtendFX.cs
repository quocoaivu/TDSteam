using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class SummonExtendFX : MonoBehaviour
	{
		private void Start()
		{
			MonoSingleton<FXPool>.Instance.InitFX(effectName);
			targetPosition = GameObject.FindGameObjectWithTag("CoinPosition");
		}

		public void Init()
		{
			base.StartCoroutine(CreateFlyingCoin());
		}

		private IEnumerator CreateFlyingCoin()
		{
			yield return new WaitForSeconds(delayTime);
			for (int i = 0; i < amountOfEffect; i++)
			{
				VisualEffectInstance coin = MonoSingleton<FXPool>.Instance.GetEffect(FXPool.FLYING_COIN);
				coin.transform.position = base.gameObject.transform.position;
				coin.Init(effectLifeTime);
				coin.GetComponent<FlyingTokenHandler>().Init(targetPosition.transform.position);
				yield return new WaitForSeconds(timeStepBetweenEffect);
			}
			yield break;
		}

		[SerializeField]
		private string effectName;

		[SerializeField]
		private int amountOfEffect;

		[SerializeField]
		private float timeStepBetweenEffect;

		[SerializeField]
		private float delayTime;

		[SerializeField]
		private float effectLifeTime;

		private GameObject targetPosition;
	}
}
