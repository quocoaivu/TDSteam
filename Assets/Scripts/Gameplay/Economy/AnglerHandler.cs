using System;
using UnityEngine;

namespace Gameplay
{
	public class AnglerHandler : MonoBehaviour
	{
        private static string BONUSMONEY_OBJECT_NAME = "BonusMoney";

        private Animator animator;

        [SerializeField]
        private int chanceToSuccessFishing;

        [SerializeField]
        private int goldAmount;

        [SerializeField]
        private Transform goldPosition;

        [SerializeField]
        private float cooldownTime;

        private float cooldownTimeTracking;

        private void Awake()
		{
			animator = base.GetComponent<Animator>();
		}

		private void Start()
		{
			cooldownTimeTracking = cooldownTime;
		}

		private void Update()
		{
			if (!MonoSingleton<GameRecord>.Instance.IsGameStart)
			{
				return;
			}
			if (cooldownTimeTracking == 0f)
			{
				ProcessFishing();
			}
			cooldownTimeTracking = Mathf.MoveTowards(cooldownTimeTracking, 0f, Time.deltaTime);
		}

		private void ProcessFishing()
		{
			if (chanceToSuccessFishing < UnityEngine.Random.Range(0, 100))
			{
				animator.SetTrigger("Catch");
			}
			else
			{
				animator.SetTrigger("Miss");
			}
			cooldownTimeTracking = cooldownTime;
		}

		public void OnCatch()
		{
			DroppedBullionHandler droppedGold = MonoSingleton<FXPool>.Instance.GetDroppedGold();
			droppedGold.gameObject.SetActive(true);
			droppedGold.transform.position = goldPosition.position;
			droppedGold.Init(goldAmount);
			MonoSingleton<GameRecord>.Instance.IncreaseMoney(goldAmount);
		}

		public void OnMiss()
		{
		}
	}
}
