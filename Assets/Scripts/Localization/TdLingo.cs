using System;
using UnityEngine;

[Serializable]
public class TdLingo : ScriptableObject
{
	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new TdLingoRecord[0];
		}
	}

	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public TdLingoRecord[] dataArray;
}
