using System;
using UnityEngine;

[Serializable]
public class SalePackSetup : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public string SheetName = string.Empty;

    [HideInInspector]
    [SerializeField]
    public string WorksheetName = string.Empty;

    public SalePackSetupRecord[] dataArray;

    private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new SalePackSetupRecord[0];
		}
	}
}
