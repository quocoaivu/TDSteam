using System;
using GameCore;
using UnityEngine;

public class RandomSpinAtBegin : BaseMonoBehaviour
{
    [SerializeField]
    private float minValue;

    [SerializeField]
    private float maxValue;

    private float currentRotationValue;

    private Vector3 localRotation;


    private void Start()
	{
		currentRotationValue = UnityEngine.Random.Range(minValue, maxValue);
		localRotation.Set(0f, 0f, currentRotationValue);
		base.gameObject.transform.rotation = Quaternion.Euler(localRotation);
	}
}
