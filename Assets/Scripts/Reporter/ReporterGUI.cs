using System;
using UnityEngine;

public class ReporterGUI : MonoBehaviour
{
	private void Awake()
	{
		reporter = base.gameObject.GetComponent<Reporter>();
	}

	private void OnGUI()
	{
		reporter.OnGUIDraw();
	}

	private Reporter reporter;
}
