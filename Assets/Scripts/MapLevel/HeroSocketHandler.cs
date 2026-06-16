using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class HeroSocketHandler : SwitchHandler
	{
		private void Awake()
		{
			GetAllComponents();
			button.enabled = false;
		}

		public void Init(HerosInputClusterHandler heroesInputGroupController)
		{
			this.heroesInputGroupController = heroesInputGroupController;
		}

		private void GetAllComponents()
		{
			button = base.GetComponent<Button>();
			image = base.GetComponent<Image>();
		}

		public override void OnClick()
		{
			base.OnClick();
			if (IsInitValue)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"bá» chá»n hero + ",
					heroID,
					" táº¡i slot ",
					slot
				}));
				heroesInputGroupController.HeroesSelectedController.UnChooseHero(slot);
				heroesInputGroupController.HeroesSelectController.UnChooseHero(heroID);
				IsInitValue = false;
				button.enabled = false;
				ViewUnChoose();
			}
		}

		public void InitHeroSlot(int slot, int heroID)
		{
			this.slot = slot;
			this.heroID = heroID;
			IsInitValue = true;
			button.enabled = true;
			SetHeroAvatar();
			SetHeroLevel();
			boosterObj.SetActive(false);
			if (GameKit.cachedHavingBooster)
			{
				boosterObj.SetActive(true);
				boosterText.text = string.Format("x{0}", GameKit.cachedBoosterMultiplier);
			}
		}

		public void SetDefaultValue()
		{
			IsInitValue = false;
			ViewUnChoose();
		}

		private void SetHeroAvatar()
		{
			image.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesAvatar/avatar_hero_{0}", heroID));
		}

		private void SetHeroLevel()
		{
			heroLevelText.text = (HeroStore.Instance.GetCurrentHeroLevel(heroID) + 1).ToString();
		}

		public void ViewUnChoose()
		{
			image.sprite = addHero;
			heroLevelText.text = string.Empty;
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		private Button button;

		private Image image;

		[SerializeField]
		private Sprite addHero;

		[SerializeField]
		private Text heroLevelText;

		public GameObject boosterObj;

		public TextMeshProUGUI boosterText;

		private int slot;

		private int heroID;

		public bool IsInitValue;

		private HerosInputClusterHandler heroesInputGroupController;
	}
}
