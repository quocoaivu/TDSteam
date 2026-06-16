using System;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

public class DailyTabEntry : MonoBehaviour
{
    [SerializeField]
    private Image heroAvatar;

    [SerializeField]
    private Image heroName;

    [SerializeField]
    private Text heroLevel;

    [Space]
    [SerializeField]
    private GameObject selectedImage;

    [SerializeField]
    private Image[] itemsAvatar;

    [SerializeField]
    private Text[] itemsQuantity;

    [Space]
    [SerializeField]
    private int day;

    [SerializeField]
    private int notiID;

    [SerializeField]
    private Text dayTitle;

    [Space]
    [SerializeField]
    private GameObject notifyDone;

    [SerializeField]
    private GameObject notifyNotDone;

    private DailyOrdealSpecs param;

    [SerializeField]
    private bool displayInputItems;

    [SerializeField]
    private bool displayRewardItems;

    [SerializeField]
    private GameObject[] listItemHolder;

    public int Day
	{
		get
		{
			return day;
		}
		set
		{
			day = value;
		}
	}

	public void Init()
	{
		param = DailyOrdealSpec.Instance.GetParameter(Day);
		InitDayTitle();
		InitHeroAvatar();
		InitItems();
		InitSelectedImage();
		InitMissionStatus();
	}

	private void InitDayTitle()
	{
		if (dayTitle)
		{
			dayTitle.text = Singleton<AlertSynopsis>.Instance.GetNotiContent(notiID) + " " + (day + 1).ToString();
		}
	}

	private void InitHeroAvatar()
	{
		int num = 0;
		if (param.input_hero_id_slot_0 >= 0)
		{
			num = param.input_hero_id_slot_0;
		}
		if (param.input_hero_id_slot_1 >= 0)
		{
			num = param.input_hero_id_slot_1;
		}
		if (param.input_hero_id_slot_2 >= 0)
		{
			num = param.input_hero_id_slot_2;
		}
		if (Day == 6)
		{
			heroAvatar.sprite = Common.AssetLoader.Load<Sprite>("HeroesAvatar/avatar_hero_combo");
			heroLevel.text = string.Empty;
			if (heroName)
			{
				heroName.gameObject.SetActive(false);
			}
		}
		else
		{
			heroAvatar.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesAvatar/avatar_hero_{0}", num));
			heroLevel.text = (DailyTrialStore.Instance.GetHeroDailyTrialLevel() + 1).ToString();
			if (heroName)
			{
				heroName.gameObject.SetActive(true);
				heroName.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesName/name_hero_{0}", num));
			}
		}
	}

	private void InitItems()
	{
		foreach (Image image in itemsAvatar)
		{
			image.gameObject.SetActive(false);
		}
		foreach (Text text in itemsQuantity)
		{
			text.text = string.Empty;
		}
		if (displayInputItems)
		{
			int[] array3 = new int[4];
			array3 = DailyOrdealSpec.Instance.getListInputItem(Day);
			int num = 0;
			for (int k = 0; k < array3.Length; k++)
			{
				if (array3[k] > 0)
				{
					itemsAvatar[num].gameObject.SetActive(true);
					itemsAvatar[num].sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_pw_{0}", k));
					itemsQuantity[num].text = array3[k].ToString();
					num++;
				}
			}
		}
		if (displayRewardItems)
		{
			int[] array4 = new int[5];
			array4 = DailyOrdealSpec.Instance.getListRewardValue(Day);
			int num2 = 0;
			if (array4[4] > 0)
			{
				itemsAvatar[num2].gameObject.SetActive(true);
				itemsAvatar[num2].sprite = Common.AssetLoader.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
				itemsQuantity[num2].text = array4[4].ToString();
				num2++;
			}
			for (int l = 0; l < array4.Length - 1; l++)
			{
				if (array4[l] > 0)
				{
					itemsAvatar[num2].gameObject.SetActive(true);
					itemsAvatar[num2].sprite = Common.AssetLoader.Load<Sprite>(string.Format("LuckyChest/Items/lucky_item_pw_{0}", l));
					itemsQuantity[num2].text = array4[l].ToString();
					num2++;
				}
			}
			for (int m = 0; m < listItemHolder.Length; m++)
			{
				if (m < num2)
				{
					listItemHolder[m].SetActive(true);
				}
				else
				{
					listItemHolder[m].SetActive(false);
				}
			}
		}
	}

	private void InitSelectedImage()
	{
		int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
		if (selectedImage)
		{
			selectedImage.SetActive(Day == currentDayIndex);
		}
	}

	private void InitMissionStatus()
	{
		if (!notifyDone || !notifyNotDone)
		{
			return;
		}
		int currentDayIndex = DailyTrialStore.Instance.GetCurrentDayIndex();
		if (day < currentDayIndex)
		{
			notifyDone.SetActive(true);
			notifyNotDone.SetActive(false);
		}
		else if (day > currentDayIndex)
		{
			notifyDone.SetActive(false);
			notifyNotDone.SetActive(false);
		}
		else
		{
			bool flag = DailyTrialStore.Instance.IsDoneMaxTierMission(day);
			notifyDone.SetActive(flag);
			notifyNotDone.SetActive(!flag);
		}
	}
}
