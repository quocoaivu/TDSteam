using System;
using GameCore;
using UnityEngine;

public class SnowfallFx : BaseMonoBehaviour
{
    [SerializeField]
    private float playTimeMin;

    [SerializeField]
    private float playTimeMax;

    [SerializeField]
    private float restTimeMin;

    [SerializeField]
    private float restTimeMax;

    private ParticleSystem particle;

    private void Awake()
	{
		particle = base.GetComponent<ParticleSystem>();
	}

	private void Start()
	{
		PlaySnow();
	}

	private void PlaySnow()
	{
		particle.Play();
		base.CustomInvoke(new Action(StopSnow), UnityEngine.Random.Range(playTimeMin, playTimeMax));
	}

	private void StopSnow()
	{
		particle.Stop();
		base.CustomInvoke(new Action(PlaySnow), UnityEngine.Random.Range(restTimeMin, restTimeMax));
	}
}
