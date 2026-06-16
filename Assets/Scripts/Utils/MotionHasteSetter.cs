using System;
using UnityEngine;

public class MotionHasteSetter : MonoBehaviour
{
    [SerializeField]
    private float minSpeed;

    [SerializeField]
    private float maxSpeed;


    private void Start()
	{
		base.GetComponent<Animator>().speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
	}
}
