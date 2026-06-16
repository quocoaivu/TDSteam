using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroAbility : SwitchHandler
	{
		public int SkillID
		{
			get
			{
				return skillID;
			}
			set
			{
				skillID = value;
			}
		}

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

		public void Init(HeroAbilityClusterHandler heroSkillGroupController, int heroID, int heroLevel)
		{
			this.heroSkillGroupController = heroSkillGroupController;
			HeroID = heroID;
			skillIcon.sprite = Common.AssetLoader.Load<Sprite>(string.Format("HeroCamp/SkillIcons/hero_{0}_skill_{1}", heroID, SkillID));
		}

		public void RefreshSkillPoint()
		{
			int skillPoint = HeroStore.Instance.GetSkillPoint(HeroID, SkillID);
			for (int i = 0; i < skillPoints.Length; i++)
			{
				skillPoints[i].SetActive(i < skillPoint);
			}
		}

		public void PlayFXUpgrade()
		{
			animator.SetTrigger("CooldownDone");
		}

		public override void OnClick()
		{
			base.OnClick();
			heroSkillGroupController.SelectSkill(SkillID);
			heroSkillGroupController.ShowSelectedImage(base.transform);
		}

		[SerializeField]
		private int skillID;

		private int heroID;

		[SerializeField]
		private Image skillIcon;

		[SerializeField]
		private GameObject[] skillPoints;

		[Space]
		[SerializeField]
		private Animator animator;

		private HeroAbilityClusterHandler heroSkillGroupController;
	}
}
