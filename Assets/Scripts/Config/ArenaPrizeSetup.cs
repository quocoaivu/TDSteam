using System;
using UnityEngine;

[Serializable]
public class ArenaPrizeSetup : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public string SheetName = string.Empty;

    [HideInInspector]
    [SerializeField]
    public string WorksheetName = string.Empty;

    public ArenaPrizeSetupRecord[] dataArray;

    private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new ArenaPrizeSetupRecord[0];
		}
	}
}
