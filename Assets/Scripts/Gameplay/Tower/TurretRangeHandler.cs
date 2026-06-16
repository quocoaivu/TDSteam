using System;
using UnityEngine;

public class TurretRangeHandler : MonoBehaviour
{
	private void Awake()
	{
		originSize = base.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
	}

	public void Update()
	{
		if (null != target)
		{
			base.transform.position = target.position;
		}
	}

	public void SetRangeAttackMax(float size)
	{
		this.size = size;
		base.gameObject.SetActive(true);
		float num = size * 2.25f / originSize;
		base.transform.localScale = new Vector3(num, num, num);
	}

	public void HideRange()
	{
		base.gameObject.SetActive(false);
		target = null;
		base.transform.position = new Vector3(-100f, 100f, 0f);
	}

	[HideInInspector]
	public Transform target;

	[SerializeField]
	private float size;

	private float originSize;
}
