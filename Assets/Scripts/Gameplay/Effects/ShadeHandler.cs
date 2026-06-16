using System;
using GameCore;
using UnityEngine;

public class ShadeHandler : BaseMonoBehaviour
{
	private void Awake()
	{
		targetObject = target.transform;
		offset = targetObject.position - base.transform.position;
	}

	private void LateUpdate()
	{
		base.transform.position = targetObject.position + offset;
	}

	public GameObject target;

	private Vector3 offset;

	private Transform targetObject;
}
