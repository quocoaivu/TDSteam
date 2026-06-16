using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class TrainSwitchHandler : SwitchHandler
	{
		public void Init(HeroAbilityClusterHandler heroSkillGroupController, int heroID, int skillID)
		{
			this.heroSkillGroupController = heroSkillGroupController;
			this.heroID = heroID;
			this.skillID = skillID;
			RefreshButtonStatus();
		}

		public override void OnClick()
		{
			base.OnClick();
			TrainSkill();
		}

		private void TrainSkill()
		{
			HeroStore.Instance.IncreaseSkillLevel(heroID, skillID);
			heroSkillGroupController.RefreshSkillPoint();
			heroSkillGroupController.CastUpgradeSkillFX(heroID, skillID);
			heroSkillGroupController.RefreshSkillInformation();
			RefreshButtonStatus();
			UISfxDirector.Instance.PlayUpgradeSuccess();
			HeroStore.Instance.OnSkillPointChange(true);
		}

		public void RefreshButtonStatus()
		{
			if (HeroStore.Instance.GetCurrentSkillPoint(heroID) >= 1 && !HeroStore.Instance.IsMaxSkill(heroID, skillID))
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
			image.sprite = interacableSprite;
		}

		private void ShowNonInteractable()
		{
			button.enabled = false;
			image.sprite = nonInteracableSprite;
		}

		[SerializeField]
		private Button button;

		[SerializeField]
		private Image image;

		[SerializeField]
		private Sprite interacableSprite;

		[SerializeField]
		private Sprite nonInteracableSprite;

		private HeroAbilityClusterHandler heroSkillGroupController;

		private int heroID;

		private int skillID;
	}
}
