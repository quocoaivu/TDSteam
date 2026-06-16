using System;
using UnityEngine;

public class AutoSpinZ : MonoBehaviour
{

    private Vector3 angle;

    [SerializeField]
    private float rotateSpd = 360f;

    private void Start()
	{
		angle = base.transform.eulerAngles;
	}

	private void Update()
	{
		angle.z = angle.z + Time.deltaTime * rotateSpd;
		base.transform.eulerAngles = angle;
	}
}
