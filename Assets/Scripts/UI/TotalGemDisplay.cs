using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class TotalGemDisplay : MonoBehaviour
{
	private void Awake()
	{
		PlayerCurrencyStore.Instance.OnGemChangeEvent += Instance_OnGemChangeEvent;
	}

	private void OnDestroy()
	{
		if (PlayerCurrencyStore.Instance != null)
		{
			PlayerCurrencyStore.Instance.OnGemChangeEvent -= Instance_OnGemChangeEvent;
		}
	}

	private void Instance_OnGemChangeEvent()
	{
		UpdateGemMessage();
	}

	private void Start()
	{
		UpdateGemMessage();
	}

	public void UpdateGemMessage()
	{
		textGem.text = PlayerCurrencyStore.Instance.GetCurrentGem().ToString();
	}

	public void PlayAnimationNotEnoughGem()
	{
		gemTextAnimator.SetTrigger("Anim");
	}

	[SerializeField]
	private Text textGem;

	[SerializeField]
	private Animator gemTextAnimator;
}
