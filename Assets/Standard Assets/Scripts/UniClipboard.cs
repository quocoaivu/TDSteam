using System;
using UnityEngine;

public class UniClipboard
{
	private static IBoard board
	{
		get
		{
			if (UniClipboard._board == null)
			{
				UniClipboard._board = new AndroidBoard();
			}
			return UniClipboard._board;
		}
	}

	public static void SetText(string str)
	{
		UnityEngine.Debug.Log("SetText");
		UniClipboard.board.SetText(str);
	}

	public static string GetText()
	{
		return UniClipboard.board.GetText();
	}

	private static IBoard _board;
}
