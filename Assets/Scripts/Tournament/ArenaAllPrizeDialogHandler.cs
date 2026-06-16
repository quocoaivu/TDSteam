using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class ArenaAllPrizeDialogHandler : GameplayDialogHandler
{
	public void Init(int curLeagueIndex)
	{
		OpenWithScaleAnimation();
		if (!isInited)
		{
			isInited = true;
			scrollHandle.offsetMin = new Vector2(scrollHandle.offsetMin.x, 0f);
			scrollHandle.offsetMax = new Vector2(scrollHandle.offsetMax.x, 0f);
			int numberOfLeagues = GameKit.GetNumberOfLeagues();
			leagueEntries.Add(sampleEntry);
			for (int i = 1; i < numberOfLeagues; i++)
			{
				ArenaAllPrizeLeagueEntry tourAllPrizeLeagueEntry = UnityEngine.Object.Instantiate<ArenaAllPrizeLeagueEntry>(sampleEntry, sampleEntry.transform.parent);
				tourAllPrizeLeagueEntry.transform.localPosition = sampleEntry.transform.localPosition + new Vector3(0f, (float)(-(float)i) * entryHeight, 0f);
				leagueEntries.Add(tourAllPrizeLeagueEntry);
			}
			for (int j = 0; j < numberOfLeagues; j++)
			{
				leagueEntries[j].Init(j);
			}
			scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, (float)numberOfLeagues * entryHeight + 50f);
		}
		Vector3 localPosition = scrollContent.localPosition;
		localPosition.y = entryHeight * (float)curLeagueIndex;
		scrollContent.localPosition = localPosition;
	}

	private List<ArenaAllPrizeLeagueEntry> leagueEntries = new List<ArenaAllPrizeLeagueEntry>();

	public ArenaAllPrizeLeagueEntry sampleEntry;

	public float entryHeight;

	public RectTransform scrollContent;

	public RectTransform scrollHandle;

	private bool isInited;
}
