using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
    public bool OnlyDeactivate;

    private new ParticleSystem particleSystem;

    private float countdown;


    private void Start()
	{
		if (particleSystem == null)
		{
			particleSystem = base.GetComponent<ParticleSystem>();
		}
	}

	private void OnEnable()
	{
		countdown = 0.5f;
	}

	private void Update()
	{
		countdown -= Time.deltaTime;
		if (countdown <= 0f && !particleSystem.IsAlive(true))
		{
			if (OnlyDeactivate)
			{
				base.gameObject.SetActive(false);
			}
			else
			{
				base.gameObject.Recycle();
			}
		}
	}


}
