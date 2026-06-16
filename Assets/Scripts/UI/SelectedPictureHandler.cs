using System;
using UnityEngine;

public class SelectedPictureHandler : MonoBehaviour
{
	public void Init(Transform targetTransform)
	{
		this.targetTransform = targetTransform;
	}

	private void Update()
	{
		if (targetTransform)
		{
			base.transform.position = targetTransform.position;
		}
	}

	private Transform targetTransform;
}
