using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class GetMoreSpeedButtonController : MonoBehaviour
{
	private void Update()
	{
		speedText.text = "X " + GameplayDirector.Instance.gameSpeedController.GameSpeed.ToString();
	}

	public void OnClick()
	{
		GameplayDirector.Instance.gameSpeedController.GameSpeed += 2f;
	}

	[SerializeField]
	private Text speedText;
}
