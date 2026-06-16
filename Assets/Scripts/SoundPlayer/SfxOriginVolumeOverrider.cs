using System;
using Gameplay;
using MetaGame;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxOriginVolumeOverrider : MonoBehaviour
{
	public void Awake()
	{
		config = Setup.Instance;
		audioSource = base.GetComponent<AudioSource>();
	}

	public void Update()
	{
		if (MonoSingleton<GameRecord>.Instance.IsPause)
		{
			audioSource.volume = 0f;
		}
		else
		{
			audioSource.volume = ((!config.Sound) ? 0f : normalVolume);
		}
	}

	[SerializeField]
	[HideInInspector]
	private AudioSource audioSource;

	[SerializeField]
	[Range(0f, 1f)]
	private float normalVolume = 1f;

	private Setup config;
}
