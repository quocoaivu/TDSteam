using System;
using Gameplay;
using UnityEngine;

public class GetMoreHealthButtonController : MonoBehaviour
{
	public void OnClick()
	{
		GameplayDirector.Instance.gameLogicController.IncreaseHealth(healthAmount);
	}

	[SerializeField]
	private int healthAmount;
}
