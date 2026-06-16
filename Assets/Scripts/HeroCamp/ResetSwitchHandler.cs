using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class ResetSwitchHandler : SwitchHandler
	{
		public void Init(HeroAbilityClusterHandler heroSkillGroupController, int heroID)
		{
			this.heroSkillGroupController = heroSkillGroupController;
			this.heroID = heroID;
			RefreshButtonStatus();
		}

		public override void OnClick()
		{
			base.OnClick();
			ResetSkillPoint();
		}

		private void ResetSkillPoint()
		{
			HeroStore.Instance.ResetSkillPoint(heroID);
			heroSkillGroupController.RefreshSkillPoint();
			heroSkillGroupController.RefreshSkillInformation();
			HeroStore.Instance.OnSkillPointChange(true);
		}

		private void RefreshButtonStatus()
		{
			if (HeroStore.Instance.IsHeroOwned(heroID))
			{
				ShowInteractable();
			}
			else
			{
				ShowNonInteractable();
			}
		}

		private void ShowInteractable()
		{
			button.enabled = true;
			image.color = Color.white;
		}

		private void ShowNonInteractable()
		{
			button.enabled = false;
			image.color = Color.gray;
		}

		[SerializeField]
		private Button button;

		[SerializeField]
		private Image image;

		private HeroAbilityClusterHandler heroSkillGroupController;

		private int heroID;
	}
}
