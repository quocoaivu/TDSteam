using System;
using MetaGame;
using UnityEngine;

namespace Gameplay
{
	public class TrooperSfxHandler : MonoBehaviour
	{
		private void Awake()
		{
			audioSource = base.GetComponent<AudioSource>();
			audioSource.ignoreListenerPause = true;
		}

		private void Update()
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			audioSource.volume = MonoSingleton<TrooperSfxDirector>.Instance.VolumeReader.GetGameplayEffectVolume();
		}

		public void PlaySelect()
		{
			if (Setup.Instance.Sound && select != null && audioSource.isActiveAndEnabled)
			{
				int num = UnityEngine.Random.Range(0, select.Length);
				audioSource.clip = select[num];
				audioSource.Play();
			}
		}

		public void PlayStartMove()
		{
			if (Setup.Instance.Sound && startMove != null && audioSource.isActiveAndEnabled)
			{
				int num = UnityEngine.Random.Range(0, startMove.Length);
				audioSource.clip = startMove[num];
				audioSource.Play();
			}
		}

		public void PlayDie()
		{
			if (Setup.Instance.Sound && die && audioSource.isActiveAndEnabled)
			{
				audioSource.clip = die;
				audioSource.Play();
			}
		}

		public void PlayRespawn()
		{
			if (Setup.Instance.Sound && respawn && audioSource.isActiveAndEnabled)
			{
				audioSource.clip = respawn;
				audioSource.Play();
			}
		}

		public void PlayAttack(int index)
		{
			if (Setup.Instance.Sound && attack != null && index >= 0 && index < attack.Length && attack[index] && audioSource.isActiveAndEnabled)
			{
				audioSource.clip = attack[index];
				audioSource.Play();
			}
		}

		public void PlayRandomAttack()
		{
			if (Setup.Instance.Sound && attack != null && audioSource.isActiveAndEnabled)
			{
				int num = UnityEngine.Random.Range(0, attack.Length);
				audioSource.clip = attack[num];
				audioSource.Play();
			}
		}

		public void PlaySpecialAttack()
		{
			if (Setup.Instance.Sound && specialAttack && audioSource.isActiveAndEnabled)
			{
				audioSource.clip = specialAttack;
				audioSource.Play();
			}
		}

		private AudioSource audioSource;

		[SerializeField]
		private AudioClip[] startMove;

		[SerializeField]
		private AudioClip[] select;

		[SerializeField]
		private AudioClip die;

		[SerializeField]
		private AudioClip respawn;

		[SerializeField]
		private AudioClip[] attack;

		[SerializeField]
		private AudioClip specialAttack;
	}
}
