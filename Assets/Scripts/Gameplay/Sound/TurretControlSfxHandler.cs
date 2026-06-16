using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class TurretControlSfxHandler : MonoSingleton<TurretControlSfxHandler>
	{
		private void Awake()
		{
			audioSource = base.GetComponent<AudioSource>();
		}

		private void Update()
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			audioSource.volume = MonoSingleton<TrooperSfxDirector>.Instance.VolumeReader.GetGameplayEffectVolume();
		}

		public void PlayBuild(int towerID)
		{
			if (Setup.Instance.Sound && buildTowers != null && towerID >= 0 && towerID < buildTowers.Length && buildTowers[towerID] != null)
			{
				audioSource.clip = buildTowers[towerID];
				audioSource.Play();
			}
		}

		public void PlaySell()
		{
			if (Setup.Instance.Sound && sell)
			{
				audioSource.clip = sell;
				audioSource.Play();
			}
		}

		public void PlayUpgradeNormal(int towerID)
		{
			if (Setup.Instance.Sound && upgradeNormals != null && towerID >= 0 && towerID < upgradeNormals.Length && upgradeNormals[towerID] != null)
			{
				audioSource.clip = upgradeNormals[towerID];
				audioSource.Play();
			}
		}

		public void PlayUpgradeUltimate()
		{
			if (Setup.Instance.Sound && upgradeUltimate != null)
			{
				audioSource.clip = upgradeUltimate;
				audioSource.Play();
			}
		}

		private AudioSource audioSource;

		[SerializeField]
		private AudioClip[] buildTowers;

		[SerializeField]
		private AudioClip[] upgradeNormals;

		[SerializeField]
		private AudioClip upgradeUltimate;

		[SerializeField]
		private AudioClip sell;
	}
}
