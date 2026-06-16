// kingdom defends from Assembly-CSharp.dll class: CFX_ShurikenThreadFix
using System;
using System.Collections;
using UnityEngine;

public class CFX_ShurikenThreadFix : MonoBehaviour
{
    private ParticleSystem[] systems;


    private void OnEnable()
	{
		systems = base.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in systems)
		{
			var em = particleSystem.emission;
			em.enabled = false;
		}
		base.StartCoroutine("WaitFrame");
	}

	private IEnumerator WaitFrame()
	{
		yield return null;
		foreach (ParticleSystem particleSystem in systems)
		{
			var em = particleSystem.emission;
			em.enabled = true;
			particleSystem.Play(true);
		}
		yield break;
	}
}
