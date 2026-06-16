using System;
using UnityEngine;

[Serializable]
public class ArenaConstantSetup : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public string SheetName = string.Empty;

    [HideInInspector]
    [SerializeField]
    public string WorksheetName = string.Empty;

    public ArenaConstantSetupRecord[] dataArray;

    private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new ArenaConstantSetupRecord[0];
		}
	}
}
