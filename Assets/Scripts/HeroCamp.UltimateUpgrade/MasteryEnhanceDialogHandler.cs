using System;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp.UltimateUpgrade
{
	public class MasteryEnhanceDialogHandler : GeneralDialogHandler
	{
		public void Init()
		{
			Open();
			heroID = HeroBarracksDialogHandler.Instance.currentHeroID;
			petID = HeroParameterManager.Instance.GetPetID(heroID);
			upgradeButtonController.Init(this, heroID);
			InitHeroPetInformation();
			UpdateUpgradeButtonState();
		}

		private void InitHeroPetInformation()
		{
			foreach (Image image in heroAvatars)
			{
				image.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroesAvatar/avatar_hero_{0}", heroID));
			}
			petActionAvatarGroupController.ShowSelectedPetActionAvatar(petID);
			petName.text = GameKit.GetLocalization(string.Format("PET_NAME_ID_{0}", petID));
			petDescription.text = GameKit.GetPetDescription(petID);
		}

		public void UpdateUpgradeButtonState()
		{
			if (HeroStore.Instance.IsPetAvailable(heroID))
			{
				if (HeroStore.Instance.IsPetUnlocked(heroID))
				{
					upgradeButtonController.Hide();
				}
				else
				{
					upgradeButtonController.Show();
				}
			}
			else
			{
				upgradeButtonController.Hide();
			}
		}

		public void CastEffectUpgrade()
		{
			effectUpgrade.SetActive(true);
			UISfxDirector.Instance.PlayUpgradeSuccess();
		}

		public override void Open()
		{
			base.Open();
			effectUpgrade.SetActive(false);
		}

		public override void Close()
		{
			base.Close();
			effectUpgrade.SetActive(false);
		}

		[SerializeField]
		private Image[] heroAvatars;

		[Space]
		[SerializeField]
		private Text petName;

		[SerializeField]
		private Text petDescription;

		[SerializeField]
		private Transform petAvatarHolder;

		[Space]
		[SerializeField]
		private PetActionPortraitClusterHandler petActionAvatarGroupController;

		[Space]
		[SerializeField]
		private EnhanceSwitchHandler upgradeButtonController;

		[Space]
		[SerializeField]
		private GameObject effectUpgrade;

		private int heroID;

		private int petID;

		private const string PET_NAME_PREFIX = "PET_NAME_ID_{0}";
	}
}
