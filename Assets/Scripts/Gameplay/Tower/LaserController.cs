using System;
using GameCore;
using UnityEngine;

namespace Gameplay
{
	public class LaserController : BaseMonoBehaviour
	{
		private void Awake()
		{
			_line = base.GetComponent<LineRenderer>();
			_line.useWorldSpace = true;
			InitSize();
		}

		public void Init()
		{
			if (!_line)
			{
				_line = base.GetComponent<LineRenderer>();
				_line.useWorldSpace = true;
			}
			isRunning = true;
			InitSize();
		}

		private void InitSize()
		{
			_line.SetPosition(0, firePosition.position);
			_line.SetPosition(1, firePosition.position);
		}

		public void Resize(Vector3 endPos)
		{
			_line.SetPosition(0, firePosition.position);
			_line.SetPosition(1, endPos);
			effectStart.SetActive(true);
			effectEnd.SetActive(true);
			effectStart.transform.position = firePosition.position;
			effectEnd.transform.position = endPos;
		}

		public void StopImmediate()
		{
			InitSize();
			isRunning = false;
			target = null;
			effectStart.SetActive(false);
			effectEnd.SetActive(false);
		}

		private LineRenderer _line;

		[SerializeField]
		private GameObject effectStart;

		[SerializeField]
		private GameObject effectEnd;

		[SerializeField]
		private Transform firePosition;

		private bool isRunning;

		private GameObject target;

		private Vector3 targetLastPos = Vector3.zero;
	}
}
