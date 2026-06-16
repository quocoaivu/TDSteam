using System;
using UnityEngine;

[Serializable]
public class DailyPrizeSetup : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public string SheetName = string.Empty;

    [HideInInspector]
    [SerializeField]
    public string WorksheetName = string.Empty;

    public DailyPrizeSetupRecord[] dataArray;

    private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new DailyPrizeSetupRecord[0];
		}
	}
}
