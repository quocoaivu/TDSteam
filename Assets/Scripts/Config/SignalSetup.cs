using System;
using UnityEngine;

[Serializable]
public class SignalSetup : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public string SheetName = string.Empty;

    [HideInInspector]
    [SerializeField]
    public string WorksheetName = string.Empty;

    public SignalSetupRecord[] dataArray;

    private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new SignalSetupRecord[0];
		}
	}
}
