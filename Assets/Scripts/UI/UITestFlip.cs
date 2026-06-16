using System;
using UnityEngine;
using UnityEngine.UI;

public class UITestFlip : MonoBehaviour
{
	private void Awake()
	{
		toggle = base.GetComponent<Toggle>();
	}

	public void OnValueChanged()
	{
		if (toggle.isOn)
		{
			panelTest.SetActive(true);
		}
		else
		{
			panelTest.SetActive(false);
		}
	}

	private Toggle toggle;

	public GameObject panelTest;
}
