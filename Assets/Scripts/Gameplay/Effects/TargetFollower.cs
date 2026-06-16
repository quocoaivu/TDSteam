using System;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
	public void Init(GameObject target, Vector3 offset)
	{
		this.target = target;
		this.offset = offset;
	}

	private void Update()
	{
		base.transform.position = target.transform.position + offset;
	}

	private GameObject target;

	private Vector3 offset;
}
