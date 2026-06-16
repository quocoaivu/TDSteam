using System;
using Data;
using Notify;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class SelectHeroSwitchHandler : SwitchHandler
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

		public void UpdateNotifyHeroSkill()
		{
			if (HeroStore.Instance.IsHeroOwned(HeroID))
			{
				generalNotify.TryShowNotify(HeroStore.Instance.GetCurrentSkillPoint(HeroID) >= 1);
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			HeroBarracksDialogHandler.Instance.currentHeroID = HeroID;
			HeroBarracksDialogHandler.Instance.ShowSelectedHeroImage(base.transform);
			HeroBarracksDialogHandler.Instance.RefreshHeroInformation();
		}

		public void UpdateHeroLevel()
		{
			if (heroLevel)
			{
				heroLevel.text = (HeroStore.Instance.GetCurrentHeroLevel(HeroID) + 1).ToString();
			}
		}

		[SerializeField]
		private int heroID;

		[SerializeField]
		private Text heroLevel;

		[SerializeField]
		private NotifyBadge generalNotify;
	}
}
