using MetaGame;
using GameCore;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicPlayer : BaseMonoBehaviour
{
    private AudioSource audioSource;

    private VolumeReader volumeReader;

    private void Awake()
	{
		audioSource = base.GetComponent<AudioSource>();
		volumeReader = base.GetComponent<VolumeReader>();
	}

	private void Start()
	{
		audioSource.loop = true;
		audioSource.Play();
	}

	private void Update()
	{
		UpdateVolume();
	}

	private void UpdateVolume()
	{
		audioSource.volume = ((!Setup.Instance.Music) ? 0f : volumeReader.GetBGMVolume());
	}

	public void StopBackgroundMusic()
	{
		audioSource.Stop();
	}

	public void PlayBackgroundMusic()
	{
		audioSource.Play();
	}
}
