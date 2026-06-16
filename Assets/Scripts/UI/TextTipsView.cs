using System;
using Gameplay;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

public class TextTipsView : MonoBehaviour
{
	public void GetContentTextTip()
	{
		int mapID = MonoSingleton<GameRecord>.Instance.MapID;
		textTipContent = Singleton<TextHintSpec>.Instance.GetRandomTextTipContent(mapID);
	}

	public void ShowContentTextTip()
	{
		textTipShow.text = textTipContent;
	}

	public void EnableTextTip()
	{
		textTipShow.enabled = true;
	}

	public void DisableTextTip()
	{
		textTipShow.enabled = false;
	}

	public Text textTipShow;

	private string textTipContent;
}
