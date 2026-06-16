using System;
using MetaGame;
using UnityEngine;
using UnityEngine.Serialization;

public class UISfxDirector : MonoBehaviour
{
	public static UISfxDirector Instance { get; set; }

	private void Awake()
	{
		if (UISfxDirector.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		UISfxDirector.Instance = this;
		audioSourceUI.ignoreListenerPause = true;
		audioSourceResult.ignoreListenerPause = true;
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		UpdateVolume();
	}

	private void UpdateVolume()
	{
		audioSourceUI.volume = volumeReader.GetUIVolume();
		audioSourceUIEffect.volume = volumeReader.GetUIEffectVolume();
		audioSourceGameplayLayer0.volume = volumeReader.GetGameplayEffectVolume();
		audioSourceGameplayLayer1.volume = volumeReader.GetGameplayEffectVolume();
	}

	public void PlayCallEnemy()
	{
		if (Setup.Instance.Sound && sfxGameplayNoti.callEnemy)
		{
			audioSourceGameplayLayer0.clip = sfxGameplayNoti.callEnemy;
			audioSourceGameplayLayer0.Play();
		}
	}

	public void PlayBeforeCallEnemy()
	{
		if (Setup.Instance.Sound && sfxGameplayNoti.beforeCallEnemy)
		{
			audioSourceGameplayLayer1.clip = sfxGameplayNoti.beforeCallEnemy;
			audioSourceGameplayLayer1.Play();
		}
	}

	public void PlayNewEnemyButton()
	{
		if (Setup.Instance.Sound && sfxGameplayNoti.newEnemyButton)
		{
			audioSourceGameplayLayer0.clip = sfxGameplayNoti.newEnemyButton;
			audioSourceGameplayLayer0.Play();
		}
	}

	public void PlayNewTipButton()
	{
		if (Setup.Instance.Sound && sfxGameplayNoti.newTipsButton)
		{
			audioSourceGameplayLayer1.clip = sfxGameplayNoti.newTipsButton;
			audioSourceGameplayLayer1.Play();
		}
	}

	public void PlayClick()
	{
		if (Setup.Instance.Sound && uiSound.click)
		{
			audioSourceUI.clip = uiSound.click;
			audioSourceUI.Play();
		}
	}

	public void PlayClosePopup()
	{
		if (Setup.Instance.Sound && uiSound.close)
		{
			audioSourceUI.clip = uiSound.close;
			audioSourceUI.Play();
		}
	}

	public void PlayOpenPopup()
	{
		if (Setup.Instance.Sound && uiSound.open)
		{
			audioSourceUI.clip = uiSound.open;
			audioSourceUI.Play();
		}
	}

	public void PlayCloseLoading()
	{
		if (Setup.Instance.Sound && uiSound.loadingClose)
		{
			audioSourceUI.clip = uiSound.loadingClose;
			audioSourceUI.Play();
		}
	}

	public void PlayOpenLoading()
	{
		if (Setup.Instance.Sound && uiSound.loadingOpen)
		{
			audioSourceUI.clip = uiSound.loadingOpen;
			audioSourceUI.Play();
		}
	}

	public void PlayVictory()
	{
		if (Setup.Instance.Sound && uiResult.victory)
		{
			audioSourceResult.clip = uiResult.victory;
			audioSourceResult.Play();
		}
	}

	public void PlayDefeat()
	{
		if (Setup.Instance.Sound && uiResult.defeat)
		{
			audioSourceResult.clip = uiResult.defeat;
			audioSourceResult.Play();
		}
	}

	public void PlayluckyChestSound()
	{
		if (Setup.Instance.Sound && uiResult.openLuckyChest)
		{
			audioSourceResult.clip = uiResult.openLuckyChest;
			audioSourceResult.Play();
		}
	}

	public void PlayStartGameAtMainMenu()
	{
		if (Setup.Instance.Sound && uiEffect.startGameAtMainMenu)
		{
			audioSourceUIEffect.clip = uiEffect.startGameAtMainMenu;
			audioSourceUIEffect.Play();
		}
	}

	public void PlayStartGameAtMapLevel()
	{
		if (Setup.Instance.Sound && uiEffect.startGameAtMapLevel)
		{
			audioSourceUIEffect.clip = uiEffect.startGameAtMapLevel;
			audioSourceUIEffect.Play();
		}
	}

	public void PlayUpgradeSuccess()
	{
		if (Setup.Instance.Sound && uiEffect.upgradeSuccess)
		{
			audioSourceUIEffect.clip = uiEffect.upgradeSuccess;
			audioSourceUIEffect.Play();
		}
	}

	public void PlayBuySuccess()
	{
		if (Setup.Instance.Sound && uiEffect.buySuccess)
		{
			audioSourceUIEffect.clip = uiEffect.buySuccess;
			audioSourceUIEffect.Play();
		}
	}

	public void PlayUnlockSuccess()
	{
		if (Setup.Instance.Sound && uiEffect.unlockSuccess)
		{
			audioSourceUIEffect.clip = uiEffect.unlockSuccess;
			audioSourceUIEffect.Play();
		}
	}

	[Space]
	[Header("Audio Sources")]
	[SerializeField]
	[FormerlySerializedAs("audioSource_UI")]
	private AudioSource audioSourceUI;

	[SerializeField]
	[FormerlySerializedAs("audioSource_Result")]
	private AudioSource audioSourceResult;

	[SerializeField]
	[FormerlySerializedAs("audioSource_UIEffect")]
	private AudioSource audioSourceUIEffect;

	[Space]
	[SerializeField]
	[FormerlySerializedAs("audioSourcesGameplay_Layer0")]
	private AudioSource audioSourceGameplayLayer0;

	[SerializeField]
	[FormerlySerializedAs("audioSourcesGameplay_Layer1")]
	private AudioSource audioSourceGameplayLayer1;

	[Space]
	[SerializeField]
	[FormerlySerializedAs("readDataVolumeAdjust")]
	private VolumeReader volumeReader;

	[Space]
	[Header("Audio Clips")]
	[SerializeField]
	private UIWindowSfxClips uiSound;

	[Space]
	[SerializeField]
	private ResultSfxClips uiResult;

	[Space]
	[SerializeField]
	private GameplayNotificationSfxClips sfxGameplayNoti;

	[Space]
	[SerializeField]
	private UISfxClips uiEffect;
}
