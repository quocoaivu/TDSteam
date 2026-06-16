using UnityEngine;

public class VolumeReader : MonoBehaviour
{
	public float GetBGMVolume()
	{
		return volumeAdjust.volumeAttribute.volumeBGM;
	}

	public float GetUIVolume()
	{
		return volumeAdjust.volumeAttribute.volumeUI;
	}

	public float GetUIEffectVolume()
	{
		return volumeAdjust.volumeAttribute.volumeUIEffect;
	}

	public float GetGameplayEffectVolume()
	{
		return volumeAdjust.volumeAttribute.volumeGameplayEffect;
	}

	[SerializeField]
	private VolumeSettings volumeAdjust;
}
