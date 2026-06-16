using System;
using Data;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class HeroSelectSwitchHandler : SwitchHandler
	{
		public int HeroID
		{
			get
			{
				return heroID;
			}
			set
			{
				heroID = value;
			}
		}

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = base.GetComponent<Button>();
			image = base.GetComponent<Image>();
		}

		private void OnEnable()
		{
			SetHeroLevel();
		}

		public void UpdateStatus()
		{
			bool flag = HeroStore.Instance.IsHeroOwned(HeroID);
			if (flag)
			{
				image.color = Color.white;
				lockImage.SetActive(false);
				base.transform.SetAsFirstSibling();
			}
			else
			{
				image.color = Color.gray;
				lockImage.SetActive(true);
				base.transform.SetAsLastSibling();
			}
		}

		public void Init(int heroID, HerosInputClusterHandler heroesInputGroupController)
		{
			HeroID = heroID;
			this.heroesInputGroupController = heroesInputGroupController;
			SetHeroAvatar();
			SetHeroLevel();
		}

		private void SetHeroAvatar()
		{
			image.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesMiniAvatar/mini_avatar_hero_{0}", HeroID));
		}

		private void SetHeroLevel()
		{
			heroLevelText.text = (HeroStore.Instance.GetCurrentHeroLevel(HeroID) + 1).ToString();
		}

		public void SetDefaultValue()
		{
			SetView_NonSelect();
		}

		public override void OnClick()
		{
			base.OnClick();
			if (HeroStore.Instance.IsHeroOwned(HeroID))
			{
				if (heroesInputGroupController.HeroesSelectedController.IsFullSlot())
				{
					return;
				}
				if (!Selected)
				{
					heroesInputGroupController.HeroesSelectedController.ChooseHero(HeroID);
					SetView_Selected();
					SendEvent_ChooseHero();
				}
			}
			else
			{
				UnityEngine.Debug.Log("Ban co muon unlock hero khong ???");
				heroesInputGroupController.AskToBuyHeroDialogHandler.Init(HeroID);
			}
		}

		private void SendEvent_ChooseHero()
		{
			string heroName = HeroParameterManager.Instance.GetHeroName(HeroID);
			//NativeSpecificServicesSource.Services.Analytics.SendEvent_ChooseHeroAtMapLevelSelect(heroName);
		}

		public void SetView_NonSelect()
		{
			Selected = false;
			base.gameObject.SetActive(true);
		}

		public void SetView_Selected()
		{
			Selected = true;
			base.gameObject.SetActive(false);
		}

		private Button button;

		private Image image;

		[SerializeField]
		private Text heroLevelText;

		[SerializeField]
		private GameObject lockImage;

		private int heroID;

		private HerosInputClusterHandler heroesInputGroupController;

		public bool Selected;
	}
}
