using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroAbilityClusterHandler : MonoBehaviour
	{
		public HeroAbilityOverview HeroSkillInformation
		{
			get
			{
				return heroSkillInformation;
			}
			set
			{
				heroSkillInformation = value;
			}
		}

		public void Init(int heroID, int currentLevel)
		{
			this.heroID = heroID;
			for (int i = 0; i < listAbilityItem.Count; i++)
			{
				listAbilityItem[i].Init(this, heroID, currentLevel);
			}
			listAbilityItem[0].OnClick();
			RefreshSkillPoint();
		}

		public void RefreshSkillPoint()
		{
			currentSkillPoint.text = HeroStore.Instance.GetCurrentSkillPoint(heroID).ToString();
			for (int i = 0; i < listAbilityItem.Count; i++)
			{
				listAbilityItem[i].RefreshSkillPoint();
			}
			trainButtonController.RefreshButtonStatus();
		}

		public void RefreshSkillInformation()
		{
			HeroSkillInformation.Init(heroID, skillID);
		}

		public void SelectSkill(int skillID)
		{
			this.skillID = skillID;
			trainButtonController.Init(this, heroID, skillID);
			resetButtonController.Init(this, heroID);
			HeroSkillInformation.Init(heroID, skillID);
		}

		public void CastUpgradeSkillFX(int heroID, int skillID)
		{
			foreach (HeroAbility heroSkill in listAbilityItem)
			{
				if (heroSkill.HeroID == heroID && heroSkill.SkillID == skillID)
				{
					heroSkill.PlayFXUpgrade();
				}
			}
		}

		public void ShowSelectedImage(Transform transform)
		{
			selectedImage.Init(transform);
		}

		[Space]
		[Header("Skill Point")]
		[SerializeField]
		private Text currentSkillPoint;

		[Space]
		[Header("List Ability Items ")]
		[SerializeField]
		private List<HeroAbility> listAbilityItem;

		[Space]
		[SerializeField]
		private HeroAbilityOverview heroSkillInformation;

		[Space]
		[SerializeField]
		private SelectedPictureHandler selectedImage;

		[Space]
		[SerializeField]
		private TrainSwitchHandler trainButtonController;

		[SerializeField]
		private ResetSwitchHandler resetButtonController;

		private int heroID;

		private int skillID;
	}
}
