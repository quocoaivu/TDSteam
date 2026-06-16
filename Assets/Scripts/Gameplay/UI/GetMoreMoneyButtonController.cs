using System;
using Gameplay;
using UnityEngine;

public class GetMoreMoneyButtonController : MonoBehaviour
{
	public void OnClick()
	{
		MonoSingleton<GameRecord>.Instance.IncreaseMoney(goldAmount);
	}

	[SerializeField]
	private int goldAmount;
}
