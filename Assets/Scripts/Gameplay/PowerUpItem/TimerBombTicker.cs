using System;
using TMPro;
using UnityEngine;

namespace Gameplay
{
	public class TimerBombTicker : MonoBehaviour
	{
		private void Update()
		{
			if (!startCountDown)
			{
				return;
			}
			if (MonoSingleton<GameRecord>.Instance.IsPause)
			{
				return;
			}
			if (textMesh)
			{
				if (intType)
				{
					textMesh.text = ((int)timeTracking + 1).ToString();
				}
				if (floatType)
				{
					textMesh.text = string.Format("{0:f1}", timeTracking);
				}
			}
			if (textMeshPro)
			{
				if (intType)
				{
					textMeshPro.text = ((int)timeTracking + 1).ToString();
				}
				if (floatType)
				{
					textMeshPro.text = string.Format("{0:f1}", timeTracking);
				}
			}
			if (IsCountDownReachZero())
			{
				FinishCountDown();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void Init(float duration)
		{
			this.duration = duration;
			timeTracking = duration;
			startCountDown = true;
		}

		private bool IsCountDownReachZero()
		{
			return timeTracking == 0f;
		}

		private void FinishCountDown()
		{
			timeTracking = 0f;
			startCountDown = false;
		}

		private float duration;

		private float timeTracking;

		private bool startCountDown;

		[SerializeField]
		private TextMesh textMesh;

		[SerializeField]
		private TextMeshPro textMeshPro;

		[SerializeField]
		private bool intType;

		[SerializeField]
		private bool floatType;
	}
}
