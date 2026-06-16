using System;
using UnityEngine;
using UnityEngine.UI;

public class ArenaTabDirector : MonoBehaviour
{
	public void SetFocus(bool isFocused)
	{
		this.isFocused = isFocused;
		// Guard unassigned serialized refs (dropped during the prefab rename/migration)
		// so an unwired tab doesn't NRE and block the whole Arena hub from opening.
		if (tabBg != null)
		{
			tabBg.color = isFocused ? Color.white : Color.gray;
		}
		if (tabTitleText != null)
		{
			Color color = tabTitleText.color;
			color.a = isFocused ? 1f : 0.5f;
			tabTitleText.color = color;
		}
	}

	public Image tabBg;

	public Text tabTitleText;

	[HideInInspector]
	public bool isFocused;
}
