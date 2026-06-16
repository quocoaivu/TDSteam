using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class SetTailoredSurgeTest : MonoBehaviour
{
	public void OnSetWaveBtnClicked()
	{
		int num = int.Parse(waveInput.text);
		GameplayDirector.Instance.SetCustomWaveForTest(num);
		UnityEngine.Debug.Log("___Next wave will be wave " + num);
	}

	public InputField waveInput;
}
