using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaRankEntryDirector : MonoBehaviour
{
	public void Init(ArenaPlayerDetail playerInfo)
	{
		highlightBg.SetActive(playerInfo.isYou);
		rankText.text = (playerInfo.rank + 1).ToString();
		userNameText.text = playerInfo.name;
		for (int i = playerInfo.heroIds.Count; i < heroIcons.Count; i++)
		{
			heroIcons[i].gameObject.SetActive(false);
		}
		int num = Mathf.Min(heroIcons.Count, playerInfo.heroIds.Count);
		for (int j = 0; j < num; j++)
		{
			heroIcons[j].gameObject.SetActive(true);
			GameKit.SetRewardSprite(new PrizeItem(PrizeKind.SingleHero, playerInfo.heroIds[j], 1, false), heroIcons[j]);
		}
		if (flagImage != null)
		{
			flagImage.sprite = Common.AssetLoader.Load<Sprite>(string.Format("CountryFlags2/{0}", playerInfo.countryCode));
		}
		timeText.text = string.Format("{0}:{1:00}.{2:000}", (int)playerInfo.time.TotalMinutes, playerInfo.time.Seconds, playerInfo.time.Milliseconds);
	}

	public Text rankText;

	public Text userNameText;

	public Text timeText;

	public List<Image> heroIcons;

	public GameObject highlightBg;

	public Image flagImage;
}
