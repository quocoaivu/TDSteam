using System;
using UnityEngine;

public class StarClusterHandler : MonoBehaviour
{
	public void DisplayStarGroup(int starAmount)
	{
		HideAllStars();
		if (starAmount >= 1)
		{
			for (int i = 0; i < listStarGroup.Length; i++)
			{
				if (i <= starAmount - 1)
				{
					listStarGroup[i].SetActive(true);
				}
			}
		}
	}

	private void HideAllStars()
	{
		foreach (GameObject gameObject in listStarGroup)
		{
			gameObject.gameObject.SetActive(false);
		}
	}

	[SerializeField]
	private GameObject[] listStarGroup;
}
