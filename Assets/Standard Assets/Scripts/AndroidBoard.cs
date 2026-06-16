using System;
using UnityEngine;

internal class AndroidBoard : IBoard
{
	public void SetText(string str)
	{
		UnityEngine.Debug.Log("Set Text At AndroidBoard: " + str);
		this.cb.CallStatic("setText", new object[]
		{
			str
		});
	}

	public string GetText()
	{
		return this.cb.CallStatic<string>("getText", new object[0]);
	}

	private AndroidJavaClass cb = new AndroidJavaClass("jp.ne.donuts.uniclipboard.Clipboard");
}
